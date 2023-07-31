using System.Collections.Generic;
using Wfm.App.Core;

namespace Wfm.App.Infrastructure.Interfaces
{
    public interface IMenuRepository
    {
        List<UserMenu_Result> GetUserMenu(System.Guid? user_id);
    }
}
