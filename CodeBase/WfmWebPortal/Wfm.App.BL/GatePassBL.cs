using System;
using System.Collections.Generic;
using Wfm.App.Core.Model;
using Wfm.App.Infrastructure.Interfaces;

namespace Wfm.App.BL
{
    public class GatePassBL
    {
        public IBaseRepository baseRepository;

        public GatePassBL(IBaseRepository baseRepo)
        {
            baseRepository = baseRepo;
        }
            
        public GatePassAllItemsMetaData GetAllItems(string rolename,string dept_id,string sub_dept_id, DateTime FROM_DATE, DateTime TO_DATE, string STATUS_ID,string BUILDING_ID)
        {
            return baseRepository.GatePassRepo.GetAllItems(rolename,dept_id, sub_dept_id, FROM_DATE, TO_DATE, STATUS_ID, BUILDING_ID);
        }
        //public GatePassAllItemsMetaData GetData(string rolename,string dept_id,string sub_dept_id,string BUILDING_ID)
        //{
        //    return baseRepository.GatePassRepo.GetData(rolename,dept_id, sub_dept_id, BUILDING_ID);
        //}

        public string Create(GatePassMetaData gatePass)
        {
            return baseRepository.GatePassRepo.Create(gatePass);
        }

        public GatePassMetaData FindWorkforce(Guid emp_id)
        {
           return baseRepository.GatePassRepo.FindWorkforce(emp_id);
        }

        public GatePassMetaData FindGatePass(Guid gatepass_id)
        {
            return baseRepository.GatePassRepo.FindGatePass(gatepass_id);
        }

        public string Update(GatePassMetaData gatePass)
        {
           return baseRepository.GatePassRepo.Update(gatePass);
        }

        public void Delete(Guid gpId)
        {
            baseRepository.GatePassRepo.Delete(gpId);
        }

        public string Out(Guid gpId)
        {
           return baseRepository.GatePassRepo.Out(gpId);
        }

        public void In(Guid gpId)
        {
            baseRepository.GatePassRepo.In(gpId);
        }
    }
}
