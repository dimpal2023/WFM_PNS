using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wfm.App.Core.Model;
using Wfm.App.Infrastructure.Repositories;

namespace Wfm.App.BL
{
    public class LeaveBL
    {
        private LeaveRepository leaveRepository;

        public LeaveBL()
        {
            leaveRepository = new LeaveRepository();
        }
        public void Create(WorkforceLeavesMetaData leave)
        {
            leaveRepository.Create(leave);
        }

        public List<WorkforceLeavesMetaData> GetLeaveAllItems()
        {
            return leaveRepository.GetLeaveAllItems();
        }

        public WorkforceLeavesMetaData Find(string emp_id)
        {
            return leaveRepository.Find(emp_id);
        }
    }
}
