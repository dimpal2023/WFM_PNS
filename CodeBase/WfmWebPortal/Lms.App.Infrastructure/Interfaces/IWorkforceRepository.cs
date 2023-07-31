using System;
using System.Collections.Generic;
using Wfm.App.Core.Model;

namespace Wfm.App.Infrastructure.Interfaces
{
    public interface IWorkforceRepository
    {
        List<WorkforceMasterMetaData1> GetWorkforceByDepartmentId(Guid departid);

        WorkforceMetaData FindWorkforce(string emp_id);
    }
}
