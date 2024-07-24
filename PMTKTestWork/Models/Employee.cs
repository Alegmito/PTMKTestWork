using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTMKTestWork.Models
{
  internal class Employee
  {
    [Key]
    public int Id { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "Full Name Can't be empty")]
    [StringLength(150)]
    public string  FullName { get; set; }

    [Required]
    public DateOnly BirthDate { get; set; }
  }
}
