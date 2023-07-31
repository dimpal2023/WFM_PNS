using System;
using System.Linq;
using Wfm.App.Core;
using Wfm.App.Infrastructure.Interfaces;

namespace Wfm.App.Infrastructure.Repositories
{
    public class CompanyRepository : ICompanyRepository
    {
        private ApplicationEntities _applicationEntity;

        public CompanyRepository()
        {
            _applicationEntity = new ApplicationEntities();
        }

        public string GetComapnyName(Guid id)
        {
            return _applicationEntity.TAB_COMPANY_MASTER.Where(x => x.COMPANY_ID == id).FirstOrDefault().COMPANY_NAME;
        } 
        public string GetBUILDING(Guid id)
        {
            return _applicationEntity.TAB_LOGIN_MASTER.Where(x => x.USER_ID == id).FirstOrDefault().BUILDING_ID.ToString();
        }
    }
}
