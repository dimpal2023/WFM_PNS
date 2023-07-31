using System;
using Wfm.App.Infrastructure.Interfaces;

namespace Wfm.App.BL
{
    public class CompanyBL
    {
        public IBaseRepository baseRepository;
        public CompanyBL(IBaseRepository baseRepo)
        {
            baseRepository = baseRepo;
        }

        public string GetCompanyName(Guid id)
        {
            return baseRepository.CompanyRepo.GetComapnyName(id);
        }
        public string GetBUILDING(Guid id)
        {
            return baseRepository.CompanyRepo.GetBUILDING(id);
        }
    }
}
