using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourAgency.BLL.Models
{
    public class SearchModel
    {
        public bool? NotFullOnly { get; set; }
        public int? MinPrice { get; set; }
        public int? MaxPrice { get; set; }
        public DateTime? MinTime { get; set; }
        public  DateTime? MaxTime { get; set; }
        public int? CountryId { get; set; }
        public string SearchString { get; set; }
    }
}
