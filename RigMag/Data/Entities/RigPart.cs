using System;
using System.ComponentModel.DataAnnotations;

namespace Data.Entities
{
    public class RigPart
    {
        [Key] public int Id { get; set; }

        [Required] [MaxLength(50)] public string Name { get; set; }

        [Required] public float Price { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime? IssuedOn { get; set; }

        [MaxLength(600)] public string Description { get; set; }

        public float? Warranty { get; set; }
    }
}