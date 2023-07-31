using System;
using System.Collections.Generic;
using Wfm.App.Core.Model;
using Wfm.App.Infrastructure.Interfaces;

namespace Wfm.App.BL
{
    public class WorkforceBL
    {
        private IBaseRepository baseRepository;

        public WorkforceBL(IBaseRepository baseRepo)
        {
            baseRepository = baseRepo;
        }

        public List<WorkforceMasterMetaData1> GetEmployeeByDepartmentId(Guid departId)
        {
            return baseRepository.WorkforceRepo.GetWorkforceByDepartmentId(departId);
        }

        public List<WorkforceMasterMetaData> GetEmployeeListByDepartmentId(Guid departId)
        {
            return baseRepository.WorkforceRepo.GetEmployeeListByDepartmentId(departId);
        }


        public List<ManPowerRequestFormMetaDataList> GetActiveMRFList()
        {
            return baseRepository.WorkforceRepo.GetMRFList();
        }
        public List<MRFDLL> GetApprovedAndNotHireMRF()
        {
            return baseRepository.WorkforceRepo.GetApprovedAndNotHireMRF();
        }
        public void Create(WorkforceMetaData workforce)
        {
            baseRepository.WorkforceRepo.Create(workforce);
        }

        public List<WorkforceMetaDataList> GetEmpAllItems(Guid? deptId, Guid? sub_dept_id, int? emptype_id, Guid? BUILDING_ID)
        {
            return baseRepository.WorkforceRepo.GetEmpAllItems(deptId, sub_dept_id, emptype_id, BUILDING_ID);
        }

        public List<WorkforceMetaDataList> GetEmpAllItems_PIB(Guid? deptId, Guid? sub_dept_id, int? emptype_id, Guid? BUILDING_ID)
        {
            return baseRepository.WorkforceRepo.GetEmpAllItems_PIB(deptId, sub_dept_id, emptype_id, BUILDING_ID);
        }


         public List<WorkforceMetaDataList> GetEmployessforAddSalary(Guid? deptId, Guid? sub_dept_id, int? emptype_id, Guid? BUILDING_ID)
        {
            return baseRepository.WorkforceRepo.GetEmployessforAddSalary(deptId, sub_dept_id, emptype_id, BUILDING_ID);
        }
        // public List<WorkforceMetaDataList> ExportEmployee(Guid? deptId, Guid? sub_dept_id, int? emptype_id, Guid? BUILDING_ID)
        //{
        //    return baseRepository.WorkforceRepo.ExportEmployee(deptId, sub_dept_id, emptype_id, BUILDING_ID);
        //}


        public List<WorkforceMetaDataList> BindWorkforceByWFType(Guid? deptId, Guid? sub_dept_id, int? emptype_id, Guid? BUILDING_ID)
        {
            return baseRepository.WorkforceRepo.BindWorkforceByWFType(deptId, sub_dept_id, emptype_id, BUILDING_ID);
        }

        public List<WorkforceMetaDataList> BindEmployeeData(Guid BUILDING_ID, int? emp_type_id)
        {
            return baseRepository.WorkforceRepo.BindEmployeeData(BUILDING_ID, emp_type_id);
        }

        public List<WorkforceMetaDataList> BindWorkforceByWFType_New(Guid BUILDING_ID, Guid deptId, Guid? sub_dept_id, int? emp_type_id, int? EMPLOYMENT_TYPE)
        {
            return baseRepository.WorkforceRepo.BindWorkforceByWFType_New(BUILDING_ID, deptId, sub_dept_id, emp_type_id, EMPLOYMENT_TYPE);
        }



        public List<WorkforceMetaDataList> GetPiecWagerWorkforceByWFTypes(Guid? deptId, Guid? sub_dept_id, int? emptype_id)
        {
            return baseRepository.WorkforceRepo.GetPiecWagerWorkforceByWFTypes(deptId, sub_dept_id, emptype_id);
        }

        public List<WorkforceMetaDataList> GetEmpAllItems()
        {
            return baseRepository.WorkforceRepo.GetEmpAllItems();
        }

        public WorkforceSalaryMetaData Find(string emp_id)
        {
            return baseRepository.WorkforceRepo.Find(emp_id);
        }
        public WorkforceSalaryMetaData FindByWFId(Guid wf_id)
        {
            return baseRepository.WorkforceRepo.FindByWFId(wf_id);
        }
        public WorkforceMetaData FindWorkforceByWFId(Guid wf_id)
        {
            return baseRepository.WorkforceRepo.FindWorkforceByWFId(wf_id);
        }
        public WorkforceMetaData FindWorkforce(string emp_id)
        {
            return baseRepository.WorkforceRepo.FindWorkforce(emp_id);
        }

        public WorkforceDailyWorkData DWFind(string emp_id)
        {
            return baseRepository.WorkforceRepo.DWFind(emp_id);
        }

        public void Salary(WorkforceSalaryMetaData salary)
        {
            baseRepository.WorkforceRepo.Salary(salary);
        }

        public List<WorkforceSalaryData> GetEmpSalary(Guid deptId, Guid? sub_dept_id, int month, int year, short wfEmpType)
        {
            return baseRepository.WorkforceRepo.GetEmpSalary(deptId, sub_dept_id, month, year, wfEmpType);
        }
        public List<ItemMasterMetaData> GetItemByDeptId(Guid DeptId)
        {
            return baseRepository.WorkforceRepo.GetItemByDeptId(DeptId);
        }

        public List<ItemOperationsMasterMetaData> GetItemOperationsByItemId(string ItemId, string ItemName,Guid DEPARTMENT_ID)
        {
            return baseRepository.WorkforceRepo.GetItemOperationsByItemId(ItemId, ItemName, DEPARTMENT_ID);
        }

        public void SaveDailyWorkMaster(WorkforceDailyWorkData dailywork)
        {
            baseRepository.WorkforceRepo.SaveDailyWorkMaster(dailywork);
        }

        public void SaveDailyWork(WorkforceDailyWorkData dailywork)
        {
            baseRepository.WorkforceRepo.SaveDailyWork(dailywork);
        }
        public object AddDailyWork(AddWorkForceItemMetaData dailywork)
        {
            return baseRepository.WorkforceRepo.AddDailyWork(dailywork);
        }
        public List<SerchDailyWorkMetaData> GetDailyWorks(FilterDailyWork filterDailyWork)
        {
            return baseRepository.WorkforceRepo.GetDailyWorks(filterDailyWork);
        }
        public List<SerchDailyWorkMetaData> SearchDailyWorkList(FilterDailyWork filterDailyWork)
        {
            return baseRepository.WorkforceRepo.SearchDailyWorkList(filterDailyWork);
        }

        public int Delete_DailyWork(Guid id, string Date)
        {
            return baseRepository.WorkforceRepo.Delete_DailyWork(id, Date);
        }
        public List<WorkforceFaultyData> GetFaultyData(Guid dept_id, int month, int year)
        {
            return baseRepository.WorkforceRepo.GetFaultyData(dept_id, month, year);
        }

        public List<WorkforceMasterMetaData> GetEmployeesBydeptIdAutoComplete(Guid departId, string emp)
        {
            return baseRepository.WorkforceRepo.GetEmployeesBydeptIdAutoComplete(departId, emp);
        }

        public void Edit(WorkforceMetaData workforce)
        {
            baseRepository.WorkforceRepo.Edit(workforce);
        }
        public void UpdateDailyWork(Guid? UNIQUE_OPERATION_ID, string QTY, Guid? ID, string MachineNo, string AvgPercentage, string WASTE, string REJECTION_ON_LOOM, string REJECTION_ON_FINISHING)
        {
            baseRepository.WorkforceRepo.UpdateDailyWork(UNIQUE_OPERATION_ID, QTY, ID, MachineNo, AvgPercentage, WASTE, REJECTION_ON_LOOM, REJECTION_ON_FINISHING);
        }
        public WorkforceMasterMetaData FindWorkforceIWithFullDetailByWFId(Guid wf_id)
        {
            return baseRepository.WorkforceRepo.FindWorkforceIWithFullDetailByWFId(wf_id);
        }

        public WorkforceDailyWorkMetaData EditDailyWork(Guid ID)
        {
            return baseRepository.WorkforceRepo.EditDailyWork(ID);
        }

        public BiometricAndAttendance GetWorkingHoursBywfId(Guid wfid, DateTime attdate)
        {
            return baseRepository.WorkforceRepo.GetWorkingHoursBywfId(wfid, attdate);
        }

        public List<WorkforceAttendance> GetAttendanceBywfid(Guid wfid, Guid deptid, int month, int year)
        {
            return baseRepository.WorkforceRepo.GetAttendanceBywfid(wfid, deptid, month, year);
        }

        public WorkforceMasterMetaData GetWorkforcByAadharNo(long aadharNo)
        {
            return baseRepository.WorkforceRepo.GetWorkforcByAadharNo(aadharNo);
        }

        //public WorkforceMasterMetaData IS_EMPID_EXIST(string EMP_ID)
        //{
        //    return baseRepository.WorkforceRepo.IS_EMPID_EXIST(EMP_ID);
        //}

        public bool IS_EMPID_EXIST(string EMP_ID, Int64? AADHAR_NO)
        {
            return baseRepository.WorkforceRepo.IS_EMPID_EXIST(EMP_ID, AADHAR_NO);
        }
        public bool IsBiometricAvailable(string bIOMETRIC_CODE, Int64? AADHAR_NO)
        {
            return baseRepository.WorkforceRepo.IsBiometricAvailable(bIOMETRIC_CODE, AADHAR_NO);
        }

        public List<WorkforceSalaryMetaData> GetworkforceSalary(Guid? deptId, int? emptype_id)
        {
            return baseRepository.WorkforceRepo.GetworkforceSalary(deptId, emptype_id);
        }
        public List<WorkforceSalaryMetaData> GetworkforceSalary(Guid? deptId, Guid? sub_dept_id, int? emptype_id, Guid? wf_id)
        {
            return baseRepository.WorkforceRepo.GetworkforceSalary(deptId, sub_dept_id, emptype_id, wf_id);
        }

        public WorkforceSalaryMetaData GetworkforceSalaryByWFId(Guid wfid)
        {
            return baseRepository.WorkforceRepo.GetworkforceSalaryByWFId(wfid);
        }

        public List<SpecialAllowanceMetaData> GetSpecialAllowanceByDept(Guid? deptId, Guid? sub_dept_id, int? emptype_id, int? year_id, Guid? BUILDING_ID2)
        {
            return baseRepository.WorkforceRepo.GetSpecialAllowanceByDept(deptId, sub_dept_id, emptype_id, year_id, BUILDING_ID2);
        }

        public bool AddSpecialAllowance(SpecialAllowanceMetaData specialAllowance)
        {
            return baseRepository.WorkforceRepo.AddSpecialAllowance(specialAllowance);

        }

        public SpecialAllowanceMetaData GetSpecialAllowanceById(Guid id)
        {
            return baseRepository.WorkforceRepo.GetSpecialAllowanceById(id);
        }

        public AddWorkForceItem GetOperationsById(Guid id,Guid? WF_ID)
        {
            return baseRepository.WorkforceRepo.GetOperationsById(id, WF_ID);
        }
       
        public List<ExportSalaryMetaData> GetWorkForceSalarySlip(DownloadSalarySlip download)
        {
            return baseRepository.WorkforceRepo.GetWorkForceSalarySlip(download);
        }
    }
}
