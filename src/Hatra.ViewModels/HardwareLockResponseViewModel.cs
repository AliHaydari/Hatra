using System;
using System.Collections.Generic;
using System.Text;

namespace Hatra.ViewModels
{
    public class HardwareLockResponseViewModel
    {
        public HardwareLockResponseViewModel()
        {
            
        }
        public HardwareLockResponseViewModel(bool isBlocked, DateTime? expDate, bool needUpdate)
        {
            IsBlocked = isBlocked;
            ExpDate = expDate;
            NeedUpdate = needUpdate;
        }

        public bool IsBlocked { get; set; }
        public DateTime? ExpDate { get; set; }
        public bool NeedUpdate { get; set; }
        public int Status { get; set; }
    }
}
