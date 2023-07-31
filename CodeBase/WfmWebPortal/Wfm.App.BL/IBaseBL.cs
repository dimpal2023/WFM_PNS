namespace Wfm.App.BL
{
    public interface IBaseBL
    {
        AccountBL AccountBL { get; set; }
        MenuBL MenuBL { get; set; }
        GatePassBL GatePassBL { get; set; }
        ShiftBL ShiftBL { get; set; }
        DepartmentBL DepartmentBL { get; set; }
        LevelBL LevelBL { get; set; }
        ManPowerRequestBL ManPowerRequestBL { get; set; }
        WorkforceBL WorkforceBL { get; set; }
        WorkFlowMappingBL WorkflowMappingBL { get; set; }
        ToolTalkBL ToolTalkBL { get; set; }
        AssetBL AssetBL { get; set; }
        UserBL UserBL { get; }
        DashBoardBL DashBoardBL { get; }
        IDCardGenerationBL IDCardGenerationBL { get; }
        WorkforceTrainningBL WorkforceTrainningBL { get; }
        CompanyBL CompanyBL { get; set; }
        MasterDataBL MasterDataBL { get; set; }
        SubDepartmentBL SubDepartmentBL { get; set; }
        ItemBL ItemBL { get; set; }
    }
}
