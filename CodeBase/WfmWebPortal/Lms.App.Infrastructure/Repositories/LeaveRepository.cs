using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Wfm.App.ConfigManager;
using Wfm.App.Core;
using Wfm.App.Core.Model;
using Wfm.App.Infrastructure.Interfaces;
using Microsoft.ApplicationBlocks.Data;
using System.Data;
using Wfm.App.Common;
using System.Data.Entity;
using System.Web;

namespace Wfm.App.Infrastructure.Repositories
{
    public class LeaveRepository
    {
        private ApplicationEntities _appEntity;

        public LeaveRepository()
        {
            _appEntity = new ApplicationEntities();
        }
        
        public void Create(WorkforceLeavesMetaData leave)
        {
            try
            {
                Wfm.App.Core.TAB_WORKFORCE_LEAVES obj = new Wfm.App.Core.TAB_WORKFORCE_LEAVES
                {
                    ID = Guid.NewGuid(),
                    WF_ID = leave.WF_ID,
                    EMP_ID = leave.HIDDENEMP_ID,
                    FROM_DATE = leave.FROM_DATE,
                    TO_DATE = leave.TO_DATE,
                    REMARKS = leave.REMARKS,
                    CREATED_DATE = DateTime.Now,
                    CREATED_BY = SessionHelper.Get<String>("Username")
                };

                _appEntity.TAB_WORKFORCE_LEAVES.Add(obj);
                _appEntity.SaveChanges();
            }
            catch (Exception ex)
            {
                AccountRepository.InsertError_Log(ex.ToString(), "Page - LeaveRepository.cs, Method - Create", HttpContext.Current.Request.Url.AbsolutePath, SessionHelper.Get<string>("LoginUserId"));
            }
        }

        public List<WorkforceLeavesMetaData> GetLeaveAllItems()
        {
            List<WorkforceLeavesMetaData> leaveList = new List<WorkforceLeavesMetaData>();

            try
            {
                leaveList = (from leave in _appEntity.TAB_WORKFORCE_LEAVES
                             join emp in _appEntity.TAB_WORKFORCE_MASTER on leave.WF_ID equals emp.WF_ID
                                    select new WorkforceLeavesMetaData
                                    {
                                        ID = leave.ID,
                                        EMP_ID = emp.EMP_ID,
                                        EMP_NAME = emp.EMP_NAME,
                                        FROM_DATE = leave.FROM_DATE,
                                        TO_DATE = leave.TO_DATE,
                                        REMARKS = leave.REMARKS
                                    }).ToList();
            }
            catch (Exception ex)
            {
                AccountRepository.InsertError_Log(ex.ToString(), "Page - LeaveRepository.cs, Method - GetLeaveAllItems", HttpContext.Current.Request.Url.AbsolutePath, SessionHelper.Get<string>("LoginUserId"));
            }

            return leaveList;
        }


        public WorkforceLeavesMetaData Find(string emp_id)
        {
            WorkforceLeavesMetaData objeave = null;
            try
            {
                objeave = (from emp in _appEntity.TAB_WORKFORCE_MASTER
                           select new WorkforceLeavesMetaData
                           {
                               EMP_ID = emp.EMP_ID,
                               EMP_NAME = emp.EMP_NAME,
                               WF_ID = emp.WF_ID,
                               HIDDENEMP_ID = emp.EMP_ID
                          }).Where(f => f.EMP_ID == emp_id).FirstOrDefault();

            }
            catch (Exception ex)
            {
                AccountRepository.InsertError_Log(ex.ToString(), "Page - LeaveRepository.cs, Method - Find", HttpContext.Current.Request.Url.AbsolutePath, SessionHelper.Get<string>("LoginUserId"));
            }

            return objeave;
        }
       
    }
}
