using FluentMigrator.Runner;
using Moq;
using PTMKTestWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTMKTests
{
  [TestClass]
  public class TaskRunnerTests
  {
    private readonly TaskRunner taskRunner;
    private readonly Mock<IMigrationRunner> migrationRunnerMock = new Mock<IMigrationRunner>();
    private readonly Mock<IEmployeeRepository> employeeRepositoryMock = new Mock<IEmployeeRepository>();

    public TaskRunnerTests()
    {
      taskRunner = new TaskRunner(employeeRepositoryMock.Object,
        migrationRunnerMock.Object);
    }

    [TestMethod]
    public void InitializeDB_ActivatesMigrationRunner()
    {
      taskRunner.InitializeDB();

      migrationRunnerMock.Verify(m => m.MigrateUp(), Times.Once);
    }
  }
}
