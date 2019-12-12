using System;

namespace TourAgency.BLL.Models
{
    public enum SortState
    {
        NameAsc,
        NameDesc,
        PriceAsc,
        PriceDesc,
        DateAsc,
        DateDesc,
    }

    public class TourSearchModel
    {
        public SortState? SortState { get; set; }
        public bool? NotFullOnly { get; set; }
        public int? MinPrice { get; set; }
        public int? MaxPrice { get; set; }
        public DateTime? MinTime { get; set; }
        public DateTime? MaxTime { get; set; }
        public int? CountryId { get; set; }
        public string SearchString { get; set; }
    }
}
