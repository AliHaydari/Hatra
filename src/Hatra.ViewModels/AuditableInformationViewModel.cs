using System;
using System.ComponentModel.DataAnnotations;

namespace Hatra.ViewModels
{
    public class AuditableInformationViewModel
    {
        public AuditableInformationViewModel()
        {

        }

        [Display(Name = "ایجاد شده با مرورگر")]
        public string CreatedByBrowserName { get; set; }

        [Display(Name = "ویرایش شده با مرورگر")]
        public string ModifiedByBrowserName { get; set; }

        [Display(Name = "ایجاد شده با IP")]
        public string CreatedByIp { get; set; }

        [Display(Name = "ویرایش شده با IP")]
        public string ModifiedByIp { get; set; }

        [Display(Name = "ایجاد شده با شناسه کاربر")]
        public int? CreatedByUserId { get; set; }

        [Display(Name = "ایجاد شده با کاربر")]
        public string CreatedByUserName { get; set; }

        [Display(Name = "ویرایش شده با شناسه کاربر")]
        public int? ModifiedByUserId { get; set; }

        [Display(Name = "ویرایش شده با کاربر")]
        public string ModifiedByUserName { get; set; }

        [Display(Name = "ایجاد شده در تاریخ")]
        public DateTimeOffset? CreatedDateTime { get; set; }

        [Display(Name = "ایجاد شده در تاریخ")]
        public string CreatedPersianDateTime { get; set; }

        [Display(Name = "ویرایش شده در تاریخ")]
        public DateTimeOffset? ModifiedDateTime { get; set; }

        [Display(Name = "ویرایش شده در تاریخ")]
        public string ModifiedPersianDateTime { get; set; }
    }
}
