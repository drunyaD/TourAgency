using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TourAgency.DAL.Entities
{
    public class Tour
    {
        [Required] public int Id { get; set; }
        [Required] public string Name { get; set; }
        [Required] public string Description { get; set; }
        [Required] public DateTime StartDate { get; set; }
        [Required] public DateTime FinishDate { get; set; }
        [Required] public int Price { get; set; }
        [Required] public int MaxCapacity { get; set; }
        public virtual ICollection<Image> Images { get; set; }
        public virtual ICollection<Node> Nodes { get; set; }
        public virtual ICollection<User> Users { get; set; }

        public Tour()
        {
            Images = new HashSet<Image>();
            Nodes = new HashSet<Node>();
            Users = new HashSet<User>();
        }
    }
}