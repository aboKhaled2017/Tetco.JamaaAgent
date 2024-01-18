using System.Globalization;

namespace Domain.Common.Utilities
{
    public static class PrimitiveExtensions
    {
        public static DateTime? ConvertNullableDateTime(this string str)
        {
            if (str == null || str.ToUpper() == "NULL")
            {
                return null;
            }

            if (DateTime.TryParseExact(str, "M/d/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result) ||
        DateTime.TryParseExact(str, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out result))
            {
                return result;
            }

            return null;
        }
        public static DateTime ConvertDateTime(this string str)
        {
            if (DateTime.TryParseExact(str, "M/d/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result) ||
        DateTime.TryParseExact(str, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out result))
            {
                return result;
            }
            return DateTime.MinValue;
        }
        public static int? ConvertNullableNumber(this string str)
        {
            if (str == null || str.ToUpper() == "NULL")
            {
                return null;
            }

            if (int.TryParse(str, out int result))
            {
                return result;
            }

            return null;
        }
        public static int ConvertNumber(this string str)
        {
            if (int.TryParse(str, out int result))
            {
                return result;
            }

            return 0;
        }

        public static decimal ConvertDecimal(this string str)
        {
            if (decimal.TryParse(str, out decimal result))
            {
                return result;
            }

            return 0;
        }
        public static decimal? ConvertNullableDecimal(this string str)
        {
            if (str == null || str.ToUpper() == "NULL")
            {
                return null;
            }
            if (decimal.TryParse(str, out decimal result))
            {
                return result;
            }
            return null;
        }
        public static bool? ConvertNullableBoolean(this string str)
        {
            if (str == null || str.ToUpper() == "NULL")
            {
                return null;
            }
            if (bool.TryParse(str, out bool result))
            {
                return result;
            }
            if (str == "1")
            {
                return true;
            }
            else if (str == "0")
            {
                return false;
            }
            return null;
        }
        public static bool? ConvertBoolean(this string str)
        {
            if (bool.TryParse(str, out bool result))
            {
                return result;
            }
            if (str == "1")
            {
                return true;
            }
            return false;
        }


    }
}
