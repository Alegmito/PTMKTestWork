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
    Task AddEmployees(List<Employee> employees);
    Task<IEnumerable<Employee>> GetAllEmployeesAsync();
    Task<IEnumerable<Employee>> GetMaleEmployeesWithSurnameStartsWithFAsync();

    Task InsertEmployeeAsync(Employee employee);
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

    public async Task<IEnumerable<Employee>> GetAllEmployeesAsync()
    {
      return await _context.Employees.AsQueryable().OrderBy(emp => emp.FullName).ToListAsync();
    }

    public async Task<IEnumerable<Employee>> GetMaleEmployeesWithSurnameStartsWithFAsync()
    {
      return await _context.Employees.AsQueryable().Where(e => e.FullName.StartsWith('F')).Where(e=>e.Gender.Equals(Gender.Male)).ToListAsync();
    }

    public async Task InsertEmployeeAsync(Employee employee)
    {
      await _context.Employees.AddAsync(employee);
      await _context.SaveChangesAsync();
    }

    public async Task AddEmployees(List<Employee> employees)
    {
      await _context.AddRangeAsync(employees);
      await _context.SaveChangesAsync();
    }
  }
}
