using System.Collections.Generic;
using Wfm.App.Core.Model;
using Wfm.App.Infrastructure.Interfaces;

namespace Wfm.App.BL
{
    public class LevelBL
    {
        private IBaseRepository baseRepository;        

        public LevelBL(IBaseRepository baseRepo)
        {
            baseRepository = baseRepo;
        }

        public List<LevelMasterMetaData> GetAllLevelMaster()
        {
            return baseRepository.LevelRepo.GetAll();
        }
    }
}
