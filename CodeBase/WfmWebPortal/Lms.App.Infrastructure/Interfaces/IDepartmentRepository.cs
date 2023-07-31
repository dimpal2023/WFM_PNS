using System;
using System.Collections.Generic;
using Wfm.App.Core.Model;

namespace Wfm.App.Infrastructure.Interfaces
{
    public interface IDepartmentRepository
    {
        List<DepartmentMasterMetaData> GetAllDepartment();
        List<DepartmentMasterMetaData> GetAllDepartmentOnlyForAdmin(Guid companyId);
    }
}
