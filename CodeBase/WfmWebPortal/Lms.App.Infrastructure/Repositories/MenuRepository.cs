using System;
using System.Collections.Generic;
using System.Linq;
using Wfm.App.Core;
using Wfm.App.Infrastructure.Interfaces;

namespace Wfm.App.Infrastructure.Repositories
{
    public class MenuRepository : IMenuRepository, System.IDisposable
    {
        private ApplicationEntities applicationEntities;

        public MenuRepository()
        {
            applicationEntities = new ApplicationEntities();
        }

        public List<UserMenu_Result> GetUserMenu(System.Guid? user_id)
        {

            return applicationEntities.USP_USER_MENU(user_id).ToList();
        }
        
        public List<TAB_MENU> GetAllMenu()
        {
            return applicationEntities.TAB_MENU.ToList();
        }

        public void Dispose()
        {
            
        }
    }
}
