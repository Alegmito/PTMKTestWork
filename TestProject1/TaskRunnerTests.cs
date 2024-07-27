using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Moq;
using PTMKTestWork;
using PTMKTestWork.Models;
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
    private readonly Mock<DirectoryContext> directoryContextMock = new();
    private readonly Mock<IEmployeeRepository> employeeRepositoryMock = new();

    public TaskRunnerTests()
    {
      taskRunner = new TaskRunner(employeeRepositoryMock.Object,
        directoryContextMock.Object);
    }

    [TestMethod]
    public async Task CreateRecord_SendsNewEmployee()
    {
      employeeRepositoryMock.Setup(mock => mock.InsertEmployeeAsync(It.IsAny<Employee>())).Returns(Task.CompletedTask);
      var args = new string[] { "2", "Ivanov Petr Sergeevich", "2009-07-12", "Male" };
      var employee = Employee.CreateEmployee(args.Skip(1).ToArray());
      await Task.Run(async () => await taskRunner.CreateEmployeeAsync(args));


      employeeRepositoryMock.Verify(mock => mock.InsertEmployeeAsync(It.Is<Employee>(emp =>
        emp.FullName == employee.FullName &&
        emp.Gender == employee.Gender &&
        emp.BirthDate == employee.BirthDate
      )));
    }

    [TestMethod]
    public async Task GetAllEmployees_OutputsAllEmployees()
    {
      StringWriter writer = new();
      Console.SetOut(writer);

      await Task.Run(async () => await taskRunner.GetAllEmployeesAsync());

      employeeRepositoryMock.Verify(mock => mock.GetAllEmployeesAsync(), Times.Once);
    }

    [TestMethod]
    public async Task GenerateEmployees_GeneratesOneMillRecords()
    {
      const int employeeBulkSize = 50000;
      const int AddEmployeesEnvocationTimes = 1000000 / employeeBulkSize;
      await Task.Run(async () => await taskRunner.FillWithRecordsAsync(employeeBulkSize));
      employeeRepositoryMock.Verify(mock => mock.AddEmployees(It.IsAny<List<Employee>>()), Times.Exactly(AddEmployeesEnvocationTimes));
    }

    [TestMethod]
    public async Task GetMaleEmployeesNameStartsWithF_GetsEmployeesAndBenchmark()
    {
      StringWriter writer = new();
      Console.SetOut(writer);

      await Task.Run(async () => await taskRunner.GetMalesWithSurnameStartsWithFAsync());

      employeeRepositoryMock.Verify(mock => mock.GetMalesWithSurnameStartsWithFAsync(), Times.Once);
      var linesCount = writer.ToString().SplitToLines().Where(line => line != "").Count();
      writer.ToString().Contains("Query completion time is");
    }

    [TestMethod]
    public void WriteEmployee_OuputsEmployees()
    {
      const int employeeCount = 10;
      var employees = new List<Employee>();
      for (int i = 0; i < employeeCount; i++)
        employees.Add(new Employee());
      
      StringWriter writer = new();
      Console.SetOut(writer);
      TaskRunner.WriteEmployees(employees);
      var linesCount = writer.ToString().SplitToLines().Where(line => line != "").Count();
      Assert.AreEqual(employeeCount, linesCount);  
    }
  }
}
