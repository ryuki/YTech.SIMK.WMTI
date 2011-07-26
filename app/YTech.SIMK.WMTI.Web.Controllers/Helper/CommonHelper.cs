using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Mvc;
using YTech.SIMK.WMTI.Enums;
using YTech.SIMK.WMTI.Core;

namespace YTech.SIMK.WMTI.Web.Controllers.Helper
{
    public class CommonHelper
    {
        private const string CONST_FACTURFORMAT = "ERHA/[TRANS]/[YEAR]/[MONTH]/[DAY]/[XXX]";
        public const string CONST_VOUCHERNO = "ERHA/VOUCHER/[YEAR]/[MONTH]/[DAY]/[XXX]";

        public static string DateFormat
        {
            get { return "dd-MMM-yyyy"; }
        }
        public static string DateTimeFormat
        {
            get { return "dd-MMM-yyyy HH:mm"; }
        }
        public static string TimeFormat
        {
            get { return "HH:mm"; }
        }
        public static string NumberFormat
        {
            get { return "N2"; }
        }

        //set culture to id-ID for standardization
        public static CultureInfo DefaultCulture
        {
            get { return CultureInfo.GetCultureInfo("id-ID"); }
        }
        /// <summary>
        /// get list of enum for jqgrid combobox
        /// </summary>
        /// <typeparam name="T">type of enum</typeparam>
        /// <param name="defaultText">default text for display</param>
        /// <returns>string</returns>
        public static string GetEnumListForGrid<T>(string defaultText)
        {
            if (!typeof(T).IsEnum)
            {
                throw new Exception("Type object must enum");
            }
            var lists = from T e in Enum.GetValues(typeof(T))
                        select new { ID = e, Name = e.ToString() };
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0}:{1};", string.Empty, defaultText);

            for (int i = 0; i < lists.Count(); i++)
            {
                var obj = lists.ElementAt(i);
                sb.AppendFormat("{0}:{1}", obj.ID, obj.Name);
                if (i < lists.Count() - 1)
                    sb.Append(";");
            }
            return (sb.ToString());
        }

        /// <summary>
        /// Will get the string value for a given enums value, this will
        /// only work if you assign the StringValue attribute to
        /// the items in your enum.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetStringValue(Enum value)
        {
            // Get the type
            Type type = value.GetType();

            // Get fieldinfo for this type
            FieldInfo fieldInfo = type.GetField(value.ToString());

            // Get the stringvalue attributes
            StringValueAttribute[] attribs = fieldInfo.GetCustomAttributes(
                typeof(StringValueAttribute), false) as StringValueAttribute[];

            // Return the first if there was a match.
            return attribs.Length > 0 ? attribs[0].StringValue : value.ToString();
        }

        public static decimal? ConvertToDecimal(string value)
        {
            decimal? result = null;
            if (!string.IsNullOrEmpty(value))
                result = decimal.Parse(value, NumberStyles.Number, DefaultCulture);
            return result;
        }

        public static string ConvertToString(decimal? value)
        {
            string result = string.Empty;
            if (value.HasValue)
                result = value.Value.ToString(NumberFormat, DefaultCulture);
            return result;
        }

        public static DateTime? ConvertToDate(string value)
        {
            DateTime? result = null;
            if (!string.IsNullOrEmpty(value))
                result = DateTime.Parse(value, DefaultCulture, DateTimeStyles.AllowWhiteSpaces);
            return result;
        }

        public static string ConvertToString(DateTime? value)
        {
            string result = string.Empty;
            if (value.HasValue)
                result = value.Value.ToString(DateFormat, DefaultCulture);
            return result;
        }
    }
}
