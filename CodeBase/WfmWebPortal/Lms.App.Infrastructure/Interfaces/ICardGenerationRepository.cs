using System;
using System.Collections.Generic;
using Wfm.App.Core.Model;

namespace Wfm.App.Infrastructure.Interfaces
{
    public interface ICardGenerationRepository
    {
        List<PartialWorkflowMasterVieweMetaData> GetAllEmployeesByCopanyIdAndDeptId(Guid? deptId, Guid? sub_dept_id, int? emptype_id, Guid? BUILDING_ID);

        List<GenerateCardViewModel> GenerateCards(IEnumerable<Guid> wfIds);
    }
}
