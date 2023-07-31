using System;
using System.Collections.Generic;
using Wfm.App.Core.Model;

namespace Wfm.App.Infrastructure.Interfaces
{
    public interface IShiftRepository
    {
        ShiftMasterMetaData Find(Guid shift_id);
        List<ShiftMasterMetaData> GetAllItems();
        void Create(ShiftMasterMetaData shift);
        void Update(ShiftMasterMetaData shift);
        void Delete(Guid shift_Id);
    }
}
