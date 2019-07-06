using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TourAgency.BLL.DTO;

namespace TourAgency.BLL.Interfaces
{
    public interface IUserService : IDisposable
    {
        void Create(UserDto userDto);
        ClaimsIdentity Authenticate(UserDto userDto);
        UserDto GetUser(string userId);
        IEnumerable<UserDto> GetUsers();
        void ChangeRole(string userId, string roleName);
    }
}
