using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourAgency.BLL.DTO
{
    public class ImageDto
    {
        public int TourId { get; set; }
        public Stream File { get; set; }
        public string FileName { get; set; }
    }
}