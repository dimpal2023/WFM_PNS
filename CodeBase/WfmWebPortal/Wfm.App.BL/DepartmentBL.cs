using System;
using System.Collections;
using System.Collections.Generic;
using Wfm.App.Core.Model;
using Wfm.App.Infrastructure.Interfaces;

namespace Wfm.App.BL
{
    public class DepartmentBL
    {
        protected IBaseRepository baseRepository;

        public DepartmentBL(IBaseRepository baseRepo)
        {
            baseRepository = baseRepo;
        }

        public List<DepartmentMasterMetaData> GetAllDepartment()
        {
            return baseRepository.DepartmentRepo.GetAllDepartment();
        }

        public List<DepartmentMasterMetaData> GetAllDepartmentOnlyForAdmin(Guid companyId)
        {
            return baseRepository.DepartmentRepo.GetAllDepartmentOnlyForAdmin(companyId);
        }

        public List<DepartmentMasterMetaData> GetDepartmentHeads()
        {
            return baseRepository.DepartmentRepo.GetDepartmentHeads();
        }

        public bool AddDepartment(DepartmentMasterMetaData metaData)
        {
            return baseRepository.DepartmentRepo.AddDepartment(metaData);
        }

        public List<DepartmentMasterMetaData> GetDepartmentByCompanyId(Guid companyId)
        {
            return baseRepository.DepartmentRepo.GetDepartmentByCompanyId(companyId);
        }

        public List<DepartmentMasterMetaData> GetDepartmentsByHeadId(string dept_head_id)
        {
            return baseRepository.DepartmentRepo.GetDepartmentsByHeadId(dept_head_id);
        }

        public DepartmentMasterMetaData GetDepartmentById(Guid id)
        {
            return baseRepository.DepartmentRepo.GetDepartmentById(id);

        }

        public List<DepartmentMasterMetaData> GetDepartmentsByUserId(Guid comp_id, Guid user_id)
        {
            return baseRepository.DepartmentRepo.GetDepartmentsByUserId(comp_id,user_id);
        }

        public bool IsDepartmentNameAvailable(string dept_Name, Guid dEPT_ID, Guid BUILDING_ID)
        {
            return baseRepository.DepartmentRepo.IsDepartmentNameAvailable(dept_Name, dEPT_ID, BUILDING_ID);
        }

        public IEnumerable<RoleMasterMetaData> GetRoleByCompanyId(Guid cOMPANY_ID)
        {
            return baseRepository.DepartmentRepo.GetRoleByCompanyId(cOMPANY_ID);
        }
    }
}
