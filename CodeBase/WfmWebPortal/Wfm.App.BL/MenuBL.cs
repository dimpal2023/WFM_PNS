using System.Collections.Generic;
using System.Linq;
using Wfm.App.Core;
using Wfm.App.Core.Model;
using Wfm.App.Infrastructure.Interfaces;

namespace Wfm.App.BL
{
    public class MenuBL
    {
        private IBaseRepository baseRepository;

        public MenuBL(IBaseRepository baseRepo)
        {
            baseRepository = baseRepo;
        }

        public Dictionary<MenuMetaData, List<SubMenuMetaData>> GetUserMenu(System.Guid? user_id)
        {
            List<UserMenu_Result> userMenu = baseRepository.MenuRepo.GetUserMenu(user_id).ToList();

            List<UserMenu_Result> uniqueMenus = userMenu.Where(m => m.MENU_NAME != string.Empty).OrderBy(x => x.MENU_ORDER).ToList();
            Dictionary<MenuMetaData, List<SubMenuMetaData>> menusList = new Dictionary<MenuMetaData, List<SubMenuMetaData>>();

            foreach(UserMenu_Result m in uniqueMenus)
            {
                MenuMetaData menu = new MenuMetaData();
                menu.NAME = m.MENU_NAME;
                menu.ICONCLASS = m.ICONCLASS;
                menu.CONTROLLER_NAME = m.MENUCONTROLERNAME;

                if (menusList.Where(x => x.Key.NAME == menu.NAME).Count() == 0)
                {
                    List<SubMenuMetaData> subMenus = userMenu.Where(t => t.MENU_NAME == m.MENU_NAME).OrderBy(x => x.SUBMENU_ORDER).Select(o => new SubMenuMetaData {
                        ACTION_NAME = o.ACTION_NAME,
                        CONTROLLER_NAME = o.SUBMENUCONTROLERNAME,
                        ACTIVE = true,
                        DESCRIPTION = string.Empty,
                        NAME = o.SUBMENU_NAME                        
                    }).ToList();

                    menusList.Add(menu, subMenus);
                }
            }

            return menusList;
        }
    }
}
