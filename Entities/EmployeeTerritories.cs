using System;
using System.Collections.Generic;

namespace Entities
{
    public partial class EmployeeTerritories
    {
        public int EmployeeId { get; set; }
        public int TerritoryId { get; set; }

        public virtual Employee Employee { get; set; }
        public virtual Territory Territory { get; set; }
    }
}
