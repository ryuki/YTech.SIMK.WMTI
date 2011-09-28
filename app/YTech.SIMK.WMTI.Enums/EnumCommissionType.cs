using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YTech.SIMK.WMTI.Enums
{
    public enum EnumCommissionType
    {
        [StringValue("Insentif Survey")]
        IncentiveSurvey,
        [StringValue("Insentif Kredit OK")]
        IncentiveApprove,
        [StringValue("Uang Transportasi")]
        TransportAllowance,
        [StringValue("Komisi")]
        Commission,
        [StringValue("Insentif")] 
        IncentiveCredit
    }
}
