using System;
using System.Collections.Generic;
using Wfm.App.Core.Model;

namespace Wfm.App.Infrastructure.Interfaces
{
    public interface ISubDepartmentRepository
    {
        List<SubDepartmentMasterMetaData> GetAllSubDepartment();
        List<SubDepartmentMasterMetaData> GetAllSubDepartmentOnlyForAdmin(Guid companyId);
        List<SubDepartmentMasterMetaData> GetSubDepartmentByCompanyId(Guid companyId);
        List<SubDepartmentMasterMetaData> GetSubDepartmentsByUserId(Guid comp_id, Guid user_id);
        List<SubDepartmentMasterMetaData> GetSubDepartmentsByDeptId(Guid comp_id, Guid user_id, Guid dept_id);
    }
}
