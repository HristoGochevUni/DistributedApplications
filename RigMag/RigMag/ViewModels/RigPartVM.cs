using System;
using System.ComponentModel.DataAnnotations;
using ConsumerApplicationService.DTOs;

namespace RigMag.ViewModels
{
    public enum RigPartType
    {
        Cooler,
        CPU,
        Drive,
        GPU,
        Motherboard,
        PCCase,
        PSU,
        RAM
    }

    public class RigPartVM
    {
        public int Id { get; set; }

        [Required] public RigPartType Type { get; set; }

        [Required] [MaxLength(50)] public string Name { get; set; }

        [Required] public float Price { get; set; }

        [DataType(DataType.Date)] public DateTime? IssuedOn { get; set; }

        [MaxLength(600)] public string Description { get; set; }

        public float? Warranty { get; set; }

        public string ImagePath { get; set; }

        public RigPartVM()
        {
        }

        public RigPartVM(RigPartDTO dto, RigPartType type)
        {
            Type = type;
            Id = dto.Id;
            Name = dto.Name;
            Price = dto.Price;
            IssuedOn = dto.IssuedOn;
            Description = dto.Description;
            Warranty = dto.Warranty;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}