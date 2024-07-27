using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTMKTestWork.Models
{

  public class DirectoryContext : DbContext
  {

    // For testing purposes
    public DirectoryContext() { }
    public DirectoryContext(DbContextOptions<DirectoryContext> options)
      : base(options)
    { }

    public DbSet<Employee> Employees { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<Employee>().ToTable("Employee");
    }

  }

  public class Employee
  {
    [Key]
    public Guid ID { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "Full Name Can't be empty")]
    [StringLength(150)]
    public string  FullName { get; set; }

    [Required]
    public DateOnly BirthDate { get; set; }

    [Required]
    public Gender Gender { get; set; }

    public int Age { get => TimeUtility.GetYears(BirthDate.ToDateTime(TimeOnly.MinValue), DateTime.Now); }

    public static Employee CreateEmployee(string[] employeeData)
    {
      return new()
      {
        ID = Guid.NewGuid(),
        FullName = employeeData[0],
        BirthDate = DateOnly.Parse(employeeData[1]),
        Gender = Enum.Parse<Gender>(employeeData[2])
      };
    }
  }

  public enum Gender
  {
    Male,
    Female
  }
}
