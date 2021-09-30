using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StarChart.Models
{
    public class CalestialObject
    {
        public int Id { get; set; }
        [Required]
        public string Nam { get; set; }
        public int? OrientedObjectId { get; set; }
        [NotMapped]
        public List<CalestialObject> Satellites { get; set; }
        public TimeSpan timeSpan { get; set; }

    }
}
