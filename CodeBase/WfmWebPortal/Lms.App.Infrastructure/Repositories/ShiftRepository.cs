using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Wfm.App.Core;
using Wfm.App.Core.Model;
using Wfm.App.Infrastructure.Interfaces;

namespace Wfm.App.Infrastructure.Repositories
{
    public class ShiftRepository : IShiftRepository
    {
        private ApplicationEntities _appEntity;

        public ShiftRepository()
        {
            _appEntity = new ApplicationEntities();
        }

        public List<ShiftMasterMetaData> GetAllItems()
        {
            List<ShiftMasterMetaData> shifts = new List<ShiftMasterMetaData>();

            shifts = _appEntity.TAB_SHIFT_MASTER.Join(
                    _appEntity.TAB_COMPANY_MASTER,
                    SM => SM.COMPANY_ID,
                    CM => CM.COMPANY_ID,
                    (SM, CM) => new ShiftMasterMetaData
                    {
                        SHIFT_ID = SM.SHIFT_ID,
                        SHIFT_NAME = SM.SHIFT_NAME,
                        ID=(int)SM.ID,
                        //SHIFT_START_TIME = SM.SHIFT_START_TIME,
                        //SHIFT_END_TIME = SM.SHIFT_END_TIME,
                        created_date = SM.created_date,
                        Created_by = SM.Created_by,
                        status = SM.status,
                        COMPANY_ID =  CM.COMPANY_ID,
                        COMPANY_NAME = CM.COMPANY_NAME                           
                    }
               ).Where(x => x.status == "Y").OrderBy(x=>x.SHIFT_NAME).ToList();


            return shifts;
        }

        public ShiftMasterMetaData Find(Guid shift_id)
        {
            Core.TAB_SHIFT_MASTER coreShift = _appEntity.TAB_SHIFT_MASTER.Where(x => x.SHIFT_ID == shift_id).FirstOrDefault();

            ShiftMasterMetaData shift = null;

            if (coreShift != null)
            {
                Core.TAB_COMPANY_MASTER company = _appEntity.TAB_COMPANY_MASTER.Where(x => x.COMPANY_ID == coreShift.COMPANY_ID).FirstOrDefault();
                List<Core.TAB_COMPANY_MASTER> companies = _appEntity.TAB_COMPANY_MASTER.ToList();

                if (company != null)
                {
                    shift = new ShiftMasterMetaData
                    {
                        SHIFT_ID = coreShift.SHIFT_ID,
                        SHIFT_NAME = coreShift.SHIFT_NAME,
                        SHIFT_START_TIME = coreShift.SHIFT_START_TIME,
                        SHIFT_END_TIME = coreShift.SHIFT_END_TIME,                                                
                        UPDATED_BY = coreShift.UPDATED_BY,
                        status = coreShift.status,                        
                        COMPANY_ID = company.COMPANY_ID,
                        COMPANY_NAME = company.COMPANY_NAME                                                    
                    };

                    if(companies != null && companies.Count > 0)
                    {
                        shift.COMPANIES = new List<CompanyMasterMetaData>();
                        foreach (Core.TAB_COMPANY_MASTER comp in companies)
                        {
                            CompanyMasterMetaData companyMaster = new CompanyMasterMetaData {
                                COMPANY_ID = comp.COMPANY_ID,
                                COMPANY_NAME = comp.COMPANY_NAME,
                                Created_by = comp.Created_by,
                                Created_date = comp.Created_date,
                                Status = comp.Status
                            };

                            shift.COMPANIES.Add(companyMaster);
                        }
                    }
                }
            }

            return shift;
        }

        public void Create(ShiftMasterMetaData shift)
        {
            if (shift.SHIFT_ID == Guid.Empty)
            {
                shift.SHIFT_ID = Guid.NewGuid();
            }

            Core.TAB_SHIFT_MASTER shiftObj = new Core.TAB_SHIFT_MASTER
            {
                SHIFT_ID = shift.SHIFT_ID,
                SHIFT_NAME = shift.SHIFT_NAME,
                SHIFT_START_TIME = shift.SHIFT_START_TIME,
                SHIFT_END_TIME = shift.SHIFT_END_TIME,
                created_date = DateTime.Now,
                UPDATED_DATE = DateTime.Now,
                Created_by = shift.Created_by,
                UPDATED_BY = shift.UPDATED_BY,
                status = "Y",                
                COMPANY_ID = Guid.Parse("14F8A732-8447-4BF8-BD07-4337317B08F1")
            };

            _appEntity.TAB_SHIFT_MASTER.Add(shiftObj);
            _appEntity.SaveChanges();
        }

        public void Update(ShiftMasterMetaData shift)
        {
            Core.TAB_SHIFT_MASTER coreShift = _appEntity.TAB_SHIFT_MASTER.Where(x => x.SHIFT_ID == shift.SHIFT_ID).FirstOrDefault();

            if (coreShift != null)
            {
                coreShift.SHIFT_NAME = shift.SHIFT_NAME;
                coreShift.SHIFT_START_TIME = shift.SHIFT_START_TIME;
                coreShift.SHIFT_END_TIME = shift.SHIFT_END_TIME;
                coreShift.UPDATED_DATE = DateTime.Now;
                coreShift.UPDATED_BY = shift.UPDATED_BY;
                coreShift.status = shift.status;          

                _appEntity.Entry(coreShift).State = EntityState.Modified;
                _appEntity.SaveChanges();
            }
        }

        public void Delete(Guid shift_Id)
        {
            Core.TAB_SHIFT_MASTER coreShift = _appEntity.TAB_SHIFT_MASTER.Where(x => x.SHIFT_ID == shift_Id).FirstOrDefault();

            ShiftMasterMetaData shift = null;

            if (coreShift != null)
            {
                shift = new ShiftMasterMetaData
                {
                    SHIFT_ID = coreShift.SHIFT_ID,
                    SHIFT_NAME = coreShift.SHIFT_NAME,
                    SHIFT_START_TIME = coreShift.SHIFT_START_TIME,
                    SHIFT_END_TIME = coreShift.SHIFT_END_TIME,
                    created_date = coreShift.created_date,
                    UPDATED_DATE = coreShift.UPDATED_DATE,
                    UPDATED_BY = coreShift.UPDATED_BY,
                    status = "N"
                };

                Update(shift);
            }
        }

        public List<CompanyMasterMetaData> GetCompanies()
        {
            List<Core.TAB_COMPANY_MASTER> companies = _appEntity.TAB_COMPANY_MASTER.ToList();
            List<CompanyMasterMetaData> companiesMaster = new List<CompanyMasterMetaData>();            

            if (companies != null && companies.Count > 0)
            {
                foreach (Core.TAB_COMPANY_MASTER comp in companies)
                {
                    CompanyMasterMetaData companyMaster = new CompanyMasterMetaData
                    {
                        COMPANY_ID = comp.COMPANY_ID,
                        COMPANY_NAME = comp.COMPANY_NAME,
                        Created_by = comp.Created_by,
                        Created_date = comp.Created_date,
                        Status = comp.Status
                    };

                    companiesMaster.Add(companyMaster);
                }
            }

            return companiesMaster;
        }
    }
}
