using Moq;
using PTMKTestWork;

namespace PTMKTests
{
  [TestClass]
  public class MainProgramTests
  {
    private readonly Mock<ITaskRunner> TaskRunnerMock = new();

    [TestMethod]
    public void RunTask_CallsInitializeDB_WhenFirstIsCalled()
    {
      const int taskNumber = 1;
      TaskRunner.RunTask(TaskRunnerMock.Object, taskNumber, []);

      TaskRunnerMock.Verify(mock => mock.InitializeDB(), Times.Once);
    }

    [TestMethod]
    public void RunTask_CallsCreateRecord_WhenTwoIsCalled()
    {
      string[] args = [];
      const int taskNumber = 2;
      TaskRunner.RunTask(TaskRunnerMock.Object, taskNumber, args);

      TaskRunnerMock.Verify(mock => mock.CreateEmployeeAsync(args), Times.Once);
    }

    [TestMethod]
    public void RunTask_CallsGetAllRectords_WhenThirdIsCalled()
    {
      const int taskNumber = 3;
      TaskRunner.RunTask(TaskRunnerMock.Object, taskNumber, []);

      TaskRunnerMock.Verify(mock => mock.GetAllEmployeesAsync(), Times.Once);
    }

    [TestMethod]
    public void RunTask_CallsFillWithRecords_WhenFourthIsCalled()
    {
      const int taskNumber = 4;
      TaskRunner.RunTask(TaskRunnerMock.Object, taskNumber, []);

      TaskRunnerMock.Verify(mock => mock.FillWithRecordsAsync(50000), Times.Once);
    }

    [TestMethod]
    public void RunTask_CallsGetMalesOptimized_WhenFifthIsCalled()
    {
      const int taskNumber = 5;
      TaskRunner.RunTask(TaskRunnerMock.Object, taskNumber, []);

      TaskRunnerMock.Verify(mock => mock.GetMalesWithSurnameStartsWithFOptimizedAsync(), Times.Once);
    }

    [TestMethod]
    public void RunTask_CallsGetMales_WhenSixthIsCalled()
    {
      const int taskNumber = 6;
      TaskRunner.RunTask(TaskRunnerMock.Object, taskNumber, []);

      TaskRunnerMock.Verify(mock => mock.GetMalesWithSurnameStartsWithFAsync(), Times.Once);
    }


  }
}