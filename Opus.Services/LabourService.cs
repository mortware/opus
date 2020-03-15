using System;

namespace Opus.Services
{
    public class LabourService : ILabourService
    {
        public decimal UnitLabourBrakeDiscReplacement()
        {
            return 90;
        }

        public decimal UnitLabourBrakePadReplacement()
        {
            return 60;
        }

        public decimal UnitLabourExhaustReplacement()
        {
            return 240;
        }

        public decimal UnitLabourOilChange()
        {
            return 30;
        }

        public decimal UnitLabourTyreReplacement()
        {
            return 30;
        }
    }
}
