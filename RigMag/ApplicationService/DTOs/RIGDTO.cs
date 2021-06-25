using System.Text;

namespace ApplicationService.DTOs
{
    public class RIGDTO : RigPartDTO
    {
        public int CpuId { get; set; }

        public int CoolerId { get; set; }

        public int DriveId { get; set; }

        public int GpuId { get; set; }

        public int MotherboardId { get; set; }

        public int PcCaseId { get; set; }

        public int PsuId { get; set; }

        public int RamId { get; set; }

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
    }
}