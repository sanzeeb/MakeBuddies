using System.Threading.Tasks;
using MakeBuddies.API.Models;

namespace MakeBuddies.API.Data
{
    public interface IAuthRepo
    {
        Task<User> Register(User user, string password);
        Task<User> LoginAsync(string userName, string password);
        Task<bool> UserExistAsync(string userName);
    }
}