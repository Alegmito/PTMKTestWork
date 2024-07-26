using FluentMigrator.Runner;
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
    public void CreateRecord(string[] cmdRecordData)
    {
      throw new NotImplementedException();
    }

    public void FillWithRecords()
    {
      throw new NotImplementedException();
    }

    public void GetAllRecords()
    {
      throw new NotImplementedException();
    }

    public void GetMalesWithSurnameSartswithF()
    {
      throw new NotImplementedException();
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
        case 2 : taskRunner.CreateRecord(args);
          break;
        case 3 : taskRunner.GetAllRecords();
          break;
        case 4 : taskRunner.FillWithRecords();
          break;
        case  5 : taskRunner.GetMalesWithSurnameSartswithF();
          break;
        default: Console.WriteLine("No such thing is task is present");
          break;
      };
    }
  }

  public interface ITaskRunner
  {
    void InitializeDB();

    //void CreateRecord(string fullName, DateOnly birthDate, Gender gender);
    void CreateRecord(string[] cmdRecordData);


    void GetAllRecords();

    void FillWithRecords();

    void GetMalesWithSurnameSartswithF();
  }

  internal enum Gender
  {
    Male,
    Female
  }
}
