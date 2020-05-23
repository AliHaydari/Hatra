using Hatra.Common.GuardToolkit;
using Hatra.DataLayer.Context;
using Hatra.Entities;
using Hatra.Services.Contracts;
using Hatra.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hatra.Services
{
    public class HardwareLockService : IHardwareLockService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly DbSet<HardwareLock> _hardwareLock;

        public HardwareLockService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _unitOfWork.CheckArgumentIsNull(nameof(_unitOfWork));

            _hardwareLock = _unitOfWork.Set<HardwareLock>();
            _hardwareLock.CheckArgumentIsNull(nameof(_hardwareLock));
        }

        public async Task<HardwareLockResponseViewModel> InsertOrUpdateAsync(HardwareLockViewModel viewModel)
        {

            if (viewModel == null || string.IsNullOrEmpty(viewModel.LockSerialNumber) || string.IsNullOrEmpty(viewModel.CpuSerialNumber) || string.IsNullOrEmpty(viewModel.ComputerName)) return new HardwareLockResponseViewModel { Status = 1 };


            var item = await _hardwareLock.FirstOrDefaultAsync(p => p.LockSerialNumber == viewModel.LockSerialNumber).ConfigureAwait(false);
            if (item == null)
            {
                item = new HardwareLock
                {
                    IsBlocked = false,
                    ComputerName = viewModel.ComputerName,
                    CpuSerialNumber = viewModel.CpuSerialNumber,
                    LockSerialNumber = viewModel.LockSerialNumber,
                    AndroidUserCount = viewModel.AndroidUserCount,
                    CompanyCount = viewModel.CompanyCount,
                    CurrentVersion = viewModel.CurrentVersion,
                    DocumentCount = viewModel.DocumentCount,
                    FinancialYearCount = viewModel.FinancialYearCount,
                    UserCount = viewModel.UserCount,
                };

                var lfy = viewModel.FinancialYears.Select(p => new HardwareLockFinancialYear
                {
                    CompanyId = p.CompanyId,
                    CompanyName = p.CompanyName,
                    FinancialYearId = p.FinancialYearId,
                    FinancialYearName = p.FinancialYearName,
                    IsArchive = p.IsArchive,
                });

            }
            return null;
        }
    }
}
