using System.ComponentModel.DataAnnotations;

namespace TourAgency.BLL.DTO
{
    public class UserDto
    {
        public string Id { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string UserName { get; set; }
        public int? CityId { get; set; }
        public string RoleId { get; set; }    
    }
}
