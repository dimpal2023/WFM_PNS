using System;
using System.Collections.Generic;
using Wfm.App.Core.Model;

namespace Wfm.App.Infrastructure.Interfaces
{
    public interface IManPowerRequestRepository
    {
        List<BuildingMasterMetaData> GetBuildings();
        List<SkillMasterMetaData> GetSkills();
        List<WFDesignationMasterMetaData> GetDesignations();
        List<DepartmentMasterMetaData> GetFloorByBuildingId(Guid buildingId);
        List<WorkforceTypeMetaData> GetEmpTypes();
        List<ManPowerRequestFormMetaDataList> GetMRFAllItems(Guid compId);
    }
}
