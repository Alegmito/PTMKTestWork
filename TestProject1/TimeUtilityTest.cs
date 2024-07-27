using PTMKTestWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTMKTests
{
  [TestClass]
  public class TimeUtilityTest
  {
    [TestMethod]
    public void GetYear_AddsNoYear_WhenSameMonthAndLessDays()
    {
      const int expected = 0;
      var actual = TimeUtility.GetYears(DateTime.Parse("2007-11-02"), DateTime.Parse("2008-11-01"));

      Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void GetYear_AddsYear_WhenSameMonthButMoreDays()
    {
      const int expected = 1;
      var actual = TimeUtility.GetYears(DateTime.Parse("2007-11-02"), DateTime.Parse("2008-11-02"));

      Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void GetYear_AddsYear_WhenBiggerMonth()
    {
      const int expected = 1;
      var actual = TimeUtility.GetYears(DateTime.Parse("2007-10-02"), DateTime.Parse("2008-11-02"));

      Assert.AreEqual(expected, actual);
    }

  }
}
