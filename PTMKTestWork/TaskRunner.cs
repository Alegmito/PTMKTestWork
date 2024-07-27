using Microsoft.EntityFrameworkCore;
using PTMKTestWork.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PTMKTestWork
{
    public class TaskRunner : ITaskRunner
  {
    public static readonly int NumberOfGeneratedEmployees = 1000000;
    private readonly IEmployeeRepository employeeRepository;
    private readonly DirectoryContext directoryContext;

    public TaskRunner(IEmployeeRepository employeeRepository,
      DirectoryContext directoryContext
      )
    {
      this.employeeRepository = employeeRepository;
      this.directoryContext = directoryContext;
    }
    
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

      for (; bulkNumber * bulkSize <= NumberOfGeneratedEmployees; bulkNumber++)
      {
        var employees = new List<Employee>();

        for (int i = 0; i < bulkSize; i++)
          employees.Add(new Employee()
          {
            ID = Guid.NewGuid(),
            FullName = Convert.ToChar(rnd.Next(0, 26) + 65).ToString(),
            BirthDate = DateOnly.Parse($"{rnd.Next(1960, 2008)}-09-21"),
            Gender = (Gender) rnd.Next(2)
          });
        
        await employeeRepository.AddEmployees(employees);  
      }
    }

    public async Task GetAllEmployeesAsync()
    {
      var employees = await employeeRepository.GetAllEmployeesAsync();
      TaskRunner.WriteEmployees(employees); 
    }

    public async Task GetMalesWithSurnameStartsWithFAsync()
    {
      var sw = Stopwatch.StartNew();
      var employees = await employeeRepository.GetMalesWithSurnameStartsWithFAsync();
      sw.Stop();
      TaskRunner.WriteEmployees(employees);
      Console.WriteLine($"Query completion time is (seconds): {sw.ElapsedMilliseconds / (double)1000}");
    }
    public async Task GetMalesWithSurnameStartsWithFOptimizedAsync()
    {
      var sw = Stopwatch.StartNew();
      var employees = await employeeRepository.GetMalesWithSurnameStartsWithFOptimizedAsync();
      sw.Stop();
      TaskRunner.WriteEmployees(employees);
      Console.WriteLine($"Optimized Query completion time is (seconds): {sw.ElapsedMilliseconds / (double)1000}");
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
        case 6 : taskRunner.GetMalesWithSurnameStartsWithFAsync().Wait();
          break;
        case 5 : taskRunner.GetMalesWithSurnameStartsWithFOptimizedAsync().Wait();
          break;

        default: Console.WriteLine("No such thing is task is present");
          break;
      };

      Console.WriteLine($"Task {task} is completed.");
    }

    public Task FillWithRecordsAsync()
    {
      throw new NotImplementedException();
    }

    public static void WriteEmployees(IEnumerable<Employee> employees)
    {
      foreach (var item in employees)
      {
        Console.WriteLine($"{item.FullName}, BirthDate: {item.BirthDate}," +
          $"Gender: {item.Gender.ToString()}, Age: {item.Age}");
      }
    }
  }

  public interface ITaskRunner
  {
    void InitializeDB();

    //void CreateRecord(string fullName, DateOnly birthDate, Gender gender);
    Task CreateEmployeeAsync(string[] cmdRecordData);

    Task GetAllEmployeesAsync();

    Task FillWithRecordsAsync(int bulkSize = 50000);

    Task GetMalesWithSurnameStartsWithFAsync();
    Task GetMalesWithSurnameStartsWithFOptimizedAsync();
  }
}
