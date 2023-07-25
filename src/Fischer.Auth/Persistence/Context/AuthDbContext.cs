using Fischer.Auth.Domain.Entities;
using Fischer.Core.Configurations;
using Fischer.Core.Configurations.API;
using Fischer.Core.Constants;
using Fischer.Core.Extensions;
using Fischer.Core.Infraestructure.Audit;
using Fischer.Core.Persistence.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Reflection;

namespace Fischer.Auth.Persistence.Context;
public sealed class AuthDbContext : IdentityDbContext<AppUser, AppRole, Guid, IdentityUserClaim<Guid>, AppUserRole, IdentityUserLogin<Guid>, AppRoleClaim, IdentityUserToken<Guid>>, IUnitOfWork
{
    private readonly IAuditService _audiService;
    private readonly ApiConfigurations _apiConfigurations;
    private readonly AppsettingsGet _appSettings;

    public AuthDbContext(DbContextOptions<AuthDbContext> options, ApiConfigurations apiConfigurations,
        IAuditService auditService, AppsettingsGet appSettings)
        : base(options)
    {
        _audiService = auditService; _apiConfigurations = apiConfigurations;
        _appSettings = appSettings;
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        var audits = _apiConfigurations.EnableAudits ? ChangeTracker.GetAuditEvents() : new List<NewAuditDto>();
        var changedRows = await base.SaveChangesAsync(cancellationToken);
        if (changedRows > 0 && audits.Any())
            await _audiService.HandleAuditAsync(audits);
        return changedRows;
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (_appSettings == null)
            return;
        var connectionString = _appSettings[CoreConstants.ConnectionStringKey];
        optionsBuilder.LogTo(Console.WriteLine, new[] { RelationalEventId.CommandExecuted }).EnableSensitiveDataLogging();
        optionsBuilder.UseNpgsql(connectionString);
    }
    public async Task<bool> CommitAsync(CancellationToken cancellationToken = default)
    {
        var sucesso = await SaveChangesAsync(cancellationToken) > 0;
        return sucesso;
    }
}
