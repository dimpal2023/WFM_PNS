using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Wfm.App.Common;
using Wfm.App.Core;
using Wfm.App.Core.Model;
using Wfm.App.Infrastructure.Interfaces;

namespace Wfm.App.Infrastructure.Repositories
{
    public class ToolTalkRepository : IToolTalkRepository
    {
        private ApplicationEntities _appEntity;

        public ToolTalkRepository()
        {
            _appEntity = new ApplicationEntities();
        }

        public void Create(ToolTalkMasterMetaData toolTalk)
        {
            try
            {
                if (toolTalk.ID == Guid.Empty)
                {
                    toolTalk.ID = Guid.NewGuid();
                }

                Wfm.App.Core.TAB_TOOL_TALK_MASTER obj = new Wfm.App.Core.TAB_TOOL_TALK_MASTER
                {
                    ID = toolTalk.ID,
                    DEPT_ID = toolTalk.DEPT_ID,
                    SUBDEPT_ID = toolTalk.SUBDEPT_ID,
                    ITEM_NAME = toolTalk.ITEM_NAME,
                    CREATED_DATE = DateTime.Now,
                    BUILDING_ID = toolTalk.BUILDING_ID
                };

                _appEntity.TAB_TOOL_TALK_MASTER.Add(obj);
                _appEntity.SaveChanges();
            }
            catch (Exception ex)
            {
                AccountRepository.InsertError_Log(ex.ToString(), "Page - ToolTalkRepository.cs, Method - Create", HttpContext.Current.Request.Url.AbsolutePath, SessionHelper.Get<string>("LoginUserId"));
            }
        }

        public ToolTalkMasterMetaData Find(Guid toolTalkId)
        {
            ToolTalkMasterMetaData tooltalk = null;
            try
            {

                var toolTalkObj = _appEntity.TAB_TOOL_TALK_MASTER.Where(x => x.ID == toolTalkId).FirstOrDefault();

                if (toolTalkObj != null)
                {
                    tooltalk = new ToolTalkMasterMetaData
                    {
                        ID = toolTalkId,
                        DEPT_ID = toolTalkObj.DEPT_ID,
                        SUBDEPT_ID = toolTalkObj.SUBDEPT_ID,
                        ITEM_NAME = toolTalkObj.ITEM_NAME,
                        BUILDING_ID = (Guid)toolTalkObj.BUILDING_ID,
                    };
                }
            }
            catch (Exception ex)
            {
                AccountRepository.InsertError_Log(ex.ToString(), "Page - ToolTalkRepository.cs, Method - Find", HttpContext.Current.Request.Url.AbsolutePath, SessionHelper.Get<string>("LoginUserId"));
            }

            return tooltalk;
        }

        public List<ToolTalkMasterMetaData> GetCheckListByDeptId(Guid deptId, Guid subDeptId)
        {
            List<ToolTalkMasterMetaData> checkListMasterMetaData = null;
            try
            {
                checkListMasterMetaData = _appEntity.TAB_TOOL_TALK_MASTER.Where(x => x.DEPT_ID == deptId && x.SUBDEPT_ID == subDeptId)
                   .Select(x => new ToolTalkMasterMetaData { ID = x.ID, ITEM_NAME = x.ITEM_NAME }).ToList();
            }
            catch (Exception ex)
            {
                AccountRepository.InsertError_Log(ex.ToString(), "Page - ToolTalkRepository.cs, Method - GetCheckListByDeptId", HttpContext.Current.Request.Url.AbsolutePath, SessionHelper.Get<string>("LoginUserId"));
            }
            return checkListMasterMetaData;
        }

        public int Delete(Guid id)
        {
            try
            {
                var configuredItem = _appEntity.TAB_TOOL_TALK_CONFIGURATION.Where(x => x.TOOL_TALK_ID == id).FirstOrDefault();

                if (configuredItem != null) return 0;

                Core.TAB_TOOL_TALK_MASTER tooltalk = _appEntity.TAB_TOOL_TALK_MASTER.Where(x => x.ID == id).FirstOrDefault();

                _appEntity.TAB_TOOL_TALK_MASTER.Remove(tooltalk);
                _appEntity.SaveChanges();
            }
            catch (Exception ex)
            {
                AccountRepository.InsertError_Log(ex.ToString(), "Page - ToolTalkRepository.cs, Method - Delete", HttpContext.Current.Request.Url.AbsolutePath, SessionHelper.Get<string>("LoginUserId"));
            }

            return 1;
        }

        public List<ToolTalkMasterMetaData> GetConfiguredCheckListBySubDeptId(Guid deptId, Guid subDeptId, Guid BUILDING_ID)
        {
            List<ToolTalkMasterMetaData> checkListMasterMetaData = null;
            try
            {
                //var configObj = _appEntity.TAB_TOOL_TALK_CONFIGURATION.Where(x => x.DEPT_ID == deptId && x.SUBDEPT_ID == subDeptId && x.BUILDING_ID == BUILDING_ID).FirstOrDefault();

                //if (configObj != null)
                //{
                //    var toolTalkItemIds = _appEntity.TAB_TOOL_TALK_CONFIG_DAILY_ITEMS.Where(x => x.CONFIG_DAILY_ID == configObj.ID).Select(x => x.TOOL_TALK_ID).ToList();
                //    checkListMasterMetaData = _appEntity.TAB_TOOL_TALK_MASTER.Where(x => toolTalkItemIds.Contains(x.ID))
                //  .Select(x => new ToolTalkMasterMetaData { ID = x.ID, ITEM_NAME = x.ITEM_NAME }).ToList();
                //}
                var Subdept = _appEntity.TAB_SUBDEPARTMENT_MASTER.Where(x => x.DEPT_ID == deptId
              && x.BUILDING_ID == BUILDING_ID).FirstOrDefault();

                if (subDeptId == new Guid("00000000-0000-0000-0000-000000000000"))
                {
                    subDeptId = Subdept.SUBDEPT_ID;
                }


                checkListMasterMetaData = _appEntity.TAB_TOOL_TALK_MASTER.Where(x => x.DEPT_ID == deptId
                && (x.SUBDEPT_ID == subDeptId)
                && x.BUILDING_ID == BUILDING_ID)
                   .Select(x => new ToolTalkMasterMetaData { ID = x.ID, ITEM_NAME = x.ITEM_NAME }).Distinct().OrderBy(x => x.ITEM_NAME).ToList();
            }
            catch (Exception ex)
            {
                AccountRepository.InsertError_Log(ex.ToString(), "Page - ToolTalkRepository.cs, Method - GetConfiguredCheckListBySubDeptId", HttpContext.Current.Request.Url.AbsolutePath, SessionHelper.Get<string>("LoginUserId"));
            }
            return checkListMasterMetaData;
        }

        public List<ToolTalkMasterMetaData> GetAllItems(string dept_id, string sub_dept_id, string BUILDING_ID)
        {
            List<ToolTalkMasterMetaData> toolTalks = new List<ToolTalkMasterMetaData>();
            Guid dept_ids = string.IsNullOrEmpty(dept_id) ? new Guid() : new Guid(dept_id);
            Guid sub_dept_ids = string.IsNullOrEmpty(sub_dept_id) ? new Guid() : new Guid(sub_dept_id);
            Guid BUILDING_IDs = string.IsNullOrEmpty(BUILDING_ID) ? new Guid() : new Guid(BUILDING_ID);
            try
            {
                toolTalks = (from ttm in _appEntity.TAB_TOOL_TALK_MASTER
                             join dm in _appEntity.TAB_DEPARTMENT_MASTER on ttm.DEPT_ID equals dm.DEPT_ID
                             join sdm in _appEntity.TAB_SUBDEPARTMENT_MASTER on ttm.SUBDEPT_ID equals sdm.SUBDEPT_ID
                             where (dept_id == "" || ttm.DEPT_ID == dept_ids)
                             && (sub_dept_id == "" || ttm.SUBDEPT_ID == sub_dept_ids)
                             && (BUILDING_ID == "" || ttm.BUILDING_ID == BUILDING_IDs)
                             select new ToolTalkMasterMetaData
                             {
                                 ID = ttm.ID,
                                 DEPT_ID = dm.DEPT_ID,
                                 DEPT_NAME = dm.DEPT_NAME,
                                 SUBDEPT_NAME = sdm.SUBDEPT_NAME,
                                 ITEM_NAME = ttm.ITEM_NAME
                             }
                         ).ToList();
            }
            catch (Exception ex)
            {
                AccountRepository.InsertError_Log(ex.ToString(), "Page - ToolTalkRepository.cs, Method - GetAllItems", HttpContext.Current.Request.Url.AbsolutePath, SessionHelper.Get<string>("LoginUserId"));
            }
            return toolTalks;
        }

        public void Update(ToolTalkMasterMetaData toolTalk)
        {
            Core.TAB_TOOL_TALK_MASTER toolTalkObj = _appEntity.TAB_TOOL_TALK_MASTER.Where(x => x.ID == toolTalk.ID).FirstOrDefault();

            try
            {
                if (toolTalkObj != null)
                {
                    toolTalkObj.DEPT_ID = toolTalk.DEPT_ID;
                    toolTalkObj.SUBDEPT_ID = toolTalk.SUBDEPT_ID;
                    toolTalkObj.ITEM_NAME = toolTalk.ITEM_NAME;
                    toolTalkObj.UPDATED_DATE = DateTime.Now;
                    toolTalkObj.BUILDING_ID = toolTalk.BUILDING_ID;

                    _appEntity.Entry(toolTalkObj).State = EntityState.Modified;
                    _appEntity.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                AccountRepository.InsertError_Log(ex.ToString(), "Page - ToolTalkRepository.cs, Method - Update", HttpContext.Current.Request.Url.AbsolutePath, SessionHelper.Get<string>("LoginUserId"));
            }
        }

        public int AddConfiguration(ToolTalkConfigurationMetaData toolTalkConfigure)
        {
            try
            {
                if (toolTalkConfigure != null)
                {
                    var configExit = _appEntity.TAB_TOOL_TALK_CONFIGURATION.Where(x => x.DEPT_ID == toolTalkConfigure.DEPT_ID && x.SUBDEPT_ID == toolTalkConfigure.SUBDEPT_ID && x.SHIFT_ID == toolTalkConfigure.SHIFT_ID).FirstOrDefault();

                    if (configExit != null) return 0;

                    Wfm.App.Core.TAB_TOOL_TALK_CONFIGURATION obj = new Wfm.App.Core.TAB_TOOL_TALK_CONFIGURATION
                    {
                        ID = Guid.NewGuid(),
                        DEPT_ID = toolTalkConfigure.DEPT_ID,
                        SUBDEPT_ID = toolTalkConfigure.SUBDEPT_ID,
                        SHIFT_ID = toolTalkConfigure.SHIFT_ID,
                        TOOL_TALK_ID = toolTalkConfigure.TOOL_TALK_CHECK_LIST.FirstOrDefault().TOOL_TALK_ID,
                        CREATED_DATE = DateTime.Now,
                        CREATED_BY = SessionHelper.Get<string>("LoginUserId")
                    };

                    foreach (var item in toolTalkConfigure.TOOL_TALK_CHECK_LIST)
                    {
                        Core.TAB_TOOL_TALK_CONFIG_DAILY_ITEMS configItem = new TAB_TOOL_TALK_CONFIG_DAILY_ITEMS
                        {
                            ID = Guid.NewGuid(),
                            TOOL_TALK_ID = item.TOOL_TALK_ID,
                            CONFIG_DAILY_ID = obj.ID,
                            CREATED_DATE = DateTime.Now,
                            CREATED_BY = SessionHelper.Get<string>("LoginUserId")
                        };

                        _appEntity.TAB_TOOL_TALK_CONFIG_DAILY_ITEMS.Add(configItem);
                    }

                    _appEntity.TAB_TOOL_TALK_CONFIGURATION.Add(obj);
                    _appEntity.SaveChanges();
                }

            }
            catch (Exception ex)
            {
                AccountRepository.InsertError_Log(ex.ToString(), "Page - ToolTalkRepository.cs, Method - AddConfiguration", HttpContext.Current.Request.Url.AbsolutePath, SessionHelper.Get<string>("LoginUserId"));
            }

            return 1;
        }

        public ToolTalkConfigurationMetaData FindConfiguration(Guid toolTalkId)
        {
            ToolTalkConfigurationMetaData configuredItems = new ToolTalkConfigurationMetaData();
            try
            {
                var configuredItem = _appEntity.TAB_TOOL_TALK_CONFIGURATION.Where(x => x.ID == toolTalkId).FirstOrDefault();

                if (configuredItem != null)
                {
                    configuredItems.ID = configuredItem.ID;
                    configuredItems.DEPT_ID = configuredItem.DEPT_ID;
                    configuredItems.SUBDEPT_ID = configuredItem.SUBDEPT_ID;
                    configuredItems.SHIFT_ID = configuredItem.SHIFT_ID;

                    var dailyItems = _appEntity.TAB_TOOL_TALK_CONFIG_DAILY_ITEMS.Where(x => x.CONFIG_DAILY_ID == configuredItem.ID).ToList();

                    configuredItems.TOOL_TALK_CHECK_LIST = new List<ToolTalkCheckList>();

                    foreach (var configuredGroup in dailyItems)
                    {
                        ToolTalkCheckList ttcl = new ToolTalkCheckList();
                        ttcl.ID = configuredGroup.ID;
                        ttcl.TOOL_TALK_ID = configuredGroup.TOOL_TALK_ID;
                        ttcl.CHECK = true;
                        ttcl.ITEM_NAME = _appEntity.TAB_TOOL_TALK_MASTER.Where(x => x.ID == ttcl.TOOL_TALK_ID).FirstOrDefault().ITEM_NAME;

                        configuredItems.TOOL_TALK_CHECK_LIST.Add(ttcl);
                    }
                }

                var allchecklists = _appEntity.TAB_TOOL_TALK_MASTER.Where(x => x.DEPT_ID == configuredItem.DEPT_ID && x.SUBDEPT_ID == configuredItem.SUBDEPT_ID).ToList();

                if (allchecklists != null)
                {
                    foreach (var item in allchecklists)
                    {
                        var existItem = configuredItems.TOOL_TALK_CHECK_LIST.Where(x => x.TOOL_TALK_ID == item.ID).FirstOrDefault();

                        if (existItem == null)
                        {
                            ToolTalkCheckList ttcl = new ToolTalkCheckList();
                            ttcl.ID = Guid.Empty;
                            ttcl.TOOL_TALK_ID = item.ID;
                            ttcl.CHECK = false;
                            ttcl.ITEM_NAME = item.ITEM_NAME;

                            configuredItems.TOOL_TALK_CHECK_LIST.Add(ttcl);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                AccountRepository.InsertError_Log(ex.ToString(), "Page - ToolTalkRepository.cs, Method - Find", HttpContext.Current.Request.Url.AbsolutePath, SessionHelper.Get<string>("LoginUserId"));
            }

            return configuredItems;
        }

        public void UpdateConfiguration(ToolTalkConfigurationMetaData configuredItem)
        {
            try
            {
                if (configuredItem != null)
                {
                    var dailyCheckListItemObj = _appEntity.TAB_TOOL_TALK_CONFIGURATION.Where(x => x.ID == configuredItem.ID).FirstOrDefault();

                    if (dailyCheckListItemObj != null)
                    {
                        dailyCheckListItemObj.DEPT_ID = configuredItem.DEPT_ID;
                        dailyCheckListItemObj.SUBDEPT_ID = configuredItem.SUBDEPT_ID;
                        dailyCheckListItemObj.SHIFT_ID = configuredItem.SHIFT_ID;
                        dailyCheckListItemObj.UPDATED_DATE = DateTime.Now;
                        dailyCheckListItemObj.UPDATED_BY = SessionHelper.Get<string>("LoginUserId");

                        var dailyItems = _appEntity.TAB_TOOL_TALK_CONFIG_DAILY_ITEMS.Where(x => x.CONFIG_DAILY_ID == dailyCheckListItemObj.ID).ToList();

                        foreach (var item in dailyItems)
                        {
                            var data = configuredItem.TOOL_TALK_CHECK_LIST.Where(x => x.ID == item.ID).FirstOrDefault();
                            if (data != null)
                            {
                                item.TOOL_TALK_ID = data.TOOL_TALK_ID;
                                item.UPDATED_BY = dailyCheckListItemObj.UPDATED_BY;
                                item.UPDATED_DATE = DateTime.Now;
                            }
                            else
                            {
                                _appEntity.TAB_TOOL_TALK_CONFIG_DAILY_ITEMS.Remove(item);
                            }
                        }

                        //add newly selected items
                        var d = _appEntity.TAB_TOOL_TALK_CONFIG_DAILY_ITEMS.Select(x => x.ID).ToList();
                        var newItems = configuredItem.TOOL_TALK_CHECK_LIST.Where(x => !d.Contains(x.ID));
                        foreach (var item in newItems)
                        {
                            Core.TAB_TOOL_TALK_CONFIG_DAILY_ITEMS configItem = new TAB_TOOL_TALK_CONFIG_DAILY_ITEMS
                            {
                                ID = Guid.NewGuid(),
                                TOOL_TALK_ID = item.TOOL_TALK_ID,
                                CONFIG_DAILY_ID = dailyCheckListItemObj.ID,
                                CREATED_DATE = DateTime.Now,
                                CREATED_BY = SessionHelper.Get<string>("LoginUserId")
                            };

                            _appEntity.TAB_TOOL_TALK_CONFIG_DAILY_ITEMS.Add(configItem);
                        }

                        _appEntity.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                AccountRepository.InsertError_Log(ex.ToString(), "Page - ToolTalkRepository.cs, Method - UpdateConfiguration", HttpContext.Current.Request.Url.AbsolutePath, SessionHelper.Get<string>("LoginUserId"));
            }
        }

        public void DeleteConfiguration(ToolTalkConfigurationMetaData configuredItem)
        {
            try
            {
                var configuredDbItem = _appEntity.TAB_TOOL_TALK_CONFIGURATION.Where(x => x.ID == configuredItem.ID).FirstOrDefault();

                if (configuredDbItem != null)
                {
                    var dailyItems = _appEntity.TAB_TOOL_TALK_CONFIG_DAILY_ITEMS.Where(x => x.CONFIG_DAILY_ID == configuredDbItem.ID).ToList();

                    foreach (var item in dailyItems)
                    {
                        _appEntity.TAB_TOOL_TALK_CONFIG_DAILY_ITEMS.Remove(item);
                    }
                }

                _appEntity.TAB_TOOL_TALK_CONFIGURATION.Remove(configuredDbItem);
                _appEntity.SaveChanges();
            }
            catch (Exception ex)
            {
                AccountRepository.InsertError_Log(ex.ToString(), "Page - ToolTalkRepository.cs, Method - DeleteConfiguration", HttpContext.Current.Request.Url.AbsolutePath, SessionHelper.Get<string>("LoginUserId"));
            }
        }

        public List<ToolTalkConfigurationMetaData> ConfiguredCheckLists()
        {
            List<ToolTalkConfigurationMetaData> configuredItems = new List<ToolTalkConfigurationMetaData>();

            try
            {
                var configuredCheckListItems = _appEntity.TAB_TOOL_TALK_CONFIGURATION.ToList();

                foreach (var configuredGroup in configuredCheckListItems)
                {
                    ToolTalkConfigurationMetaData ttc = new ToolTalkConfigurationMetaData();
                    ttc.ID = configuredGroup.ID;
                    ttc.DEPT_ID = configuredGroup.DEPT_ID;
                    ttc.SUBDEPT_ID = configuredGroup.SUBDEPT_ID;
                    ttc.SHIFT_ID = configuredGroup.SHIFT_ID;

                    ttc.DEPT_NAME = _appEntity.TAB_DEPARTMENT_MASTER.Where(x => x.DEPT_ID == ttc.DEPT_ID).FirstOrDefault().DEPT_NAME;
                    ttc.SUBDEPT_NAME = _appEntity.TAB_SUBDEPARTMENT_MASTER.Where(x => x.SUBDEPT_ID == ttc.SUBDEPT_ID).FirstOrDefault().SUBDEPT_NAME;
                    ttc.SHIFT_NAME = _appEntity.TAB_SHIFT_MASTER.Where(x => x.SHIFT_ID == ttc.SHIFT_ID).FirstOrDefault().SHIFT_NAME;

                    ttc.TOOL_TALK_CHECK_LIST = new List<ToolTalkCheckList>();

                    var dailyItems = _appEntity.TAB_TOOL_TALK_CONFIG_DAILY_ITEMS.Where(x => x.CONFIG_DAILY_ID == configuredGroup.ID).ToList();

                    foreach (var obj in dailyItems)
                    {
                        ToolTalkCheckList ttcl = new ToolTalkCheckList();
                        ttcl.ID = obj.ID;
                        ttcl.TOOL_TALK_ID = obj.TOOL_TALK_ID;
                        ttcl.CHECK = true;
                        ttcl.ITEM_NAME = _appEntity.TAB_TOOL_TALK_MASTER.Where(x => x.ID == ttcl.TOOL_TALK_ID).FirstOrDefault().ITEM_NAME;

                        ttc.TOOL_TALK_CHECK_LIST.Add(ttcl);
                    }

                    configuredItems.Add(ttc);
                }
            }
            catch (Exception ex)
            {
                AccountRepository.InsertError_Log(ex.ToString(), "Page - ToolTalkRepository.cs, Method - ConfiguredCheckLists", HttpContext.Current.Request.Url.AbsolutePath, SessionHelper.Get<string>("LoginUserId"));
            }

            return configuredItems;
        }

        public int CreateDailyCheckList(ToolTalkDailyCheckListMetaData toolTalkCheckList)
        {
            try
            {
                if (toolTalkCheckList != null)
                {
                    //var checkListObj = _appEntity.TAB_TOOL_TALK_DAILY_CHECKLIST.Where(x => x.DEPT_ID == toolTalkCheckList.DEPT_ID && x.SUBDEPT_ID == toolTalkCheckList.SUBDEPT_ID && x.WF_ID == x.WF_ID).FirstOrDefault();

                    //if (checkListObj != null) return 1;

                    Core.TAB_TOOL_TALK_DAILY_CHECKLIST obj = new Core.TAB_TOOL_TALK_DAILY_CHECKLIST
                    {
                        ID = Guid.NewGuid(),
                        BUILDING_ID = toolTalkCheckList.BUILDING_ID,
                        DEPT_ID = toolTalkCheckList.DEPT_ID,
                        //SUBDEPT_ID = toolTalkCheckList.SUBDEPT_ID,
                        SHIFT_AUTOID = toolTalkCheckList.SHIFT_AUTOID,
                        DELIVERED_BY = toolTalkCheckList.DELIVERED_BY,
                        //WF_ID = toolTalkCheckList.WF_ID,
                        EMP_NAME = toolTalkCheckList.EMP_NAME.TrimEnd(','),
                        TOOL_TALK_ID = toolTalkCheckList.TOOL_TALK_CHECK_LIST.FirstOrDefault().ID,
                        //DATE = toolTalkCheckList.DATE,
                        DATE = DateTime.Now,
                        CREATED_DATE = DateTime.Now,
                        CREATED_BY = SessionHelper.Get<string>("LoginUserId")
                    };

                    _appEntity.TAB_TOOL_TALK_DAILY_CHECKLIST.Add(obj);

                    foreach (var item in toolTalkCheckList.TOOL_TALK_CHECK_LIST)
                    {
                        Core.TAB_TOOL_TALK_CONFIG_DAILY_ITEMS configDailyItems = new TAB_TOOL_TALK_CONFIG_DAILY_ITEMS
                        {
                            ID = Guid.NewGuid(),
                            TOOL_TALK_ID = item.TOOL_TALK_ID,
                            CONFIG_DAILY_ID = obj.ID,
                            CREATED_DATE = DateTime.Now,
                            CREATED_BY = obj.CREATED_BY
                        };

                        _appEntity.TAB_TOOL_TALK_CONFIG_DAILY_ITEMS.Add(configDailyItems);
                    }

                    _appEntity.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                AccountRepository.InsertError_Log(ex.ToString(), "Page - ToolTalkRepository.cs, Method - CreateDailyCheckList", HttpContext.Current.Request.Url.AbsolutePath, SessionHelper.Get<string>("LoginUserId"));
                return 0;
            }
            return 2;
        }

        public List<ToolTalkDailyCheckListMetaData> GetAllDailyCheckLists(Guid deptId, Guid subDeptId, Guid BUILDING_ID)
        {
            List<ToolTalkDailyCheckListMetaData> dailyCheckLists = new List<ToolTalkDailyCheckListMetaData>();

            try
            {

                var configuredItems = _appEntity.TAB_TOOL_TALK_DAILY_CHECKLIST.Where(
                    x => (x.DEPT_ID == deptId || deptId == new Guid("00000000-0000-0000-0000-000000000000"))
                    && (x.SUBDEPT_ID == subDeptId || subDeptId == new Guid("00000000-0000-0000-0000-000000000000"))
                    && (x.BUILDING_ID == BUILDING_ID)
                    ).OrderByDescending(x => x.DATE).ToList();
                foreach (var configuredGroup in configuredItems)
                {
                    ToolTalkDailyCheckListMetaData ttdc = new ToolTalkDailyCheckListMetaData();
                    ttdc.ID = configuredGroup.ID;
                    ttdc.DEPT_ID = configuredGroup.DEPT_ID;
                    ttdc.SUBDEPT_ID = configuredGroup.SUBDEPT_ID;
                    ttdc.WF_ID = configuredGroup.WF_ID;
                    ttdc.TOOL_TALK_ID = configuredGroup.TOOL_TALK_ID;
                    ttdc.DATE = configuredGroup.DATE;
                    ttdc.DELIVERED_BY = configuredGroup.DELIVERED_BY;

                    ttdc.DEPT_NAME = _appEntity.TAB_DEPARTMENT_MASTER.Where(x => x.DEPT_ID == ttdc.DEPT_ID).FirstOrDefault().DEPT_NAME;
                    //ttdc.SUBDEPT_NAME = _appEntity.TAB_SUBDEPARTMENT_MASTER.Where(x => x.SUBDEPT_ID == ttdc.SUBDEPT_ID).FirstOrDefault().SUBDEPT_NAME;
                    //ttdc.EMP_NAME = _appEntity.TAB_WORKFORCE_MASTER.Where(x => x.WF_ID == ttdc.WF_ID).FirstOrDefault().EMP_NAME;
                    ttdc.EMP_NAME = configuredGroup.EMP_NAME;

                    ttdc.TOOL_TALK_CHECK_LIST = new List<ToolTalkCheckList>();

                    var dailyItems = _appEntity.TAB_TOOL_TALK_CONFIG_DAILY_ITEMS.Where(x => x.CONFIG_DAILY_ID == configuredGroup.ID).ToList();

                    foreach (var obj in dailyItems)
                    {
                        ToolTalkCheckList ttcl = new ToolTalkCheckList();
                        ttcl.ID = obj.ID;
                        ttcl.TOOL_TALK_ID = obj.TOOL_TALK_ID;
                        ttcl.CHECK = true;
                        ttcl.ITEM_NAME = _appEntity.TAB_TOOL_TALK_MASTER.Where(x => x.ID == ttcl.TOOL_TALK_ID).FirstOrDefault().ITEM_NAME;

                        ttdc.TOOL_TALK_CHECK_LIST.Add(ttcl);
                    }

                    dailyCheckLists.Add(ttdc);
                }
            }
            catch (Exception ex)
            {
                AccountRepository.InsertError_Log(ex.ToString(), "Page - ToolTalkRepository.cs, Method - GetAllDailyCheckLists", HttpContext.Current.Request.Url.AbsolutePath, SessionHelper.Get<string>("LoginUserId"));
            }

            return dailyCheckLists;
        }

        public void DeleteDailyCheckList(ToolTalkDailyCheckListMetaData dailyCheckListItem)
        {
            try
            {
                var dailyCheckListItemObj = _appEntity.TAB_TOOL_TALK_DAILY_CHECKLIST.Where(x => x.ID == dailyCheckListItem.ID).FirstOrDefault();

                if (dailyCheckListItemObj != null)
                {
                    var dailyCheckLists = _appEntity.TAB_TOOL_TALK_CONFIG_DAILY_ITEMS.Where(x => x.CONFIG_DAILY_ID == dailyCheckListItemObj.ID).ToList();

                    foreach (var item in dailyCheckLists)
                    {
                        //Core.TAB_TOOL_TALK_CONFIG_DAILY_ITEMS dailyitem = _appEntity.TAB_TOOL_TALK_CONFIG_DAILY_ITEMS.Where(x => x.ID == item.ID).FirstOrDefault();

                        _appEntity.TAB_TOOL_TALK_CONFIG_DAILY_ITEMS.Remove(item);
                        _appEntity.SaveChanges();
                    }

                    _appEntity.TAB_TOOL_TALK_DAILY_CHECKLIST.Remove(dailyCheckListItemObj);
                    _appEntity.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                AccountRepository.InsertError_Log(ex.ToString(), "Page - ToolTalkRepository.cs, Method - DeleteDailyCheckList", HttpContext.Current.Request.Url.AbsolutePath, SessionHelper.Get<string>("LoginUserId"));
            }
        }

        public void UpdateDailyCheckList(ToolTalkDailyCheckListMetaData dailyCheckListItem)
        {
            try
            {
                if (dailyCheckListItem != null)
                {
                    var dailyCheckListItemObj = _appEntity.TAB_TOOL_TALK_DAILY_CHECKLIST.Where(x => x.ID == dailyCheckListItem.ID).FirstOrDefault();

                    if (dailyCheckListItemObj != null)
                    {
                        //dailyCheckListItemObj.WF_ID = dailyCheckListItem.WF_ID;
                        dailyCheckListItemObj.EMP_NAME = dailyCheckListItem.EMP_NAME;
                        dailyCheckListItemObj.DELIVERED_BY = dailyCheckListItem.DELIVERED_BY;
                        //dailyCheckListItemObj.DATE = dailyCheckListItem.DATE;
                        dailyCheckListItemObj.UPDATED_DATE = DateTime.Now;
                        dailyCheckListItemObj.UPDATED_BY = SessionHelper.Get<string>("LoginUserId");

                        var dailyItems = _appEntity.TAB_TOOL_TALK_CONFIG_DAILY_ITEMS.Where(x => x.CONFIG_DAILY_ID == dailyCheckListItemObj.ID).ToList();

                        foreach (var item in dailyItems)
                        {
                            var data = dailyCheckListItem.TOOL_TALK_CHECK_LIST.Where(x => x.ID == item.ID).FirstOrDefault();
                            if (data != null)
                            {
                                item.TOOL_TALK_ID = data.TOOL_TALK_ID;
                                item.UPDATED_BY = dailyCheckListItemObj.UPDATED_BY;
                                item.UPDATED_DATE = DateTime.Now;
                            }
                            else
                            {
                                _appEntity.TAB_TOOL_TALK_CONFIG_DAILY_ITEMS.Remove(item);
                            }
                        }

                        //add newly selected items
                        var d = _appEntity.TAB_TOOL_TALK_CONFIG_DAILY_ITEMS.Where(x => x.CONFIG_DAILY_ID == dailyCheckListItemObj.ID).Select(x => x.ID).ToList();
                        var newItems = dailyCheckListItem.TOOL_TALK_CHECK_LIST.Where(x => !d.Contains(x.ID));
                        foreach (var item in newItems)
                        {
                            Core.TAB_TOOL_TALK_CONFIG_DAILY_ITEMS configItem = new TAB_TOOL_TALK_CONFIG_DAILY_ITEMS
                            {
                                ID = Guid.NewGuid(),
                                TOOL_TALK_ID = item.TOOL_TALK_ID,
                                CONFIG_DAILY_ID = dailyCheckListItemObj.ID,
                                CREATED_DATE = DateTime.Now,
                                CREATED_BY = SessionHelper.Get<string>("LoginUserId")
                            };

                            _appEntity.TAB_TOOL_TALK_CONFIG_DAILY_ITEMS.Add(configItem);
                        }

                        _appEntity.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                AccountRepository.InsertError_Log(ex.ToString(), "Page - ToolTalkRepository.cs, Method - UpdateDailyCheckList", HttpContext.Current.Request.Url.AbsolutePath, SessionHelper.Get<string>("LoginUserId"));
            }
        }

        public ToolTalkDailyCheckListMetaData FindDailyCheckList(Guid dailyCheckListId)
        {
            ToolTalkDailyCheckListMetaData dailyCheckListItems = new ToolTalkDailyCheckListMetaData();
            try
            {
                var dailyCheckListItem = _appEntity.TAB_TOOL_TALK_DAILY_CHECKLIST.Where(x => x.ID == dailyCheckListId).FirstOrDefault();

                var configDailyItems = _appEntity.TAB_TOOL_TALK_CONFIG_DAILY_ITEMS.Where(x => x.CONFIG_DAILY_ID == dailyCheckListItem.ID).ToList();

                if (dailyCheckListItem != null)
                {
                    dailyCheckListItems.ID = dailyCheckListItem.ID;
                    dailyCheckListItems.BUILDING_ID = (Guid)dailyCheckListItem.BUILDING_ID;
                    dailyCheckListItems.SHIFT_AUTOID = (int)dailyCheckListItem.SHIFT_AUTOID;
                    dailyCheckListItems.DELIVERED_BY = dailyCheckListItem.DELIVERED_BY;
                    dailyCheckListItems.EMP_NAME = dailyCheckListItem.EMP_NAME;
                    dailyCheckListItems.DEPT_ID = dailyCheckListItem.DEPT_ID;
                    dailyCheckListItems.SUBDEPT_ID = dailyCheckListItem.SUBDEPT_ID;
                    dailyCheckListItems.WF_ID = dailyCheckListItem.WF_ID;
                    dailyCheckListItems.TOOL_TALK_ID = dailyCheckListItem.TOOL_TALK_ID;
                    dailyCheckListItems.DATE = dailyCheckListItem.DATE;
                    var workforceObj = _appEntity.TAB_WORKFORCE_MASTER.Where(x => x.WF_ID == dailyCheckListItem.WF_ID).FirstOrDefault();
                    //dailyCheckListItems.EMP_NAME = string.Concat(workforceObj.EMP_NAME, " - ", workforceObj.EMP_ID);
                    //dailyCheckListItems.WF_EMP_TYPE = workforceObj.WF_EMP_TYPE;

                    dailyCheckListItems.TOOL_TALK_CHECK_LIST = new List<ToolTalkCheckList>();

                    //adding selected daily check lisy item
                    foreach (var item in configDailyItems)
                    {
                        ToolTalkCheckList ttcl = new ToolTalkCheckList();
                        ttcl.ID = item.ID;
                        ttcl.TOOL_TALK_ID = item.TOOL_TALK_ID;
                        ttcl.CHECK = true;
                        ttcl.ITEM_NAME = _appEntity.TAB_TOOL_TALK_MASTER.Where(x => x.ID == ttcl.TOOL_TALK_ID).FirstOrDefault().ITEM_NAME;

                        dailyCheckListItems.TOOL_TALK_CHECK_LIST.Add(ttcl);
                    }
                }

                //adding remaing expect selected check list item
                var allchecklists = _appEntity.TAB_TOOL_TALK_MASTER.Where(x => x.DEPT_ID == dailyCheckListItem.DEPT_ID && x.SUBDEPT_ID == dailyCheckListItem.SUBDEPT_ID).ToList();

                if (allchecklists != null)
                {
                    foreach (var item in allchecklists)
                    {
                        var existItem = dailyCheckListItems.TOOL_TALK_CHECK_LIST.Where(x => x.TOOL_TALK_ID == item.ID).FirstOrDefault();

                        if (existItem == null)
                        {
                            ToolTalkCheckList ttcl = new ToolTalkCheckList();
                            ttcl.ID = Guid.Empty;
                            ttcl.TOOL_TALK_ID = item.ID;
                            ttcl.CHECK = false;
                            ttcl.ITEM_NAME = item.ITEM_NAME;

                            dailyCheckListItems.TOOL_TALK_CHECK_LIST.Add(ttcl);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                AccountRepository.InsertError_Log(ex.ToString(), "Page - ToolTalkRepository.cs, Method - FindDailyCheckList", HttpContext.Current.Request.Url.AbsolutePath, SessionHelper.Get<string>("LoginUserId"));
            }

            return dailyCheckListItems;
        }


    }
}
