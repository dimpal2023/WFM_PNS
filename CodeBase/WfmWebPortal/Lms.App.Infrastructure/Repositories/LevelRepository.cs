using System.Collections.Generic;
using System.Linq;
using Wfm.App.Core;
using Wfm.App.Core.Model;
using Wfm.App.Infrastructure.Interfaces;

namespace Wfm.App.Infrastructure.Repositories
{
    public class LevelRepository : ILevelRepository
    {
        private ApplicationEntities _appEntity;

        public LevelRepository()
        {
            _appEntity = new ApplicationEntities();
        }

        public List<LevelMasterMetaData> GetAll()
        {
            List<LevelMasterMetaData> levelMasterMetaDatas = _appEntity.TAB_LEVEL_MASTER.Select(x => new LevelMasterMetaData
            {
                LEVEL_ID = x.LEVEL_ID,
                LEVEL_NAME = x.LEVEL_NAME
            }).OrderBy(x=>x.LEVEL_NAME).ToList();
            return levelMasterMetaDatas;
        }
    }
}
