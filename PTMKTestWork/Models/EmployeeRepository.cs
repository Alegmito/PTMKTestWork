using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PTMKTestWork.Models
{
    public interface IEmployeeRepository : IDisposable
    {
        Task AddEmployees(List<Employee> employees);
        Task<IEnumerable<Employee>> GetAllEmployeesAsync();
        Task<IEnumerable<Employee>> GetMalesWithSurnameStartsWithFAsync();
        Task<IEnumerable<Employee>> GetMalesWithSurnameStartsWithFOptimizedAsync();

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
            if (!disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            disposed = true;
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

        public async Task<IEnumerable<Employee>> GetMalesWithSurnameStartsWithFAsync()
        {
            return (await _context.Employees.AsQueryable().ToListAsync()).Where(emp => emp.FullName.StartsWith('F') && emp.Gender.Equals(Gender.Male));
        }

        public async Task<IEnumerable<Employee>> GetMalesWithSurnameStartsWithFOptimizedAsync()
        {
            return await _context.Employees.AsQueryable().Where(e => EF.Functions.Like(e.FullName, "F%"))
              .Where(e => e.Gender.Equals(Gender.Male)).ToListAsync();
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
