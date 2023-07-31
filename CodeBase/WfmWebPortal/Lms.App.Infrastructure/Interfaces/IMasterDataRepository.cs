using System;
using System.Collections.Generic;
using Wfm.App.Core.Model;

namespace Wfm.App.Infrastructure.Interfaces
{
    public interface IMasterDataRepository
    {
        List<AgencyMasterMetaData> GetAgencyList();
        List<CompanyMasterMetaData> GetCompanyList();
        List<WorkforceEduMasterMetaData> GetEducationList();
        List<MartialStatusMasterMetaData> GetMartialStatusList();
        List<EmplTypeMasterMetaData> GetEmpTypeList();
        List<StateMetaData> GetStateList();
        List<StateCityMetaData> GetStateCityList();
        List<EmpStatusMasterMetaData> GetEmpStatusList();
        List<DepartmentMasterMetaData> GetDepartmentList();
        List<StateCityMetaData> GetCityList(String stateId);
    }
}
