using System.Web.Mvc;
using Unity;
using Unity.Mvc5;
using Wfm.App.Infrastructure.Interfaces;
using Wfm.App.Infrastructure.Repositories;

namespace Wfm.App.BL
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            container.RegisterType<IAccountRepository, AccountRepository>();
            container.RegisterType<IGatePassRepository, GatePassRepository>();
            container.RegisterType<IShiftRepository, ShiftRepository>();
            container.RegisterType<IMenuRepository, MenuRepository>();
            container.RegisterType<ILevelRepository, LevelRepository>();
            container.RegisterType<IManPowerRequestRepository, ManPowerRequestRepository>();
            container.RegisterType<IWorkforceRepository, WorkforceRepository>();
            container.RegisterType<IDepartmentRepository, DepartmentRepository>();
            container.RegisterType<IToolTalkRepository, ToolTalkRepository>();
            container.RegisterType<IAssetRepository, AssetRepository>();
            container.RegisterType<ICompanyRepository, CompanyRepository>();
            container.RegisterType<IMasterDataRepository, MasterDataRepository>();
            container.RegisterType<IWorkFlowMappingRepository, WorkFlowMappingRepository>();
            container.RegisterType<IDepartmentRepository, DepartmentRepository>();
            container.RegisterType<ICardGenerationRepository, CardGenerationRepository>();

            container.RegisterType<IBaseBL, BaseBL>();
            container.RegisterType<IBaseRepository, BaseRepository>();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}