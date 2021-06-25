using System.Collections.Generic;

namespace RigMag.ViewModels
{
    public class RigPartsVM
    {
        public RigPartType Type { get; set; }
        public IEnumerable<RigPartVM> RigParts { get; set; }

        public RigPartsVM()
        {
        }

        public RigPartsVM(RigPartType type, IEnumerable<RigPartVM> rigParts)
        {
            Type = type;
            RigParts = rigParts;
        }
    }
}