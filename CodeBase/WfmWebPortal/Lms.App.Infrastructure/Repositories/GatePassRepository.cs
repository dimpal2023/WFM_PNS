using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Wfm.App.Common;
using Wfm.App.Core;
using Wfm.App.Core.Model;
using Wfm.App.Infrastructure.Interfaces;
using TAB_GATEPASS = Wfm.App.Core.TAB_GATEPASS;

namespace Wfm.App.Infrastructure.Repositories
{
    public class GatePassRepository : IGatePassRepository
    {
        private ApplicationEntities _appEntity;

        public GatePassRepository()
        {
            _appEntity = new ApplicationEntities();
        }

        public GatePassAllItemsMetaData GetAllItems(string rolename, string dept_id, string sub_dept_id, DateTime FROM_DATE, DateTime TO_DATE, string STATUS_ID, string BUILDING_ID)
        {
            GatePassAllItemsMetaData gatePass = new GatePassAllItemsMetaData();
            Guid dept_ids = string.IsNullOrEmpty(dept_id) ? new Guid() : new Guid(dept_id);
            Guid sub_dept_ids = string.IsNullOrEmpty(sub_dept_id) ? new Guid() : new Guid(sub_dept_id);
            Guid BUILDING_IDs = string.IsNullOrEmpty(BUILDING_ID) ? new Guid() : new Guid(BUILDING_ID);
            string LoginUserId = SessionHelper.Get<string>("LoginUserId");
            string LoginType = SessionHelper.Get<string>("LoginType");

            try
            {
                if (FROM_DATE.ToString() == "01/01/2001 12:00:00 AM" || TO_DATE.ToString() == "01/01/2001 12:00:00 AM")
                {
                    gatePass.ALLITEMS = (from GP in _appEntity.TAB_GATEPASS
                                         join DM in _appEntity.TAB_DEPARTMENT_MASTER on GP.DEPT_ID equals DM.DEPT_ID
                                         //join SM in _appEntity.TAB_SUBDEPARTMENT_MASTER on GP.SUBDEPT_ID equals SM.SUBDEPT_ID
                                         where (dept_id == "" || GP.DEPT_ID == dept_ids)
                                         && (sub_dept_id == "" || GP.SUBDEPT_ID == sub_dept_ids)
                                         && (STATUS_ID == "0" || GP.STATUS.ToString() == STATUS_ID)
                                         && (GP.BUILDING_ID == BUILDING_IDs)
                                         && (GP.CREATED_BY == LoginUserId || (LoginType == "ADMIN - IT" || LoginType == "SECURITY"))
                                         select new GatePassMetaData
                                         {
                                             ID = GP.ID,
                                             START_DATE = GP.START_DATE,
                                             END_DATE = GP.END_DATE,
                                             OUT_TIME = GP.OUT_TIME,
                                             IN_TIME = GP.IN_TIME,
                                             ACTUAL_OUTTIME = GP.ACTUAL_OUTTIME.Value,
                                             ACTUAL_INTIME = GP.ACTUAL_INTIME.Value,
                                             PURPOSE = GP.PURPOSE,
                                             WORKFORCE_IDS = GP.WORKFORCE_IDS,
                                             STATUS = GP.STATUS,
                                             MOBILE_NO = GP.MOBILE_NO,
                                             DEPT_NAME = DM.DEPT_NAME,
                                             //SUBDEPT_NAME = SM.SUBDEPT_NAME,
                                             CREATED_BY = GP.CREATED_BY
                                         }).OrderByDescending(x => x.ID).ToList();
                }
                else
                {
                    //DateTime FROMDATE = Convert.ToDateTime(FROM_DATE.ToString());
                    //DateTime TODATE = Convert.ToDateTime(TO_DATE);

                    gatePass.ALLITEMS = (from GP in _appEntity.TAB_GATEPASS
                                         join DM in _appEntity.TAB_DEPARTMENT_MASTER on GP.DEPT_ID equals DM.DEPT_ID
                                         //join SM in _appEntity.TAB_SUBDEPARTMENT_MASTER on GP.SUBDEPT_ID equals SM.SUBDEPT_ID
                                         where (dept_id == "" || GP.DEPT_ID == dept_ids)
                                         && (sub_dept_id == "" || GP.SUBDEPT_ID == sub_dept_ids)
                                         && (STATUS_ID == "0" || GP.STATUS.ToString() == STATUS_ID)
                                         && (GP.BUILDING_ID == BUILDING_IDs)
                                         && (GP.CREATED_BY == LoginUserId || (LoginType == "ADMIN - IT" || LoginType == "SECURITY"))
                                         && (DbFunctions.TruncateTime(GP.START_DATE) >= FROM_DATE && DbFunctions.TruncateTime(GP.END_DATE) <= TO_DATE)

                                         select new GatePassMetaData
                                         {
                                             ID = GP.ID,
                                             START_DATE = GP.START_DATE,
                                             END_DATE = GP.END_DATE,
                                             OUT_TIME = GP.OUT_TIME,
                                             IN_TIME = GP.IN_TIME,
                                             ACTUAL_OUTTIME = GP.ACTUAL_OUTTIME.Value,
                                             ACTUAL_INTIME = GP.ACTUAL_INTIME.Value,
                                             PURPOSE = GP.PURPOSE,
                                             WORKFORCE_IDS = GP.WORKFORCE_IDS,
                                             STATUS = GP.STATUS,
                                             MOBILE_NO = GP.MOBILE_NO,
                                             DEPT_NAME = DM.DEPT_NAME,
                                             //SUBDEPT_NAME = SM.SUBDEPT_NAME,
                                             CREATED_BY = GP.CREATED_BY
                                         }).OrderByDescending(x => x.ID).ToList();
                }

                //gatePass.ALLITEMS= result;
                //}
            }
            catch (Exception ex)
            {
                AccountRepository.InsertError_Log(ex.ToString(), "Page - GatePassRepository.cs, Method - GetAllItems", HttpContext.Current.Request.Url.AbsolutePath, SessionHelper.Get<string>("LoginUserId"));
            }

            return gatePass;
        }

        public GatePassMetaData FindWorkforce(Guid emp_id)
        {
            GatePassMetaData gatepass = null;
            try
            {
                var wfobj = _appEntity.TAB_WORKFORCE_MASTER.Where(x => x.WF_ID == emp_id).FirstOrDefault();

                if (wfobj != null)
                {
                    gatepass = new GatePassMetaData
                    {
                        START_DATE = DateTime.Now.Date,
                        END_DATE = DateTime.Now.Date,
                        IN_TIME = new TimeSpan(0, 0, 0),
                        OUT_TIME = new TimeSpan(0, 0, 0),
                        ACTUAL_OUTTIME = new TimeSpan(0, 0, 0),
                        ACTUAL_INTIME = new TimeSpan(0, 0, 0),
                        PURPOSE = "Official",
                        REMARK = string.Empty,
                        STATUS = true,
                        WORKFORCE = new WorkforceMasterMetaData
                        {
                            WF_ID = wfobj.WF_ID,
                            EMP_ID = wfobj.EMP_ID,
                            EMP_NAME = wfobj.EMP_NAME,
                            MOBILE_NO = wfobj.MOBILE_NO,
                            FATHER_NAME = wfobj.FATHER_NAME,
                            EMAIL_ID = wfobj.EMAIL_ID
                        }
                    };
                }
            }
            catch (Exception ex)
            {
                AccountRepository.InsertError_Log(ex.ToString(), "Page - GatePassRepository.cs, Method - Find", HttpContext.Current.Request.Url.AbsolutePath, SessionHelper.Get<string>("LoginUserId"));
            }

            return gatepass;
        }

        public GatePassMetaData FindGatePass(Guid gatepass_id)
        {
            GatePassMetaData gatepass = null;
            try
            {
                var gpObj = _appEntity.TAB_GATEPASS.Where(x => x.ID == gatepass_id).FirstOrDefault();

                if (gpObj != null)
                {
                    //var workforceobj = _appEntity.TAB_WORKFORCE_MASTER.Where(x => x.WF_ID == gpObj.WORKFORCE_ID).FirstOrDefault();

                    gatepass = new GatePassMetaData
                    {
                        ID = gpObj.ID,
                        START_DATE = gpObj.START_DATE,
                        END_DATE = gpObj.END_DATE,
                        IN_TIME = gpObj.IN_TIME,
                        OUT_TIME = gpObj.OUT_TIME,
                        ACTUAL_OUTTIME = gpObj.ACTUAL_OUTTIME.Value,
                        ACTUAL_INTIME = gpObj.ACTUAL_INTIME.Value,
                        PURPOSE = gpObj.PURPOSE,
                        REMARK = gpObj.REMARK,
                        STATUS = gpObj.STATUS,
                        MOBILE_NO = gpObj.MOBILE_NO,
                        WORKFORCE_IDS = gpObj.WORKFORCE_IDS,

                        DEPT_ID = gpObj.DEPT_ID,
                        WF_EMP_TYPE = gpObj.WF_EMP_TYPE,
                        SUBDEPT_ID = gpObj.SUBDEPT_ID,
                        BUILDING_ID = (Guid)(gpObj.BUILDING_ID)
                    };
                }
            }
            catch (Exception ex)
            {
                AccountRepository.InsertError_Log(ex.ToString(), "Page - GatePassRepository.cs, Method - FindGatePass", HttpContext.Current.Request.Url.AbsolutePath, SessionHelper.Get<string>("LoginUserId"));
            }

            return gatepass;
        }

        public string Create(GatePassMetaData gatePass)
        {
            try
            {
                //string res = CheckEMP_Present(gatePass.EMP_NAME);

                //if (res != "")
                //{
                //    return res.Remove(res.LastIndexOf(",")) + " Absent today.";
                //}

                if (gatePass.ID == Guid.Empty)
                {
                    gatePass.ID = Guid.NewGuid();
                }
                string WORKFORCE_IDS = "";
                int i = 0;
                int len = gatePass.EMP_NAME.Length - 1;
                foreach (var role_id in gatePass.EMP_NAME)
                {
                    WORKFORCE_IDS += role_id;
                    if (len != i)
                    {
                        WORKFORCE_IDS += ", ";
                    }
                    i++;
                }


                TAB_GATEPASS obj = new TAB_GATEPASS
                {
                    ID = gatePass.ID,
                    START_DATE = gatePass.START_DATE,
                    END_DATE = gatePass.END_DATE,
                    OUT_TIME = gatePass.OUT_TIME,
                    IN_TIME = gatePass.IN_TIME,
                    ACTUAL_OUTTIME = gatePass.ACTUAL_OUTTIME,
                    ACTUAL_INTIME = gatePass.ACTUAL_INTIME,
                    PURPOSE = gatePass.PURPOSE,
                    REMARK = gatePass.REMARK,
                    STATUS = true,
                    DEPT_ID = gatePass.DEPT_ID,
                    WF_EMP_TYPE = gatePass.WF_EMP_TYPE,
                    CREATED_DATE = DateTime.Now,
                    CREATED_BY = SessionHelper.Get<string>("LoginUserId"),
                    SUBDEPT_ID = gatePass.SUBDEPT_ID,
                    WORKFORCE_IDS = WORKFORCE_IDS,
                    MOBILE_NO = gatePass.MOBILE_NO,
                    BUILDING_ID = gatePass.BUILDING_ID
                };

                _appEntity.TAB_GATEPASS.Add(obj);
                _appEntity.SaveChanges();
                return "true";


            }
            catch (Exception ex)
            {
                AccountRepository.InsertError_Log(ex.ToString(), "Page - GatePassRepository.cs, Method - Create", HttpContext.Current.Request.Url.AbsolutePath, SessionHelper.Get<string>("LoginUserId"));
                return "false";
            }
        }
        public string CheckEMP_Present(string[] WF_IDS)
        {
            string AbsentEmp = "";
            string[] values = WF_IDS;
            for (int k = 0; k < values.Length; k++)
            {
                string data = values[k].Trim();

                Core.TAB_WORKFORCE_MASTER EMP = _appEntity.TAB_WORKFORCE_MASTER.Where(x => (x.EMP_NAME+" - "+x.EMP_ID) == data).FirstOrDefault();

                DateTime date1 = DateTime.Now.Date;

                Core.TAB_BIOMETRIC atte = _appEntity.TAB_BIOMETRIC.Where(x => x.BIOMETRIC_CODE == EMP.BIOMETRIC_CODE && x.ATTENDANCE_DATE == date1 && x.Present == "1").FirstOrDefault();

                if (atte == null)
                {
                    AbsentEmp += data + ", ";
                }
            }
            return AbsentEmp;
        }
        public string CheckEMP_Present1(string WF_IDS)
        {
            string AbsentEmp = "";
            string[] values = WF_IDS.Split(',');
            for (int k = 0; k < values.Length; k++)
            {
                string data = values[k].Trim();

                Core.TAB_WORKFORCE_MASTER EMP = _appEntity.TAB_WORKFORCE_MASTER.Where(x => (x.EMP_NAME + " - " + x.EMP_ID) == data).FirstOrDefault();

                DateTime date1 = DateTime.Now.Date;

                Core.TAB_BIOMETRIC atte = _appEntity.TAB_BIOMETRIC.Where(x => x.BIOMETRIC_CODE == EMP.BIOMETRIC_CODE && x.ATTENDANCE_DATE == date1 && x.Present == "1").FirstOrDefault();

                if (atte == null)
                {
                    AbsentEmp += data + ", ";
                }
            }
            return AbsentEmp;
        }
        public string Update(GatePassMetaData gatePass)
        {
            try
            {
                //string res = CheckEMP_Present(gatePass.EMP_NAME);

                //if (res != "")
                //{
                //    return res.Remove(res.LastIndexOf(",")) + " Absent today.";
                //}
                Core.TAB_GATEPASS pass = _appEntity.TAB_GATEPASS.Where(x => x.ID == gatePass.ID).FirstOrDefault();
                string WORKFORCE_IDS = "";
                int i = 0;
                int len = gatePass.EMP_NAME.Length - 1;
                foreach (var role_id in gatePass.EMP_NAME)
                {
                    WORKFORCE_IDS += role_id;
                    if (len != i)
                    {
                        WORKFORCE_IDS += ", ";
                    }
                    i++;
                }
                if (pass != null)
                {
                    pass.START_DATE = gatePass.START_DATE;
                    pass.END_DATE = gatePass.END_DATE;
                    pass.OUT_TIME = gatePass.OUT_TIME;
                    pass.IN_TIME = gatePass.IN_TIME;
                    pass.ACTUAL_OUTTIME = gatePass.ACTUAL_OUTTIME;
                    pass.ACTUAL_INTIME = gatePass.ACTUAL_INTIME;
                    pass.REMARK = gatePass.REMARK;
                    pass.PURPOSE = gatePass.PURPOSE;
                    pass.STATUS = gatePass.STATUS;
                    pass.DEPT_ID = gatePass.DEPT_ID;
                    pass.WF_EMP_TYPE = gatePass.WF_EMP_TYPE;
                    pass.UPDATED_BY = SessionHelper.Get<string>("LoginUserId");
                    pass.UPDATED_DATE = DateTime.Now;
                    pass.WORKFORCE_ID = gatePass.WORKFORCE.WF_ID;
                    pass.SUBDEPT_ID = gatePass.SUBDEPT_ID;
                    pass.WORKFORCE_IDS = WORKFORCE_IDS;
                    pass.MOBILE_NO = gatePass.MOBILE_NO;
                    _appEntity.SaveChanges();
                   
                }
                return "true";
            }
            catch (Exception ex)
            {
                AccountRepository.InsertError_Log(ex.ToString(), "Page - GatePassRepository.cs, Method - Update", HttpContext.Current.Request.Url.AbsolutePath, SessionHelper.Get<string>("LoginUserId"));
                return "false";
            }
        }

        public void Delete(Guid gatePassId)
        {
            try
            {
                Core.TAB_GATEPASS pass = _appEntity.TAB_GATEPASS.Where(x => x.ID == gatePassId).FirstOrDefault();

                if (pass != null)
                {
                    pass.STATUS = false;

                    _appEntity.Entry(pass).State = EntityState.Modified;
                    _appEntity.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                AccountRepository.InsertError_Log(ex.ToString(), "Page - GatePassRepository.cs, Method - Delete", HttpContext.Current.Request.Url.AbsolutePath, SessionHelper.Get<string>("LoginUserId"));
            }
        }

        public string Out(Guid gatePassId)
        {
            try
            {
                Core.TAB_GATEPASS pass = _appEntity.TAB_GATEPASS.Where(x => x.ID == gatePassId).FirstOrDefault();

                GatePassMetaData gatePass = null;

                if (pass != null)
                {

                    //string res = CheckEMP_Present1(pass.WORKFORCE_IDS);

                    //if (res == "")
                    //{
                        DateTime date = DateTime.Now;
                        gatePass = new GatePassMetaData
                        {
                            ID = pass.ID,
                            ACTUAL_OUTTIME = new TimeSpan(date.Hour, date.Minute, date.Second),
                            STATUS = true
                        };

                        pass.ID = gatePass.ID;
                        pass.ACTUAL_OUTTIME = gatePass.ACTUAL_OUTTIME;
                        pass.STATUS = gatePass.STATUS;

                        _appEntity.Entry(pass).State = EntityState.Modified;
                        _appEntity.SaveChanges();
                        return "true";
                    //}
                    //else
                    //{
                    //    return res.Remove(res.LastIndexOf(",")) + " Absent today.";
                    //}

                }
                return "false";
            }
            catch (Exception ex)
            {
                AccountRepository.InsertError_Log(ex.ToString(), "Page - GatePassRepository.cs, Method - Out", HttpContext.Current.Request.Url.AbsolutePath, SessionHelper.Get<string>("LoginUserId"));
                return "false";
            }
        }

        public void In(Guid gatePassId)
        {
            try
            {
                Core.TAB_GATEPASS pass = _appEntity.TAB_GATEPASS.Where(x => x.ID == gatePassId).FirstOrDefault();

                GatePassMetaData gatePass = null;

                if (pass != null)
                {
                    DateTime date = DateTime.Now;
                    gatePass = new GatePassMetaData
                    {
                        ID = pass.ID,
                        ACTUAL_INTIME = new TimeSpan(date.Hour, date.Minute, date.Second),
                        STATUS = false
                    };

                    pass.ID = gatePass.ID;
                    pass.ACTUAL_INTIME = gatePass.ACTUAL_INTIME;
                    pass.STATUS = gatePass.STATUS;

                    _appEntity.Entry(pass).State = EntityState.Modified;
                    _appEntity.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                AccountRepository.InsertError_Log(ex.ToString(), "Page - GatePassRepository.cs, Method - In", HttpContext.Current.Request.Url.AbsolutePath, SessionHelper.Get<string>("LoginUserId"));
            }
        }

    }
}
