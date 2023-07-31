using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wfm.App.Common;
using Wfm.App.Core;
using Wfm.App.Core.Model;
using Wfm.App.Infrastructure.Interfaces;
using System.Configuration;
using System.Web;

namespace Wfm.App.Infrastructure.Repositories
{
    public class AssetRepository : IAssetRepository
    {
        private ApplicationEntities _appEntity;

        public AssetRepository()
        {
            _appEntity = new ApplicationEntities();
        }
        public AssetMappingMasterMetaData Find(Guid asset_id)
        {
            AssetMappingMasterMetaData asset = null;
            try
            {
                var asset_result = _appEntity.TAB_ASSET_MASTER.Where(x => x.ASSET_ID == asset_id).FirstOrDefault();

                if (asset_result != null)
                {
                    asset = new AssetMappingMasterMetaData
                    {
                        ASSET_ID = asset_result.ASSET_ID,
                        CREATED_DATE = DateTime.Now.Date,
                        ASSET_LIFE = asset_result.ASSET_LIFE,
                        ASSET_NAME = asset_result.ASSET_NAME,
                        DEPARTMENT_ID = asset_result.DEPARTMENT_ID,
                        BUILDING_ID = asset_result.BUILDING_ID,
                        IS_ACTIVE = asset_result.IS_ACTIVE,
                        SUBDEPT_ID = asset_result.SUBDEPT_ID,
                        REFUNDABLE = asset_result.REFUNDABLE,
                        COMPANY_ID = asset_result.COMPANY_ID,
                        CREATED_BY = SessionHelper.Get<string>("LoginUserId")
                    };
                }

            }
            catch (Exception ex)
            {
                AccountRepository.InsertError_Log(ex.ToString(), "Page - AssetRepository.cs, Method - Find", HttpContext.Current.Request.Url.AbsolutePath, SessionHelper.Get<string>("LoginUserId"));
            }

            return asset;
        }

        public void SubmitEmployeeAssetsOnExit(ExitManagementMetaData model)
        {
            try
            {
                DateTime dt = DateTime.Now;
                var configuredItem = _appEntity.TAB_WORKFORCE_EXIT.Where(x => x.WF_ID == model.WF_ID).FirstOrDefault();

                if (configuredItem != null)
                {
                    Core.TAB_WORKFORCE_EXIT model1 = _appEntity.TAB_WORKFORCE_EXIT.Where(x => x.WF_ID == model.WF_ID).FirstOrDefault();

                    _appEntity.TAB_WORKFORCE_EXIT.Remove(model1);

                }

                TAB_WORKFORCE_EXIT wORKFORCE_EXIT = new TAB_WORKFORCE_EXIT
                {
                    EXIT_ID = Guid.NewGuid(),
                    DEPT_ID = model.DEPT_ID,
                    WF_ID = model.WF_ID,
                    IS_NOTICE_SERVE = model.IS_NOTICE_SERVE,
                    NOTICE_DAYS = Convert.ToByte(model.NOTICE_DAYS),
                    REASON_OF_LEAVING = model.REASON_OF_LEAVING,
                    RESIGNATION_DATE = model.RESIGNATION_DATE.Value,
                    CREATED_BY = SessionHelper.Get<string>("LoginUserId"),
                    CREATED_DATE = DateTime.Now,
                    COMPANY_ID = SessionHelper.Get<Guid>("CompanyId")
                };
                var workforce = _appEntity.TAB_WORKFORCE_MASTER.Where(x => x.WF_ID == model.WF_ID).FirstOrDefault();
                workforce.EXIT_DATE = wORKFORCE_EXIT.RESIGNATION_DATE.AddDays(wORKFORCE_EXIT.NOTICE_DAYS);
                workforce.EXIT_REASON = model.REASON_OF_LEAVING;

                List<TAB_ASSET_ALLOCATION> employeAssetList = (from aa in _appEntity.TAB_ASSET_ALLOCATION
                                                               join am in _appEntity.TAB_ASSET_MASTER on aa.ASSET_ID equals am.ASSET_ID
                                                               where aa.WF_ID == model.WF_ID && am.REFUNDABLE == "Y"
                                                               select aa).ToList();
                if (model.AssetMappingMetaDatas != null)
                {
                    model.AssetMappingMetaDatas = model.AssetMappingMetaDatas.Where(x => x.IS_REFUNDABLE == true).ToList();
                    foreach (var item in model.AssetMappingMetaDatas)
                    {
                        var asset_allocation = employeAssetList.Where(x => x.ASSET_ALLOCATION_ID == item.ASSET_ALLOCATION_ID).FirstOrDefault();
                        if (asset_allocation != null)
                        {
                            asset_allocation.IS_REFOUND = "Y";
                            asset_allocation.ASSET_HANDOVER_DATE = dt;
                        }
                    }
                }

                var exitApprovals = (from wm in _appEntity.TAB_WORKFLOW_MASTER
                                     join wmm in _appEntity.TAB_WORKFLOW_MAPPING_MASTER on wm.WORKFLOW_ID equals wmm.WORKFLOW_ID
                                     join ua in _appEntity.TAB_LOGIN_MASTER on wmm.USER_ID equals ua.USER_ID
                                     where wm.WORKFLOW_NAME == "EXIT WORKFLOW"
                                     select new { wmm.USER_ID, ua.MAIL_ID }).ToList();
                if (exitApprovals.Count > 0)
                {
                    List<TAB_ALL_MAIL> all_mail = new List<TAB_ALL_MAIL>();
                    List<TAB_WORKFORCE_EXIT_APPROVER> exit_approver = new List<TAB_WORKFORCE_EXIT_APPROVER>();
                    var templete = _appEntity.TAB_MAIL_TEMPLATE.Where(x => x.TEMPLATE_FOR == "ApproveAndReject").FirstOrDefault();
                    if (templete != null)
                    {
                        var toUserId = exitApprovals.Select(x => x.USER_ID).ToArray();
                        var toUsers = _appEntity.TAB_LOGIN_MASTER.Where(x => toUserId.Contains(x.USER_ID)).ToList();
                        var fromUser = SessionHelper.Get<string>("Username");
                        List<TAB_ALL_MAIL> mails = new List<TAB_ALL_MAIL>();
                        var exitWorkforce = _appEntity.TAB_WORKFORCE_MASTER.Where(x => x.WF_ID == model.WF_ID).FirstOrDefault();
                        foreach (var item in toUsers)
                        {
                            StringBuilder sb = new StringBuilder();
                            sb.Append(templete.TEMPLATE_CONTANT);
                            sb.Replace("[TONAME]", item.USER_NAME);
                            sb.Replace("[APPROVALTYPE]", "Exit");
                            sb.Replace("[TASKNAME]", exitWorkforce.EMP_NAME + "(" + exitWorkforce.EMP_ID + ")");
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
                            TAB_WORKFORCE_EXIT_APPROVER approver = new TAB_WORKFORCE_EXIT_APPROVER
                            {
                                EXIT_APPROVER_ID = Guid.NewGuid(),
                                EXIT_ID = wORKFORCE_EXIT.EXIT_ID,
                                APPROVE_BY = item.USER_ID
                            };
                            exit_approver.Add(approver);
                            mails.Add(mail);
                        }
                        _appEntity.TAB_ALL_MAIL.AddRange(mails);
                        _appEntity.TAB_WORKFORCE_EXIT_APPROVER.AddRange(exit_approver);
                    }
                }
                _appEntity.TAB_WORKFORCE_EXIT.Add(wORKFORCE_EXIT);
                _appEntity.SaveChanges();
            }
            catch (Exception ex)
            {
                AccountRepository.InsertError_Log(ex.ToString(), "Page - AssetRepository.cs, Method - SubmitEmployeeAssetsOnExit", HttpContext.Current.Request.Url.AbsolutePath, SessionHelper.Get<string>("LoginUserId"));
            }
        }

        public string Employee_Transfer(Guid BUILDING_ID, Guid DEPT_ID, Guid SUBDEPT_ID, int EMPLOYMENT_TYPE, Guid? WF_ID)
        {
            try
            {
                TAB_EMPLOYEE_TRANSFER obj = new TAB_EMPLOYEE_TRANSFER
                {
                    WF_ID = WF_ID,
                    BUILDING_ID = BUILDING_ID,
                    DEPT_ID = DEPT_ID,
                    SUB_DEPT_ID = SUBDEPT_ID,
                    EMPLOYMENT_TYPE = EMPLOYMENT_TYPE,
                    REQUESTED_DATE = DateTime.Now,
                    REQUESTED_BY = SessionHelper.Get<string>("Username"),
                    IS_APPROVED = 0,
                };
                _appEntity.TAB_EMPLOYEE_TRANSFER.Add(obj);
                _appEntity.SaveChanges();
                return "true";
            }
            catch (Exception ex)
            {
                AccountRepository.InsertError_Log(ex.ToString(), "Page - AssetRepository.cs, Method - Employee_Transfer", HttpContext.Current.Request.Url.AbsolutePath, SessionHelper.Get<string>("LoginUserId"));
                return "false";
            }
        }

        public void UpdatExitApprovalsByMappingId(IEnumerable<Guid> twfmIds, string status, string remark, string exitSearchULR)
        {
            try
            {
                var exitApprovals = _appEntity.TAB_WORKFORCE_EXIT_APPROVER.Where(x => twfmIds.Contains(x.EXIT_APPROVER_ID)).ToList();
                foreach (var item in exitApprovals)
                {
                    item.APPROVER_STATUS = status;
                    item.APPROVE_DATE = DateTime.Now;
                    item.REMARK = remark;
                }
                var exitIds = exitApprovals.Select(x => x.EXIT_ID).Distinct().ToArray();
                var tblExit = _appEntity.TAB_WORKFORCE_EXIT.Where(x => exitIds.Contains(x.EXIT_ID)).Select(x => new { x.CREATED_BY, x.WF_ID }).ToList();
                var toUserId = tblExit.Select(x => x.CREATED_BY).ToArray();
                var templete = _appEntity.TAB_MAIL_TEMPLATE.Where(x => x.TEMPLATE_FOR == "ApproveAndRejectStatus").FirstOrDefault();

                if (templete != null)
                {
                    var fromUser = SessionHelper.Get<string>("Username");
                    List<TAB_ALL_MAIL> mails = new List<TAB_ALL_MAIL>();
                    string taskStatus = status == "Y" ? "Approve" : "Reject";

                    foreach (var item in tblExit)
                    {
                        var toUser = _appEntity.TAB_LOGIN_MASTER.Where(x => x.USER_LOGIN_ID == item.CREATED_BY).FirstOrDefault();
                        var wf_user = _appEntity.TAB_WORKFORCE_MASTER.Where(x => x.WF_ID == item.WF_ID).FirstOrDefault();
                        StringBuilder sb = new StringBuilder();
                        sb.Append(templete.TEMPLATE_CONTANT);
                        sb.Replace("[TONAME]", toUser.USER_NAME);
                        sb.Replace("[APPROVALTYPE]", "Exit");
                        sb.Replace("[TASKSTATUS]", taskStatus + $" for {wf_user.EMP_NAME}({wf_user.EMP_ID})");
                        sb.Replace("[FROMNAME]", fromUser);
                        sb.Replace("[REDIRECTURL]", exitSearchULR);
                        var mail = new TAB_ALL_MAIL
                        {
                            CC_MAIL = templete.CC_MAIL,
                            MAIL_CONTENT = sb.ToString(),
                            MAIL_INSERT_DATE = DateTime.Now,
                            TO_MAIL = toUser.MAIL_ID,
                            MAIL_REMARK = "Hiring " + taskStatus,
                            USER_ID = SessionHelper.Get<string>("LoginUserId")
                        };
                        mails.Add(mail);

                        Wfm.App.Core.TAB_WORKFORCE_MASTER obj = _appEntity.TAB_WORKFORCE_MASTER.Where(x => x.WF_ID == item.WF_ID).FirstOrDefault();

                        if (obj != null)
                        {
                            obj.STATUS = "N";
                        }
                    }
                    _appEntity.TAB_ALL_MAIL.AddRange(mails);

                }
                _appEntity.SaveChanges();
            }
            catch (Exception ex)
            {
                AccountRepository.InsertError_Log(ex.ToString(), "Page - AssetRepository.cs, Method - UpdatExitApprovalsByMappingId", HttpContext.Current.Request.Url.AbsolutePath, SessionHelper.Get<string>("LoginUserId"));
                return;
            }
        }

        public List<ExitApprovalMetaData> GetExitApprovals(Guid deptId, Guid? sub_dept_id, string status, Guid BUILDING_ID)
        {
            string ST = "";
            if (status == "")
            {
                ST = "All";
            }
            else if (status == "N")
            {
                ST = null;
            }
            else if (status == "Y")
            {
                ST = "Y";
            }
            Guid cmp_id = SessionHelper.Get<Guid>("CompanyId");
            //var LoginType = SessionHelper.Get<string>("LoginType");
            var result = (from exit in _appEntity.TAB_WORKFORCE_EXIT
                          join wf in _appEntity.TAB_WORKFORCE_MASTER on exit.WF_ID equals wf.WF_ID
                          join approval in _appEntity.TAB_WORKFORCE_EXIT_APPROVER on exit.EXIT_ID equals approval.EXIT_ID
                          join user in _appEntity.TAB_LOGIN_MASTER on approval.APPROVE_BY equals user.USER_ID
                          where exit.COMPANY_ID == cmp_id
                          && exit.DEPT_ID == (deptId != new Guid() ? deptId : exit.DEPT_ID)
                          && wf.SUBDEPT_ID == (sub_dept_id != null ? sub_dept_id : wf.SUBDEPT_ID)
                          && wf.BUILDING_ID == (BUILDING_ID != new Guid("00000000-0000-0000-0000-000000000000") ? BUILDING_ID : wf.BUILDING_ID)
                          && approval.APPROVER_STATUS == (ST != "All" ? ST : approval.APPROVER_STATUS)
                          //&& approval.APPROVER_STATUS == (status=="Y"?status: (status=="N"?"":(status==""?approval.APPROVER_STATUS:status)))
                          select new ExitApprovalMetaData
                          {
                              APPROVER_NAME = user.USER_NAME,
                              APPROVER_STATUS = (approval.APPROVER_STATUS == "Y" ? "Yes" : "No"),
                              APPROVE_DATE = approval.APPROVE_DATE,
                              IS_NOTICE_SERVE = exit.IS_NOTICE_SERVE == "Y" ? "Yes" : "No",
                              NOTICE_DAYS = exit.NOTICE_DAYS,
                              REASON_OF_LEAVING = exit.REASON_OF_LEAVING,
                              RESIGNATION_DATE = exit.RESIGNATION_DATE,
                              WORKFORCE_NAME = wf.EMP_NAME,
                              EMP_ID = wf.EMP_ID,
                              EXIT_DATE = exit.RESIGNATION_DATE,
                              EXIT_APPROVER_ID = approval.EXIT_APPROVER_ID,
                              USER_ID = user.USER_ID
                          }).ToList();
            return result;
        }


        public ExitManagementMetaData GetEmployeesExitApprovalDetails(Guid wf_id)
        {
            ExitManagementMetaData exitManagementMeta = new ExitManagementMetaData();
            try
            {
                var exitEmployeeData = _appEntity.TAB_WORKFORCE_EXIT.Where(x => x.WF_ID == wf_id).FirstOrDefault();
                if (exitEmployeeData != null)
                {
                    exitManagementMeta = new ExitManagementMetaData
                    {
                        IS_NOTICE_SERVE = exitEmployeeData.IS_NOTICE_SERVE == "Y" ? "Yes" : "No",
                        NOTICE_DAYS = exitEmployeeData.NOTICE_DAYS,
                        REASON_OF_LEAVING = exitEmployeeData.REASON_OF_LEAVING,
                        RESIGNATION_DATE = exitEmployeeData.RESIGNATION_DATE,
                        EXIT_DATE = exitEmployeeData.RESIGNATION_DATE.AddDays(exitEmployeeData.NOTICE_DAYS)
                    };
                    exitManagementMeta.WorkforceExitApprovers = (from wea in _appEntity.TAB_WORKFORCE_EXIT_APPROVER
                                                                 join user in _appEntity.TAB_LOGIN_MASTER on wea.APPROVE_BY equals user.USER_ID
                                                                 where wea.EXIT_ID == exitEmployeeData.EXIT_ID
                                                                 select new WorkforceExitApprover
                                                                 {
                                                                     EMP_ID = user.USER_LOGIN_ID,
                                                                     APPROVER_NAME = user.USER_NAME,
                                                                     APPROVER_STATUS = wea.APPROVER_STATUS,
                                                                     APPROVE_DATE = wea.APPROVE_DATE
                                                                 }).ToList();


                }
                exitManagementMeta.AssetMappingMetaDatas = GetAssignedAssetByEmpId(wf_id);
            }
            catch (Exception ex)
            {
                AccountRepository.InsertError_Log(ex.ToString(), "Page - AssetRepository.cs, Method - GetEmployeesExitApprovalDetails", HttpContext.Current.Request.Url.AbsolutePath, SessionHelper.Get<string>("LoginUserId"));
            }

            return exitManagementMeta;
        }

        public List<AssetMappingMetaData> GetAssignedAssetByEmpId(Guid wf_id)
        {
            List<AssetMappingMetaData> assetMappings = new List<AssetMappingMetaData>();
            try
            {
                assetMappings = (from aa in _appEntity.TAB_ASSET_ALLOCATION
                                 join am in _appEntity.TAB_ASSET_MASTER on aa.ASSET_ID equals am.ASSET_ID
                                 where aa.WF_ID == wf_id
                                 select new AssetMappingMetaData
                                 {
                                     ASSET_ALLOCATION_ID = aa.ASSET_ALLOCATION_ID,
                                     ASSET_ID = am.ASSET_ID,
                                     ASSET_NAME = am.ASSET_NAME,
                                     ASSET_LIFE = am.ASSET_LIFE,
                                     REFUNDABLE = am.REFUNDABLE == "Y" ? "Yes" : "No",
                                     IS_REFUNDABLE = am.REFUNDABLE == "Y" ? true : false,
                                     IS_ACTIVE = am.IS_ACTIVE,
                                     IS_REFOUND = aa.IS_REFOUND,
                                     QUANTITY = aa.QUANTITY,
                                     ALLOCATION_DATE = aa.ASSET_ASSIGN_DATE
                                 }).ToList();
            }
            catch (Exception ex)
            {
                AccountRepository.InsertError_Log(ex.ToString(), "Page - AssetRepository.cs, Method - GetAssignedAssetByEmpId", HttpContext.Current.Request.Url.AbsolutePath, SessionHelper.Get<string>("LoginUserId"));
            }

            return assetMappings;
        }

        public void AddAssetAllocation(AssetAllocationMetaDataForm assetAllocation)
        {
            List<TAB_ASSET_ALLOCATION> assetList = new List<TAB_ASSET_ALLOCATION>();
            try
            {
                DateTime dt = DateTime.Now;
                foreach (var asset in assetAllocation.ListMetaDatas)
                {
                    TAB_ASSET_ALLOCATION asset_allcoation = new TAB_ASSET_ALLOCATION()
                    {
                        ASSET_ALLOCATION_ID = Guid.NewGuid(),
                        ASSET_ASSIGN_BY = assetAllocation.ASSIGN_BY,
                        ASSET_ASSIGN_DATE = dt,
                        ASSET_ID = asset.ASSET_ID,
                        ASSET_TYPE = asset.ASSET_TYPE,
                        PURPOSE = asset.PURPOSE,
                        QUANTITY = asset.QUANTITY.Value,
                        DEPT_ID = assetAllocation.DEPT_ID,
                        WF_ID = assetAllocation.WF_ID,
                        COMPANY_ID = assetAllocation.COMPANY_ID,
                        IS_ACTIVE = "Y"
                    };
                    assetList.Add(asset_allcoation);
                }
                _appEntity.TAB_ASSET_ALLOCATION.AddRange(assetList);
                _appEntity.SaveChanges();
            }
            catch (Exception ex)
            {
                AccountRepository.InsertError_Log(ex.ToString(), "Page - AssetRepository.cs, Method - AddAssetAllocation", HttpContext.Current.Request.Url.AbsolutePath, SessionHelper.Get<string>("LoginUserId"));
            }
        }

        public List<AssetMasterMetaData> GetAssetByDepartmentId(Guid department_id, Guid sub_dept_id)
        {
            return _appEntity.TAB_ASSET_MASTER.Where(x => x.DEPARTMENT_ID == department_id && x.SUBDEPT_ID == sub_dept_id).Select(x =>
                      new AssetMasterMetaData
                      {
                          ASSET_ID = x.ASSET_ID,
                          ASSET_NAME = x.ASSET_NAME
                      }).ToList();
        }

        public List<AssetMappingMasterMetaData> GetAllAsset()
        {
            string createdBy = SessionHelper.Get<string>("LoginUserId");
            List<AssetMappingMasterMetaData> assetMappings = _appEntity.TAB_ASSET_MASTER.Join(
                    _appEntity.TAB_COMPANY_MASTER,
                    AM => AM.COMPANY_ID,
                    CM => CM.COMPANY_ID,
                    (AM, CM) => new AssetMappingMasterMetaData
                    {
                        ASSET_ID = AM.ASSET_ID,
                        ASSET_LIFE = AM.ASSET_LIFE,
                        ASSET_NAME = AM.ASSET_NAME,
                        REFUNDABLE = AM.REFUNDABLE == "Y" ? "Yes" : "No",
                        IS_ACTIVE = AM.IS_ACTIVE == "Y" ? "Yes" : "No",
                        COMPANY_ID = CM.COMPANY_ID,
                        CREATED_BY = createdBy,
                        COMPANY_NAME = CM.COMPANY_NAME
                    }
               ).Where(x => x.IS_ACTIVE == "Yes").ToList();
            return assetMappings;
        }

        public void Delete(Guid asset_id)
        {
            try
            {
                Core.TAB_ASSET_MASTER asset_result = _appEntity.TAB_ASSET_MASTER.Where(x => x.ASSET_ID == asset_id).FirstOrDefault();
                if (asset_result != null)
                {
                    asset_result.IS_ACTIVE = "N";
                    _appEntity.Entry(asset_result).State = EntityState.Modified;
                    _appEntity.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                AccountRepository.InsertError_Log(ex.ToString(), "Page - AssetRepository.cs, Method - Delete", HttpContext.Current.Request.Url.AbsolutePath, SessionHelper.Get<string>("LoginUserId"));
            }
        }

        public void Create(AssetMappingMasterMetaData asset)
        {
            try
            {
                if (asset.ASSET_ID == Guid.Empty)
                {
                    asset.ASSET_ID = Guid.NewGuid();
                }
                Wfm.App.Core.TAB_ASSET_MASTER obj = new Wfm.App.Core.TAB_ASSET_MASTER
                {
                    ASSET_ID = asset.ASSET_ID,
                    ASSET_LIFE = asset.ASSET_LIFE,
                    ASSET_NAME = asset.ASSET_NAME,
                    CREATED_BY = SessionHelper.Get<string>("LoginUserId"),
                    COMPANY_ID = asset.COMPANY_ID,
                    CREATED_DATE = DateTime.Now,
                    DEPARTMENT_ID = asset.DEPARTMENT_ID.Value,
                    SUBDEPT_ID = asset.SUBDEPT_ID.Value,
                    BUILDING_ID = asset.BUILDING_ID.Value,
                    IS_ACTIVE = "Y",
                    REFUNDABLE = asset.REFUNDABLE
                };

                _appEntity.TAB_ASSET_MASTER.Add(obj);
                _appEntity.SaveChanges();
            }
            catch (Exception ex)
            {
                AccountRepository.InsertError_Log(ex.ToString(), "Page - AssetRepository.cs, Method - Create", HttpContext.Current.Request.Url.AbsolutePath, SessionHelper.Get<string>("LoginUserId"));
            }
        }

        public void Update(AssetMappingMasterMetaData asset)
        {
            try
            {
                Core.TAB_ASSET_MASTER asset_result = _appEntity.TAB_ASSET_MASTER.Where(x => x.ASSET_ID == asset.ASSET_ID).FirstOrDefault();
                if (asset_result != null)
                {
                    asset_result.ASSET_LIFE = asset.ASSET_LIFE;
                    asset_result.ASSET_NAME = asset.ASSET_NAME;
                    asset_result.DEPARTMENT_ID = asset.DEPARTMENT_ID.Value;
                    asset_result.SUBDEPT_ID = asset.SUBDEPT_ID.Value;
                    asset_result.BUILDING_ID = asset.BUILDING_ID.Value;
                    asset_result.REFUNDABLE = asset.REFUNDABLE;
                    asset_result.IS_ACTIVE = asset.IS_ACTIVE == null ? "Y" : asset.IS_ACTIVE;
                    _appEntity.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                AccountRepository.InsertError_Log(ex.ToString(), "Page - AssetRepository.cs, Method - Update", HttpContext.Current.Request.Url.AbsolutePath, SessionHelper.Get<string>("LoginUserId"));
            }
        }

        public string TransferApprovalList(string deptId, string sub_dept_id, string status, string BUILDING_ID, string CurrentUser, string UserType)
        {
            string constr = ConfigurationManager.ConnectionStrings["db_con"].ConnectionString;
            DataSet ds = new DataSet();
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("Proc_TransferEmployeeList"))
                {
                    cmd.Connection = con;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@BUILDING_ID", BUILDING_ID);
                    cmd.Parameters.AddWithValue("@DEPT", deptId);
                    cmd.Parameters.AddWithValue("@SUB_DEPT", sub_dept_id);
                    cmd.Parameters.AddWithValue("@Status", status);
                    cmd.Parameters.AddWithValue("@CurrentUser", CurrentUser);
                    cmd.Parameters.AddWithValue("@UserType", UserType);
                    cmd.Parameters.AddWithValue("@OpCode", "41");
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        cmd.CommandTimeout = 1000;
                        sda.Fill(ds);
                    }
                    string json = "";
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        json += dr[0];
                    }
                    return json;
                }
            }

        }

        public string ApprovedTransfer(string TransferID, string ApprovedBy, string Remark)
        {
            try
            {
                string constr = ConfigurationManager.ConnectionStrings["db_con"].ConnectionString;
                DataSet ds = new DataSet();
                using (SqlConnection con = new SqlConnection(constr))
                {
                    using (SqlCommand cmd = new SqlCommand("Proc_TransferEmployeeList"))
                    {
                        cmd.Connection = con;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@TransferID", TransferID);
                        cmd.Parameters.AddWithValue("@ApprovedBy", ApprovedBy);
                        cmd.Parameters.AddWithValue("@Remark", Remark);
                        cmd.Parameters.AddWithValue("@OpCode", "42");
                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            cmd.CommandTimeout = 1000;
                            sda.Fill(ds);
                        }
                    }
                }
                return "true";
            }
            catch (Exception ex)
            {
                AccountRepository.InsertError_Log(ex.ToString(), "Page - AssetRepository.cs, Method - EmployeeTransfer", HttpContext.Current.Request.Url.AbsolutePath, SessionHelper.Get<string>("LoginUserId"));
                return "false";
            }
        }

        public string GetAllAssetAllocation_Details(string BUILDING_ID, string DEPT_ID, string SUBDEPT_ID, string WF_EMP_TYPE, string EMP_NAME)
        {
            string constr = ConfigurationManager.ConnectionStrings["db_con"].ConnectionString;
            DataSet ds = new DataSet();
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("Proc_AssetAllocation_List"))
                {
                    cmd.Connection = con;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@BUILDING_ID", BUILDING_ID);
                    cmd.Parameters.AddWithValue("@DEPT", DEPT_ID);
                    cmd.Parameters.AddWithValue("@SUB_DEPT", SUBDEPT_ID);
                    cmd.Parameters.AddWithValue("@WF_EMP_TYPE", WF_EMP_TYPE);
                    cmd.Parameters.AddWithValue("@EMP_NAME", EMP_NAME);
                    cmd.Parameters.AddWithValue("@OpCode", "41");
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        cmd.CommandTimeout = 1000;
                        sda.Fill(ds);
                    }
                    string json = "";
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        json += dr[0];
                    }
                    return json;
                }
            }
        }
    }
}