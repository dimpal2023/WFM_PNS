using System.Collections.Generic;
using Wfm.App.Core.Model;

namespace Wfm.App.Infrastructure.Interfaces
{
    public interface ILevelRepository
    {
        List<LevelMasterMetaData> GetAll();
    }
}
