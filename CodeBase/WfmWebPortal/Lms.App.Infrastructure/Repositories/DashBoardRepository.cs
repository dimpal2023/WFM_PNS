using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using Wfm.App.Common;
using Wfm.App.Core;
using Wfm.App.Core.Model;

namespace Wfm.App.Infrastructure.Repositories
{
    public class DashBoardRepository
    {
        private ApplicationEntities _appEntity;

        public DashBoardRepository()
        {
            _appEntity = new ApplicationEntities();
        }

        public List<DepartmentMasterMetaData> GetDepartmentByCompanyId(Guid companyId)
        {
            List<DepartmentMasterMetaData> departmentMasterMetaDatas = null;

            try
            {
                Guid loggedUserGuid = Utility.GetLoggedInUserId();

                if (loggedUserGuid != Guid.Empty)
                {
                    var departments = (from DM in _appEntity.TAB_DEPARTMENT_MASTER
                                       join UDM in _appEntity.TAB_USER_DEPARTMENT_MAPPING on DM.DEPT_ID equals UDM.DEPT_ID
                                       join LM in _appEntity.TAB_LOGIN_MASTER on UDM.USER_ID equals LM.USER_ID
                                       where LM.USER_ID == loggedUserGuid && DM.status == "Y" && DM.COMPANY_ID == companyId
                                       select new DepartmentMasterMetaData
                                       {
                                           DEPT_ID = DM.DEPT_ID,
                                           DEPT_NAME = DM.DEPT_NAME
                                       }).OrderBy(x => x.DEPT_NAME).ToList();

                    return departments;
                }
            }
            catch (Exception ex)
            {
                AccountRepository.InsertError_Log(ex.ToString(), "Page - DashboardRepository.cs, Method - GetDepartmentByCompanyId", HttpContext.Current.Request.Url.AbsolutePath, SessionHelper.Get<string>("LoginUserId"));
            }
            return departmentMasterMetaDatas;
        }

        public DashBoardJSONMetaData GetDashboardDataJSON(Guid deptId, Guid subDeptId, Guid companyId, Guid BUILDING_ID)
        {
            DashBoardJSONMetaData dashBoard = new DashBoardJSONMetaData();
            try
            {
                string Depts = "1"; string SubDepts = "1"; string Units = "1";
                if (deptId == new Guid("00000000-0000-0000-0000-000000000000"))
                {
                    Depts = "";
                }
                if (subDeptId == new Guid("00000000-0000-0000-0000-000000000000"))
                {
                    SubDepts = "";
                }
                if (BUILDING_ID == new Guid("00000000-0000-0000-0000-000000000000"))
                {
                    Units = "";
                }
                Guid loggedUserGuid = Utility.GetLoggedInUserId();
                string Role = SessionHelper.Get<string>("LoginUserId");
                
                var UNIT = (from udm in _appEntity.TAB_USER_DEPARTMENT_MAPPING
                            join dept in _appEntity.TAB_DEPARTMENT_MASTER on udm.DEPT_ID equals dept.DEPT_ID
                            where udm.USER_ID == loggedUserGuid
                            select new
                            {
                                dept.BUILDING_ID
                            }
                          ).FirstOrDefault();

                if (loggedUserGuid != new Guid("32816b3a-674a-4d3a-ad87-159ff5600350") && Units == "")
                {
                    BUILDING_ID = (Guid)UNIT.BUILDING_ID;
                    Units = "1";
                }


                DateTime endDate = DateTime.Now;
                int day = Convert.ToInt32(endDate.Day);
                DateTime StartDate = endDate.AddDays(-(day));

                // if (Role == "admin")
                // {
                //     dashBoard.TolalNewWorkforceCount30Days = _appEntity.TAB_WORKFORCE_MASTER.Where(x => x.DOJ >= StartDate
                //&& x.DOJ <= endDate
                //&& x.STATUS == "Y"
                //&& (x.DEPT_ID == deptId || Depts == "")
                //&& (x.SUBDEPT_ID == subDeptId || SubDepts == "")
                //&& (x.BUILDING_ID == BUILDING_ID || Units == "")).Count();
                // }
                // else
                // {
                dashBoard.Role = Role;
                dashBoard.TolalNewWorkforceCount30Days = (from wm in _appEntity.TAB_WORKFORCE_MASTER
                                                          join udm in _appEntity.TAB_USER_DEPARTMENT_MAPPING on wm.SUBDEPT_ID equals udm.SUBDEPT_ID
                                                          where wm.DOJ >= StartDate
                                                          && wm.DOJ <= endDate
                                                          && wm.STATUS == "Y"
                                                          && (wm.DEPT_ID == deptId || Depts == "")
                                                          && (wm.SUBDEPT_ID == subDeptId || SubDepts == "")
                                                          && (wm.BUILDING_ID == BUILDING_ID || Units == "")
                                                          && (udm.USER_ID == loggedUserGuid)
                                                          select new
                                                          {
                                                              QUANTITY = 1
                                                          }
                                          ).Count();
                //}




                //if (Role == "admin")
                //{
                //    dashBoard.TolalWorkforceCount = _appEntity.TAB_WORKFORCE_MASTER.Where(x => x.STATUS == "Y"
                //&& (x.DEPT_ID == deptId || Depts == "")
                //&& (x.SUBDEPT_ID == subDeptId || SubDepts == "")
                //&& (x.BUILDING_ID == BUILDING_ID || Units == "")).Count();
                //}
                //else
                //{
                dashBoard.TolalWorkforceCount = (from wm in _appEntity.TAB_WORKFORCE_MASTER
                                                 join udm in _appEntity.TAB_USER_DEPARTMENT_MAPPING on wm.SUBDEPT_ID equals udm.SUBDEPT_ID
                                                 where wm.STATUS == "Y"
                                                 && (wm.DEPT_ID == deptId || Depts == "")
                                                 && (wm.SUBDEPT_ID == subDeptId || SubDepts == "")
                                                 && (wm.BUILDING_ID == BUILDING_ID || Units == "")
                                                 && udm.USER_ID == loggedUserGuid
                                                 select new
                                                 {
                                                     W_QUANTITY = 1
                                                 }
                                                ).Count();
                //}
                dashBoard.TotalTransfer = (from wm in _appEntity.TAB_EMPLOYEE_TRANSFER
                                         
                                           where wm.IS_APPROVED == 1
                                           && (wm.DEPT_ID == deptId || Depts == "")
                                           && (wm.SUB_DEPT_ID == subDeptId || SubDepts == "")
                                           && (wm.BUILDING_ID == BUILDING_ID || Units == "")
                                         
                                           select new
                                           {
                                               W_QUANTITY = 1
                                           }
                                                   ).Count();

                dashBoard.TotalExit = (from wm in _appEntity.TAB_WORKFORCE_MASTER
                                       join exit in _appEntity.TAB_WORKFORCE_EXIT on wm.WF_ID equals exit.WF_ID
                                       join eap in _appEntity.TAB_WORKFORCE_EXIT_APPROVER on exit.EXIT_ID equals eap.EXIT_ID
                                       where eap.APPROVER_STATUS != "Y"  && (wm.DEPT_ID == deptId || Depts == "") 
                                       && (wm.SUBDEPT_ID == subDeptId || SubDepts == "")
                                       && (wm.BUILDING_ID == BUILDING_ID || Units == "")
                                       select new
                                       {
                                           W_QUANTITY = 1
                                       }
                                                ).Count();




                dashBoard.TotalPendingMRF = (from mrf in _appEntity.TAB_MRF
                                             join mrfd in _appEntity.TAB_MRF_DETAILS on mrf.MRP_INETRNAL_ID equals mrfd.MRP_INETRNAL_ID
                                             join udm in _appEntity.TAB_USER_DEPARTMENT_MAPPING on mrfd.SUBDEPT_ID equals udm.SUBDEPT_ID
                                             where mrf.MRF_STATUS == "OPEN"
                                             && mrf.COMPANY_ID == companyId
                                             && (mrfd.FLOOR_ID.Value == deptId || Depts == "")
                                             && (mrfd.SUBDEPT_ID == subDeptId || SubDepts == "")
                                             && (mrfd.BUILDING_ID == BUILDING_ID || Units == "")
                                             && (udm.USER_ID == loggedUserGuid)
                                             select new
                                             {
                                                 WOKFORCE_QUANTITY = 1
                                             }
                                     ).Count();



                dashBoard.TotalFreezingStrength = (from mrf in _appEntity.TAB_SUBDEPARTMENT_MASTER
                                                   join udm in _appEntity.TAB_USER_DEPARTMENT_MAPPING on mrf.SUBDEPT_ID equals udm.SUBDEPT_ID
                                                   where (mrf.DEPT_ID == deptId || Depts == "")
                                                   && (mrf.SUBDEPT_ID == subDeptId || SubDepts == "")
                                                   && (mrf.BUILDING_ID == BUILDING_ID || Units == "")
                                                   && (udm.USER_ID == loggedUserGuid)
                                                   select new
                                                   {
                                                       FreezingStrength = (int?)mrf.FreezingStrength
                                                   }
                                       ).Sum(x => x.FreezingStrength).Value;


                dashBoard.PendingHiring = (from mrf in _appEntity.TAB_MRF
                                           join mrfd in _appEntity.TAB_MRF_DETAILS on mrf.MRP_INETRNAL_ID equals mrfd.MRP_INETRNAL_ID
                                           join udm in _appEntity.TAB_USER_DEPARTMENT_MAPPING on mrfd.SUBDEPT_ID equals udm.SUBDEPT_ID
                                           where mrf.MRF_STATUS == "Approved"
                                           && mrf.COMPANY_ID == companyId
                                           && (mrfd.FLOOR_ID.Value == deptId || Depts == "")
                                           && (mrfd.SUBDEPT_ID == subDeptId || SubDepts == "")
                                           && (mrfd.BUILDING_ID == BUILDING_ID || Units == "")
                                           && (udm.USER_ID == loggedUserGuid)
                                           select new
                                           {
                                               WOKFORCE_QUANTITY = (int?)mrfd.WOKFORCE_QUANTITY - mrfd.HIRING_QUANTITY
                                           }
                                         ).Sum(x => x.WOKFORCE_QUANTITY).Value;

                dashBoard.OnTraining = (from training in _appEntity.TAB_TRAINNING_WORKFORCE
                                        join training_map in _appEntity.TAB_TRAINNING_WORKFORCE_MAPPING on training.TRAINNING_WORKFORCE_ID equals training_map.TRAINNING_WORKFORCE_ID
                                        join wf in _appEntity.TAB_WORKFORCE_MASTER on training.WF_ID equals wf.WF_ID
                                        where (wf.DEPT_ID == deptId || Depts == "")
                                        && (wf.SUBDEPT_ID == subDeptId || SubDepts == "")
                                        && (wf.BUILDING_ID == BUILDING_ID || Units == "")
                                        && training.CMP_ID == companyId
                                        && training.ISCOMPLETED == "N"
                                        && training.STATUS == "Y"
                                        select training
                                        ).Count();

                Dictionary<int, string> months = new Dictionary<int, string>();
                for (int i = 1; i < 13; i++)
                {
                    string month = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(i);
                    months.Add(i, month);
                }


                DateTime startDate = endDate.AddDays(-365);
                var salaryPaidData =
                (from d in _appEntity.TAB_WORKFORCE_SALARY
                 join wf in _appEntity.TAB_WORKFORCE_MASTER on d.WF_ID equals wf.WF_ID
                 where wf.DEPT_ID == deptId && wf.COMPANY_ID == companyId
                 && d.PAID_STATUS == "Paid" && (d.CREATED_ON >= startDate && d.CREATED_ON <= endDate)
                 group d by new
                 {
                     Year = d.CREATED_ON.Value.Year,
                     Month = d.CREATED_ON.Value.Month
                 } into g
                 select new
                 {
                     Year = g.Key.Year,
                     Month = g.Key.Month,
                     Total = (g.Sum(x => x.TOTAL_WAGES_AFTER_DEDUCTION)),
                 }).ToList();

                var salaryUnPaidData =
                (from d in _appEntity.TAB_WORKFORCE_SALARY
                 join wf in _appEntity.TAB_WORKFORCE_MASTER on d.WF_ID equals wf.WF_ID
                 where wf.DEPT_ID == deptId && wf.COMPANY_ID == companyId
                 && d.PAID_STATUS == "UnPaid" && (d.CREATED_ON >= startDate && d.CREATED_ON <= endDate)
                 group d by new
                 {
                     Year = d.CREATED_ON.Value.Year,
                     Month = d.CREATED_ON.Value.Month
                 } into g
                 select new
                 {
                     Year = g.Key.Year,
                     Month = g.Key.Month,
                     Total = (g.Sum(x => x.TOTAL_WAGES_AFTER_DEDUCTION)),
                 }).ToList();

                var result = (from m in months
                              join p in salaryPaidData on m.Key equals p.Month into LJoin
                              join up in salaryUnPaidData on m.Key equals up.Month into LjoinUP
                              from p in LJoin.DefaultIfEmpty()
                              from up in LjoinUP.DefaultIfEmpty()
                              select new MonthAndTotal
                              {
                                  MonthName = m.Value,
                                  Processed = p == null ? 0 : Convert.ToDecimal(p.Total),
                                  Failed = up == null ? 0 : Convert.ToDecimal(up.Total)
                              }
                             ).ToList();

                dashBoard.MonthAndTotals = result;

            }
            catch (Exception ex)
            {
                AccountRepository.InsertError_Log(ex.ToString(), "Page - DashboardRepository.cs, Method - GetDashboardDataJSON", HttpContext.Current.Request.Url.AbsolutePath, SessionHelper.Get<string>("LoginUserId"));
            }

            return dashBoard;
        }

    }
}
