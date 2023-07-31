using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Wfm.App.Common;
using Wfm.App.Core;
using Wfm.App.Core.Model;
using Wfm.App.Infrastructure.Interfaces;

namespace Wfm.App.Infrastructure.Repositories
{
    //Testing
    public class CardGenerationRepository : ICardGenerationRepository
    {
        private ApplicationEntities db;
        public CardGenerationRepository()
        {
            db = new ApplicationEntities();
        }
        public List<PartialWorkflowMasterVieweMetaData> GetAllEmployeesByCopanyIdAndDeptId(Guid? deptId, Guid? sub_dept_id, int? emptype_id, Guid? BUILDING_ID)
        {
            Guid cmp_id = SessionHelper.Get<Guid>("CompanyId"); 
            List<PartialWorkflowMasterVieweMetaData> wfms = (from wfm in db.TAB_WORKFORCE_MASTER
                                                             join company in db.TAB_COMPANY_MASTER on wfm.COMPANY_ID equals company.COMPANY_ID
                                                             join dept in db.TAB_DEPARTMENT_MASTER on wfm.DEPT_ID equals dept.DEPT_ID
                                                             join desig in db.TAB_WF_DESIGNATION_MASTER on wfm.WF_DESIGNATION_ID equals desig.WF_DESIGNATION_ID
                                                             where company.COMPANY_ID == cmp_id && dept.DEPT_ID == deptId
                                                             && wfm.SUBDEPT_ID==(sub_dept_id != null? sub_dept_id : wfm.SUBDEPT_ID)
                                                             && wfm.BUILDING_ID==(BUILDING_ID != null? BUILDING_ID : wfm.BUILDING_ID)
                                                             && wfm.WF_EMP_TYPE==(emptype_id != null? emptype_id : wfm.WF_EMP_TYPE)
                                                             && wfm.STATUS=="Y"
                                                             select new PartialWorkflowMasterVieweMetaData
                                                             {
                                                                 WF_ID = wfm.WF_ID,
                                                                 COMPANY_NAME = company.COMPANY_NAME,
                                                                 EMP_NAME = wfm.EMP_NAME,
                                                                 EMP_ID = wfm.EMP_ID,
                                                                 DESIGNATION_NAME = desig.WF_DESIGNATION_NAME,
                                                                 DEPT_NAME = dept.DEPT_NAME,
                                                                 BIOMETRIC_CODE = wfm.BIOMETRIC_CODE,
                                                                 MOBILE_NO = wfm.MOBILE_NO
                                                             }).ToList();

            return wfms;
        }

        public List<WorkforceTypeMetaData> GetOnRollOrContracts(Guid comp_id)
        {
            List<WorkforceTypeMetaData> result = db.TAB_WORFORCE_TYPE.Where(x => x.COMPANY_ID == comp_id)
                .Select(x => new WorkforceTypeMetaData
                {
                    EMP_TYPE=x.EMP_TYPE,
                    WF_EMP_TYPE=x.WF_EMP_TYPE
                }).OrderBy(x=>x.EMP_TYPE).ToList();
            return result;
           
        }

        public List<GenerateCardViewModel> GenerateCards(IEnumerable<Guid> wfIds)
        {
            List<GenerateCardWorkflowMasterVieweMetaData> wfms = (from wfm in db.TAB_WORKFORCE_MASTER
                                                                  join wfphoto in db.TAB_WORKFORCE_PHOTO_MASTER on wfm.WF_ID equals wfphoto.WF_ID into jwf_phot
                                                                  join company in db.TAB_COMPANY_MASTER on wfm.COMPANY_ID equals company.COMPANY_ID
                                                                  join agency in db.TAB_AGENCY_MASTER on wfm.AGENCY_ID equals agency.AGENCY_ID into jwf_agency
                                                                  join dept in db.TAB_DEPARTMENT_MASTER on wfm.DEPT_ID equals dept.DEPT_ID
                                                                  join desig in db.TAB_WF_DESIGNATION_MASTER on wfm.WF_DESIGNATION_ID equals desig.WF_DESIGNATION_ID
                                                                  from wfphoto in jwf_phot.DefaultIfEmpty()
                                                                  from agency in jwf_agency.DefaultIfEmpty()
                                                                  where wfIds.Contains(wfm.WF_ID)
                                                                  select new GenerateCardWorkflowMasterVieweMetaData
                                                                  {
                                                                      WF_ID = wfm.WF_ID,
                                                                      COMPANY_NAME = company.COMPANY_NAME,
                                                                      EMP_NAME = wfm.EMP_NAME,
                                                                      EMP_ID = wfm.EMP_ID,
                                                                      DESIGNATION_NAME = desig.WF_DESIGNATION_NAME,
                                                                      DEPT_NAME = dept.DEPT_NAME,
                                                                      BIOMETRIC_CODE = wfm.BIOMETRIC_CODE,
                                                                      MOBILE_NO = wfm.MOBILE_NO,
                                                                      ADDRESS1 = company.ADDRESS1,
                                                                      ADDRESS2 = company.ADDRESS2,
                                                                      AGENCY_NAME = agency!=null?agency.AGENCY_NAME:null,
                                                                      AGENCY_ADDRESS1=agency!=null? agency.AGENCY_ADDRESS:null,
                                                                      AGENCY_ADDRESS2=null,
                                                                      EMP_EMG_MOB = wfm.ALTERNATE_NO,
                                                                      WF_EMP_TYPE=wfm.WF_EMP_TYPE,
                                                                      EMP_LOC_ADDR = wfm.PRESENT_ADDRESS,
                                                                      EMP_PERM_ADDR = wfm.PERMANENT_ADDRESS,
                                                                      PHOTO = wfphoto != null ? wfphoto.PHOTO : null,
                                                                      DOJ=wfm.DOJ
                                                                  }).ToList();

            List<GenerateCardViewModel> generateCards = new List<GenerateCardViewModel>();
            var generateOnRoleCard = db.TAB_MAIL_TEMPLATE.Where(x => x.TEMPLATE_FOR == "GenerateOnRoleCard").FirstOrDefault();
            var generateOnAgentCard_AK = db.TAB_MAIL_TEMPLATE.Where(x => x.TEMPLATE_FOR == "GenerateOnAgentCard_AK").FirstOrDefault();
            var generateOnAgentCard_SD = db.TAB_MAIL_TEMPLATE.Where(x => x.TEMPLATE_FOR == "GenerateOnAgentCard_SD").FirstOrDefault();
            string IssueDate = DateTime.Now.ToString("dd/MM/yyyy");
            foreach (var emp in wfms)
            {
                StringBuilder builder = new StringBuilder();
                if (emp.WF_EMP_TYPE== (short)Enum_WFEmpType.Contract)
                {
                    if(emp.AGENCY_NAME== "A.K. AGENCIES")
                    {
                        builder.Append(generateOnAgentCard_AK.TEMPLATE_CONTANT);
                    }
                    else
                    {
                        builder.Append(generateOnAgentCard_SD.TEMPLATE_CONTANT);
                    }
                    
                    builder.Replace("[CMP_NAME]", emp.AGENCY_NAME);
                    builder.Replace("[ADDRESS1]", emp.AGENCY_ADDRESS1);
                    builder.Replace("[ADDRESS2]", emp.AGENCY_ADDRESS2);
                    builder.Replace("[DOJ]", emp.DOJ.Value!=null?emp.DOJ.Value.ToString("dd/M/yyyy", CultureInfo.InvariantCulture):"");
                }
                else
                {
                    builder.Append(generateOnRoleCard.TEMPLATE_CONTANT);
                    builder.Replace("[CMP_NAME]", emp.COMPANY_NAME);
                    builder.Replace("[ADDRESS1]", emp.ADDRESS1);
                    builder.Replace("[ADDRESS2]", emp.ADDRESS2);
                }
                builder.Replace("[EMP_NAME]", emp.EMP_NAME);
                builder.Replace("[EMP_CODE]", emp.EMP_ID);
                builder.Replace("[BIO_CODE]", emp.BIOMETRIC_CODE);
                builder.Replace("[DEPARTMENT]", emp.DEPT_NAME);
                builder.Replace("[DESIGNATION]", emp.DESIGNATION_NAME);
                builder.Replace("[EMP_MOB]", emp.MOBILE_NO);
                builder.Replace("[LOC_ADDR]", emp.EMP_LOC_ADDR);
                builder.Replace("[PERM_ADDR]", emp.EMP_PERM_ADDR);
                builder.Replace("[EMG_MOB]", emp.EMP_EMG_MOB);
                builder.Replace("[IssueDate]", IssueDate);
                if (emp.PHOTO == null)
                {
                    builder.Replace("[WF_SRC_URL]", "/Content/IdCardImages/profile.png");
                }
                else
                {
                    string base65Img = "data:image/png;base64," + Convert.ToBase64String(emp.PHOTO, 0, emp.PHOTO.Length);
                    builder.Replace("[WF_SRC_URL]", base65Img);
                }
                // builder.Replace("[EMP_IMG]",);
                builder.Append(" <br />");
                GenerateCardViewModel generateCard = new GenerateCardViewModel
                {
                    EmployeeCard = builder.ToString()
                };
                generateCards.Add(generateCard);
            }

            return generateCards;
        }
    }

    public enum Enum_WFEmpType:short
    {
        OnRoll=1,
        Contract
    }
}
