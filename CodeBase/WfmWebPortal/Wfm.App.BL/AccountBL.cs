using Wfm.App.Core;
using Wfm.App.Infrastructure.Interfaces;

namespace Wfm.App.BL
{
    public class AccountBL
    {
        private IBaseRepository baseRepository;

        public AccountBL(IBaseRepository baseRepo)
        {
            baseRepository = baseRepo;
        }

        public AccountValidateUser_Result ValidateUser(string username, string password)
        {
            return baseRepository.AccountRepo.ValidateUser(username, password);
        }

        public bool SendUserPasswordByLoginId(string userLoginId)
        {
            return baseRepository.AccountRepo.SendUserPasswordByLoginId(userLoginId);
        }
    }
}
