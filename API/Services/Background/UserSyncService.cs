namespace API.Services.Background;

using System.Diagnostics;
using API.Services.Employee;

public sealed class UserSyncService(
    IServiceProvider services,
    ILogger<UserSyncService> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        while (!ct.IsCancellationRequested)
        {
            try
            {
                var sw = Stopwatch.StartNew();
                await SynchronizeUsersAsync();
                sw.Stop();

                logger.LogInformation("User synchronization completed in {Elapsed}", sw.Elapsed);

                await Task.Delay(TimeSpan.FromHours(12), ct);
            }
            catch (OperationCanceledException)
            {
                return;
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error occurred while synchronizing users");
                await Task.Delay(TimeSpan.FromMinutes(5), ct);
            }
        }
    }

    private async Task SynchronizeUsersAsync()
    {
        using var scope = services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var employeeService = scope.ServiceProvider.GetRequiredService<EmployeeService>();
        var identityService = scope.ServiceProvider.GetRequiredService<IdentityService>();

        var employees = await employeeService.GetEmployeesAsync();
        var employeeRoles = await employeeService.GetEmployeeGroupsAsync();
        var employeesToSync = employees.Where(x => employeeRoles.ContainsKey(x.EmployeeId));

        var users = await dbContext.Users.ToDictionaryAsync(x => x.UserName);
        var usersToSuspend = users.Values.Where(x => !employeeRoles.ContainsKey(x.UserName));

        foreach (var employee in employeesToSync)
        {
            if (!users.TryGetValue(employee.EmployeeId, out var user))
            {
                await identityService.CreateUserAsync(employee, employeeRoles[employee.EmployeeId]);
                logger.LogInformation("User {Name} created", employee.Name);
                continue;
            }

            if (employee.IsSuspended && user.IsSuspended)
            {
                continue;
            }

            await identityService.UpdateUserAsync(user, employee,
                employeeRoles[employee.EmployeeId]);
        }

        foreach (var user in usersToSuspend.Where(x => !x.IsSuspended))
        {
            await identityService.SuspendUserAsync(user);
        }
    }
}