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
        /// <summary>
        /// 0 Success
        /// 1 Error Empty Financial List
        /// 2 Error Saving Data
        /// 3 Error Invalid Model State
        /// </summary>
        public int Status { get; set; }
    }
}
