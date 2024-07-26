// See https://aka.ms/new-console-template for more information
using PTMKTestWork;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using PTMKTestWork.Models;
using PTMKTestWork.Configuration;
if (args.Length < 0)
{
  Console.WriteLine("world!");
  return;
}

var configuration = new ConfigurationBuilder()
  .AddUserSecrets<Program>()
  .Build();

var connstr = configuration["DirectoryDB:ConnectionString"];

var services = new ServiceCollection()
  .AddDbContext<DirectoryContext>(options =>
options.UseSqlServer(connstr))
  .AddScoped<ITaskRunner, TaskRunner>()
  .AddScoped<IEmployeeRepository, EmployeeRepository>()
  .Configure<DBOptions>(configuration.GetSection(DBOptions.DirectoryDB))
  .BuildServiceProvider();

int task = 0;
int.TryParse(args[0], out task);

using (var scope = services.CreateScope())
{
  scope.ServiceProvider.GetService<DirectoryContext>().Database.EnsureCreated();

  TaskRunner.RunTask(
  scope.ServiceProvider.GetRequiredService<ITaskRunner>(),
  task,
  args);
}
