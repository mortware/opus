using System;
using System.Collections.Generic;
using System.Text;

namespace Opus.Services
{
    public interface IPricingService
    {
        decimal UnitCostTyreReplacement();
        decimal UnitCostBrakeDiscReplacement();
        decimal UnitCostBrakePadReplacement();
        decimal UnitCostOilChange();
        decimal UnitCostExhaustReplacement();

    }
}
