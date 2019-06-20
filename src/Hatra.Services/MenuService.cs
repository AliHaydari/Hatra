using EFSecondLevelCache.Core;
using Hatra.Common.GuardToolkit;
using Hatra.DataLayer.Context;
using Hatra.Entities;
using Hatra.Services.Contracts;
using Hatra.ViewModels;
using Hatra.ViewModels.Paged;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hatra.Services
{
    public class MenuService : IMenuService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly DbSet<Menu> _menus;

        private const string PageAddress = "/page/";
        private const string CategoryAddress = "/category/";

        public MenuService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _unitOfWork.CheckArgumentIsNull(nameof(_unitOfWork));

            _menus = _unitOfWork.Set<Menu>();
            _menus.CheckArgumentIsNull(nameof(_menus));
        }

        public async Task<List<MenuViewModel>> GetAllAsync()
        {
            return await (from menu in _menus
                          join parent in _menus on menu.ParentId equals
                              parent.Id into parent
                          from menuParent in parent.DefaultIfEmpty()
                          orderby menu.ParentId, menu.Order
                          select new MenuViewModel()
                          {
                              Id = menu.Id,
                              Name = menu.Name,
                              Link = menu.Link,
                              ParentId = menuParent.Id,
                              ParentName = menuParent.Name,
                              Order = menu.Order,
                              Type = menu.Type,
                              IsShow = menu.IsShow,
                              IsMegaMenu = menu.IsMegaMenu,
                          })
                .Cacheable()
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<PagedAdminMenuViewModel> GetAllPagedAsync(int pageNumber, int recordsPerPage)
        {
            var skipRecords = pageNumber * recordsPerPage;

            var query = (from menu in _menus
                         join parent in _menus on menu.ParentId equals
                             parent.Id into parent
                         from menuParent in parent.DefaultIfEmpty()
                         orderby menu.ParentId, menu.Order
                         select new MenuViewModel()
                         {
                             Id = menu.Id,
                             Name = menu.Name,
                             Link = menu.Link,
                             ParentId = menuParent.Id,
                             ParentName = menuParent.Name,
                             Order = menu.Order,
                             Type = menu.Type,
                             IsShow = menu.IsShow,
                             IsMegaMenu = menu.IsMegaMenu,
                         })
                .AsNoTracking();

            return new PagedAdminMenuViewModel()
            {
                Paging =
                {
                    TotalItems = await query.CountAsync(),
                },

                MenuViewModels = await query.Skip(skipRecords).Take(recordsPerPage).ToListAsync(),
            };
        }

        public async Task<List<DropDownMenuViewModel>> GetAllParentAsync()
        {
            var result = await (from menu in _menus
                                join parent in _menus on menu.ParentId equals
                                    parent.Id into parent
                                from menuParent in parent.DefaultIfEmpty()
                                orderby menu.ParentId, menu.Order
                                select new MenuViewModel()
                                {
                                    Id = menu.Id,
                                    Name = menu.Name,
                                    Link = menu.Link,
                                    ParentId = menuParent.Id,
                                    ParentName = menuParent.Name,
                                    Order = menu.Order,
                                    Type = menu.Type,
                                    IsShow = menu.IsShow,
                                    IsMegaMenu = menu.IsMegaMenu,
                                })
                .Cacheable()
                .AsNoTracking()
                .ToListAsync();

            var menus = new List<DropDownMenuViewModel>();

            foreach (var menu in result)
            {
                if (menu.ParentId.HasValue)
                {
                    if (menus.Any(p => p.Id == menu.ParentId))
                    {
                        menus.Add(new DropDownMenuViewModel()
                        {
                            Id = menu.Id,
                            Name = menus.FirstOrDefault(p => p.Id == menu.ParentId)?.Name + @" // " + menu.Name,
                        });
                    }
                    else
                    {
                        menus.Add(new DropDownMenuViewModel()
                        {
                            Id = menu.Id,
                            Name = menu.Name + " < " + menu.ParentName,
                        });
                    }
                }
                else
                {
                    menus.Add(new DropDownMenuViewModel()
                    {
                        Id = menu.Id,
                        Name = menu.Name,
                    });
                }

            }

            return menus;
        }

        public async Task<MenuViewModel> GetByIdAsync(int id)
        {
            var entity = await _menus.FirstOrDefaultAsync(p => p.Id == id);

            if (entity != null)
            {
                return new MenuViewModel(entity);
            }

            return null;
        }

        public async Task<bool> InsertAsync(MenuViewModel viewModel)
        {
            bool isShow;

            if (await CheckIsShowAvailableMainMenu())
            {
                isShow = viewModel.IsShow;
            }
            else
            {
                isShow = false;
            }

            if (viewModel.CategoryId.HasValue && viewModel.CategoryId != 0)
            {
                var slugUrl = viewModel.CategorySlugUrl == null ? "" : $@"/{viewModel.CategorySlugUrl}";
                viewModel.Link = CategoryAddress + viewModel.CategoryId + slugUrl;
            }
            else if (viewModel.PageId.HasValue && viewModel.PageId != 0)
            {
                var slugUrl = viewModel.PageSlugUrl == null ? "" : $@"/{viewModel.PageSlugUrl}";
                viewModel.Link = PageAddress + viewModel.PageId + slugUrl;
            }

            var entity = new Menu()
            {
                Id = viewModel.Id,
                Name = viewModel.Name,
                Link = viewModel.Link ?? "#",
                ParentId = viewModel.ParentId,
                Order = viewModel.Order,
                Type = viewModel.Type,
                IsShow = isShow,
                IsMegaMenu = !viewModel.ParentId.HasValue && viewModel.IsMegaMenu,
            };

            await _menus.AddAsync(entity);
            var result = await _unitOfWork.SaveChangesAsync();
            return result != 0;
        }

        public async Task<bool> UpdateAsync(MenuViewModel viewModel)
        {
            var entity = await _menus.FirstOrDefaultAsync(p => p.Id == viewModel.Id);

            if (entity != null)
            {
                bool isShow;

                if (await CheckIsShowAvailableMainMenu())
                {
                    isShow = viewModel.IsShow;
                }
                else
                {
                    isShow = false;
                }

                if (viewModel.CategoryId.HasValue && viewModel.CategoryId != 0)
                {
                    var slugUrl = viewModel.CategorySlugUrl == null ? "" : $@"/{viewModel.CategorySlugUrl}";
                    viewModel.Link = CategoryAddress + viewModel.CategoryId + slugUrl;
                }
                else if (viewModel.PageId.HasValue && viewModel.PageId != 0)
                {
                    var slugUrl = viewModel.PageSlugUrl == null ? "" : $@"/{viewModel.PageSlugUrl}";
                    viewModel.Link = PageAddress + viewModel.PageId + slugUrl;
                }

                entity.Name = viewModel.Name;
                entity.Link = viewModel.Link ?? "#";
                entity.ParentId = viewModel.ParentId;
                entity.Order = viewModel.Order;
                entity.Type = viewModel.Type;
                entity.IsShow = isShow;
                entity.IsMegaMenu = !viewModel.ParentId.HasValue && viewModel.IsMegaMenu;

                var result = await _unitOfWork.SaveChangesAsync();
                return result != 0;
            }

            return await Task.FromResult(false);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _menus.FirstOrDefaultAsync(p => p.Id == id);

            if (entity != null)
            {
                _menus.Remove(entity);
                var result = await _unitOfWork.SaveChangesAsync();
                return result != 0;
            }

            return await Task.FromResult(false);
        }

        public async Task<bool> CheckExistAsync(int id)
        {
            return await _menus.AnyAsync(p => p.Id == id);
        }

        public async Task<bool> CheckExistNameAsync(int? id, string name)
        {
            return id == null
                ? await _menus.AnyAsync(p => p.Name == name)
                : await _menus.AnyAsync(p => p.Id != id && p.Name == name);
        }

        public async Task<bool> CheckExistRelationAsync(int id)
        {
            var result = await _menus
                .Include(p => p.SubMenus)
                .Where(p => p.Id == id)
                .AnyAsync(p => p.SubMenus.Any());

            return await Task.FromResult(result);
        }

        private async Task<bool> CheckIsShowAvailableMainMenu()
        {
            var items = await _menus
                .CountAsync(p => p.IsShow == true && p.ParentId == null);

            if (items > 7) return false;

            return true;
        }

        public async Task<int> GetNextOrder()
        {
            return (await _menus.MaxAsync(p => (int?)p.Order)).GetValueOrDefault() + 1;
        }
    }
}
