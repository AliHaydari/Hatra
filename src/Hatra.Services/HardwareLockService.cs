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
using Microsoft.EntityFrameworkCore.Query.Expressions;

namespace Hatra.Services
{
    public class HardwareLockService : IHardwareLockService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly DbSet<HardwareLock> _hardwareLock;
        private readonly DbSet<HardwareLockFinancialYear> _hardwareLockFinancial;

        public HardwareLockService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _unitOfWork.CheckArgumentIsNull(nameof(_unitOfWork));

            _hardwareLock = _unitOfWork.Set<HardwareLock>();
            _hardwareLock.CheckArgumentIsNull(nameof(_hardwareLock));

            _hardwareLockFinancial = _unitOfWork.Set<HardwareLockFinancialYear>();
            _hardwareLockFinancial.CheckArgumentIsNull(nameof(_hardwareLock));
        }

        public async Task<HardwareLockResponseViewModel> InsertOrUpdateAsync(HardwareLockViewModel viewModel)
        {

            if (viewModel == null || string.IsNullOrEmpty(viewModel.LockSerialNumber) || string.IsNullOrEmpty(viewModel.CpuSerialNumber) || string.IsNullOrEmpty(viewModel.ComputerName)) return new HardwareLockResponseViewModel { Status = 1 };


            try
            {
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
                        FinancialYears = viewModel.FinancialYears.Select(p => new HardwareLockFinancialYear
                        {
                            CompanyId = p.CompanyId,
                            CompanyName = p.CompanyName,
                            FinancialYearId = p.FinancialYearId,
                            FinancialYearName = p.FinancialYearName,
                            IsArchive = p.IsArchive,
                            DbName = p.DbName
                        }).ToList(),
                    };

                    await _hardwareLock.AddAsync(item).ConfigureAwait(false);
                }
                else
                {
                    item.IsBlocked = false;
                    item.ComputerName = viewModel.ComputerName;
                    item.CpuSerialNumber = viewModel.CpuSerialNumber;
                    item.LockSerialNumber = viewModel.LockSerialNumber;
                    item.AndroidUserCount = viewModel.AndroidUserCount;
                    item.CompanyCount = viewModel.CompanyCount;
                    item.CurrentVersion = viewModel.CurrentVersion;
                    item.DocumentCount = viewModel.DocumentCount;
                    item.FinancialYearCount = viewModel.FinancialYearCount;
                    item.UserCount = viewModel.UserCount;

                    var fys = await _hardwareLockFinancial.Where(p => p.HardwareLockId == item.Id).ToListAsync().ConfigureAwait(false);

                    foreach (var model in viewModel.FinancialYears)
                    {
                        var row = fys.FirstOrDefault(p =>
                            p.DbName == model.DbName && p.FinancialYearId == model.FinancialYearId &&
                            p.CompanyId == model.CompanyId);

                        if (row != null)
                        {
                            row.FinancialYearName = model.FinancialYearName;
                            row.CompanyName = model.CompanyName;
                            row.IsArchive = model.IsArchive;
                        }
                        else
                        {
                            row = new HardwareLockFinancialYear
                            {
                                HardwareLockId = item.Id,
                                DbName = model.DbName,
                                CompanyName = model.CompanyName,
                                CompanyId = model.CompanyId,
                                FinancialYearId = model.FinancialYearId,
                                FinancialYearName = model.FinancialYearName,
                                IsArchive = model.IsArchive,
                            };
                            await _hardwareLockFinancial.AddAsync(row).ConfigureAwait(false);
                        }
                    }

                    if (fys.Count > viewModel.FinancialYears.Count)
                    {
                        var toDelete = (from f in fys
                                        join v in viewModel.FinancialYears on f.DbName equals v.DbName into fj
                                        from res in fj.DefaultIfEmpty()
                                        where res == null
                                        select f).ToList();

                        _hardwareLockFinancial.RemoveRange(toDelete);


                    }


                }

                await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);

                return new HardwareLockResponseViewModel
                {
                    Status = 0,
                    ExpDate = item.ExpireDate,
                    IsBlocked = item.IsBlocked
                };

            }
            catch (Exception)
            {
                return new HardwareLockResponseViewModel { Status = 2 };
            }

        }
    }
}
