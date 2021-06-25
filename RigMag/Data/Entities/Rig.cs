using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities
{
    public class Rig : RigPart
    {
        [Required] public int CPU_Id { get; set; }
        [Required] public int Cooler_Id { get; set; }
        [Required] public int Drive_Id { get; set; }
        [Required] public int GPU_Id { get; set; }
        [Required] public int Motherboard_Id { get; set; }
        [Required] public int PCCase_Id { get; set; }
        [Required] public int PSU_Id { get; set; }
        [Required] public int RAM_Id { get; set; }
    }
}