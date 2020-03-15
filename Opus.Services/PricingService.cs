namespace Opus.Services
{
    public class PricingService : IPricingService
    {
        public decimal UnitCostBrakeDiscReplacement()
        {
            return 100;
        }

        public decimal UnitCostBrakePadReplacement()
        {
            return 50;
        }

        public decimal UnitCostExhaustReplacement()
        {
            return 175;
        }

        public decimal UnitCostOilChange()
        {
            return 20;
        }

        public decimal UnitCostTyreReplacement()
        {
            return 200;
        }
    }
}
