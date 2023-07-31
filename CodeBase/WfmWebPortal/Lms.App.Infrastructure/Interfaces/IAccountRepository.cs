using Wfm.App.Core;

namespace Wfm.App.Infrastructure.Interfaces
{
    public interface IAccountRepository
    {
        AccountValidateUser_Result ValidateUser(string username, string password);
    }
}
