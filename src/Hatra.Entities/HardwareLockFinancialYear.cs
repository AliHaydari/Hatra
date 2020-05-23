using Hatra.Entities.AuditableEntity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hatra.Entities
{
    public class HardwareLockFinancialYear: IAuditableEntity
    {
        public int Id { get; set; }
        public int HardwareLockId { get; set; }
        public Guid CompanyId { get; set; }
        public string CompanyName { get; set; }
        public Guid FinancialYearId { get; set; }
        public string FinancialYearName { get; set; }
        public bool IsArchive { get; set; }
        public string DbName { get; set; }

        public HardwareLock HardwareLock { get; set; }
    }
}
