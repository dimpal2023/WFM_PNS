using System;
using System.Collections.Generic;
using Wfm.App.Core.Model;

namespace Wfm.App.Infrastructure.Interfaces
{
    public interface IGatePassRepository
    {
        GatePassAllItemsMetaData GetAllItems(string rolename, string dept_id, string sub_dept_id, DateTime FROM_DATE, DateTime TO_DATE, string STATUS_ID,string BUILDING_ID);
        GatePassMetaData FindWorkforce(Guid emp_id);
        GatePassMetaData FindGatePass(Guid gatepass_id);
        string Create(GatePassMetaData gatePass);
        string Update(GatePassMetaData gatePass);
        void Delete(Guid gatePassId);
        string Out(Guid gatePassId);
        void In(Guid gatePassId);
    }
}
