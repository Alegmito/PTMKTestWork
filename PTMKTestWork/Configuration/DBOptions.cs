using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTMKTestWork.Configuration
{
  internal class DBOptions
  {
    public const string DirectoryDB = "DirectoryDB";

    public string ConnectionString { get; set; } = String.Empty;
  }
}
