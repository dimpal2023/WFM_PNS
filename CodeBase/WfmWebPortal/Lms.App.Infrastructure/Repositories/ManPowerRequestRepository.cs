using System;
using System.Collections.Generic;
using Wfm.App.Core;
using Wfm.App.Core.Model;
using System.Linq;
using System.Text;
using Wfm.App.Common;
using Wfm.App.Infrastructure.Interfaces;
using System.Web;

namespace Wfm.App.Infrastructure.Repositories
{
    public class ManPowerRequestRepository : IManPowerRequestRepository
    {
        private ApplicationEntities _appEntity;

        public ManPowerRequestRepository()
        {
            _appEntity = new ApplicationEntities();
        }

        public List<BuildingMasterMetaData> GetBuildings()
        {
            string Role = SessionHelper.Get<string>("LoginUserId");
            string Unit_ID = SessionHelper.Get<string>("BUILDING_ID");
            if (Role == "admin")
            {
                return _appEntity.TAB_BUILDING_MASTER.Where(x => x.status == "Y").Select(s => new BuildingMasterMetaData
                {
                    BUILDING_NAME = s.BUILDING_NAME,
                    BUILDING_ID = s.BUILDING_ID
                }).ToList();
            }
            else
            {
                return _appEntity.TAB_BUILDING_MASTER.Where(x => x.BUILDING_ID == new Guid(Unit_ID)).Select(s => new BuildingMasterMetaData
                {
                    BUILDING_NAME = s.BUILDING_NAME,
                    BUILDING_ID = s.BUILDING_ID
                }).OrderBy(x => x.BUILDING_NAME).ToList();
            }

        }

        public List<BuildingMasterMetaData> GetBuildings_Transfer()
        {
            return _appEntity.TAB_BUILDING_MASTER.Where(x => x.status == "Y").Select(s => new BuildingMasterMetaData
            {
                BUILDING_NAME = s.BUILDING_NAME,
                BUILDING_ID = s.BUILDING_ID
            }).OrderBy(x => x.BUILDING_NAME).ToList();
        }

        public List<SkillMasterMetaData> GetSkills()
        {
            return _appEntity.TAB_SKILL_MASTER.Where(x => x.status == "Y").Select(s => new SkillMasterMetaData
            {
                SKILL_ID = s.SKILL_ID,
                SKILL_NAME = s.SKILL_NAME
            }).OrderBy(x => x.SKILL_NAME).ToList();
        }

        public List<WFDesignationMasterMetaData> GetDesignations()
        {
            return _appEntity.TAB_WF_DESIGNATION_MASTER.Where(x => x.STATUS == "Y").Select(s => new WFDesignationMasterMetaData
            {
                WF_DESIGNATION_ID = s.WF_DESIGNATION_ID,
                WF_DESIGNATION_NAME = s.WF_DESIGNATION_NAME
            }).ToList();
        }

        public List<WFDesignationMasterMetaData> GetDesignationBySkill(Guid SKILL_ID)
        {
            return _appEntity.TAB_WF_DESIGNATION_MASTER.Where(x => x.STATUS == "Y" && x.SKILL_ID == SKILL_ID).Select(s => new WFDesignationMasterMetaData
            {
                WF_DESIGNATION_ID = s.WF_DESIGNATION_ID,
                WF_DESIGNATION_NAME = s.WF_DESIGNATION_NAME
            }).ToList();
        }

        public List<RECMasterMetaData> GetMPRHiring()
        {
            return _appEntity.TAB_REC_MASTER.Where(x => x.STATUS == "Y").Select(s => new RECMasterMetaData
            {
                REC_TYPE = s.REC_TYPE,
                REC_NAME = s.REC_NAME,
                WORKFLOW_ID = s.WORKFLOW_ID,
                COMPANY_ID = s.COMPANY_ID
            }).Take(2).ToList();
        }

        public List<DepartmentMasterMetaData> GetFloorByBuildingId(Guid buildingId)
        {
            //string Role = SessionHelper.Get<string>("LoginUserId");
            Guid loggedInUserId = Utility.GetLoggedInUserId();
            string LoginUserId = SessionHelper.Get<string>("LoginUserId");
            string LoginType = SessionHelper.Get<string>("LoginType");
            if (LoginType == "ADMIN - IT")
            {
                return (from department in _appEntity.TAB_DEPARTMENT_MASTER
                            //join user_depart_map in _appEntity.TAB_USER_DEPARTMENT_MAPPING on department.DEPT_ID equals user_depart_map.DEPT_ID
                            //join subdept in _appEntity.TAB_SUBDEPARTMENT_MASTER on department.DEPT_ID equals subdept.DEPT_ID
                            //join login_master in _appEntity.TAB_LOGIN_MASTER on user_depart_map.USER_ID equals login_master.USER_ID
                        where department.BUILDING_ID == buildingId
                        && department.status == "Y"
                        //&& user_depart_map.USER_ID == loggedInUserId
                        select new DepartmentMasterMetaData
                        {
                            DEPT_ID = department.DEPT_ID,
                            DEPT_NAME = department.DEPT_NAME
                        }).Distinct().OrderBy(x => x.DEPT_NAME).ToList();
            }
            else
            {
                return (from department in _appEntity.TAB_DEPARTMENT_MASTER
                        join user_depart_map in _appEntity.TAB_USER_DEPARTMENT_MAPPING on department.DEPT_ID equals user_depart_map.DEPT_ID
                        join subdept in _appEntity.TAB_SUBDEPARTMENT_MASTER on department.DEPT_ID equals subdept.DEPT_ID
                        join login_master in _appEntity.TAB_LOGIN_MASTER on user_depart_map.USER_ID equals login_master.USER_ID
                        where department.BUILDING_ID == buildingId
                        && department.status == "Y"
                        && user_depart_map.USER_ID == loggedInUserId
                        select new DepartmentMasterMetaData
                        {
                            DEPT_ID = department.DEPT_ID,
                            DEPT_NAME = department.DEPT_NAME
                        }).Distinct().OrderBy(x => x.DEPT_NAME).ToList();
            }
        }

        public List<DepartmentMasterMetaData> GetAllFloorByBuildingId(Guid buildingId)
        {
            //string Role = SessionHelper.Get<string>("LoginUserId");
            Guid loggedInUserId = Utility.GetLoggedInUserId();
            string LoginUserId = SessionHelper.Get<string>("LoginUserId");
            string LoginType = SessionHelper.Get<string>("LoginType");
            return (from department in _appEntity.TAB_DEPARTMENT_MASTER
                    where department.BUILDING_ID == buildingId
                    && department.status == "Y"
                    //&& user_depart_map.USER_ID == loggedInUserId
                    select new DepartmentMasterMetaData
                    {
                        DEPT_ID = department.DEPT_ID,
                        DEPT_NAME = department.DEPT_NAME
                    }).Distinct().OrderBy(x => x.DEPT_NAME).ToList();
        }

        public void UpdatMRFApprovalsByMappingId(IEnumerable<Guid> twfmIds, string status, string remark, string mrfSearchULR)
        {
            try
            {
                DateTime dt = DateTime.Now;
                var mrfApprovals = _appEntity.TAB_MRF_APPROVER.Where(m => twfmIds.Contains(m.MRF_APPROVER_ID)).ToList();
                foreach (var item in mrfApprovals)
                {
                    item.APPROVER_STATUS = status;
                    item.APPROVER_REMARK = remark;
                    item.APPROVE_DATE = dt;
                    var mrf = _appEntity.TAB_MRF.Where(x => x.MRP_INETRNAL_ID == item.MRP_INETRNAL_ID).FirstOrDefault();
                    if (mrf.MRF_STATUS == "Open")
                    {
                        mrf.MRF_STATUS = "Progress";
                    }
                    var mrfPendingApproval = _appEntity.TAB_MRF_APPROVER.Where(x => x.APPROVER_STATUS == null && x.MRP_INETRNAL_ID == item.MRP_INETRNAL_ID && x.APPROVE_BY != item.APPROVE_BY).ToList();
                    if (mrfPendingApproval.Count < 1)
                    {
                        if (status == "Y")
                        {
                            mrf.MRF_STATUS = "Approved";
                        }
                        else
                        {
                            mrf.MRF_STATUS = "Reject";
                        }
                    }
                }

                var mrfIds = mrfApprovals.Select(x => x.MRP_INETRNAL_ID).Distinct().ToArray();
                var toUsers = (from mrf_detail in _appEntity.TAB_MRF_DETAILS
                               join user in _appEntity.TAB_LOGIN_MASTER on mrf_detail.CREATED_BY equals user.USER_LOGIN_ID
                               where mrfIds.Contains(mrf_detail.MRP_INETRNAL_ID)
                               select new
                               {
                                   mrf_detail.MRP_INETRNAL_ID,
                                   user.USER_NAME,
                                   user.MAIL_ID,
                               }
                                ).ToList();
                var templete = _appEntity.TAB_MAIL_TEMPLATE.Where(x => x.TEMPLATE_FOR == "ApproveAndRejectStatus").FirstOrDefault();

                if (templete != null)
                {
                    var fromUser = SessionHelper.Get<string>("Username");
                    List<TAB_ALL_MAIL> mails = new List<TAB_ALL_MAIL>();
                    string taskStatus = status == "Y" ? "Approve" : "Reject";
                    foreach (var item in toUsers)
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.Append(templete.TEMPLATE_CONTANT);
                        sb.Replace("[TONAME]", item.USER_NAME);
                        sb.Replace("[APPROVALTYPE]", "Hiring");
                        sb.Replace("[TASKSTATUS]", taskStatus);
                        sb.Replace("[FROMNAME]", fromUser);
                        sb.Replace("[REDIRECTURL]", mrfSearchULR + item.MRP_INETRNAL_ID);
                        var mail = new TAB_ALL_MAIL
                        {
                            CC_MAIL = templete.CC_MAIL,
                            MAIL_CONTENT = sb.ToString(),
                            MAIL_INSERT_DATE = dt,
                            TO_MAIL = item.MAIL_ID,
                            MAIL_REMARK = "Hiring " + taskStatus,
                            USER_ID = SessionHelper.Get<string>("LoginUserId")
                        };
                        mails.Add(mail);
                    }
                    _appEntity.TAB_ALL_MAIL.AddRange(mails);
                }
                _appEntity.SaveChanges();
            }
            catch (Exception ex)
            {
                AccountRepository.InsertError_Log(ex.ToString(), "Page - ManPowerRequestRepository.cs, Method - UpdatMRFApprovalsByMappingId", HttpContext.Current.Request.Url.AbsolutePath, SessionHelper.Get<string>("LoginUserId"));
            }
        }

        public List<MRFApprovalMetadata> GetMRFApprovalByDepartmentId(Guid deptId, Guid? sub_dept_id, string status, Guid BUILDING_ID)
        {
            Guid cmp_id = SessionHelper.Get<Guid>("CompanyId");
            List<MRFApprovalMetadata> mrfMetaDataLists =
                (from mrf in _appEntity.TAB_MRF
                 join mrfd in _appEntity.TAB_MRF_DETAILS on mrf.MRP_INETRNAL_ID equals mrfd.MRP_INETRNAL_ID
                 join mefa in _appEntity.TAB_MRF_APPROVER on mrf.MRP_INETRNAL_ID equals mefa.MRP_INETRNAL_ID
                 join user in _appEntity.TAB_LOGIN_MASTER on mefa.APPROVE_BY equals user.USER_ID
                 join rec in _appEntity.TAB_REC_MASTER on mrf.REC_TYPE equals rec.REC_TYPE
                 join buil in _appEntity.TAB_BUILDING_MASTER on mrfd.BUILDING_ID equals buil.BUILDING_ID
                 join floor in _appEntity.TAB_DEPARTMENT_MASTER on mrfd.FLOOR_ID equals floor.DEPT_ID
                 join skill in _appEntity.TAB_SKILL_MASTER on mrfd.SKILL_ID equals skill.SKILL_ID
                 join desig in _appEntity.TAB_WF_DESIGNATION_MASTER on mrfd.WF_DESIGNATION_ID equals desig.WF_DESIGNATION_ID
                 join empType in _appEntity.TAB_WORFORCE_TYPE on mrfd.WF_EMP_TYPE equals empType.WF_EMP_TYPE
                 where mrf.COMPANY_ID == cmp_id
                 && mrfd.FLOOR_ID == (deptId != new Guid() ? deptId : mrfd.FLOOR_ID)
                 && mrfd.SUBDEPT_ID == (sub_dept_id != null ? sub_dept_id : mrfd.SUBDEPT_ID)
                 && mrfd.BUILDING_ID == (BUILDING_ID != new Guid("00000000-0000-0000-0000-000000000000") ? BUILDING_ID : mrfd.BUILDING_ID)
                 && mrf.MRF_STATUS == (status != "" ? status : mrf.MRF_STATUS)
                 select new MRFApprovalMetadata
                 {
                     BUILDING_NAME = buil.BUILDING_NAME,
                     EMP_TYPE = empType.EMP_TYPE,
                     FLOOR_NAME = floor.DEPT_NAME,
                     MRP_INETRNAL_ID = mrf.MRP_INETRNAL_ID,
                     QUANTITY = mrfd.WOKFORCE_QUANTITY,
                     REC_NAME = rec.REC_NAME,
                     SKILL_NAME = skill.SKILL_NAME,
                     WF_DESIGNATION_NAME = desig.WF_DESIGNATION_NAME,
                     MRF_STATUS = mrf.MRF_STATUS,
                     MRF_CODE = mrf.MRF_CODE,
                     MRF_APPROVER_ID = mefa.MRF_APPROVER_ID,
                     USER_NAME = user.USER_NAME,
                     USER_ID = user.USER_ID,
                     MRF_Date = mrf.CREATED_DATE,
                     APPROVER_STATUS = (mefa.APPROVER_STATUS == "Y" ? "Yes" : (mefa.APPROVER_STATUS == "N" ? "No" : ""))

                 }).ToList();
            return mrfMetaDataLists.OrderByDescending(x => x.MRF_CODE).ToList();
        }

        public List<MRFApprovalMetadata> GetMRFApprovalByMRFId(Guid companyId, Guid mrf_id)
        {
            List<MRFApprovalMetadata> mrfMetaDataLists =
                (from mrf in _appEntity.TAB_MRF
                 join mrfd in _appEntity.TAB_MRF_DETAILS on mrf.MRP_INETRNAL_ID equals mrfd.MRP_INETRNAL_ID
                 join mefa in _appEntity.TAB_MRF_APPROVER on mrf.MRP_INETRNAL_ID equals mefa.MRP_INETRNAL_ID
                 join user in _appEntity.TAB_LOGIN_MASTER on mefa.APPROVE_BY equals user.USER_ID
                 join rec in _appEntity.TAB_REC_MASTER on mrf.REC_TYPE equals rec.REC_TYPE
                 join buil in _appEntity.TAB_BUILDING_MASTER on mrfd.BUILDING_ID equals buil.BUILDING_ID
                 join floor in _appEntity.TAB_DEPARTMENT_MASTER on mrfd.FLOOR_ID equals floor.DEPT_ID
                 join skill in _appEntity.TAB_SKILL_MASTER on mrfd.SKILL_ID equals skill.SKILL_ID
                 join desig in _appEntity.TAB_WF_DESIGNATION_MASTER on mrfd.WF_DESIGNATION_ID equals desig.WF_DESIGNATION_ID
                 join empType in _appEntity.TAB_WORFORCE_TYPE on mrfd.WF_EMP_TYPE equals empType.WF_EMP_TYPE
                 where mrf.COMPANY_ID == companyId && mrfd.MRP_INETRNAL_ID == mrf_id
                 select new MRFApprovalMetadata
                 {
                     BUILDING_NAME = buil.BUILDING_NAME,
                     EMP_TYPE = empType.EMP_TYPE,
                     FLOOR_NAME = floor.DEPT_NAME,
                     MRP_INETRNAL_ID = mrf.MRP_INETRNAL_ID,
                     QUANTITY = mrfd.WOKFORCE_QUANTITY,
                     REC_NAME = rec.REC_NAME,
                     SKILL_NAME = skill.SKILL_NAME,
                     WF_DESIGNATION_NAME = desig.WF_DESIGNATION_NAME,
                     MRF_STATUS = mrf.MRF_STATUS,
                     MRF_CODE = mrf.MRF_CODE,
                     MRF_APPROVER_ID = mefa.MRF_APPROVER_ID,
                     USER_NAME = user.USER_NAME,
                     USER_ID = user.USER_ID,
                     APPROVER_STATUS = (mefa.APPROVER_STATUS == "Y" ? "Yes" : (mefa.APPROVER_STATUS == "N" ? "No" : ""))
                 }).ToList();
            return mrfMetaDataLists;
        }

        public List<WorkforceTypeMetaData> GetEmpTypes()
        {
            return _appEntity.TAB_WORFORCE_TYPE.Where(x => x.STATUS == "Y").Select(s => new WorkforceTypeMetaData
            {
                WF_EMP_TYPE = s.WF_EMP_TYPE,
                EMP_TYPE = s.EMP_TYPE
            }).OrderBy(x => x.EMP_TYPE).ToList();
        }

        public bool Create(ManPowerRequestFormMetaData data)
        {
            try
            {
                DateTime dt = DateTime.Now;
                string ReplaceType = "";
                if (data.REC_TYPE == 1)
                {
                    ReplaceType = data.ReplaceType;
                }
                TAB_MRF tAB_MRF = new TAB_MRF
                {
                    MRP_INETRNAL_ID = Guid.NewGuid(),
                    COMPANY_ID = data.COMPANY_ID,
                    MRF_ID = Guid.NewGuid().ToString(),
                    MRF_STATUS = "Open",
                    CREATED_DATE = dt,
                    REC_TYPE = data.REC_TYPE,
                    WORKFLOW_ID = data.WORKFLOW_ID,
                    ReplaceType = ReplaceType,
                };
                TAB_MRF_DETAILS tAB_MRF_DETAILS = new TAB_MRF_DETAILS
                {
                    TAB_MRF_DETAIL_ID = Guid.NewGuid(),
                    BUILDING_ID = data.BUILDING_ID,
                    FLOOR_ID = data.DEPT_ID,
                    SUBDEPT_ID = data.SUBDEPT_ID,
                    MRP_INETRNAL_ID = tAB_MRF.MRP_INETRNAL_ID,
                    SKILL_ID = data.SKILL_ID,
                    CREATED_BY = SessionHelper.Get<string>("LoginUserId"),
                    WF_DESIGNATION_ID = data.WF_DESIGNATION_ID,
                    WF_EMP_TYPE = data.WF_EMP_TYPE,
                    CREATED_DATE = dt,
                    WOKFORCE_QUANTITY = data.QUANTITY
                };
                var approvalIds = _appEntity.TAB_WORKFLOW_MAPPING_MASTER.Where(x => x.WORKFLOW_ID == data.WORKFLOW_ID).ToList();
                List<TAB_MRF_APPROVER> mrf_approval = new List<TAB_MRF_APPROVER>();
                var templete = _appEntity.TAB_MAIL_TEMPLATE.Where(x => x.TEMPLATE_FOR == "ApproveAndReject").FirstOrDefault();
                if (templete != null)
                {
                    var toUserId = approvalIds.Select(x => x.USER_ID).ToArray();
                    var toUsers = _appEntity.TAB_LOGIN_MASTER.Where(x => toUserId.Contains(x.USER_ID)).ToList();
                    var fromUser = SessionHelper.Get<string>("Username");
                    List<TAB_ALL_MAIL> mails = new List<TAB_ALL_MAIL>();
                    foreach (var item in toUsers)
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.Append(templete.TEMPLATE_CONTANT);
                        sb.Replace("[TONAME]", item.USER_NAME);
                        sb.Replace("[APPROVALTYPE]", "Hiring");
                        sb.Replace("[TASKNAME]", fromUser);
                        sb.Replace("[FROMNAME]", fromUser);
                        sb.Replace("[REDIRECTURL]", fromUser);
                        var mail = new TAB_ALL_MAIL
                        {
                            CC_MAIL = templete.CC_MAIL,
                            MAIL_CONTENT = sb.ToString(),
                            MAIL_INSERT_DATE = dt,
                            TO_MAIL = item.MAIL_ID,
                            MAIL_REMARK = "Hiring",
                            USER_ID = SessionHelper.Get<string>("LoginUserId")
                        };
                        var model = new TAB_MRF_APPROVER()
                        {
                            APPROVE_BY = item.USER_ID,
                            MRF_APPROVER_ID = Guid.NewGuid(),
                            MRP_INETRNAL_ID = tAB_MRF.MRP_INETRNAL_ID
                        };
                        mrf_approval.Add(model);
                        mails.Add(mail);
                    }
                    _appEntity.TAB_ALL_MAIL.AddRange(mails);
                    _appEntity.TAB_MRF_APPROVER.AddRange(mrf_approval);
                }
                _appEntity.TAB_MRF.Add(tAB_MRF);
                _appEntity.TAB_MRF_DETAILS.Add(tAB_MRF_DETAILS);
                _appEntity.SaveChanges();
            }
            catch (Exception ex)
            {
                AccountRepository.InsertError_Log(ex.ToString(), "Page - ManPowerRequestRepository.cs, Method - Create", HttpContext.Current.Request.Url.AbsolutePath, SessionHelper.Get<string>("LoginUserId"));
                return false;
            }
            return true;
        }

        public bool Edit(ManPowerRequestFormMetaData data)
        {
            try
            {
                string ReplaceType = "";
                if (data.REC_TYPE == 1)
                {
                    ReplaceType = data.ReplaceType;
                }
                TAB_MRF tAB_MRF = _appEntity.TAB_MRF.Where(x => x.MRP_INETRNAL_ID == data.MRP_INETRNAL_ID).FirstOrDefault();
                tAB_MRF.COMPANY_ID = data.COMPANY_ID;
                tAB_MRF.REC_TYPE = data.REC_TYPE;
                tAB_MRF.WORKFLOW_ID = data.WORKFLOW_ID;
                tAB_MRF.ReplaceType = ReplaceType;

                TAB_MRF_DETAILS tAB_MRF_DETAILS = _appEntity.TAB_MRF_DETAILS.Where(x => x.MRP_INETRNAL_ID == data.MRP_INETRNAL_ID).FirstOrDefault();
                tAB_MRF_DETAILS.BUILDING_ID = data.BUILDING_ID;
                tAB_MRF_DETAILS.FLOOR_ID = data.DEPT_ID;
                tAB_MRF_DETAILS.SUBDEPT_ID = data.SUBDEPT_ID;
                tAB_MRF_DETAILS.SKILL_ID = data.SKILL_ID;
                tAB_MRF_DETAILS.WF_DESIGNATION_ID = data.WF_DESIGNATION_ID;
                tAB_MRF_DETAILS.WF_EMP_TYPE = data.WF_EMP_TYPE;
                tAB_MRF_DETAILS.UPDATED_DATE = DateTime.Now;
                tAB_MRF_DETAILS.UPDATED_BY = SessionHelper.Get<string>("LoginUserId");
                tAB_MRF_DETAILS.WOKFORCE_QUANTITY = data.QUANTITY;
                _appEntity.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                AccountRepository.InsertError_Log(ex.ToString(), "Page - ManPowerRequestRepository.cs, Method - Edit", HttpContext.Current.Request.Url.AbsolutePath, SessionHelper.Get<string>("LoginUserId"));
                return false;
            }
        }

        public List<ManPowerRequestFormMetaDataList> GetMRFAllItems(Guid companyId)
        {

            List<ManPowerRequestFormMetaDataList> mrfMetaDataLists = (from mrf in _appEntity.TAB_MRF
                                                                      join mrfd in _appEntity.TAB_MRF_DETAILS on mrf.MRP_INETRNAL_ID equals mrfd.MRP_INETRNAL_ID
                                                                      join rec in _appEntity.TAB_REC_MASTER on mrf.REC_TYPE equals rec.REC_TYPE
                                                                      join buil in _appEntity.TAB_BUILDING_MASTER on mrfd.BUILDING_ID equals buil.BUILDING_ID
                                                                      join floor in _appEntity.TAB_DEPARTMENT_MASTER on mrfd.FLOOR_ID equals floor.DEPT_ID
                                                                      join skill in _appEntity.TAB_SKILL_MASTER on mrfd.SKILL_ID equals skill.SKILL_ID
                                                                      join desig in _appEntity.TAB_WF_DESIGNATION_MASTER on mrfd.WF_DESIGNATION_ID equals desig.WF_DESIGNATION_ID
                                                                      join empType in _appEntity.TAB_WORFORCE_TYPE on mrfd.WF_EMP_TYPE equals empType.WF_EMP_TYPE
                                                                      join udm in _appEntity.TAB_USER_DEPARTMENT_MAPPING on floor.DEPT_ID equals udm.DEPT_ID
                                                                      join subdept in _appEntity.TAB_SUBDEPARTMENT_MASTER on udm.DEPT_ID equals subdept.DEPT_ID
                                                                      where mrf.COMPANY_ID == companyId
                                                                      select new ManPowerRequestFormMetaDataList
                                                                      {
                                                                          BUILDING_NAME = buil.BUILDING_NAME,
                                                                          EMP_TYPE = empType.EMP_TYPE,
                                                                          FLOOR_NAME = floor.DEPT_NAME,
                                                                          SUBDEPT_NAME = subdept.SUBDEPT_NAME,
                                                                          MRP_INETRNAL_ID = mrf.MRP_INETRNAL_ID,
                                                                          QUANTITY = mrfd.WOKFORCE_QUANTITY,
                                                                          REC_NAME = rec.REC_NAME,
                                                                          SKILL_NAME = skill.SKILL_NAME,
                                                                          WF_DESIGNATION_NAME = desig.WF_DESIGNATION_NAME,
                                                                          MRF_STATUS = mrf.MRF_STATUS,
                                                                          MRF_CODE = mrf.MRF_CODE,
                                                                          HIRING_QUANTITY = mrfd.HIRING_QUANTITY
                                                                      })
                                                                      .OrderBy(x => x.FLOOR_NAME)
                                                                      .GroupBy(c => c.MRP_INETRNAL_ID).Select(t => t.FirstOrDefault())
                                                                      .ToList();
            return mrfMetaDataLists;
        }
        public List<ManPowerRequestFormMetaDataList> GetMRFAllItems1(Guid companyId, string dept_id, string sub_dept_id, string BUILDING_ID)
        {
            Guid dept_ids = string.IsNullOrEmpty(dept_id) ? new Guid() : new Guid(dept_id);
            Guid sub_dept_ids = string.IsNullOrEmpty(sub_dept_id) ? new Guid() : new Guid(sub_dept_id);
            Guid BUILDING_IDs = string.IsNullOrEmpty(BUILDING_ID) ? new Guid() : new Guid(BUILDING_ID);
            string LoginUserId = SessionHelper.Get<string>("LoginUserId");
            string LoginType = SessionHelper.Get<string>("LoginType");

            List<ManPowerRequestFormMetaDataList> mrfMetaDataLists = (from mrf in _appEntity.TAB_MRF
                                                                      join mrfd in _appEntity.TAB_MRF_DETAILS on mrf.MRP_INETRNAL_ID equals mrfd.MRP_INETRNAL_ID
                                                                      join rec in _appEntity.TAB_REC_MASTER on mrf.REC_TYPE equals rec.REC_TYPE
                                                                      join buil in _appEntity.TAB_BUILDING_MASTER on mrfd.BUILDING_ID equals buil.BUILDING_ID
                                                                      join floor in _appEntity.TAB_DEPARTMENT_MASTER on mrfd.FLOOR_ID equals floor.DEPT_ID
                                                                      join skill in _appEntity.TAB_SKILL_MASTER on mrfd.SKILL_ID equals skill.SKILL_ID
                                                                      join desig in _appEntity.TAB_WF_DESIGNATION_MASTER on mrfd.WF_DESIGNATION_ID equals desig.WF_DESIGNATION_ID
                                                                      join empType in _appEntity.TAB_WORFORCE_TYPE on mrfd.WF_EMP_TYPE equals empType.WF_EMP_TYPE
                                                                      //join udm in _appEntity.TAB_USER_DEPARTMENT_MAPPING on floor.DEPT_ID equals udm.DEPT_ID
                                                                      join subdept in _appEntity.TAB_SUBDEPARTMENT_MASTER on floor.DEPT_ID equals subdept.DEPT_ID
                                                                      where mrf.COMPANY_ID == companyId && (dept_id == "" || mrfd.FLOOR_ID == dept_ids)
                                                                      && (sub_dept_id == "" || mrfd.SUBDEPT_ID == sub_dept_ids)
                                                                      && (BUILDING_ID == "" || mrfd.BUILDING_ID == BUILDING_IDs)
                                                                      && (mrfd.CREATED_BY == LoginUserId || LoginType == "ADMIN - IT")
                                                                      select new ManPowerRequestFormMetaDataList
                                                                      {
                                                                          BUILDING_NAME = buil.BUILDING_NAME,
                                                                          EMP_TYPE = empType.EMP_TYPE,
                                                                          FLOOR_NAME = floor.DEPT_NAME,
                                                                          SUBDEPT_NAME = subdept.SUBDEPT_NAME,
                                                                          MRP_INETRNAL_ID = mrf.MRP_INETRNAL_ID,
                                                                          QUANTITY = mrfd.WOKFORCE_QUANTITY,
                                                                          REC_NAME = rec.REC_NAME,
                                                                          SKILL_NAME = skill.SKILL_NAME,
                                                                          WF_DESIGNATION_NAME = desig.WF_DESIGNATION_NAME,
                                                                          MRF_STATUS = mrf.MRF_STATUS,
                                                                          MRF_CODE = mrf.MRF_CODE,
                                                                          HIRING_QUANTITY = mrfd.HIRING_QUANTITY,
                                                                          CreationDate = mrfd.CREATED_DATE,
                                                                          CreatedBy = mrfd.CREATED_BY
                                                                      })
                                                                      .OrderBy(x => x.FLOOR_NAME)
                                                                      .GroupBy(c => c.MRP_INETRNAL_ID).Select(t => t.FirstOrDefault())
                                                                      .ToList();
            return mrfMetaDataLists;
        }

        public ManPowerRequestFormMetaData GetMRFByMRF_INETRNAL_ID(Guid mrf_INETRNAL_ID)
        {
            ManPowerRequestFormMetaData mRFMetaData = (from mrf in _appEntity.TAB_MRF
                                                       join mrfd in _appEntity.TAB_MRF_DETAILS on mrf.MRP_INETRNAL_ID equals mrfd.MRP_INETRNAL_ID
                                                       where mrf.MRP_INETRNAL_ID.Equals(mrf_INETRNAL_ID)
                                                       select new ManPowerRequestFormMetaData
                                                       {
                                                           DEPT_ID = mrfd.FLOOR_ID.Value,
                                                           SUBDEPT_ID = mrfd.SUBDEPT_ID,
                                                           MRP_INETRNAL_ID = mrf.MRP_INETRNAL_ID,
                                                           BUILDING_ID = mrfd.BUILDING_ID.Value,
                                                           QUANTITY = mrfd.WOKFORCE_QUANTITY,
                                                           REC_TYPE = mrf.REC_TYPE.Value,
                                                           REMARK = "",
                                                           MRF_STATUS = mrf.MRF_STATUS,
                                                           SKILL_ID = mrfd.SKILL_ID.Value,
                                                           WF_DESIGNATION_ID = mrfd.WF_DESIGNATION_ID.Value,
                                                           WF_EMP_TYPE = mrfd.WF_EMP_TYPE.Value,
                                                           ReplaceType = mrf.ReplaceType,
                                                       }).FirstOrDefault();
            return mRFMetaData;
        }

        public ManPowerRequestFormMetaData GetMRFDetailsByMRF_INETRNAL_ID(Guid mrf_INETRNAL_ID)
        {
            ManPowerRequestFormMetaData mRFMetaData = (from mrfd in _appEntity.TAB_MRF_DETAILS
                                                       where mrfd.MRP_INETRNAL_ID.Equals(mrf_INETRNAL_ID)
                                                       select new ManPowerRequestFormMetaData
                                                       {
                                                           DEPT_ID = (Guid)mrfd.FLOOR_ID,
                                                           SUBDEPT_ID = mrfd.SUBDEPT_ID,
                                                           MRP_INETRNAL_ID = mrfd.MRP_INETRNAL_ID,
                                                           BUILDING_ID = mrfd.BUILDING_ID.Value,
                                                           QUANTITY = mrfd.WOKFORCE_QUANTITY,
                                                           SKILL_ID = mrfd.SKILL_ID.Value,
                                                           WF_DESIGNATION_ID = mrfd.WF_DESIGNATION_ID.Value,
                                                           WF_EMP_TYPE = mrfd.WF_EMP_TYPE.Value,
                                                       }).FirstOrDefault();
            return mRFMetaData;
        }

    }
}
