using Microsoft.EntityFrameworkCore;
using PTMKTestWork.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PTMKTestWork
{
  public class TaskRunner : ITaskRunner
  {
    private readonly IEmployeeRepository employeeRepository;
    private readonly DirectoryContext directoryContext;

    public TaskRunner(IEmployeeRepository employeeRepository,
      DirectoryContext directoryContext
      )
    {
      this.employeeRepository = employeeRepository;
      this.directoryContext = directoryContext;
    }

    public TaskRunner() { }
    public async Task CreateEmployeeAsync(string[] cmdRecordData)
    {
      if (cmdRecordData.Length < 4)
        throw new ArgumentException("Not enough arguments");

      await employeeRepository
        .InsertEmployeeAsync(Employee.CreateEmployee(cmdRecordData.Skip(1).ToArray()));
    }

    public async Task FillWithRecordsAsync(int bulkSize = 50000)
    {      
      int bulkNumber = 1;
      Random rnd = new Random();

      for (; bulkNumber * bulkSize <= 1000000; bulkNumber++)
      {
        var employees = new List<Employee>();

        for (int i = 0; i < bulkSize; i++)
          employees.Add(new Employee()
          {
            ID = Guid.NewGuid(),
            FullName = Convert.ToChar(rnd.Next(0, 26) + 65).ToString(),
            BirthDate = DateOnly.Parse("1990-09-21"),
            Gender = (Gender) rnd.Next(1)
          });
        
        await employeeRepository.AddEmployees(employees);  
      }
    }

    public async Task GetAllEmployeesAsync()
    {
      var employees = await employeeRepository.GetAllEmployeesAsync();
      foreach (var item in employees)
      {
        Console.WriteLine($"{item.FullName}, BirthDate: {item.BirthDate}," +
          $"Gender: {item.Gender.ToString()}, Age: {item.Age}");
      }
    }

    public async Task GetMalesWithSurnameSartswithFAsync()
    {
      await employeeRepository.GetMaleEmployeesWithSurnameStartsWithFAsync();
    }

    public void InitializeDB()
    {
      directoryContext.Database.MigrateAsync().Wait();
    }

    public static void RunTask(ITaskRunner taskRunner, int task, string[] args)
    {
      switch (task)
      {
        case 1: taskRunner.InitializeDB();
          break;
        case 2 : taskRunner.CreateEmployeeAsync(args).Wait();
          break;
        case 3 : taskRunner.GetAllEmployeesAsync().Wait();
          break;
        case 4 : taskRunner.FillWithRecordsAsync().Wait();
          break;
        case  5 : taskRunner.GetMalesWithSurnameSartswithFAsync().Wait();
          break;
        default: Console.WriteLine("No such thing is task is present");
          break;
      };
    }

    public Task FillWithRecordsAsync()
    {
      throw new NotImplementedException();
    }
  }

  public interface ITaskRunner
  {
    void InitializeDB();

    //void CreateRecord(string fullName, DateOnly birthDate, Gender gender);
    Task CreateEmployeeAsync(string[] cmdRecordData);

    Task GetAllEmployeesAsync();

    Task FillWithRecordsAsync(int bulkSize = 50000);

    Task GetMalesWithSurnameSartswithFAsync();
  }
}
