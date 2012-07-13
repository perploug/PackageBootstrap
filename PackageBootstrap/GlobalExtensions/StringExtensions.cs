using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Text;

namespace PackageBootstrap.Extensions
{
    public static class StringExtensions
    {
        public static string Replace(this string original, string oldValue, string newValue, StringComparison comparisionType)
        {
            if (oldValue == null)
                throw new ArgumentNullException("oldValue");
            if (newValue == null)
                throw new ArgumentNullException("newValue");

            var result = original;

            if (oldValue != newValue)
            {
                int index = -1;
                int lastIndex = 0;

                var buffer = new StringBuilder();

                while ((index = original.IndexOf(oldValue, index + 1, comparisionType)) >= 0)
                {
                    buffer.Append(original, lastIndex, index - lastIndex);
                    buffer.Append(newValue);

                    lastIndex = index + oldValue.Length;
                }
                buffer.Append(original, lastIndex, original.Length - lastIndex);

                result = buffer.ToString();
            }
            return result;
        }


        public static double TryParseDouble(this string stringToParse, double defaultValue)
        {
            return TryParseDouble(stringToParse, NumberStyles.Any, defaultValue);
        }

        public static double TryParseDouble(this string stringToParse, NumberStyles numberStyles = NumberStyles.Any, double defaultValue = default(double))
        {
            double parsedValue;
            return (double.TryParse(stringToParse, numberStyles, CultureInfo.InvariantCulture, out parsedValue)) ? parsedValue : defaultValue;
        }

        public static int TryParseInt(this string stringToParse, int defaultValue)
        {
            return TryParseInt(stringToParse, NumberStyles.Any, defaultValue);
        }

        public static DateTime? TryParseDateTime(this string stringToParse)
        {
            DateTime datetime;
            if (DateTime.TryParse(stringToParse, out datetime))
                return datetime;

            return null;
        }

        public static TEnum TryParseEnum<TEnum>(this string stringToParse) where TEnum : struct
        {
            TEnum value;
            if (Enum.TryParse<TEnum>(stringToParse, out value))
                return value;

            return default(TEnum);
        }

        public static bool TryParseBool(this string stringToParse, bool defaultValue = default(bool))
        {
            bool parsedValue;
            return (bool.TryParse(stringToParse, out parsedValue)) ? parsedValue : defaultValue;
        }

        public static bool? ParseBool(this string stringToParse, bool? defaultValue = default(bool?))
        {
            bool parsedValue;
            return (bool.TryParse(stringToParse, out parsedValue)) ? parsedValue : defaultValue;
        }

        public static int TryParseInt(this string stringToParse, NumberStyles numberStyles = NumberStyles.Any, int defaultValue = default(int))
        {
            int parsedValue;
            return (int.TryParse(stringToParse, numberStyles, CultureInfo.InvariantCulture, out parsedValue)) ? parsedValue : defaultValue;
        }

        public static int? ParseInt(this string stringToParse, int? defaultValue)
        {
            return ParseInt(stringToParse, NumberStyles.Any, defaultValue);
        }

        public static int? ParseInt(this string stringToParse, NumberStyles numberStyles = NumberStyles.Any, int? defaultValue = default(int?))
        {
            int parsedValue;
            return (int.TryParse(stringToParse, numberStyles, CultureInfo.InvariantCulture, out parsedValue)) ? parsedValue : defaultValue;
        }

        public static Guid? ParseGuid(this string stringToParse, Guid? defaultValue = default(Guid?))
        {
            Guid parsedValue;
            return (Guid.TryParse(stringToParse, out parsedValue)) ? parsedValue : defaultValue;
        }

        public static string InvariantFormat(this string format, params object[] args)
        {
            return String.Format(CultureInfo.InvariantCulture, format, args);
        }

        /// <summary>
        /// Returns null if the string is null or empty
        /// </summary>
        /// <param name="text">string to test</param>
        /// <returns>Null if the string is null or empty, otherwise the original string</returns>
        public static string AsNullIfEmpty(this string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return null;
            }
            return text;
        }

        /// <summary>
        /// Returns null if the string is null or whitespace
        /// </summary>
        /// <param name="text">string to test</param>
        /// <returns>Null if the string is null or empty, otherwise the original string</returns>
        public static string AsNullIfWhitespace(this string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return null;
            }
            return text;
        }

        /*
        public static string NamedFormat(this string format, object source)
        {
            return NamedFormat(format, null, source, '{', '}');
        }

        public static string NamedInvariantFormat(this string format, object source)
        {
            return NamedFormat(format, CultureInfo.InvariantCulture, source, '{', '}');
        }

        public static string NamedFormat(this string format, IFormatProvider provider, object source)
        {
            return NamedFormat(format, provider, source, '{', '}');
        }

        public static string NamedInvariantFormat(this string format, object source, char startChar, char endChar)
        {
            return NamedFormat(format, CultureInfo.InvariantCulture, source, startChar, endChar);
        }

        public static string UcFirst(this string str)
        {
            if (String.IsNullOrWhiteSpace(str)) return str;
            if (str.Length == 1) return str.ToUpperInvariant();

            return str[0].ToString().ToUpperInvariant() + str.Substring(1);
        }

        public static string NamedFormat(this string format, IFormatProvider provider, object source, char startChar, char endChar)
        {
            if (format == null)
                throw new ArgumentNullException("format");

            if (startChar != '{')
                format = format.Replace("{", "{{");
            if (endChar != '}')
                format = format.Replace("}", "}}");

            var values = new List<object>();
            var rewrittenFormat =
                Regex.Replace(
                    format,
                    @"(?<start>\{0})+(?<property>[\w\.\[\]]+)(?<format>:[^}}]+)?(?<end>\{1})+".InvariantFormat(startChar, endChar),
                    delegate(Match m)
                    {
                        var startGroup = m.Groups["start"];
                        var propertyGroup = m.Groups["property"];
                        var formatGroup = m.Groups["format"];
                        var endGroup = m.Groups["end"];

                        values.Add((propertyGroup.Value == "0")
                                       ? source
                                       : Eval(source, propertyGroup.Value));

                        var openings = startGroup.Captures.Count;
                        var closings = endGroup.Captures.Count;

                        return openings > closings || openings % 2 == 0
                                   ? m.Value
                                   : new string('{', openings) + (values.Count - 1)
                                     + formatGroup.Value
                                     + new string('}', closings);
                    },
                    RegexOptions.Compiled
                    | RegexOptions.CultureInvariant
                    | RegexOptions.IgnoreCase);

            return string.Format(provider, rewrittenFormat, values.ToArray());
        }

        

        
        private static object Eval(object source, string expression)
        {
            try
            {
                return DataBinder.Eval(source, expression);
            }
            catch (HttpException e)
            {
                throw new FormatException(null, e);
            }
        }*/
    }
}