using System;
using System.Collections.Generic;
using Wfm.App.Core.Model;
using Wfm.App.Infrastructure.Interfaces;

namespace Wfm.App.BL
{
    public class ShiftBL
    {
        public IBaseRepository baseRepository;
        public ShiftBL(IBaseRepository baseRepo)
        {
            baseRepository = baseRepo;
        }

        public List<ShiftMasterMetaData> GetAllItems()
        {
            return baseRepository.ShiftRepo.GetAllItems();
        }

        public void Create(ShiftMasterMetaData gatePass)
        {
            baseRepository.ShiftRepo.Create(gatePass);
        }

        public ShiftMasterMetaData Find(Guid shift_id)
        {
            return baseRepository.ShiftRepo.Find(shift_id);
        }

        public void Update(ShiftMasterMetaData shift)
        {
            baseRepository.ShiftRepo.Update(shift);
        }

        public void Delete(Guid shift_Id)
        {
            baseRepository.ShiftRepo.Delete(shift_Id);
        }

        public List<CompanyMasterMetaData> GetCompanies()
        {
            return baseRepository.ShiftRepo.GetCompanies();
        }
    }
}
