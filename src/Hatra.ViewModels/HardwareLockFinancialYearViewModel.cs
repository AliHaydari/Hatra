using Hatra.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Hatra.ViewModels
{
    public class HardwareLockFinancialYearViewModel
    {
        public HardwareLockFinancialYearViewModel()
        {

        }

        public HardwareLockFinancialYearViewModel(HardwareLockFinancialYear hardwareLockFinancialYear)
        {
            Id = hardwareLockFinancialYear.Id;
            CompanyId = hardwareLockFinancialYear.CompanyId;
            CompanyName = hardwareLockFinancialYear.CompanyName;
            FinancialYearId = hardwareLockFinancialYear.FinancialYearId;
            FinancialYearName = hardwareLockFinancialYear.FinancialYearName;
            IsArchive = hardwareLockFinancialYear.IsArchive;
        }

        [HiddenInput]
        public int Id { get; set; }
        [Required(ErrorMessage = "(*)")]
        public Guid CompanyId { get; set; }
        [Required(ErrorMessage = "(*)")]
        public Guid FinancialYearId { get; set; }

        [Required(ErrorMessage = "(*)")]
        [Display(Name = "نام شرکت")]
        [StringLength(100, ErrorMessage = "{0} باید حداقل {2} و حداکثر {1} حرف باشند.", MinimumLength = 2)]
        public string CompanyName { get; set; }

        [Required(ErrorMessage = "(*)")]
        [Display(Name = "نام سال مالی")]
        [StringLength(100, ErrorMessage = "{0} باید حداقل {2} و حداکثر {1} حرف باشند.", MinimumLength = 2)]
        public string FinancialYearName { get; set; }

        public bool IsArchive { get; set; }
        [Required(ErrorMessage = "(*)")]
        [Display(Name = "نام پایگاه داده")]
        [StringLength(100, ErrorMessage = "{0} باید حداقل {2} و حداکثر {1} حرف باشند.", MinimumLength = 2)]
        public string DbName { get; set; }

    }
}
