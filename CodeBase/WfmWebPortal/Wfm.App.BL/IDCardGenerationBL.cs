using System;
using System.Collections.Generic;
using Wfm.App.Core.Model;
using Wfm.App.Infrastructure.Interfaces;

namespace Wfm.App.BL
{
    public class IDCardGenerationBL
    {
        private IBaseRepository baseRepository;
        public IDCardGenerationBL(IBaseRepository baseRepo)
        {
            baseRepository = baseRepo;
        }

        public List<PartialWorkflowMasterVieweMetaData> GetAllEmployeesByCopanyIdAndDeptId(Guid? deptId, Guid? sub_dept_id, int? emptype_id, Guid? BUILDING_ID)
        {
            return baseRepository.IDCardGenerationRepo.GetAllEmployeesByCopanyIdAndDeptId(deptId,sub_dept_id, emptype_id, BUILDING_ID);
        }

        public List<GenerateCardViewModel> GenerateCards(IEnumerable<Guid> wfIds)
        {
            return baseRepository.IDCardGenerationRepo.GenerateCards(wfIds);

        }

        public List<WorkforceTypeMetaData> GetOnRollOrContracts(Guid comp_id)
        {
            return baseRepository.IDCardGenerationRepo.GetOnRollOrContracts(comp_id);
        }
    }

}