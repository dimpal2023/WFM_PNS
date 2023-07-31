using Wfm.App.Infrastructure.Interfaces;

namespace Wfm.App.BL
{
    public class BaseBL : IBaseBL
    {
        public BaseBL(IBaseRepository baseRepo)
        {
            this.AccountBL = new AccountBL(baseRepo);
            this.GatePassBL = new GatePassBL(baseRepo);
            this.MenuBL = new MenuBL(baseRepo);
            this.ShiftBL = new ShiftBL(baseRepo);
            this.DepartmentBL = new DepartmentBL(baseRepo);
            this.LevelBL = new LevelBL(baseRepo);
            this.WorkforceBL = new WorkforceBL(baseRepo);
            this.ManPowerRequestBL = new ManPowerRequestBL(baseRepo);
            this.WorkflowMappingBL = new WorkFlowMappingBL(baseRepo);
            this.ToolTalkBL = new ToolTalkBL(baseRepo);
            this.AssetBL = new AssetBL(baseRepo);
            this.UserBL = new UserBL(baseRepo);
            this.DashBoardBL = new DashBoardBL(baseRepo);
            this.IDCardGenerationBL = new IDCardGenerationBL(baseRepo);
            this.WorkforceTrainningBL = new WorkforceTrainningBL(baseRepo);
            this.CompanyBL = new CompanyBL(baseRepo);
            this.MasterDataBL = new MasterDataBL(baseRepo);
            this.SubDepartmentBL = new SubDepartmentBL(baseRepo);
            this.ItemBL = new ItemBL(baseRepo);
        }

        public AccountBL AccountBL { get; set; }
        public MenuBL MenuBL { get; set; }
        public GatePassBL GatePassBL { get; set; }
        public ShiftBL ShiftBL { get; set; }
        public DepartmentBL DepartmentBL { get; set; }
        public LevelBL LevelBL { get; set; }
        public ManPowerRequestBL ManPowerRequestBL { get; set; }
        public WorkforceBL WorkforceBL { get; set; }
        public WorkFlowMappingBL WorkflowMappingBL { get; set; }
        public LeaveBL LeaveBL { get; set; }
        public ToolTalkBL ToolTalkBL { get; set; }
        public AssetBL AssetBL { get; set; }
        public UserBL UserBL { get; set; }
        public DashBoardBL DashBoardBL { get; set; }
        public IDCardGenerationBL IDCardGenerationBL { get; set; }
        public WorkforceTrainningBL WorkforceTrainningBL { get; set; }
        public CompanyBL CompanyBL { get; set; }
        public MasterDataBL MasterDataBL { get; set; }
        public SubDepartmentBL SubDepartmentBL { get; set; }
        public ItemBL ItemBL { get; set; }
    }
}
