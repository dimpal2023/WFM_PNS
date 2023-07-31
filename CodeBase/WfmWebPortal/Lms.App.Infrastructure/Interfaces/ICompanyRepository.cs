using System;

namespace Wfm.App.Infrastructure.Interfaces
{
    public interface ICompanyRepository
    {
        string GetComapnyName(Guid id);
    }
}
