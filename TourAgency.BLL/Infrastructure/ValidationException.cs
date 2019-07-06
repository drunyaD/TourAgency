using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourAgency.BLL.Infrastructure
{
    public class ValidationException: Exception
    {
        public string Property { get; set; }
        public ValidationException(string message, string prop) : base(message)
        {
            Property = prop;
        }
    }
}
