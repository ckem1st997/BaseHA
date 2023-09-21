using Kendo.Mvc.Extensions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using NetTopologySuite.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace Share.BaseCore.Extensions
{


    public static class Error
    {
        [DebuggerStepThrough]
        public static Exception Application(string message, params object[] args)
        {
            return new ApplicationException(message.FormatWith(args));
        }

        [DebuggerStepThrough]
        public static Exception Application(Exception innerException, string message, params object[] args)
        {
            return new ApplicationException(message.FormatWith(args), innerException);
        }

        [DebuggerStepThrough]
        public static Exception ArgumentNullOrEmpty(Func<string> arg)
        {
            string name = arg.Method.Name;
            return new ArgumentException("String parameter '{0}' cannot be null or all whitespace.", name);
        }

        [DebuggerStepThrough]
        public static Exception ArgumentNull(string argName)
        {
            return new ArgumentNullException(argName);
        }

        [DebuggerStepThrough]
        public static Exception ArgumentNull<T>(Func<T> arg)
        {
            string message = "Argument of type '{0}' cannot be null".FormatInvariant(typeof(T));
            string name = arg.Method.Name;
            return new ArgumentNullException(name, message);
        }

        [DebuggerStepThrough]
        public static Exception ArgumentOutOfRange<T>(Func<T> arg)
        {
            string name = arg.Method.Name;
            return new ArgumentOutOfRangeException(name);
        }

        [DebuggerStepThrough]
        public static Exception ArgumentOutOfRange(string argName)
        {
            return new ArgumentOutOfRangeException(argName);
        }

        [DebuggerStepThrough]
        public static Exception ArgumentOutOfRange(string argName, string message, params object[] args)
        {
            return new ArgumentOutOfRangeException(argName, string.Format(CultureInfo.CurrentCulture, message, args));
        }

        [DebuggerStepThrough]
        public static Exception Argument(string argName, string message, params object[] args)
        {
            return new ArgumentException(string.Format(CultureInfo.CurrentCulture, message, args), argName);
        }

        [DebuggerStepThrough]
        public static Exception Argument<T>(Func<T> arg, string message, params object[] args)
        {
            string name = arg.Method.Name;
            return new ArgumentException(message.FormatWith(args), name);
        }

        [DebuggerStepThrough]
        public static Exception InvalidOperation(string message, params object[] args)
        {
            return InvalidOperation(message, null, args);
        }

        [DebuggerStepThrough]
        public static Exception InvalidOperation(string message, Exception innerException, params object[] args)
        {
            return new InvalidOperationException(message.FormatWith(args), innerException);
        }

        [DebuggerStepThrough]
        public static Exception InvalidOperation<T>(string message, Func<T> member)
        {
            return InvalidOperation(message, null, member);
        }

        [DebuggerStepThrough]
        public static Exception InvalidOperation<T>(string message, Exception innerException, Func<T> member)
        {
            Guard.NotNull(message, "message");
            Guard.NotNull(member, "member");
            return new InvalidOperationException(message.FormatWith(member.Method.Name), innerException);
        }

        [DebuggerStepThrough]
        public static Exception InvalidCast(Type fromType, Type toType)
        {
            return InvalidCast(fromType, toType, null);
        }

        [DebuggerStepThrough]
        public static Exception InvalidCast(Type fromType, Type toType, Exception innerException)
        {
            return new InvalidCastException("Cannot convert from type '{0}' to '{1}'.".FormatWith(fromType.FullName, toType.FullName), innerException);
        }

        [DebuggerStepThrough]
        public static Exception NotSupported()
        {
            return new NotSupportedException();
        }

        [DebuggerStepThrough]
        public static Exception NotImplemented()
        {
            return new NotImplementedException();
        }

        [DebuggerStepThrough]
        public static Exception ObjectDisposed(string objectName)
        {
            return new ObjectDisposedException(objectName);
        }

        [DebuggerStepThrough]
        public static Exception ObjectDisposed(string objectName, string message, params object[] args)
        {
            return new ObjectDisposedException(objectName, string.Format(CultureInfo.CurrentCulture, message, args));
        }

        [DebuggerStepThrough]
        public static Exception NoElements()
        {
            return new InvalidOperationException("Sequence contains no elements.");
        }

        [DebuggerStepThrough]
        public static Exception MoreThanOneElement()
        {
            return new InvalidOperationException("Sequence contains more than one element.");
        }
    }

    public static class RegularExpressions
    {
        internal static readonly string ValidRealPattern = "^([-]|[.]|[-.]|[0-9])[0-9]*[.]*[0-9]+$";

        internal static readonly string ValidIntegerPattern = "^([-]|[0-9])[0-9]*$";

        internal static readonly Regex HasTwoDot = new Regex("[0-9]*[.][0-9]*[.][0-9]*", RegexOptions.Compiled);

        internal static readonly Regex HasTwoMinus = new Regex("[0-9]*[-][0-9]*[-][0-9]*", RegexOptions.Compiled);

        public static readonly Regex IsAlpha = new Regex("[^a-zA-Z]", RegexOptions.Compiled);

        public static readonly Regex IsAlphaNumeric = new Regex("[^a-zA-Z0-9]", RegexOptions.Compiled);

        public static readonly Regex IsNotNumber = new Regex("[^0-9.-]", RegexOptions.Compiled);

        public static readonly Regex IsPositiveInteger = new Regex("\\d{1,10}", RegexOptions.Compiled);

        public static readonly Regex IsNumeric = new Regex("(" + ValidRealPattern + ")|(" + ValidIntegerPattern + ")", RegexOptions.Compiled);

        public static readonly Regex IsEmail = new Regex("^(?:[\\w\\!\\#\\$\\%\\&\\'\\*\\+\\-\\/\\=\\?\\^\\`\\{\\|\\}\\~]+\\.)*[\\w\\!\\#\\$\\%\\&\\'\\*\\+\\-\\/\\=\\?\\^\\`\\{\\|\\}\\~]+@(?:(?:(?:[a-zA-Z0-9](?:[a-zA-Z0-9\\-](?!\\.)){0,61}[a-zA-Z0-9]?\\.)+[a-zA-Z0-9](?:[a-zA-Z0-9\\-](?!$)){0,61}[a-zA-Z0-9]?)|(?:\\[(?:(?:[01]?\\d{1,2}|2[0-4]\\d|25[0-5])\\.){3}(?:[01]?\\d{1,2}|2[0-4]\\d|25[0-5])\\]))$", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Singleline);

        public static readonly Regex IsGuid = new Regex("\\{?[a-fA-F0-9]{8}(?:-(?:[a-fA-F0-9]){4}){3}-[a-fA-F0-9]{12}\\}?", RegexOptions.Compiled);

        public static readonly Regex IsBase64Guid = new Regex("[a-zA-Z0-9+/=]{22,24}", RegexOptions.Compiled);

        public static readonly Regex IsCultureCode = new Regex("^[a-z]{2}(-[A-Z]{2})?$", RegexOptions.Compiled | RegexOptions.Singleline);

        public static readonly Regex IsYearRange = new Regex("^(\\d{4})-(\\d{4})$", RegexOptions.Compiled);

        public static readonly Regex IsIban = new Regex("[a-zA-Z]{2}[0-9]{2}[a-zA-Z0-9]{4}[0-9]{7}([a-zA-Z0-9]?){0,16}", RegexOptions.Compiled | RegexOptions.Singleline);

        public static readonly Regex IsBic = new Regex("([a-zA-Z]{4}[a-zA-Z]{2}[a-zA-Z0-9]{2}([a-zA-Z0-9]{3})?)", RegexOptions.Compiled | RegexOptions.Singleline);
    }

    public static class StringExtensions
    {
        private delegate void ActionLine(TextWriter textWriter, string line);

        public const string CarriageReturnLineFeed = "\r\n";

        public const string Empty = "";

        public const char CarriageReturn = '\r';

        public const char LineFeed = '\n';

        public const char Tab = '\t';

        [DebuggerStepThrough]
        public static int ToInt(this char value)
        {
            if (value >= '0' && value <= '9')
            {
                return value - 48;
            }

            if (value >= 'a' && value <= 'f')
            {
                return value - 97 + 10;
            }

            if (value >= 'A' && value <= 'F')
            {
                return value - 65 + 10;
            }

            return -1;
        }

     

        

        public static char TryRemoveDiacritic(this char c)
        {
            switch (c)
            {
                case 'Đ':
                    return 'D';
                case 'đ':
                    return 'd';
                default:
                    {
                        string text = c.ToString().Normalize(NormalizationForm.FormD);
                        if (text.Length > 1)
                        {
                            return text[0];
                        }

                        return c;
                    }
            }
        }

        [DebuggerStepThrough]
        public static T ToEnum<T>(this string value, T defaultValue)
        {
            if (!value.HasValue())
            {
                return defaultValue;
            }

            try
            {
                return (T)Enum.Parse(typeof(T), value, ignoreCase: true);
            }
            catch (ArgumentException)
            {
                return defaultValue;
            }
        }

        [DebuggerStepThrough]
        public static string ToSafe(this string value, string defaultValue = null)
        {
            if (!string.IsNullOrEmpty(value))
            {
                return value;
            }

            return defaultValue ?? string.Empty;
        }

        [DebuggerStepThrough]
        public static string EmptyNull(this string value)
        {
            return (value ?? string.Empty).Trim();
        }

        [DebuggerStepThrough]
        public static string NullEmpty(this string value)
        {
            return string.IsNullOrEmpty(value) ? null : value;
        }

        //
        // Summary:
        //     Formats a string to an invariant culture
        //
        // Parameters:
        //   format:
        //     The format string.
        //
        //   objects:
        //     The objects.
        [DebuggerStepThrough]
        public static string FormatInvariant(this string format, params object[] objects)
        {
            return string.Format(CultureInfo.InvariantCulture, format, objects);
        }

        //
        // Summary:
        //     Formats a string to the current culture.
        //
        // Parameters:
        //   format:
        //     The format string.
        //
        //   objects:
        //     The objects.
        [DebuggerStepThrough]
        public static string FormatCurrent(this string format, params object[] objects)
        {
            return string.Format(CultureInfo.CurrentCulture, format, objects);
        }

        //
        // Summary:
        //     Formats a string to the current UI culture.
        //
        // Parameters:
        //   format:
        //     The format string.
        //
        //   objects:
        //     The objects.
        [DebuggerStepThrough]
        public static string FormatCurrentUI(this string format, params object[] objects)
        {
            return string.Format(CultureInfo.CurrentUICulture, format, objects);
        }

        [DebuggerStepThrough]
        public static string FormatWith(this string format, params object[] args)
        {
            return format.FormatWith(CultureInfo.CurrentCulture, args);
        }

        [DebuggerStepThrough]
        public static string FormatWith(this string format, IFormatProvider provider, params object[] args)
        {
            return string.Format(provider, format, args);
        }

        //
        // Summary:
        //     Determines whether this instance and another specified System.String object have
        //     the same value.
        //
        // Parameters:
        //   value:
        //     The string to check equality.
        //
        //   comparing:
        //     The comparing with string.
        //
        // Returns:
        //     true if the value of the comparing parameter is the same as this string; otherwise,
        //     false.
        [DebuggerStepThrough]
        public static bool IsCaseSensitiveEqual(this string value, string comparing)
        {
            return string.CompareOrdinal(value, comparing) == 0;
        }

        //
        // Summary:
        //     Determines whether this instance and another specified System.String object have
        //     the same value.
        //
        // Parameters:
        //   value:
        //     The string to check equality.
        //
        //   comparing:
        //     The comparing with string.
        //
        // Returns:
        //     true if the value of the comparing parameter is the same as this string; otherwise,
        //     false.
        [DebuggerStepThrough]
        public static bool IsCaseInsensitiveEqual(this string value, string comparing)
        {
            return string.Compare(value, comparing, StringComparison.OrdinalIgnoreCase) == 0;
        }

        //
        // Summary:
        //     Determines whether the string is null, empty or all whitespace.
        [DebuggerStepThrough]
        public static bool IsEmpty(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }

        //
        // Summary:
        //     Determines whether the string is all white space. Empty string will return false.
        //
        // Parameters:
        //   value:
        //     The string to test whether it is all white space.
        //
        // Returns:
        //     true if the string is all white space; otherwise, false.
        [DebuggerStepThrough]
        public static bool IsWhiteSpace(this string value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            if (value.Length == 0)
            {
                return false;
            }

            for (int i = 0; i < value.Length; i++)
            {
                if (!char.IsWhiteSpace(value[i]))
                {
                    return false;
                }
            }

            return true;
        }

        //[DebuggerStepThrough]
        //public static bool HasValue(this string value)
        //{
        //    return !string.IsNullOrWhiteSpace(value);
        //}

       

        //
        // Summary:
        //     Mask by replacing characters with asterisks.
        //
        // Parameters:
        //   value:
        //     The string
        //
        //   length:
        //     Number of characters to leave untouched.
        //
        // Returns:
        //     The mask string
        [DebuggerStepThrough]
        public static string Mask(this string value, int length)
        {
            if (value.HasValue())
            {
                return value.Substring(0, length) + new string('*', value.Length - length);
            }

            return value;
        }

        private static bool IsWebUrlInternal(string value, bool schemeIsOptional)
        {
            if (string.IsNullOrEmpty(value))
            {
                return false;
            }

            value = value.Trim().ToLowerInvariant();
            if (schemeIsOptional && value.StartsWith("//"))
            {
                value = "http:" + value;
            }

            return Uri.IsWellFormedUriString(value, UriKind.Absolute) && (value.StartsWith("http://") || value.StartsWith("https://") || value.StartsWith("ftp://"));
        }

        [DebuggerStepThrough]
        public static bool IsWebUrl(this string value)
        {
            return IsWebUrlInternal(value, schemeIsOptional: false);
        }

        [DebuggerStepThrough]
        public static bool IsWebUrl(this string value, bool schemeIsOptional)
        {
            return IsWebUrlInternal(value, schemeIsOptional);
        }

        [DebuggerStepThrough]
        public static bool IsEmail(this string value)
        {
            return !string.IsNullOrEmpty(value) && RegularExpressions.IsEmail.IsMatch(value.Trim());
        }

        [DebuggerStepThrough]
        public static bool IsNumeric(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return false;
            }

            return !RegularExpressions.IsNotNumber.IsMatch(value) && !RegularExpressions.HasTwoDot.IsMatch(value) && !RegularExpressions.HasTwoMinus.IsMatch(value) && RegularExpressions.IsNumeric.IsMatch(value);
        }

        //
        // Summary:
        //     Ensures that a string only contains numeric values
        //
        // Parameters:
        //   str:
        //     Input string
        //
        // Returns:
        //     Input string with only numeric values, empty string if input is null or empty
        [DebuggerStepThrough]
        public static string EnsureNumericOnly(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return string.Empty;
            }

            return new string(str.Where((char c) => char.IsDigit(c)).ToArray());
        }

        [DebuggerStepThrough]
        public static bool IsAlpha(this string value)
        {
            return RegularExpressions.IsAlpha.IsMatch(value);
        }

        [DebuggerStepThrough]
        public static bool IsAlphaNumeric(this string value)
        {
            return RegularExpressions.IsAlphaNumeric.IsMatch(value);
        }

        [DebuggerStepThrough]
        public static string Truncate(this string value, int maxLength, string suffix = "")
        {
            if (suffix == null)
            {
                throw new ArgumentNullException("suffix");
            }

            Guard.IsPositive(maxLength, "maxLength");
            int num = maxLength - suffix.Length;
            if (num <= 0)
            {
                throw Error.Argument("maxLength", "Length of suffix string is greater or equal to maximumLength");
            }

            if (value != null && value.Length > maxLength)
            {
                string text = value.Substring(0, num);
                text = text.Trim();
                return text + suffix;
            }

            return value;
        }

        //
        // Summary:
        //     Removes all redundant whitespace (empty lines, double space etc.). Use ~! literal
        //     to keep whitespace wherever necessary.
        //
        // Parameters:
        //   input:
        //     Input
        //
        // Returns:
        //     The compacted string
        public static string Compact(this string input, bool removeEmptyLines = false)
        {
            Guard.NotNull(input, "input");
            StringBuilder stringBuilder = new StringBuilder();
            string[] array = input.Trim().GetLines(trimLines: true, removeEmptyLines).ToArray();
            string[] array2 = array;
            foreach (string text in array2)
            {
                int length = text.Length;
                StringBuilder stringBuilder2 = new StringBuilder(length);
                bool flag = false;
                bool flag2 = false;
                int num = 0;
                bool flag3 = false;
                for (num = 0; num < length; num++)
                {
                    char c = text[num];
                    flag3 = num == length - 1;
                    if (char.IsWhiteSpace(c))
                    {
                        if (flag)
                        {
                            stringBuilder2.Append(' ');
                        }

                        flag2 = false;
                        flag = false;
                        continue;
                    }

                    flag2 = c == '~' && !flag3 && text[num + 1] == '!';
                    flag = true;
                    if (flag2)
                    {
                        stringBuilder2.Append(' ');
                        num++;
                    }
                    else
                    {
                        stringBuilder2.Append(c);
                    }
                }

                stringBuilder.AppendLine(stringBuilder2.ToString().Trim().Trim(','));
            }

            return stringBuilder.ToString().Trim();
        }

        //
        // Summary:
        //     Splits the input string by carriage return.
        //
        // Parameters:
        //   input:
        //     The string to split
        //
        // Returns:
        //     A sequence with string items per line
        public static IEnumerable<string> GetLines(this string input, bool trimLines = false, bool removeEmptyLines = false)
        {
            if (input.IsEmpty())
            {
                yield break;
            }

            using StringReader sr = new StringReader(input);
            while (true)
            {
                string text;
                string line = (text = sr.ReadLine());
                if (text == null)
                {
                    break;
                }

                if (trimLines)
                {
                    line = line.Trim();
                }

                if (!removeEmptyLines || !line.IsEmpty())
                {
                    yield return line;
                }
            }
        }

        //
        // Summary:
        //     Ensure that a string starts with a string.
        //
        // Parameters:
        //   value:
        //     The target string
        //
        //   startsWith:
        //     The string the target string should start with
        //
        // Returns:
        //     The resulting string
        [DebuggerStepThrough]
        public static string EnsureStartsWith(this string value, string startsWith)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            if (startsWith == null)
            {
                throw new ArgumentNullException("startsWith");
            }

            return value.StartsWith(startsWith) ? value : (startsWith + value);
        }

        //
        // Summary:
        //     Ensures the target string ends with the specified string.
        //
        // Parameters:
        //   endWith:
        //     The target.
        //
        //   value:
        //     The value.
        //
        // Returns:
        //     The target string with the value string at the end.
        [DebuggerStepThrough]
        public static string EnsureEndsWith(this string value, string endWith)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            if (endWith == null)
            {
                throw new ArgumentNullException("endWith");
            }

            if (value.Length >= endWith.Length)
            {
                if (string.Compare(value, value.Length - endWith.Length, endWith, 0, endWith.Length, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    return value;
                }

                string text = value.TrimEnd(null);
                if (string.Compare(text, text.Length - endWith.Length, endWith, 0, endWith.Length, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    return value;
                }
            }

            return value + endWith;
        }

        [DebuggerStepThrough]
        public static string UrlEncode(this string value)
        {
            return HttpUtility.UrlEncode(value);
        }

        [DebuggerStepThrough]
        public static string UrlDecode(this string value)
        {
            return HttpUtility.UrlDecode(value);
        }

        [DebuggerStepThrough]
        public static string AttributeEncode(this string value)
        {
            return HttpUtility.HtmlAttributeEncode(value);
        }

        [DebuggerStepThrough]
        public static string HtmlEncode(this string value)
        {
            return HttpUtility.HtmlEncode(value);
        }

        [DebuggerStepThrough]
        public static string HtmlDecode(this string value)
        {
            return HttpUtility.HtmlDecode(value);
        }



        //
        // Summary:
        //     Replaces pascal casing with spaces. For example "CustomerId" would become "Customer
        //     Id". Strings that already contain spaces are ignored.
        //
        // Parameters:
        //   value:
        //     String to split
        //
        // Returns:
        //     The string after being split
        [DebuggerStepThrough]
        public static string SplitPascalCase(this string value)
        {
            StringBuilder stringBuilder = new StringBuilder();
            char[] array = value.ToCharArray();
            stringBuilder.Append(array[0]);
            for (int i = 1; i < array.Length - 1; i++)
            {
                char c = array[i];
                if (char.IsUpper(c) && (char.IsLower(array[i + 1]) || char.IsLower(array[i - 1])))
                {
                    stringBuilder.Append(" ");
                }

                stringBuilder.Append(c);
            }

            if (array.Length > 1)
            {
                stringBuilder.Append(array[^1]);
            }

            return stringBuilder.ToString();
        }

        //
        // Summary:
        //     Splits a string into a string array
        //
        // Parameters:
        //   value:
        //     String value to split
        //
        //   separator:
        //     If null then value is searched for a common delimiter like pipe, semicolon or
        //     comma
        //
        // Returns:
        //     String array
        [DebuggerStepThrough]
        public static string[] SplitSafe(this string value, string separator)
        {
            if (string.IsNullOrEmpty(value))
            {
                return new string[0];
            }

            if (separator == null)
            {
                for (int i = 0; i < value.Length; i++)
                {
                    char c = value[i];
                    if (c == ';' || c == ',' || c == '|')
                    {
                        return value.Split(new char[1] { c }, StringSplitOptions.RemoveEmptyEntries);
                    }

                    if (c == '\r' && ((i + 1 < value.Length) & (value[i + 1] == '\n')))
                    {
                        return value.GetLines(trimLines: false, removeEmptyLines: true).ToArray();
                    }
                }

                return new string[1] { value };
            }

            return value.Split(new string[1] { separator }, StringSplitOptions.RemoveEmptyEntries);
        }

        //
        // Summary:
        //     Splits a string into two strings
        //
        // Returns:
        //     true: success, false: failure
        [DebuggerStepThrough]
        public static bool SplitToPair(this string value, out string leftPart, out string rightPart, string delimiter, bool splitAfterLast = false)
        {
            leftPart = value;
            rightPart = "";
            if (string.IsNullOrEmpty(value) || string.IsNullOrEmpty(delimiter))
            {
                return false;
            }

            int num = (splitAfterLast ? value.LastIndexOf(delimiter) : value.IndexOf(delimiter));
            if (num == -1)
            {
                return false;
            }

            leftPart = value.Substring(0, num);
            rightPart = value.Substring(num + delimiter.Length);
            return true;
        }

       

      

        [DebuggerStepThrough]
        public static bool IsEnclosedIn(this string value, string enclosedIn)
        {
            return value.IsEnclosedIn(enclosedIn, StringComparison.CurrentCulture);
        }

        [DebuggerStepThrough]
        public static bool IsEnclosedIn(this string value, string enclosedIn, StringComparison comparisonType)
        {
            if (string.IsNullOrEmpty(enclosedIn))
            {
                return false;
            }

            if (enclosedIn.Length == 1)
            {
                return value.IsEnclosedIn(enclosedIn, enclosedIn, comparisonType);
            }

            if (enclosedIn.Length % 2 == 0)
            {
                int num = enclosedIn.Length / 2;
                return value.IsEnclosedIn(enclosedIn.Substring(0, num), enclosedIn.Substring(num, num), comparisonType);
            }

            return false;
        }

        [DebuggerStepThrough]
        public static bool IsEnclosedIn(this string value, string start, string end)
        {
            return value.IsEnclosedIn(start, end, StringComparison.CurrentCulture);
        }

        [DebuggerStepThrough]
        public static bool IsEnclosedIn(this string value, string start, string end, StringComparison comparisonType)
        {
            return value.StartsWith(start, comparisonType) && value.EndsWith(end, comparisonType);
        }

        public static string RemoveEncloser(this string value, string encloser)
        {
            return value.RemoveEncloser(encloser, StringComparison.CurrentCulture);
        }

        public static string RemoveEncloser(this string value, string encloser, StringComparison comparisonType)
        {
            if (value.IsEnclosedIn(encloser, comparisonType))
            {
                int num = encloser.Length / 2;
                return value.Substring(num, value.Length - num * 2);
            }

            return value;
        }

        public static string RemoveEncloser(this string value, string start, string end)
        {
            return value.RemoveEncloser(start, end, StringComparison.CurrentCulture);
        }

        public static string RemoveEncloser(this string value, string start, string end, StringComparison comparisonType)
        {
            if (value.IsEnclosedIn(start, end, comparisonType))
            {
                return value.Substring(start.Length, value.Length - (start.Length + end.Length));
            }

            return value;
        }

        //
        // Summary:
        //     Debug.WriteLine
        [DebuggerStepThrough]
        public static void Dump(this string value, bool appendMarks = false)
        {
            Debug.WriteLine(value);
            Debug.WriteLineIf(appendMarks, "------------------------------------------------");
        }

        //
        // Summary:
        //     Smart way to create a HTML attribute with a leading space.
        //
        // Parameters:
        //   value:
        //     Name of the attribute.
        //
        //   name:
        //
        //   htmlEncode:
        public static string ToAttribute(this string value, string name, bool htmlEncode = true)
        {
            if (name.IsEmpty())
            {
                return "";
            }

            if (value == "" && name != "value" && !name.StartsWith("data"))
            {
                return "";
            }

            if (name == "maxlength" && (value == "" || value == "0"))
            {
                return "";
            }

            if (name == "checked" || name == "disabled" || name == "multiple")
            {
                if (value == "" || string.Compare(value, "false", ignoreCase: true) == 0)
                {
                    return "";
                }

                value = ((string.Compare(value, "true", ignoreCase: true) == 0) ? name : value);
            }

            if (name.StartsWith("data"))
            {
                name = name.Insert(4, "-");
            }

            return $" {name}=\"{(htmlEncode ? HttpUtility.HtmlEncode(value) : value)}\"";
        }

        //
        // Summary:
        //     Appends grow and uses delimiter if the string is not empty.
        [DebuggerStepThrough]
        public static string Grow(this string value, string grow, string delimiter)
        {
            if (string.IsNullOrEmpty(value))
            {
                return string.IsNullOrEmpty(grow) ? "" : grow;
            }

            if (string.IsNullOrEmpty(grow))
            {
                return string.IsNullOrEmpty(value) ? "" : value;
            }

            return value + delimiter + grow;
        }

        //
        // Summary:
        //     Returns n/a if string is empty else self.
        [DebuggerStepThrough]
        public static string NaIfEmpty(this string value)
        {
            return string.IsNullOrWhiteSpace(value) ? "n/a" : value;
        }

        //
        // Summary:
        //     Replaces substring with position x1 to x2 by replaceBy.
        [DebuggerStepThrough]
        public static string Replace(this string value, int x1, int x2, string replaceBy = null)
        {
            if (!string.IsNullOrWhiteSpace(value) && x1 > 0 && x2 > x1 && x2 < value.Length)
            {
                return value.Substring(0, x1) + replaceBy.EmptyNull() + value.Substring(x2 + 1);
            }

            return value;
        }

      


        [DebuggerStepThrough]
        public static string TrimSafe(this string value)
        {
            return value.HasValue() ? value.Trim() : value;
        }

        [DebuggerStepThrough]
        public static string Slugify(this string value, bool allowSpace = false, char[] allowChars = null)
        {
            string text = string.Empty;
            try
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    StringBuilder stringBuilder = new StringBuilder();
                    bool flag = false;
                    foreach (char c in value)
                    {
                        if (c == ' ' || c == '-')
                        {
                            if (allowSpace && c == ' ')
                            {
                                stringBuilder.Append(' ');
                            }
                            else if (!flag)
                            {
                                stringBuilder.Append('-');
                            }

                            flag = true;
                            continue;
                        }

                        flag = false;
                        if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '_')
                        {
                            stringBuilder.Append(c);
                        }
                        else if (allowChars?.Contains(c) ?? false)
                        {
                            stringBuilder.Append(c);
                        }
                        else
                        {
                            if (c < '\u0080')
                            {
                                continue;
                            }

                            switch (c)
                            {
                                case 'ä':
                                    stringBuilder.Append("ae");
                                    continue;
                                case 'ö':
                                    stringBuilder.Append("oe");
                                    continue;
                                case 'ü':
                                    stringBuilder.Append("ue");
                                    continue;
                                case 'ß':
                                    stringBuilder.Append("ss");
                                    continue;
                                case 'Ä':
                                    stringBuilder.Append("AE");
                                    continue;
                                case 'Ö':
                                    stringBuilder.Append("OE");
                                    continue;
                                case 'Ü':
                                    stringBuilder.Append("UE");
                                    continue;
                            }

                            char c2 = c.TryRemoveDiacritic();
                            if ((c2 >= 'a' && c2 <= 'z') || (c2 >= 'A' && c2 <= 'Z'))
                            {
                                stringBuilder.Append(c2);
                            }
                        }
                    }

                    if (stringBuilder.Length > 0)
                    {
                        text = stringBuilder.ToString().Trim(' ', '-');
                        Regex regex = new Regex("(-{2,})");
                        text = regex.Replace(text, "-");
                        text = text.Replace("__", "_");
                    }
                }
            }
            catch (Exception exception)
            {
            }

            return (text.Length > 0) ? text : "null";
        }

        public static string SanitizeHtmlId(this string value)
        {
            return TagBuilder.CreateSanitizedId(value, "");
        }

      

        [DebuggerStepThrough]
        public static bool IsMatch(this string input, string pattern, RegexOptions options = RegexOptions.IgnoreCase | RegexOptions.Multiline)
        {
            return Regex.IsMatch(input, pattern, options);
        }

        [DebuggerStepThrough]
        public static bool IsMatch(this string input, string pattern, out Match match, RegexOptions options = RegexOptions.IgnoreCase | RegexOptions.Multiline)
        {
            match = Regex.Match(input, pattern, options);
            return match.Success;
        }

        public static string RegexRemove(this string input, string pattern, RegexOptions options = RegexOptions.IgnoreCase | RegexOptions.Multiline)
        {
            return Regex.Replace(input, pattern, string.Empty, options);
        }

        public static string RegexReplace(this string input, string pattern, string replacement, RegexOptions options = RegexOptions.IgnoreCase | RegexOptions.Multiline)
        {
            return Regex.Replace(input, pattern, replacement, options);
        }

        [DebuggerStepThrough]
        public static string ToValidFileName(this string input, string replacement = "-")
        {
            return input.ToValidPathInternal(isPath: false, replacement);
        }

        [DebuggerStepThrough]
        public static string ToValidPath(this string input, string replacement = "-")
        {
            return input.ToValidPathInternal(isPath: true, replacement);
        }

        private static string ToValidPathInternal(this string input, bool isPath, string replacement)
        {
            HashSet<char> hashSet = new HashSet<char>(isPath ? System.IO.Path.GetInvalidPathChars() : System.IO.Path.GetInvalidFileNameChars());
            replacement = replacement ?? "-";
            StringBuilder stringBuilder = new StringBuilder();
            string text = input.ToSafe();
            foreach (char c in text)
            {
                if (hashSet.Contains(c))
                {
                    stringBuilder.Append(replacement);
                }
                else
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString();
        }

        [DebuggerStepThrough]
        public static int[] ToIntArray(this string s)
        {
            return Array.ConvertAll(s.SplitSafe(","), (string v) => int.Parse(v.Trim()));
        }

        [DebuggerStepThrough]
        public static bool ToIntArrayContains(this string s, int value, bool defaultValue)
        {
            if (s == null)
            {
                return defaultValue;
            }

            int[] array = s.ToIntArray();
            if (array == null || array.Count() <= 0)
            {
                return defaultValue;
            }

            return array.Contains(value);
        }

        [DebuggerStepThrough]
        public static string RemoveInvalidXmlChars(this string s)
        {
            if (s.IsEmpty())
            {
                return s;
            }

            return Regex.Replace(s, "[^\\u0009\\u000A\\u000D\\u0020-\\uD7FF\\uE000-\\uFFFD]", "", RegexOptions.Compiled);
        }

        [DebuggerStepThrough]
        public static string ReplaceCsvChars(this string s)
        {
            if (s.IsEmpty())
            {
                return "";
            }

            s = s.Replace(';', ',');
            s = s.Replace('\r', ' ');
            s = s.Replace('\n', ' ');
            return s.Replace("'", "");
        }

        [DebuggerStepThrough]
        public static string HighlightKeywords(this string input, string keywords, string preMatch = "<strong>", string postMatch = "</strong>")
        {
            Guard.NotNull(preMatch, "preMatch");
            Guard.NotNull(postMatch, "postMatch");
            if (string.IsNullOrWhiteSpace(input) || string.IsNullOrWhiteSpace(keywords))
            {
                return input;
            }

            string text = string.Join("|", (from x in keywords.Trim().Split(' ', '-')
                                            select x.Trim() into x
                                            where !string.IsNullOrWhiteSpace(x)
                                            select Regex.Escape(x)).Distinct());
            if (!string.IsNullOrWhiteSpace(text))
            {
                Regex regex = new Regex(text, RegexOptions.IgnoreCase);
                input = regex.Replace(input, (Match m) => preMatch + m.Value.EmptyNull().HtmlEncode() + postMatch);
            }

            return input;
        }

        public static string RemoveDiacritics(this string text)
        {
            string text2 = text.Normalize(NormalizationForm.FormD);
            StringBuilder stringBuilder = new StringBuilder();
            string text3 = text2;
            foreach (char c in text3)
            {
                UnicodeCategory unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC).Replace('Đ', 'D')
                .Replace('đ', 'd');
        }

        public static bool Contains(this string source, string value, StringComparison comparisonType)
        {
            return source.IndexOf(value, comparisonType) >= 0;
        }

       

        public static string JavaScriptStringEncode(this string value)
        {
            return HttpUtility.JavaScriptStringEncode(value);
        }

      
      
      
    }
    public static class LinqExtensions
    {
        public static PropertyInfo ExtractPropertyInfo(this LambdaExpression propertyAccessor)
        {
            return propertyAccessor.ExtractMemberInfo() as PropertyInfo;
        }

        public static FieldInfo ExtractFieldInfo(this LambdaExpression propertyAccessor)
        {
            return propertyAccessor.ExtractMemberInfo() as FieldInfo;
        }

        [SuppressMessage("ReSharper", "CanBeReplacedWithTryCastAndCheckForNull")]
        public static MemberInfo ExtractMemberInfo(this LambdaExpression propertyAccessor)
        {
            if (propertyAccessor == null)
                throw new ArgumentNullException(nameof(propertyAccessor));

            MemberInfo info;
            try
            {
                MemberExpression operand;
                // o => o.PropertyOrField
                LambdaExpression expression = propertyAccessor;
                // If the property is not an Object, then the member access expression will be wrapped in a conversion expression
                // (object)o.PropertyOrField

                if (expression.Body is UnaryExpression body)
                {
                    // o.PropertyOrField
                    operand = (MemberExpression)body.Operand;
                }
                else
                {
                    // o.PropertyOrField
                    operand = (MemberExpression)expression.Body;
                }

                // Member
                MemberInfo member = operand.Member;
                info = member;
            }
            catch (Exception e)
            {
                throw new ArgumentException("The property or field accessor expression is not in the expected format 'o => o.PropertyOrField'.", e);
            }

            return info;
        }

    }

    public class Guard
    {
        internal const string AgainstMessage = "Assertion evaluation failed with 'false'.";

        internal const string ImplementsMessage = "Type '{0}' must implement type '{1}'.";

        internal const string InheritsFromMessage = "Type '{0}' must inherit from type '{1}'.";

        internal const string IsTypeOfMessage = "Type '{0}' must be of type '{1}'.";

        internal const string IsEqualMessage = "Compared objects must be equal.";

        internal const string IsPositiveMessage = "Argument '{0}' must be a positive value. Value: '{1}'.";

        internal const string IsTrueMessage = "True expected for '{0}' but the condition was False.";

        internal const string NotNegativeMessage = "Argument '{0}' cannot be a negative value. Value: '{1}'.";

        private Guard()
        {
        }

        [DebuggerStepThrough]
        public static void NotNull(object arg, string argName)
        {
            if (arg == null)
            {
                throw new ArgumentNullException(argName);
            }
        }

        [DebuggerStepThrough]
        public static void NotEmpty(string arg, string argName)
        {
            if (string.IsNullOrWhiteSpace(arg))
            {
                throw Error.Argument(argName, "String parameter '{0}' cannot be null or all whitespace.", argName);
            }
        }

        [DebuggerStepThrough]
        public static void NotEmpty<T>(ICollection<T> arg, string argName)
        {
            if (arg == null || !arg.Any())
            {
                throw Error.Argument(argName, "Collection cannot be null and must contain at least one item.");
            }
        }

        [DebuggerStepThrough]
        public static void NotEmpty(Guid arg, string argName)
        {
            if (arg == Guid.Empty)
            {
                throw Error.Argument(argName, "Argument '{0}' cannot be an empty guid.", argName);
            }
        }

        [DebuggerStepThrough]
        public static void InRange<T>(T arg, T min, T max, string argName) where T : struct, IComparable<T>
        {
            if (arg.CompareTo(min) < 0 || arg.CompareTo(max) > 0)
            {
                throw Error.ArgumentOutOfRange(argName, "The argument '{0}' must be between '{1}' and '{2}'.", argName, min, max);
            }
        }

        [DebuggerStepThrough]
        public static void NotOutOfLength(string arg, int maxLength, string argName)
        {
            if (arg.Trim().Length > maxLength)
            {
                throw Error.Argument(argName, "Argument '{0}' cannot be more than {1} characters long.", argName, maxLength);
            }
        }

        [DebuggerStepThrough]
        public static void NotNegative<T>(T arg, string argName, string message = "Argument '{0}' cannot be a negative value. Value: '{1}'.") where T : struct, IComparable<T>
        {
            if (arg.CompareTo(default(T)) < 0)
            {
                throw Error.ArgumentOutOfRange(argName, message.FormatInvariant(argName, arg));
            }
        }

        [DebuggerStepThrough]
        public static void NotZero<T>(T arg, string argName) where T : struct, IComparable<T>
        {
            if (arg.CompareTo(default(T)) == 0)
            {
                throw Error.ArgumentOutOfRange(argName, "Argument '{0}' must be greater or less than zero. Value: '{1}'.", argName, arg);
            }
        }

        [DebuggerStepThrough]
        public static void Against<TException>(bool assertion, string message = "Assertion evaluation failed with 'false'.") where TException : Exception
        {
            if (assertion)
            {
                throw (TException)Activator.CreateInstance(typeof(TException), message);
            }
        }

        [DebuggerStepThrough]
        public static void Against<TException>(Func<bool> assertion, string message = "Assertion evaluation failed with 'false'.") where TException : Exception
        {
            if (assertion())
            {
                throw (TException)Activator.CreateInstance(typeof(TException), message);
            }
        }

        [DebuggerStepThrough]
        public static void IsPositive<T>(T arg, string argName, string message = "Argument '{0}' must be a positive value. Value: '{1}'.") where T : struct, IComparable<T>
        {
            if (arg.CompareTo(default(T)) < 1)
            {
                throw Error.ArgumentOutOfRange(argName, message.FormatInvariant(argName));
            }
        }

        [DebuggerStepThrough]
        public static void IsTrue(bool arg, string argName, string message = "True expected for '{0}' but the condition was False.")
        {
            if (!arg)
            {
                throw Error.Argument(argName, message.FormatInvariant(argName));
            }
        }

        [DebuggerStepThrough]
        public static void IsEnumType(Type arg, string argName)
        {
            NotNull(arg, argName);
            if (!arg.IsEnum)
            {
                throw Error.Argument(argName, "Type '{0}' must be a valid Enum type.", arg.FullName);
            }
        }

        [DebuggerStepThrough]
        public static void IsEnumType(Type enumType, object arg, string argName)
        {
            NotNull(arg, argName);
            if (!Enum.IsDefined(enumType, arg))
            {
                throw Error.ArgumentOutOfRange(argName, "The value of the argument '{0}' provided for the enumeration '{1}' is invalid.", argName, enumType.FullName);
            }
        }

       
        [DebuggerStepThrough]
        public static void PagingArgsValid(int indexArg, int sizeArg, string indexArgName, string sizeArgName)
        {
            NotNegative(indexArg, indexArgName, "PageIndex cannot be below 0");
            if (indexArg > 0)
            {
                IsPositive(sizeArg, sizeArgName, "PageSize cannot be below 1 if a PageIndex greater 0 was provided.");
            }
            else
            {
                NotNegative(sizeArg, sizeArgName);
            }
        }

        //
        // Summary:
        //     Throws proper exception if the class reference is null.
        //
        // Parameters:
        //   value:
        //     Class reference to check.
        //
        // Type parameters:
        //   TValue:
        //
        // Exceptions:
        //   T:System.InvalidOperationException:
        //     If class reference is null.
        [DebuggerStepThrough]
        public static void NotNull<TValue>(Func<TValue> value)
        {
            if (value() == null)
            {
                throw new InvalidOperationException("'{0}' cannot be null.".FormatInvariant(value));
            }
        }

        [DebuggerStepThrough]
        [Obsolete("Use NotNull() with nameof operator instead")]
        public static void ArgumentNotNull(object arg, string argName)
        {
            if (arg == null)
            {
                throw new ArgumentNullException(argName);
            }
        }

        [DebuggerStepThrough]
        [Obsolete("Use NotNull() with nameof operator instead")]
        public static void ArgumentNotNull<T>(Func<T> arg)
        {
            if (arg() == null)
            {
                throw new ArgumentNullException(GetParamName(arg));
            }
        }

        [DebuggerStepThrough]
        public static void Arguments<T1, T2>(Func<T1> arg1, Func<T2> arg2)
        {
            if (arg1() == null)
            {
                throw new ArgumentNullException(GetParamName(arg1));
            }

            if (arg2() == null)
            {
                throw new ArgumentNullException(GetParamName(arg2));
            }
        }

        [DebuggerStepThrough]
        public static void Arguments<T1, T2, T3>(Func<T1> arg1, Func<T2> arg2, Func<T3> arg3)
        {
            if (arg1() == null)
            {
                throw new ArgumentNullException(GetParamName(arg1));
            }

            if (arg2() == null)
            {
                throw new ArgumentNullException(GetParamName(arg2));
            }

            if (arg3() == null)
            {
                throw new ArgumentNullException(GetParamName(arg3));
            }
        }

        [DebuggerStepThrough]
        public static void Arguments<T1, T2, T3, T4>(Func<T1> arg1, Func<T2> arg2, Func<T3> arg3, Func<T4> arg4)
        {
            if (arg1() == null)
            {
                throw new ArgumentNullException(GetParamName(arg1));
            }

            if (arg2() == null)
            {
                throw new ArgumentNullException(GetParamName(arg2));
            }

            if (arg3() == null)
            {
                throw new ArgumentNullException(GetParamName(arg3));
            }

            if (arg4() == null)
            {
                throw new ArgumentNullException(GetParamName(arg4));
            }
        }

        [DebuggerStepThrough]
        public static void Arguments<T1, T2, T3, T4, T5>(Func<T1> arg1, Func<T2> arg2, Func<T3> arg3, Func<T4> arg4, Func<T5> arg5)
        {
            if (arg1() == null)
            {
                throw new ArgumentNullException(GetParamName(arg1));
            }

            if (arg2() == null)
            {
                throw new ArgumentNullException(GetParamName(arg2));
            }

            if (arg3() == null)
            {
                throw new ArgumentNullException(GetParamName(arg3));
            }

            if (arg4() == null)
            {
                throw new ArgumentNullException(GetParamName(arg4));
            }

            if (arg5() == null)
            {
                throw new ArgumentNullException(GetParamName(arg5));
            }
        }

        [DebuggerStepThrough]
        [Obsolete("Use NotEmpty() with nameof operator instead")]
        public static void ArgumentNotEmpty(Func<string> arg)
        {
            if (arg().IsEmpty())
            {
                string paramName = GetParamName(arg);
                throw Error.Argument(paramName, "String parameter '{0}' cannot be null or all whitespace.", paramName);
            }
        }

        [DebuggerStepThrough]
        [Obsolete("Use NotEmpty() with nameof operator instead")]
        public static void ArgumentNotEmpty(string arg, string argName)
        {
            if (arg.IsEmpty())
            {
                throw Error.Argument(argName, "String parameter '{0}' cannot be null or all whitespace.", argName);
            }
        }

        [DebuggerStepThrough]
        public static void InheritsFrom<TBase>(Type type)
        {
            InheritsFrom<TBase>(type, "Type '{0}' must inherit from type '{1}'.".FormatInvariant(type.FullName, typeof(TBase).FullName));
        }

        [DebuggerStepThrough]
        public static void InheritsFrom<TBase>(Type type, string message)
        {
            if (type.BaseType != typeof(TBase))
            {
                throw new InvalidOperationException(message);
            }
        }

        [DebuggerStepThrough]
        public static void Implements<TInterface>(Type type, string message = "Type '{0}' must implement type '{1}'.")
        {
            if (!typeof(TInterface).IsAssignableFrom(type))
            {
                throw new InvalidOperationException(message.FormatInvariant(type.FullName, typeof(TInterface).FullName));
            }
        }

        [DebuggerStepThrough]
        public static void IsSubclassOf<TBase>(Type type)
        {
            Type typeFromHandle = typeof(TBase);
            if (!typeFromHandle.IsSubClass(type))
            {
                throw new InvalidOperationException("Type '{0}' must be a subclass of type '{1}'.".FormatInvariant(type.FullName, typeFromHandle.FullName));
            }
        }

        [DebuggerStepThrough]
        public static void IsTypeOf<TType>(object instance)
        {
            IsTypeOf<TType>(instance, "Type '{0}' must be of type '{1}'.".FormatInvariant(instance.GetType().Name, typeof(TType).FullName));
        }

        [DebuggerStepThrough]
        public static void IsTypeOf<TType>(object instance, string message)
        {
            if (!(instance is TType))
            {
                throw new InvalidOperationException(message);
            }
        }

        [DebuggerStepThrough]
        public static void IsEqual<TException>(object compare, object instance, string message = "Compared objects must be equal.") where TException : Exception
        {
            if (!compare.Equals(instance))
            {
                throw (TException)Activator.CreateInstance(typeof(TException), message);
            }
        }

        [DebuggerStepThrough]
        public static void HasDefaultConstructor<T>()
        {
            HasDefaultConstructor(typeof(T));
        }

        [DebuggerStepThrough]
        public static void HasDefaultConstructor(Type t)
        {
            if (!t.HasDefaultConstructor())
            {
                throw Error.InvalidOperation("The type '{0}' must have a default parameterless constructor.", t.FullName);
            }
        }

        [DebuggerStepThrough]
        private static string GetParamName<T>(Expression<Func<T>> expression)
        {
            string result = string.Empty;
            MemberExpression memberExpression = expression.Body as MemberExpression;
            if (memberExpression != null)
            {
                result = memberExpression.Member.Name;
            }

            return result;
        }

        [DebuggerStepThrough]
        private static string GetParamName<T>(Func<T> expression)
        {
            return expression.Method.Name;
        }
    }
    public static class TypeExtensions
    {
        private static readonly Type[] s_predefinedTypes = new Type[5]
        {
            typeof(string),
            typeof(decimal),
            typeof(DateTime),
            typeof(TimeSpan),
            typeof(Guid)
        };

        public static string AssemblyQualifiedNameWithoutVersion(this Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            if (type.AssemblyQualifiedName != null)
            {
                string[] array = type.AssemblyQualifiedName!.Split(new char[1] { ',' });
                return $"{array[0].Trim()}, {array[1].Trim()}";
            }

            return null;
        }

        public static bool IsNumericType(this Type type)
        {
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.SByte:
                case TypeCode.Byte:
                case TypeCode.Int16:
                case TypeCode.UInt16:
                case TypeCode.Int32:
                case TypeCode.UInt32:
                case TypeCode.Int64:
                case TypeCode.UInt64:
                case TypeCode.Single:
                case TypeCode.Double:
                case TypeCode.Decimal:
                    return true;
                case TypeCode.Object:
                    {
                        if (type.IsNullable(out var elementType))
                        {
                            return elementType.IsNumericType();
                        }

                        return false;
                    }
                default:
                    return false;
            }
        }

        public static bool IsSequenceType(this Type type)
        {
            if (type == typeof(string))
            {
                return false;
            }

            return type.IsArray || typeof(IEnumerable).IsAssignableFrom(type);
        }

        public static bool IsSequenceType(this Type type, out Type elementType)
        {
            elementType = null;
            if (type == typeof(string))
            {
                return false;
            }

            Type implementingType;
            if (type.IsArray)
            {
                elementType = type.GetElementType();
            }
            else if (type.IsSubClass(typeof(IEnumerable<>), out implementingType))
            {
                Type[] genericArguments = implementingType.GetGenericArguments();
                if (genericArguments.Length == 1)
                {
                    elementType = genericArguments[0];
                }
            }

            return elementType != null;
        }

        public static bool IsPredefinedSimpleType(this Type type)
        {
            if (type.IsPrimitive && type != typeof(IntPtr) && type != typeof(UIntPtr))
            {
                return true;
            }

            if (type.IsEnum)
            {
                return true;
            }

            return s_predefinedTypes.Any((Type t) => t == type);
        }

        public static bool IsStruct(this Type type)
        {
            if (type.IsValueType)
            {
                return !type.IsPredefinedSimpleType();
            }

            return false;
        }

        public static bool IsPredefinedGenericType(this Type type)
        {
            if (type.IsGenericType)
            {
                type = type.GetGenericTypeDefinition();
                return type == typeof(Nullable<>);
            }

            return false;
        }

        public static bool IsPredefinedType(this Type type)
        {
            if (!type.IsPredefinedSimpleType() && !type.IsPredefinedGenericType() && type != typeof(byte[]))
            {
                return string.Compare(type.FullName, "System.Xml.Linq.XElement", StringComparison.Ordinal) == 0;
            }

            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsPlainObjectType(this Type type)
        {
            return type.IsClass && !type.IsSequenceType() && !type.IsPredefinedType();
        }

        public static bool IsInteger(this Type type)
        {
            TypeCode typeCode = Type.GetTypeCode(type);
            TypeCode typeCode2 = typeCode;
            if ((uint)(typeCode2 - 5) <= 7u)
            {
                return true;
            }

            return false;
        }

        //
        // Summary:
        //     Gets the underlying type of a System.Nullable`1 type.
        public static Type GetNonNullableType(this Type type)
        {
            if (!type.IsNullable(out var elementType))
            {
                return type;
            }

            return elementType;
        }

        public static bool IsNullable(this Type type, out Type elementType)
        {
            if (type != null && type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                elementType = type.GetGenericArguments()[0];
            }
            else
            {
                elementType = type;
            }

            return elementType != type;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsEnumType(this Type type)
        {
            return type.GetNonNullableType().IsEnum;
        }

        public static bool IsConstructable(this Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            if (type.IsAbstract || type.IsInterface || type.IsArray || type.IsGenericTypeDefinition || type == typeof(void))
            {
                return false;
            }

            if (!type.HasDefaultConstructor())
            {
                return false;
            }

            return true;
        }

        [DebuggerStepThrough]
        public static bool IsAnonymous(this Type type)
        {
            if (type.IsGenericType)
            {
                Type genericTypeDefinition = type.GetGenericTypeDefinition();
                if (genericTypeDefinition.IsClass && genericTypeDefinition.IsSealed && genericTypeDefinition.Attributes.HasFlag(TypeAttributes.AnsiClass))
                {
                    object[] customAttributes = genericTypeDefinition.GetCustomAttributes(typeof(CompilerGeneratedAttribute), inherit: false);
                    if (customAttributes != null && customAttributes.Length != 0)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        [DebuggerStepThrough]
        public static bool HasDefaultConstructor(this Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            if (type.IsValueType)
            {
                return true;
            }

            return type.GetConstructors(BindingFlags.Instance | BindingFlags.Public).Any((ConstructorInfo ctor) => ctor.GetParameters().Length == 0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsSubClass(this Type type, Type check)
        {
            Type implementingType;
            return type.IsSubClass(check, out implementingType);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsSubClass(this Type type, Type check, out Type implementingType)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            if (check == null)
            {
                throw new ArgumentNullException("check");
            }

            return IsSubClassInternal(type, type, check, out implementingType);
        }

        private static bool IsSubClassInternal(Type initialType, Type currentType, Type check, out Type implementingType)
        {
            if (currentType == check)
            {
                implementingType = currentType;
                return true;
            }

            if (check.IsInterface && (initialType.IsInterface || currentType == initialType))
            {
                Type[] interfaces = currentType.GetInterfaces();
                foreach (Type currentType2 in interfaces)
                {
                    if (IsSubClassInternal(initialType, currentType2, check, out implementingType))
                    {
                        if (check == implementingType)
                        {
                            implementingType = currentType;
                        }

                        return true;
                    }
                }
            }

            if (currentType.IsGenericType && !currentType.IsGenericTypeDefinition && IsSubClassInternal(initialType, currentType.GetGenericTypeDefinition(), check, out implementingType))
            {
                implementingType = currentType;
                return true;
            }

            if (currentType.BaseType == null)
            {
                implementingType = null;
                return false;
            }

            return IsSubClassInternal(initialType, currentType.BaseType, check, out implementingType);
        }

        public static bool IsCompatibleWith(this Type source, Type target)
        {
            if (source == target)
            {
                return true;
            }

            if (!target.IsValueType)
            {
                return target.IsAssignableFrom(source);
            }

            Type nonNullableType = source.GetNonNullableType();
            Type nonNullableType2 = target.GetNonNullableType();
            if (nonNullableType == source || nonNullableType2 != target)
            {
                TypeCode typeCode = (nonNullableType.IsEnum ? TypeCode.Object : Type.GetTypeCode(nonNullableType));
                TypeCode typeCode2 = (nonNullableType2.IsEnum ? TypeCode.Object : Type.GetTypeCode(nonNullableType2));
                switch (typeCode)
                {
                    case TypeCode.SByte:
                        switch (typeCode2)
                        {
                            case TypeCode.SByte:
                            case TypeCode.Int16:
                            case TypeCode.Int32:
                            case TypeCode.Int64:
                            case TypeCode.Single:
                            case TypeCode.Double:
                            case TypeCode.Decimal:
                                return true;
                        }

                        break;
                    case TypeCode.Byte:
                        {
                            TypeCode typeCode3 = typeCode2;
                            TypeCode typeCode4 = typeCode3;
                            if ((uint)(typeCode4 - 6) > 9u)
                            {
                                break;
                            }

                            return true;
                        }
                    case TypeCode.Int16:
                        switch (typeCode2)
                        {
                            case TypeCode.Int16:
                            case TypeCode.Int32:
                            case TypeCode.Int64:
                            case TypeCode.Single:
                            case TypeCode.Double:
                            case TypeCode.Decimal:
                                return true;
                        }

                        break;
                    case TypeCode.UInt16:
                        {
                            TypeCode typeCode9 = typeCode2;
                            TypeCode typeCode10 = typeCode9;
                            if ((uint)(typeCode10 - 8) > 7u)
                            {
                                break;
                            }

                            return true;
                        }
                    case TypeCode.Int32:
                        switch (typeCode2)
                        {
                            case TypeCode.Int32:
                            case TypeCode.Int64:
                            case TypeCode.Single:
                            case TypeCode.Double:
                            case TypeCode.Decimal:
                                return true;
                        }

                        break;
                    case TypeCode.UInt32:
                        {
                            TypeCode typeCode13 = typeCode2;
                            TypeCode typeCode14 = typeCode13;
                            if ((uint)(typeCode14 - 10) > 5u)
                            {
                                break;
                            }

                            return true;
                        }
                    case TypeCode.Int64:
                        {
                            TypeCode typeCode11 = typeCode2;
                            TypeCode typeCode12 = typeCode11;
                            if (typeCode12 != TypeCode.Int64 && (uint)(typeCode12 - 13) > 2u)
                            {
                                break;
                            }

                            return true;
                        }
                    case TypeCode.UInt64:
                        {
                            TypeCode typeCode7 = typeCode2;
                            TypeCode typeCode8 = typeCode7;
                            if ((uint)(typeCode8 - 12) > 3u)
                            {
                                break;
                            }

                            return true;
                        }
                    case TypeCode.Single:
                        {
                            TypeCode typeCode5 = typeCode2;
                            TypeCode typeCode6 = typeCode5;
                            if ((uint)(typeCode6 - 13) > 1u)
                            {
                                break;
                            }

                            return true;
                        }
                    default:
                        if (nonNullableType == nonNullableType2)
                        {
                            return true;
                        }

                        break;
                }
            }

            return false;
        }

        public static string GetTypeName(this Type type)
        {
            if (type.IsNullable(out var elementType))
            {
                return elementType.Name + "?";
            }

            return type.Name;
        }


         

        //
        // Summary:
        //     Given a MethodBase for a property's get or set method, return the corresponding
        //     property info.
        //
        // Parameters:
        //   method:
        //     MethodBase for the property's get or set method.
        //
        // Returns:
        //     PropertyInfo for the property, or null if method is not part of a property.
        public static PropertyInfo GetPropertyFromMethod(this MethodBase method)
        {
            Guard.NotNull(method, "method");
            PropertyInfo result = null;
            if (method.IsSpecialName)
            {
                Type declaringType = method.DeclaringType;
                if (declaringType != null && (method.Name.StartsWith("get_", StringComparison.InvariantCulture) || method.Name.StartsWith("set_", StringComparison.InvariantCulture)))
                {
                    string name = method.Name.Substring(4);
                    result = declaringType.GetProperty(name);
                }
            }

            return result;
        }

        internal static Type FindIEnumerable(this Type seqType)
        {
            if (seqType == null || seqType == typeof(string))
            {
                return null;
            }

            if (seqType.IsArray)
            {
                return typeof(IEnumerable<>).MakeGenericType(seqType.GetElementType());
            }

            if (seqType.IsGenericType)
            {
                Type[] genericArguments = seqType.GetGenericArguments();
                Type[] array = genericArguments;
                foreach (Type type in array)
                {
                    Type type2 = typeof(IEnumerable<>).MakeGenericType(type);
                    if (type2.IsAssignableFrom(seqType))
                    {
                        return type2;
                    }
                }
            }

            Type[] interfaces = seqType.GetInterfaces();
            if (interfaces.Length != 0)
            {
                Type[] array2 = interfaces;
                foreach (Type seqType2 in array2)
                {
                    Type type3 = seqType2.FindIEnumerable();
                    if (type3 != null)
                    {
                        return type3;
                    }
                }
            }

            if (seqType.BaseType != null && seqType.BaseType != typeof(object))
            {
                return seqType.BaseType.FindIEnumerable();
            }

            return null;
        }
    }
    public interface IOrdered
    {
        int Ordinal { get; }
    }
}
