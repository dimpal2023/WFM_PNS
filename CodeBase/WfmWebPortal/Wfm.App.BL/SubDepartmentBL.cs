using System;
using System.Collections.Generic;
using Wfm.App.Core.Model;
using Wfm.App.Infrastructure.Interfaces;

namespace Wfm.App.BL
{
    public class SubDepartmentBL
    {
        protected IBaseRepository baseRepository;

        public SubDepartmentBL(IBaseRepository baseRepo)
        {
            baseRepository = baseRepo;
        }

        public List<SubDepartmentMasterMetaData> GetAllSubDepartment()
        {
            return baseRepository.SubDepartmentRepo.GetAllSubDepartment();
        }

        public List<SubDepartmentMasterMetaData> GetAllSubDepartmentOnlyForAdmin(Guid companyId)
        {
            return baseRepository.SubDepartmentRepo.GetAllSubDepartmentOnlyForAdmin(companyId);
        }

        public List<SubDepartmentMasterMetaData> GetSubDepartmentByCompanyId(Guid companyId)
        {
            return baseRepository.SubDepartmentRepo.GetSubDepartmentByCompanyId(companyId);
        }
        public List<SubDepartmentMasterMetaData> GetSubDepartmentsByUserId(Guid comp_id, Guid user_id)
        {
            return baseRepository.SubDepartmentRepo.GetSubDepartmentsByUserId(comp_id, user_id);
        }

        public List<SubDepartmentMasterMetaData> GetSubDepartmentsByDeptId(Guid? dept_id)
        {
            return baseRepository.SubDepartmentRepo.GetSubDepartmentsByDeptId(dept_id);
        }
        public List<SubDepartmentMasterMetaData> GetSubDepartmentsByDeptId_N(Guid? dept_id, Guid? BUILDING_ID2)
        {
            return baseRepository.SubDepartmentRepo.GetSubDepartmentsByDeptId_N(dept_id, BUILDING_ID2);
        }
        //public List<SubDepartmentMasterMetaData> GetEmpName(Guid? DEPT_ID, Guid? SUBDEPT_ID, Guid? WF_EMP_TYPE)
        //{
        //    return baseRepository.SubDepartmentRepo.GetEmpName(DEPT_ID, SUBDEPT_ID, WF_EMP_TYPE);
        //}

        public List<SubDepartmentMasterMetaData> GetSubDepartmentsByDeptId(Guid comp_id, Guid user_id, Guid dept_id)
        {
            return baseRepository.SubDepartmentRepo.GetSubDepartmentsByDeptId(comp_id, user_id, dept_id);
        }
        public List<SubDepartmentMasterMetaData> GetAllSubDepartmentByDepartmentId(Guid comp_id, Guid user_id, Guid dept_id)
        {
            return baseRepository.SubDepartmentRepo.GetAllSubDepartmentByDepartmentId(comp_id, user_id, dept_id);
        }

       public List<SubDepartmentMasterMetaData> GetCityByState(Guid State_ID)
        {
            return baseRepository.SubDepartmentRepo.GetCityByState(State_ID);
        }

      
        public bool AddSubDepartment(SubDepartmentMasterMetaData metaData)
        {
            return baseRepository.SubDepartmentRepo.AddSubDepartment(metaData);
        }

        public SubDepartmentMasterMetaData GetSubDepartmentById(Guid id)
        {
            return baseRepository.SubDepartmentRepo.GetSubDepartmentById(id);
        }

        public bool IsSubDepartmentNameAvailable(string subDept_Name, Guid dEPT_ID, Guid sUBDEPT_ID, Guid BUILDING_ID)
        {
            return baseRepository.SubDepartmentRepo.IsSubDepartmentNameAvailable(subDept_Name,dEPT_ID,sUBDEPT_ID, BUILDING_ID);

        }
    }
}
