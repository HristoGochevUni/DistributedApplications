using System.Text;

namespace ConsumerApplicationService.DTOs
{
    public class RigDTO : RigPartDTO
    {
        public int CpuId { get; set; }
        public CPUDTO Cpu { get; set; }

        public int CoolerId { get; set; }
        public CoolerDTO Cooler { get; set; }

        public int DriveId { get; set; }
        public DriveDTO Drive { get; set; }

        public int GpuId { get; set; }
        public GPUDTO Gpu { get; set; }

        public int MotherboardId { get; set; }
        public MotherboardDTO Motherboard { get; set; }

        public int PcCaseId { get; set; }
        public PCCaseDTO PcCase { get; set; }

        public int PsuId { get; set; }
        public PSUDTO Psu { get; set; }

        public int RamId { get; set; }
        public RAMDTO Ram { get; set; }
        
        public override bool Validate()
        {
            var thisOk = base.Validate();
            var cpuOk = CpuId > 0;
            var coolerOk = CoolerId > 0;
            var driveOk = DriveId > 0;
            var gpuOk = GpuId > 0;
            var motherboardOk = MotherboardId > 0;
            var pcCaseOk = PcCaseId > 0;
            var psuOk = PsuId > 0;
            var ramOk = RamId > 0;
            return thisOk && cpuOk && coolerOk && driveOk && gpuOk && motherboardOk && pcCaseOk &&
                   psuOk && ramOk;
        }
        
        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append(base.ToString());
            builder.Append($",\nCPU: \n{Cpu}");
            builder.Append($",\nCooler: \n{Cooler}");
            builder.Append($",\nDrive: \n{Drive}");
            builder.Append($",\nGPU: \n{Gpu}");
            builder.Append($",\nMotherboard: \n{Motherboard}");
            builder.Append($",\nPC Case: \n{PcCase}");
            builder.Append($",\nPSU: \n{Psu}");
            builder.Append($",\nRAM: \n{Ram}");
            return builder.ToString();
        }
    }
}