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
        string Create(UserDto userDto);
        ClaimsIdentity Authenticate(UserDto userDto);
        UserDto GetUser(string userId);
        UserDto GetUserByName(string userName);
        IEnumerable<UserDto> GetUsers();
        IEnumerable<UserDto> GetUsersByTour(int tourId);
        void ChangeRole(string userId, RoleDto roleDto);
        RoleDto GetRole(string roleId);
        IEnumerable<RoleDto> GetRoles();
    }
}
