using System;
using System.ComponentModel.DataAnnotations;
using ConsumerApplicationService.DTOs;

namespace RigMag.ViewModels
{
    public class RigVM
    {
        public int Id { get; set; }

        [Required] [MaxLength(50)] public string Name { get; set; }

        [Required] public float Price { get; set; }

        [DataType(DataType.Date)] public DateTime? IssuedOn { get; set; }

        [MaxLength(600)] public string Description { get; set; }

        public float? Warranty { get; set; }

        public string ImagePath { get; set; }
        public int CpuId { get; set; }
        public RigPartVM CpuVm { get; set; }
        public int CoolerId { get; set; }
        public RigPartVM CoolerVm { get; set; }
        public int DriveId { get; set; }
        public RigPartVM DriveVm { get; set; }
        public int GpuId { get; set; }
        public RigPartVM GpuVm { get; set; }
        public int MotherboardId { get; set; }
        public RigPartVM MotherboardVm { get; set; }
        public int PcCaseId { get; set; }
        public RigPartVM PcCaseVm { get; set; }
        public int PsuId { get; set; }
        public RigPartVM PsuVm { get; set; }
        public int RamId { get; set; }
        public RigPartVM RamVm { get; set; }

        public RigVM()
        {
        }

        public RigVM(RigDTO dto)
        {
            Id = dto.Id;
            Name = dto.Name;
            Price = dto.Price;
            IssuedOn = dto.IssuedOn;
            Description = dto.Description;
            Warranty = dto.Warranty;
            CpuId = dto.CpuId;
            CpuVm = new RigPartVM(dto.Cpu, RigPartType.CPU);
            CoolerId = dto.CoolerId;
            CoolerVm = new RigPartVM(dto.Cooler, RigPartType.Cooler);
            DriveId = dto.DriveId;
            DriveVm = new RigPartVM(dto.Drive, RigPartType.Drive);
            GpuId = dto.GpuId;
            GpuVm = new RigPartVM(dto.Gpu, RigPartType.GPU);
            MotherboardId = dto.MotherboardId;
            MotherboardVm = new RigPartVM(dto.Motherboard, RigPartType.Motherboard);
            PcCaseId = dto.PcCaseId;
            PcCaseVm = new RigPartVM(dto.PcCase, RigPartType.PCCase);
            PsuId = dto.PsuId;
            PsuVm = new RigPartVM(dto.Psu, RigPartType.PSU);
            RamId = dto.RamId;
            RamVm = new RigPartVM(dto.Ram, RigPartType.RAM);
        }
    }
}