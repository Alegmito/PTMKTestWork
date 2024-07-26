using Microsoft.EntityFrameworkCore;
using PTMKTestWork.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PTMKTestWork
{
  public interface IEmployeeRepository : IDisposable
  {
    Task<IEnumerable<Employee>> GetAllEmployees();
    Task<IEnumerable<Employee>> GetMaleEmployeesWithSurnameStartsWithF();

    Task InsertEmployee(Employee employee);
  }

  public class EmployeeRepository : IEmployeeRepository, IDisposable
  {
    private readonly DirectoryContext _context;

    public EmployeeRepository(DirectoryContext context)
    {
      _context = context;
    }

    private bool disposed = false;
    public virtual void Dispose(bool disposing)
    {
      if (!this.disposed)
      {
        if (disposing)
        {
          _context.Dispose();
        }
      }
      this.disposed = true;
    }

    public void Dispose()
    {
      Dispose(true);
      GC.SuppressFinalize(this);
    }

    public async Task<IEnumerable<Employee>> GetAllEmployees()
    {
      return await _context.Employees.AsQueryable().ToListAsync();
    }

    public async Task<IEnumerable<Employee>> GetMaleEmployeesWithSurnameStartsWithF()
    {
      return await _context.Employees.AsQueryable().Where(e => e.FullName.StartsWith('F')).Where(e=>e.Gender.Equals(Gender.Male)).ToListAsync();
    }

    public async Task InsertEmployee(Employee employee)
    {
      throw new NotImplementedException();
    }
  }
}
