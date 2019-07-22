using Hatra.Common.GuardToolkit;
using Hatra.Common.PersianToolkit;
using Hatra.Entities.AuditableEntity;
using Hatra.Entities.Identity;
using Hatra.ViewModels.Identity.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using System.Threading;
using System;
using Hatra.Entities;
using Hatra.DataLayer.Mappings;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using EFSecondLevelCache.Core;
using EFSecondLevelCache.Core.Contracts;
using Hatra.Entities.Localization;

namespace Hatra.DataLayer.Context
{
    /// <summary>
    /// More info: http://www.dotnettips.info/post/2577
    /// and http://www.dotnettips.info/post/2578
    /// plus http://www.dotnettips.info/post/2491
    /// </summary>
    public class ApplicationDbContext :
        IdentityDbContext<User, Role, int, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>,
        IUnitOfWork
    {
        // we can't use constructor injection anymore, because we are using the `AddDbContextPool<>`
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        #region BaseClass

        public virtual DbSet<AppLogItem> AppLogItems { get; set; }
        public virtual DbSet<AppSqlCache> AppSqlCache { get; set; }
        public virtual DbSet<AppDataProtectionKey> AppDataProtectionKeys { get; set; }

        public void AddRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
        {
            Set<TEntity>().AddRange(entities);
        }

        public void ExecuteSqlCommand(string query)
        {
            Database.ExecuteSqlCommand(query);
        }

        public void ExecuteSqlCommand(string query, params object[] parameters)
        {
            Database.ExecuteSqlCommand(query, parameters);
        }

        public T GetShadowPropertyValue<T>(object entity, string propertyName) where T : IConvertible
        {
            var value = this.Entry(entity).Property(propertyName).CurrentValue;
            return value != null
                ? (T)Convert.ChangeType(value, typeof(T), CultureInfo.InvariantCulture)
                : default(T);
        }

        public object GetShadowPropertyValue(object entity, string propertyName)
        {
            return this.Entry(entity).Property(propertyName).CurrentValue;
        }

        public void MarkAsChanged<TEntity>(TEntity entity) where TEntity : class
        {
            Update(entity);
        }

        public void RemoveRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
        {
            Set<TEntity>().RemoveRange(entities);
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            ChangeTracker.DetectChanges(); //NOTE: changeTracker.Entries<T>() will call it automatically.

            beforeSaveTriggers();

            var changedEntityNames = this.GetChangedEntityNames();

            ChangeTracker.AutoDetectChangesEnabled = false; // for performance reasons, to avoid calling DetectChanges() again.
            var result = base.SaveChanges(acceptAllChangesOnSuccess);
            ChangeTracker.AutoDetectChangesEnabled = true;

            this.GetService<IEFCacheServiceProvider>().InvalidateCacheDependencies(changedEntityNames);

            return result;
        }

        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges();

            beforeSaveTriggers();

            var changedEntityNames = this.GetChangedEntityNames();

            ChangeTracker.AutoDetectChangesEnabled = false; // for performance reasons, to avoid calling DetectChanges() again.
            var result = base.SaveChanges();
            ChangeTracker.AutoDetectChangesEnabled = true;

            this.GetService<IEFCacheServiceProvider>().InvalidateCacheDependencies(changedEntityNames);

            return result;
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            ChangeTracker.DetectChanges();

            beforeSaveTriggers();

            var changedEntityNames = this.GetChangedEntityNames();

            ChangeTracker.AutoDetectChangesEnabled = false; // for performance reasons, to avoid calling DetectChanges() again.
            var result = base.SaveChangesAsync(cancellationToken);
            ChangeTracker.AutoDetectChangesEnabled = true;

            this.GetService<IEFCacheServiceProvider>().InvalidateCacheDependencies(changedEntityNames);

            return result;
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken())
        {
            ChangeTracker.DetectChanges();

            beforeSaveTriggers();

            var changedEntityNames = this.GetChangedEntityNames();

            ChangeTracker.AutoDetectChangesEnabled = false; // for performance reasons, to avoid calling DetectChanges() again.
            var result = base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
            ChangeTracker.AutoDetectChangesEnabled = true;

            this.GetService<IEFCacheServiceProvider>().InvalidateCacheDependencies(changedEntityNames);

            return result;
        }

        private void beforeSaveTriggers()
        {
            validateEntities();
            setShadowProperties();
            this.ApplyCorrectYeKe();
        }

        private void setShadowProperties()
        {
            // we can't use constructor injection anymore, because we are using the `AddDbContextPool<>`
            var httpContextAccessor = this.GetService<IHttpContextAccessor>();
            httpContextAccessor.CheckArgumentIsNull(nameof(httpContextAccessor));
            ChangeTracker.SetAuditableEntityPropertyValues(httpContextAccessor);
        }

        private void validateEntities()
        {
            var errors = this.GetValidationErrors();
            if (!string.IsNullOrWhiteSpace(errors))
            {
                // we can't use constructor injection anymore, because we are using the `AddDbContextPool<>`
                var loggerFactory = this.GetService<ILoggerFactory>();
                loggerFactory.CheckArgumentIsNull(nameof(loggerFactory));
                var logger = loggerFactory.CreateLogger<ApplicationDbContext>();
                logger.LogError(errors);
                throw new InvalidOperationException(errors);
            }
        }

        #endregion

        public virtual DbSet<Brand> Brands { get; set; }
        public virtual DbSet<Category> Categories { set; get; }
        public virtual DbSet<Folder> Folders { set; get; }
        public virtual DbSet<Menu> Menus { set; get; }
        public virtual DbSet<Page> Pages { get; set; }
        public virtual DbSet<PageImage> PageImages { get; set; }
        public virtual DbSet<Picture> Pictures { get; set; }
        public virtual DbSet<SlideShow> SlideShows { get; set; }
        public virtual DbSet<ContactUs> ContactUses { get; set; }
        public virtual DbSet<UsefulLink> UsefulLinks { get; set; }
        public virtual DbSet<StaticContent> StaticContents { get; set; }
        public virtual DbSet<VisitorsStatistics> VisitorsStatistics { get; set; }
        public virtual DbSet<Language> Languages { get; set; }
        public virtual DbSet<LocaleStringResource> LocaleStringResources { get; set; }
        public virtual DbSet<LocalizedProperty> LocalizedProperties { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // it should be placed here, otherwise it will rewrite the following settings!
            base.OnModelCreating(builder);
            // we can't use constructor injection anymore, because we are using the `AddDbContextPool<>`
            var siteSettings = this.GetService<IOptionsSnapshot<SiteSettings>>();
            siteSettings.CheckArgumentIsNull(nameof(siteSettings));
            siteSettings.Value.CheckArgumentIsNull(nameof(siteSettings.Value));
            // Adds all of the ASP.NET Core Identity related mappings at once.
            builder.AddCustomIdentityMappings(siteSettings.Value);

            // Custom application mappings
            builder.AddCustomEntityMappings();

            // This should be placed here, at the end.
            builder.AddAuditableShadowProperties();
        }
    }
}