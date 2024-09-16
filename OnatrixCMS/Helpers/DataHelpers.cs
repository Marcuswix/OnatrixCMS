using System.Globalization;
using static Umbraco.Cms.Core.Constants.HttpContext;

namespace OnatrixCMS.Helpers
{
    public class DataHelpers()
    {
        public static string ConvertMonthNumberToMonthText(string projectDate)
        {
            if(!string.IsNullOrEmpty(projectDate))
            {
                string dateFormatted = projectDate.Substring(0, 9).Trim();

                CultureInfo provider = new CultureInfo("en-US");

                DateTime dateDate = DateTime.ParseExact(dateFormatted, "M/d/yyyy", provider);

                int day = dateDate.Day;
                string month = dateDate.Month.ToString();
                int year = dateDate.Year;
                string monthName = "";

                switch (month)
                {
                    case "1":
                        monthName = "Jan";
                        break;
                    case "2":
                        monthName = "Feb";
                        break;
                    case "3":
                        monthName = "March";
                        break;
                    case "4":
                        monthName = "April";
                        break;
                    case "5":
                        monthName = "May";
                        break;
                    case "6":
                        monthName = "June";
                        break;
                    case "7":
                        monthName = "July";
                        break;
                    case "8":
                        monthName = "Aug";
                        break;
                    case "9":
                        monthName = "Sep";
                        break;
                    case "10":
                        monthName = "Oct";
                        break;
                    case "11":
                        monthName = "Nov";
                        break;
                    case "12":
                        monthName = "Dec";
                        break;
                    default:
                        monthName = "N/A";
                        break;
                }

                string dateOnPage = monthName + " " + day + ", " + year;

                return dateOnPage;
            }

            return "";
        }     
	}
}
