using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TourAgency.WEB.Models
{
    public class RegisterModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        [MinLength(3)]
        [MaxLength(60)]
        public string Email { get; set; }

        [Required]
        [MinLength(6)]
        [MaxLength(60)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(60)]
        public string UserName { get; set; }
        public int? CityId { get; set; }
    }
}