using Hatra.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Hatra.ViewModels
{
    public class HardwareLockViewModel
    {
        public HardwareLockViewModel()
        {
            
        }
        public HardwareLockViewModel(HardwareLock hardwareLock)
        {
            Id = hardwareLock.Id;
            ComputerName = hardwareLock.ComputerName;
            CpuSerialNumber = hardwareLock.CpuSerialNumber;
            LockSerialNumber = hardwareLock.LockSerialNumber;
            CompanyCount = hardwareLock.CompanyCount;
            FinancialYearCount = hardwareLock.FinancialYearCount;
            DocumentCount = hardwareLock.DocumentCount;
            UserCount = hardwareLock.UserCount;
            AndroidUserCount = hardwareLock.AndroidUserCount;
            IsBlocked = hardwareLock.IsBlocked;
            CurrentVersion = hardwareLock.CurrentVersion;
            ExpireDate = hardwareLock.ExpireDate;
            OwnerName = hardwareLock.OwnerName;

            FinancialYears = hardwareLock.FinancialYears.Select(financialYear => new HardwareLockFinancialYearViewModel
            {
                CompanyId = financialYear.CompanyId,
                FinancialYearId = financialYear.FinancialYearId,
                Id = financialYear.Id,
                CompanyName = financialYear.CompanyName,
                FinancialYearName = financialYear.FinancialYearName,
                IsArchive = financialYear.IsArchive,
            }).ToList();
        }

        [HiddenInput]
        public int Id { get; set; }

        [Required(ErrorMessage = "(*)")]
        [Display(Name = "نام کامپیوتر")]
        [StringLength(300, ErrorMessage = "{0} باید حداقل {2} و حداکثر {1} حرف باشند.", MinimumLength = 2)]
        public string ComputerName { get; set; }

        [Display(Name = "شماره سریال Cpu")]
        [Required(ErrorMessage = "(*)")]
        [StringLength(300, ErrorMessage = "{0} باید حداقل {2} و حداکثر {1} حرف باشند.", MinimumLength = 2)]
        public string CpuSerialNumber { get; set; }

        [Display(Name = "شماره سریال قفل")]
        [Required(ErrorMessage = "(*)")]
        [StringLength(20, ErrorMessage = "{0} باید حداقل {2} و حداکثر {1} حرف باشند.", MinimumLength = 2)]
        public string LockSerialNumber { get; set; }

        [Display(Name = "تعداد شرکت")]
        public int CompanyCount { get; set; }

        [Display(Name = "تعداد سال مالی")]
        public int FinancialYearCount { get; set; }

        [Display(Name = "تعداد سند")]
        public int DocumentCount { get; set; }

        [Display(Name = "تعداد کاربران")]
        public int UserCount { get; set; }

        [Display(Name = "تعداد کاربران اندروید")]
        public int AndroidUserCount { get; set; }
        [Required(ErrorMessage = "(*)")]
        public List<HardwareLockFinancialYearViewModel> FinancialYears { get; set; }
        public bool IsBlocked { get; set; }
        [Required(ErrorMessage = "(*)")]
        public string CurrentVersion { get; set; }
        public DateTime? ExpireDate { get; set; }
        public string OwnerName { get; set; }
    }
}
