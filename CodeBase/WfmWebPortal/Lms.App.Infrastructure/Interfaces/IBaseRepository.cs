using Wfm.App.Infrastructure.Repositories;

namespace Wfm.App.Infrastructure.Interfaces
{
    public interface IBaseRepository
    {
        AccountRepository AccountRepo { get; }
        MenuRepository MenuRepo { get; }
        GatePassRepository GatePassRepo { get; }
        ShiftRepository ShiftRepo { get; }
        DepartmentRepository DepartmentRepo { get; }
        LevelRepository LevelRepo { get; }
        ManPowerRequestRepository ManPowerRequestRepo { get; }
        WorkforceRepository WorkforceRepo { get; }
        WorkFlowMappingRepository WorkflowMappingRepo { get; }
        ToolTalkRepository ToolTalkRepo { get; }
        AssetRepository AssetRepo { get; }
        UserRepository UserRepo { get; }
        DashBoardRepository DashBoardRepo { get; }
        CardGenerationRepository IDCardGenerationRepo { get; }
        WorkforceTrainningRepository WorkforceTrainningRepo { get; }
        CompanyRepository CompanyRepo { get; }
        MasterDataRepository MasterDataRepo { get; }
        SubDepartmentRepository SubDepartmentRepo { get; }
        ItemRepository ItemRepo { get; }
    }
}
