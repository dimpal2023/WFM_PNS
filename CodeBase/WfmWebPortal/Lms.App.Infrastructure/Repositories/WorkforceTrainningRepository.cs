using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wfm.App.Common;
using Wfm.App.Core;
using Wfm.App.Core.Model;
namespace Wfm.App.Infrastructure.Repositories
{
    public class WorkforceTrainningRepository
    {
        private ApplicationEntities db;
        public WorkforceTrainningRepository()
        {
            db = new ApplicationEntities();
        }
        #region TrainningMaster
        public bool AddTrainningMaster(AddTrainningMetaData trainning)
        {
            try
            {
                DateTime dt = DateTime.Now;
                TAB_TRAINNING_MASTER model = new TAB_TRAINNING_MASTER
                {
                    CMP_ID = trainning.CMP_ID,
                    TRAINNING_ID = Guid.NewGuid(),
                    STATUS = "Y",
                    //BUILDING_ID=trainning.BUILDING_ID,
                    CREATED_BY = SessionHelper.Get<string>("LoginUserId"),
                    CREATED_DATE = dt,
                    //DEPT_ID = trainning.DEPT_ID.Value,
                    //SUBDEPT_ID = trainning.SUBDEPT_ID.Value,
                    TRAINNING_NAME = trainning.TRAINNING_NAME,
                    EXPECTEDDATE = trainning.EXPECTED_DATE
                };
                db.TAB_TRAINNING_MASTER.Add(model);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        public bool UpdateTrainningMaster(AddTrainningMetaData trainning)
        {
            try
            {
                var result = db.TAB_TRAINNING_MASTER.Find(trainning.TRAINNING_ID);
                if (result != null)
                {
                    //result.DEPT_ID = trainning.DEPT_ID.Value;
                    result.TRAINNING_NAME = trainning.TRAINNING_NAME;
                    //result.BUILDING_ID = trainning.BUILDING_ID; 
                    //result.SUBDEPT_ID = trainning.SUBDEPT_ID.Value;
                    result.STATUS = trainning.STATUS;
                    result.UPDATED_BY = trainning.UPDATED_BY;
                    result.UPDATED_DATE = trainning.UPDATED_DATE;
                    result.EXPECTEDDATE = trainning.EXPECTED_DATE;
                    db.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public void UpdateWorkForceTrainningByMappingId(IEnumerable<Guid> twfmIds, string status, string remark)
        {
            List<TAB_TRAINNING_WORKFORCE_MAPPING> list = db.TAB_TRAINNING_WORKFORCE_MAPPING.Where(x => twfmIds.Contains(x.TRAINNING_MAPPING_ID)).ToList();
            foreach (var item in list)
            {
                item.ISCOMPLETED = status;
                item.ACTUAL_END_DATE = item.TRAINNING_END_DATE;
                item.ACTUAL_START_DATE = item.TRAINNING_START_DATE;
                item.REMARK = remark;
            }
            db.SaveChanges();
        }

        public List<TrainningMasterMetaData> GetTrainningMaster(Guid dept_id, Guid? sub_dept_id)
        {
            Guid cmp_id = SessionHelper.Get<Guid>("CompanyId");
            List<TrainningMasterMetaData> trainnings = new List<TrainningMasterMetaData>();
            try
            {
                trainnings = (from tr in db.TAB_TRAINNING_MASTER
                              join cmp in db.TAB_COMPANY_MASTER on tr.CMP_ID equals cmp.COMPANY_ID
                              //join dpt in db.TAB_DEPARTMENT_MASTER on tr.DEPT_ID equals dpt.DEPT_ID
                              //join subdpt in db.TAB_SUBDEPARTMENT_MASTER on tr.SUBDEPT_ID equals subdpt.SUBDEPT_ID
                              where //dpt.DEPT_ID == dept_id && 
                              cmp.COMPANY_ID == cmp_id && tr.STATUS == "Y"
                              // && tr.SUBDEPT_ID == (sub_dept_id != null ? sub_dept_id : tr.SUBDEPT_ID)
                              select new TrainningMasterMetaData
                              {
                                  CMP_NAME = cmp.COMPANY_NAME,
                                  //DEPT_NAME = dpt.DEPT_NAME,
                                  //SUB_NAME = subdpt.SUBDEPT_NAME,
                                  TRAINNING_NAME = tr.TRAINNING_NAME,
                                  TRAINNING_ID = tr.TRAINNING_ID,
                                  STATUS = tr.STATUS == "Y" ? "Yes" : "No",
                                  EXPECTED_DATE = tr.EXPECTEDDATE
                              }).ToList();
            }
            catch (Exception ex)
            {
                return trainnings;
            }
            return trainnings;
        }

        public List<TrainningMasterMetaData> GetTrainningMaster_N()
        {
            Guid cmp_id = SessionHelper.Get<Guid>("CompanyId");
            List<TrainningMasterMetaData> trainnings = new List<TrainningMasterMetaData>();
            try
            {
                trainnings = (from tr in db.TAB_TRAINNING_MASTER
                              join cmp in db.TAB_COMPANY_MASTER on tr.CMP_ID equals cmp.COMPANY_ID
                              //join dpt in db.TAB_DEPARTMENT_MASTER on tr.DEPT_ID equals dpt.DEPT_ID
                              //join subdpt in db.TAB_SUBDEPARTMENT_MASTER on tr.SUBDEPT_ID equals subdpt.SUBDEPT_ID
                              where  cmp.COMPANY_ID == cmp_id && tr.STATUS=="Y"
                              select new TrainningMasterMetaData
                              {
                                  CMP_NAME = cmp.COMPANY_NAME,
                                  //DEPT_NAME = dpt.DEPT_NAME,
                                  //SUB_NAME = subdpt.SUBDEPT_NAME,
                                  TRAINNING_NAME = tr.TRAINNING_NAME,
                                  TRAINNING_ID = tr.TRAINNING_ID,
                                  STATUS = tr.STATUS == "Y" ? "Yes" : "No",
                                  EXPECTED_DATE = tr.EXPECTEDDATE
                              }).ToList();
            }
            catch (Exception ex)
            {
                return trainnings;
            }
            return trainnings;
        }
        public AddTrainningMetaData GetTrainningMasterByTrainningId(Guid cmp_id, Guid trainning_id)
        {
            AddTrainningMetaData trainnings = new AddTrainningMetaData();
            try
            {
                trainnings = (from tr in db.TAB_TRAINNING_MASTER
                              where tr.TRAINNING_ID == trainning_id && tr.CMP_ID == cmp_id 
                              select new AddTrainningMetaData
                              {
                                  //BUILDING_ID = (Guid)tr.BUILDING_ID,
                                  //DEPT_ID = tr.DEPT_ID,
                                  //SUBDEPT_ID = tr.SUBDEPT_ID,
                                  TRAINNING_NAME = tr.TRAINNING_NAME,
                                  TRAINNING_ID = tr.TRAINNING_ID,
                                  STATUS = tr.STATUS,
                                  EXPECTED_DATE = tr.EXPECTEDDATE
                              }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                return trainnings;
            }
            return trainnings;
        }

        public List<TrainningMasterMetaData> GetTrainningMasterByDepartId(Guid deptId, Guid sub_deptId)
        {
            Guid cmp_id = SessionHelper.Get<Guid>("CompanyId");
            return db.TAB_TRAINNING_MASTER.Where(x=>x.CMP_ID == cmp_id && x.STATUS == "Y")
            //return db.TAB_TRAINNING_MASTER
                .Select(x => new TrainningMasterMetaData
                {
                    TRAINNING_ID = x.TRAINNING_ID,
                    TRAINNING_NAME = x.TRAINNING_NAME
                }).ToList();
        }
        public List<TrainningMasterMetaData> GetTrainningMasterByDepartId1(Guid deptId, Guid sub_deptId, Guid Company)
        {
            Guid cmp_id = SessionHelper.Get<Guid>("CompanyId");
            return db.TAB_TRAINNING_MASTER.Where(x =>x.CMP_ID == Company && x.STATUS == "Y")
            //return db.TAB_TRAINNING_MASTER
                .Select(x => new TrainningMasterMetaData
                {
                    TRAINNING_ID = x.TRAINNING_ID,
                    TRAINNING_NAME = x.TRAINNING_NAME
                }).ToList();
        }
        public bool AddWorkforceTrainning(TrainningWorkforceMetaData model)
        {
            DateTime dt = DateTime.Now;
            string WORKFORCE_IDS = "";
            int i = 0;
            int len = model.EMP_NAMES.Length - 1;
            foreach (var role_id in model.EMP_NAMES)
            {
                WORKFORCE_IDS += role_id;
                if (len != i)
                {
                    WORKFORCE_IDS += ", ";
                }
                i++;
            }
            TAB_TRAINNING_WORKFORCE workforce = new TAB_TRAINNING_WORKFORCE
            {
                CMP_ID = model.CMP_ID.Value,
                CREATED_DATE = dt,
                ISCOMPLETED = "N",
                STATUS = "Y",
                TRAINNING_WORKFORCE_ID = Guid.NewGuid(),
                WF_ID = model.WF_ID,
                WORKFORCES = WORKFORCE_IDS,
                DEPT_ID = model.DEPT_ID,
                SUB_DEPT_ID = model.SUBDEPT_ID,
                CREATED_BY = SessionHelper.Get<string>("LoginUserId"),
                EMP_TYPES=model.WF_EMP_TYPE,
                BUILDING_ID=model.BUILDING_ID,
                TRAINNING_PHOTO = model.PHOTO
            };
            List<TAB_TRAINNING_WORKFORCE_MAPPING> trainnings = new List<TAB_TRAINNING_WORKFORCE_MAPPING>();
            foreach (var item in model.ListMetaDatas)
            {
                TAB_TRAINNING_WORKFORCE_MAPPING trainning = new TAB_TRAINNING_WORKFORCE_MAPPING
                {
                    CREATED_BY = SessionHelper.Get<string>("LoginUserId"),
                    CREATED_DATE = dt,
                    ISCOMPLETED = item.ISCOMPLETED,
                    TRAINNING_START_DATE = item.START_DATE.Value,
                    TRAINNING_END_DATE = item.START_DATE.Value,
                    STATUS = "Y",
                    TRAINNING_ID = item.TRAINNING_ID,
                    TRAINNING_MAPPING_ID = Guid.NewGuid(),
                    TRAINNING_WORKFORCE_ID = workforce.TRAINNING_WORKFORCE_ID,
                    TRANING_TIMES = item.Time,
                    VENUE = item.Venue,
                    TRAINNER_NAME=item.TRAINNER_NAME

                };
                trainnings.Add(trainning);
            }
            db.TAB_TRAINNING_WORKFORCE.Add(workforce);
            db.TAB_TRAINNING_WORKFORCE_MAPPING.AddRange(trainnings);
            db.SaveChanges();
            return true;
        }

        public List<GetTRAINNING_WORKFORCE> GetEmployeeTrainningStatus(Guid wf_id, Guid cmp_id)
        {
            List<GetTRAINNING_WORKFORCE> list = new List<GetTRAINNING_WORKFORCE>();
            var result = (from twf in db.TAB_TRAINNING_WORKFORCE
                          join twfm in db.TAB_TRAINNING_WORKFORCE_MAPPING on twf.TRAINNING_WORKFORCE_ID equals twfm.TRAINNING_WORKFORCE_ID
                          join wf in db.TAB_WORKFORCE_MASTER on twf.WF_ID equals wf.WF_ID
                          join t in db.TAB_TRAINNING_MASTER on twfm.TRAINNING_ID equals t.TRAINNING_ID
                          join d in db.TAB_DEPARTMENT_MASTER on t.DEPT_ID equals d.DEPT_ID
                          join sd in db.TAB_SUBDEPARTMENT_MASTER on t.SUBDEPT_ID equals sd.SUBDEPT_ID
                          where twf.CMP_ID == cmp_id && twf.WF_ID == wf_id
                          select new
                          {
                              twf.TRAINNING_WORKFORCE_ID,
                              wf.EMP_NAME,
                              twf.ISCOMPLETED,
                              twf.CREATED_DATE,
                              twf.STATUS,
                              d.DEPT_NAME,
                              sd.SUBDEPT_NAME,
                              t.TRAINNING_NAME,
                              ISTRAINNINGCOMPLETED = twfm.ISCOMPLETED,
                              twfm.TRAINNING_START_DATE,
                              twfm.TRAINNING_END_DATE,
                              twfm.TRAINNING_MAPPING_ID
                          }).ToList();

            list = result.GroupBy(t => new { t.TRAINNING_WORKFORCE_ID, t.EMP_NAME, t.ISCOMPLETED, t.CREATED_DATE, t.STATUS }).Select(g => new GetTRAINNING_WORKFORCE
            {
                CREATED_DATE = g.Key.CREATED_DATE,
                ISCOMPLETED = g.Key.ISCOMPLETED == "Y" ? "Yes" : "No",
                STATUS = g.Key.STATUS,
                WORKFORCE_NAME = g.Key.EMP_NAME,
                TRAINNING_WORKFORCE_ID = g.Key.TRAINNING_WORKFORCE_ID,
                TrainningMapping = result.Where(x => x.TRAINNING_WORKFORCE_ID == g.Key.TRAINNING_WORKFORCE_ID).Select(x => new TRAINNING_WORKFORCE_MAPPING
                {
                    DEPT_NAME = x.DEPT_NAME,
                    TRAINNING_NAME = x.TRAINNING_NAME,
                    SUBDEPT_NAME = x.SUBDEPT_NAME,
                    ISTRAINNINGCOMPLETED = x.ISTRAINNINGCOMPLETED == "Y" ? "Yes" : "No",
                    TRAINNING_START_DATE = x.TRAINNING_START_DATE,
                    TRAINNING_END_DATE = x.TRAINNING_END_DATE,
                    TRAINNING_MAPPING_ID = x.TRAINNING_MAPPING_ID
                }).ToList()
            }).ToList();
            return list;
        }
         public string SaveAttendTranning_Emp(Guid wf_id, Guid cmp_id,string EmpList)
        {
            try
            {
                var result = db.TAB_TRAINNING_WORKFORCE_MAPPING.Where(x => x.TRAINNING_MAPPING_ID == wf_id).FirstOrDefault();
                result.PRESENTED_EMP = EmpList.TrimEnd(',');
                db.SaveChanges();
                return "true";
            }
            catch(Exception ex)
            {
                return ex.ToString();
            }
            
        }



        public List<GetTRAINNING_WORKFORCE> GetTrainningWorkforceList(string DEPT_IDs, string SUB_DEPT_IDs, DateTime FROM_DATE, DateTime TO_DATE, Guid cmp_id,string BUILDING_ID)
        {
            Guid DEPT_ID = string.IsNullOrEmpty(DEPT_IDs) ? new Guid() : new Guid(DEPT_IDs);
            Guid SUB_DEPT_ID = string.IsNullOrEmpty(SUB_DEPT_IDs) ? new Guid() : new Guid(SUB_DEPT_IDs);
            Guid BUILDING_IDs = string.IsNullOrEmpty(BUILDING_ID) ? new Guid() : new Guid(BUILDING_ID);
            string LoginUserId = SessionHelper.Get<string>("LoginUserId");
            string LoginType = SessionHelper.Get<string>("LoginType");


            List<GetTRAINNING_WORKFORCE> list = new List<GetTRAINNING_WORKFORCE>();
            if (FROM_DATE.ToString() == "01/01/2001 12:00:00 AM" || TO_DATE.ToString() == "01/01/2001 12:00:00 AM")
            {
                list = (from twf in db.TAB_TRAINNING_WORKFORCE
                        join twfm in db.TAB_TRAINNING_WORKFORCE_MAPPING on twf.TRAINNING_WORKFORCE_ID equals twfm.TRAINNING_WORKFORCE_ID
                        join tm in db.TAB_TRAINNING_MASTER on twfm.TRAINNING_ID equals tm.TRAINNING_ID
                        join d in db.TAB_DEPARTMENT_MASTER on twf.DEPT_ID equals d.DEPT_ID
                        join sd in db.TAB_SUBDEPARTMENT_MASTER on twf.SUB_DEPT_ID equals sd.SUBDEPT_ID
                        where twf.CMP_ID == cmp_id
                        && (DEPT_IDs == "" || twf.DEPT_ID == DEPT_ID)
                        && (SUB_DEPT_IDs == "" || twf.SUB_DEPT_ID == SUB_DEPT_ID)
                        && (twf.BUILDING_ID == BUILDING_IDs)
                        && (twf.CREATED_BY == LoginUserId || LoginType == "ADMIN - IT")
                        select new GetTRAINNING_WORKFORCE
                        {
                            TRAINNING_WORKFORCE_ID = twf.TRAINNING_WORKFORCE_ID,
                            WORKFORCE_NAME = twf.WORKFORCES,
                            ISCOMPLETED = tm.TRAINNING_NAME,
                            CREATED_DATE = twfm.TRAINNING_START_DATE,
                            STATUS = twfm.PRESENTED_EMP,
                            DEPT_NAME = d.DEPT_NAME,
                            SUBDEPT_NAME = sd.SUBDEPT_NAME,
                            CREATED_BY = twf.CREATED_BY,
                            UPDATED_BY= twfm.TRAINNER_NAME,
                            VENUE=twfm.VENUE
                        }).OrderByDescending(x=>x.CREATED_DATE).ToList();
            }
            else
            {
                list = (from twf in db.TAB_TRAINNING_WORKFORCE
                        join twfm in db.TAB_TRAINNING_WORKFORCE_MAPPING on twf.TRAINNING_WORKFORCE_ID equals twfm.TRAINNING_WORKFORCE_ID
                        join tm in db.TAB_TRAINNING_MASTER on twfm.TRAINNING_ID equals tm.TRAINNING_ID
                        join d in db.TAB_DEPARTMENT_MASTER on twf.DEPT_ID equals d.DEPT_ID
                        join sd in db.TAB_SUBDEPARTMENT_MASTER on twf.SUB_DEPT_ID equals sd.SUBDEPT_ID
                        where twf.CMP_ID == cmp_id
                        && (DEPT_IDs == "" || twf.DEPT_ID == DEPT_ID)
                        && (SUB_DEPT_IDs == "" || twf.SUB_DEPT_ID == SUB_DEPT_ID)
                        && (twfm.TRAINNING_START_DATE >= FROM_DATE && twfm.TRAINNING_START_DATE <= TO_DATE)
                        && (twf.CREATED_BY == LoginUserId || LoginType == "ADMIN - IT")
                        select new GetTRAINNING_WORKFORCE
                        {
                            TRAINNING_WORKFORCE_ID = twf.TRAINNING_WORKFORCE_ID,
                            WORKFORCE_NAME = twf.WORKFORCES,
                            ISCOMPLETED = tm.TRAINNING_NAME,
                            CREATED_DATE = twfm.TRAINNING_START_DATE,
                            STATUS = twfm.PRESENTED_EMP,
                            DEPT_NAME = d.DEPT_NAME,
                            SUBDEPT_NAME = sd.SUBDEPT_NAME,
                            CREATED_BY = twf.CREATED_BY,
                            UPDATED_BY = twfm.TRAINNER_NAME,
                            VENUE = twfm.VENUE
                        }).OrderByDescending(x => x.CREATED_DATE).ToList();
            }

           return list;
        }

        public TRAINNING_WORKFORCE_MAPPING UpdateEmployeeTrainningStatus(Guid wftm_id)
        {

            var result = (from wft in db.TAB_TRAINNING_WORKFORCE
                          join Wftm in db.TAB_TRAINNING_WORKFORCE_MAPPING on wft.TRAINNING_WORKFORCE_ID equals Wftm.TRAINNING_WORKFORCE_ID
                          where wft.TRAINNING_WORKFORCE_ID == wftm_id

                          select new TRAINNING_WORKFORCE_MAPPING
                          {
                              BUILDING_ID = (Guid)(wft.BUILDING_ID),
                              DEPT_ID = wft.DEPT_ID,
                              ISTRAINNINGCOMPLETED = wft.ISCOMPLETED,
                              WORKFORCE_NAME = wft.WORKFORCES,
                              SUBDEPT_ID = wft.SUB_DEPT_ID,
                              WF_EMP_TYPE = (short)wft.EMP_TYPES,
                              TRAINNING_WORKFORCE_ID = wft.TRAINNING_WORKFORCE_ID,
                              PHOTO = wft.TRAINNING_PHOTO == null ? null : wft.TRAINNING_PHOTO,
                          }).FirstOrDefault();
             
            return result;
           
        }

        public List<AddWorkforceMappingTrainning> UpdateEmployeeTrainningList(Guid wftm_id)
        {

            var result = (from wft in db.TAB_TRAINNING_WORKFORCE
                          join Wftm in db.TAB_TRAINNING_WORKFORCE_MAPPING on wft.TRAINNING_WORKFORCE_ID equals Wftm.TRAINNING_WORKFORCE_ID
                          join t in db.TAB_TRAINNING_MASTER on Wftm.TRAINNING_ID equals t.TRAINNING_ID
                          //join wf in db.TAB_WORKFORCE_MASTER on wft.WF_ID equals wf.WF_ID
                          where Wftm.TRAINNING_WORKFORCE_ID == wftm_id

                          select new AddWorkforceMappingTrainning
                          {
                              TRAINNING_MAPPING_ID=Wftm.TRAINNING_MAPPING_ID,
                              TRAINNING_ID = Wftm.TRAINNING_ID,
                              START_DATE = Wftm.TRAINNING_START_DATE,
                              END_DATE = Wftm.TRAINNING_END_DATE,
                              Venue = Wftm.VENUE,
                              Time=Wftm.TRANING_TIMES,
                              PRESENTED_EMP = Wftm.PRESENTED_EMP,
                              TRAINNER_NAME =Wftm.TRAINNER_NAME,
                              //PHOTO = wft == null ? null : wft.TRAINNING_PHOTO,
                          }).ToList();
            
            return result;
            //return result;

        }
        public bool UpdateEmployeeTrainningStatus(TRAINNING_WORKFORCE_MAPPING wftm)
        {
            string WORKFORCE_IDS = "";
            int i = 0;
            int len = wftm.EMP_NAMES.Length - 1;
            foreach (var role_id in wftm.EMP_NAMES)
            {
                WORKFORCE_IDS += role_id;
                if (len != i)
                {
                    WORKFORCE_IDS += ", ";
                }
                i++;
            }
            foreach (var item in wftm.ListMetaDatas)
            {
                var result = db.TAB_TRAINNING_WORKFORCE_MAPPING.Where(x => x.TRAINNING_MAPPING_ID == item.TRAINNING_MAPPING_ID).FirstOrDefault();

                result.TRAINNING_ID = item.TRAINNING_ID;
                result.TRAINNING_START_DATE = Convert.ToDateTime(item.START_DATE);
                result.TRAINNING_END_DATE = Convert.ToDateTime(item.START_DATE);
                result.TRANING_TIMES = item.Time;
                result.VENUE = item.Venue;
                result.UPDATED_BY = wftm.CreatedBy;
                result.UPDATED_DATE = DateTime.Now;
                result.TRAINNER_NAME = item.TRAINNER_NAME;
            }
            
            var result1 = db.TAB_TRAINNING_WORKFORCE.Where(x => x.TRAINNING_WORKFORCE_ID == wftm.TRAINNING_WORKFORCE_ID).FirstOrDefault();
            result1.BUILDING_ID = wftm.BUILDING_ID;
            result1.ISCOMPLETED = wftm.ISTRAINNINGCOMPLETED;
            result1.DEPT_ID = wftm.DEPT_ID;
            result1.SUB_DEPT_ID = wftm.SUBDEPT_ID;
            result1.EMP_TYPES = wftm.WF_EMP_TYPE;
            result1.WORKFORCES = WORKFORCE_IDS;
            result1.UPDATED_BY = wftm.CreatedBy;
            result1.UPDATED_DATE = DateTime.Now;
            if(wftm.PHOTO != null)
            {
                result1.TRAINNING_PHOTO = wftm.PHOTO;
            }
           
            db.SaveChanges();

            return true;
        }


        public List<TRAINNING_WORKFORCE_MAPPING> GetWorkForceTrainningForApprovalByDepartmentId(Guid dept_id, Guid? sub_dept_id, int? workforce_type_id, string trainning_status,Guid BUILDING_ID)
        {
            Guid cmp_id = SessionHelper.Get<Guid>("CompanyId");
            List<TRAINNING_WORKFORCE_MAPPING> list = new List<TRAINNING_WORKFORCE_MAPPING>();
            list = (from twf in db.TAB_TRAINNING_WORKFORCE
                    join twfm in db.TAB_TRAINNING_WORKFORCE_MAPPING on twf.TRAINNING_WORKFORCE_ID equals twfm.TRAINNING_WORKFORCE_ID
                    join wf in db.TAB_WORKFORCE_MASTER on twf.WF_ID equals wf.WF_ID
                    join t in db.TAB_TRAINNING_MASTER on twfm.TRAINNING_ID equals t.TRAINNING_ID
                    join d in db.TAB_DEPARTMENT_MASTER on wf.DEPT_ID equals d.DEPT_ID
                    where twf.CMP_ID == cmp_id && wf.DEPT_ID == dept_id
                       && twfm.ISCOMPLETED == (trainning_status != "" ? trainning_status : twfm.ISCOMPLETED)
                     && wf.WF_EMP_TYPE == (workforce_type_id != null ? workforce_type_id : wf.WF_EMP_TYPE)
                     && wf.SUBDEPT_ID == (sub_dept_id != null ? sub_dept_id : wf.SUBDEPT_ID)
                     && wf.BUILDING_ID == (BUILDING_ID != null ? BUILDING_ID : wf.BUILDING_ID)
                    select new TRAINNING_WORKFORCE_MAPPING
                    {
                        TRAINNING_WORKFORCE_ID = twf.TRAINNING_WORKFORCE_ID,
                        WORKFORCE_NAME = wf.EMP_NAME,
                        WORKFORCE_CODE = wf.EMP_ID,
                        DEPT_NAME = d.DEPT_NAME,
                        TRAINNING_NAME = t.TRAINNING_NAME,
                        ISTRAINNINGCOMPLETED = twfm.ISCOMPLETED == "Y" ? "Yes" : "No",
                        TRAINNING_START_DATE = twfm.TRAINNING_START_DATE,
                        TRAINNING_END_DATE = twfm.TRAINNING_END_DATE,
                        TRAINNING_MAPPING_ID = twfm.TRAINNING_MAPPING_ID
                    }).ToList();
            return list;

        }

        #endregion

    }
}