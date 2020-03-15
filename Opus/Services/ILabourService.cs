using System;
using System.Collections.Generic;
using System.Text;

namespace Opus.Services
{
    public interface ILabourService
    {
        decimal UnitLabourTyreReplacement();
        decimal UnitLabourBrakeDiscReplacement();
        decimal UnitLabourBrakePadReplacement();
        decimal UnitLabourOilChange();
        decimal UnitLabourExhaustReplacement();
    }
}
