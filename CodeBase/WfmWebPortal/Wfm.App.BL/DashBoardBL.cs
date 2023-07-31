using System;
using System.Collections.Generic;
using Wfm.App.Core.Model;
using Wfm.App.Infrastructure.Interfaces;
using Wfm.App.Infrastructure.Repositories;

namespace Wfm.App.BL
{
    public class DashBoardBL
    {
        private IBaseRepository baseRepository;

        public DashBoardBL(IBaseRepository baseRepo)
        {
            baseRepository = baseRepo;
        }

        public List<DepartmentMasterMetaData> GetDepartmentByCompanyId(Guid companyId)
        {
            return baseRepository.DashBoardRepo.GetDepartmentByCompanyId(companyId);
        }

        public DashBoardJSONMetaData GetDashboardDataJSON(Guid deptId, Guid subDeptId, Guid companyId,Guid BUILDING_ID)
        {
            return baseRepository.DashBoardRepo.GetDashboardDataJSON(deptId, subDeptId, companyId, BUILDING_ID);
        }
    }
}
