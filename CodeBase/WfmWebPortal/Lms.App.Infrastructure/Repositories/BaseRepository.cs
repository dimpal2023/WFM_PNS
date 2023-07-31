using System;
using Wfm.App.Infrastructure.Interfaces;

namespace Wfm.App.Infrastructure.Repositories
{
    public class BaseRepository : IBaseRepository
    {

        public BaseRepository()
        {
            this.AccountRepo = new AccountRepository();
            this.GatePassRepo = new GatePassRepository();
            this.MenuRepo = new MenuRepository();
            this.ShiftRepo = new ShiftRepository();
            this.DepartmentRepo = new DepartmentRepository();
            this.LevelRepo = new LevelRepository();
            this.WorkforceRepo = new WorkforceRepository();
            this.ManPowerRequestRepo = new ManPowerRequestRepository();
            this.WorkflowMappingRepo = new WorkFlowMappingRepository();
            this.ToolTalkRepo = new ToolTalkRepository();
            this.AssetRepo = new AssetRepository();
            this.UserRepo = new UserRepository();
            this.DashBoardRepo = new DashBoardRepository();
            this.IDCardGenerationRepo = new CardGenerationRepository();
            this.WorkforceTrainningRepo = new WorkforceTrainningRepository();
            this.CompanyRepo = new CompanyRepository();
            this.MasterDataRepo = new MasterDataRepository();
            this.SubDepartmentRepo = new SubDepartmentRepository();
            this.ItemRepo = new ItemRepository();
        }

        public AccountRepository AccountRepo { get; }

        public AssetRepository AssetRepo { get; }

        public CompanyRepository CompanyRepo { get; }

        public DashBoardRepository DashBoardRepo { get; }

        public DepartmentRepository DepartmentRepo { get; }

        public GatePassRepository GatePassRepo { get; }
        public CardGenerationRepository IDCardGenerationRepo { get; }

        public LevelRepository LevelRepo { get; }

        public ManPowerRequestRepository ManPowerRequestRepo { get; }

        public MasterDataRepository MasterDataRepo { get; }

        public MenuRepository MenuRepo { get; }

        public ShiftRepository ShiftRepo { get; }

        public ToolTalkRepository ToolTalkRepo { get; }

        public UserRepository UserRepo { get; }

        public WorkFlowMappingRepository WorkflowMappingRepo { get; }

        public WorkforceRepository WorkforceRepo { get; }

        public WorkforceTrainningRepository WorkforceTrainningRepo { get; }

        public SubDepartmentRepository SubDepartmentRepo { get; }
        public ItemRepository ItemRepo { get; }
    }
}
