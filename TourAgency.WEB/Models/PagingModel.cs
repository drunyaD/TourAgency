using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TourAgency.WEB.Models
{
    public class PagingModel
    {
        const int maxPageSize = 20;

        public int PageNumber { get; set; } = 1;

        public int _pageSize { get; set; } = 10;

        public int PageSize
        {

            get { return _pageSize; }
            set
            {
                _pageSize = (value > maxPageSize) ? maxPageSize : value;
            }
        }
    }
}