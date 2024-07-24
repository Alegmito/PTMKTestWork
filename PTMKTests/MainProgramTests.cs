using Moq;
using PTMKTestWork;
using System.Security.Cryptography.X509Certificates;

namespace PTMKTests
{
  public class MainProgramTests
  {
    private readonly Mock<ITaskRunner> TaskRunnerMock = new Mock<ITaskRunner>();

    [Fact]
    public void RunTask_CallsInitializeDB_WhenFirstIsCalled()
    {
      const int taskNumber = 1;
      TaskRunner.RunTask(TaskRunnerMock.Object, taskNumber, []);

      TaskRunnerMock.Verify(mock => mock.InitializeDB(), Times.Once);
    }

    [Fact]
    public void RunTask_CallsCreateRecord_WhenTwoIsCalled()
    {
      string[] args = [];
      const int taskNumber = 2;
      TaskRunner.RunTask(TaskRunnerMock.Object, taskNumber, args);

      TaskRunnerMock.Verify(mock => mock.CreateRecord(args), Times.Once);
    }

    [Fact]
    public void RunTask_CallsGetAllRectords_WhenThirdIsCalled()
    {
      const int taskNumber = 3;
      TaskRunner.RunTask(TaskRunnerMock.Object, taskNumber, []);

      TaskRunnerMock.Verify(mock => mock.GetAllRecords(), Times.Once);
    }

    [Fact]
    public void RunTask_CallsFillWithRecords_WhenFourthIsCalled()
    {
      const int taskNumber = 4;
      TaskRunner.RunTask(TaskRunnerMock.Object, taskNumber, []);

      TaskRunnerMock.Verify(mock => mock.FillWithRecords(), Times.Once);
    }

    [Fact]
    public void RunTask_CallsGetMales_WhenFirstIsCalled()
    {
      const int taskNumber = 5;
      TaskRunner.RunTask(TaskRunnerMock.Object, taskNumber, []);

      TaskRunnerMock.Verify(mock => mock.GetMalesWithSurnameSartswithF(), Times.Once);
    }


  }
}