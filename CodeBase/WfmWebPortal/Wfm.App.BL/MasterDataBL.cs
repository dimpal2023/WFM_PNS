using System;
using System.Collections.Generic;
using Wfm.App.Core.Model;
using Wfm.App.Infrastructure.Interfaces;

namespace Wfm.App.BL
{
    public class MasterDataBL
    {
        public IBaseRepository baseRepository;
        public MasterDataBL(IBaseRepository baseRepo)
        {
            baseRepository = baseRepo;
        }

        public List<CompanyMasterMetaData> GetCompanyList()
        {
            return baseRepository.MasterDataRepo.GetCompanyList();
        }

        public List<BuildingMasterMetaData> GetBuildingList()
        {
            return baseRepository.MasterDataRepo.GetBuildingList();
        }
        public List<DepartmentMasterMetaData> GetDepartmentList()
        {
            return baseRepository.MasterDataRepo.GetDepartmentList();
        }

        public List<AgencyMasterMetaData> GetAgencyList()
        {
            return baseRepository.MasterDataRepo.GetAgencyList();
        }
         public List<BankMasterMetaData> GetAllBank()
        {
            return baseRepository.MasterDataRepo.GetAllBank();
        }
         public List<BankMasterMetaData> SelectBankData(Guid Bank_ID)
        {
            return baseRepository.MasterDataRepo.SelectBankData(Bank_ID);
        }
         public ExportSalaryMetaData CheckUAN(string UAN_NO)
        {
            return baseRepository.MasterDataRepo.CheckUAN(UAN_NO);
        }
         public ExportSalaryMetaData CheckPAN(string PAN)
        {
            return baseRepository.MasterDataRepo.CheckPAN(PAN);
        }
         public ExportSalaryMetaData CheckEPF(string EPF)
        {
            return baseRepository.MasterDataRepo.CheckEPF(EPF);
        }
          public ExportSalaryMetaData CheckESIC(string ESIC)
        {
            return baseRepository.MasterDataRepo.CheckESIC(ESIC);
        }
          public ExportSalaryMetaData CheckAccountNo(string ACC)
        {
            return baseRepository.MasterDataRepo.CheckAccountNo(ACC);
        }


        public List<WorkforceEduMasterMetaData> GetEducationList()
        {
            return baseRepository.MasterDataRepo.GetEducationList();
        }

        public List<MartialStatusMasterMetaData> GetMartialStatusList()
        {
            return baseRepository.MasterDataRepo.GetMartialStatusList();
        }

        public List<EmpStatusMasterMetaData> GetEmpStatusList()
        {
            return baseRepository.MasterDataRepo.GetEmpStatusList();
        }

        public List<StateCityMetaData> GetStateCityList()
        {
            return baseRepository.MasterDataRepo.GetStateCityList();
        }

        public List<StateMetaData> GetStateList()
        {
            return baseRepository.MasterDataRepo.GetStateList();
        }

        public List<WFDesignationMasterMetaData> GetDesignationList()
        {
            return baseRepository.MasterDataRepo.GetDesignationList();
        }

        public List<SkillMasterMetaData> GetSkillList()
        {
            return baseRepository.MasterDataRepo.GetSkillList();
        }

       public List<EmplTypeMasterMetaData> GetEmpTypeList()
        {
            return baseRepository.MasterDataRepo.GetEmpTypeList();
        }

        public List<StateCityMetaData> GetCityList(String stateId)
        {
            return baseRepository.MasterDataRepo.GetCityList(stateId);
        }

        public List<WorkforceTypeMetaData> GetWF_EMP_TYPE()
        {
            return baseRepository.MasterDataRepo.GetWF_EMP_TYPE();
        }
    }
}
