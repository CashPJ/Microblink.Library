using System;
using System.Text;

namespace Microblink.Extensions
{
    public static class StringExtension
    {
        /// <summary>
        /// Converts string to MD5
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToMD5(this string input)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }

        /// <summary>
        /// Coding sugar for extension string.IsNullOrEmpty(input)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this string input)
        {
            return string.IsNullOrEmpty(input);
        }

        /// <summary>
        /// Coding sugar for null check
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsNull(this string input)
        {
            return input == null;
        }

        /// <summary>
        /// Converts string to int.
        /// <para>If string is not parsable, returns defaultValue or 0.</para>
        /// </summary>
        /// <param name="input"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static int ToInt(this string input, int? defaultValue = null)
        {            
            var defaultIntValue = defaultValue ?? default;
            return input.ToIntNullable(defaultIntValue).Value;
        }

        /// <summary>
        /// Converts string to nullable int.
        /// <para>If string is not parsable and default value is not set, returns null.</para>
        /// </summary>
        /// <param name="input"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static int? ToIntNullable(this string input, int? defaultValue = null)
        {
            int result;            
            bool success = int.TryParse(input, out result);

            if (!success)
                return defaultValue;

            return result;
        }

        /// <summary>
        /// String obfuscation
        /// </summary>
        /// <param name="s"></param>
        /// <param name="obfuscationSymbol"></param>
        /// <param name="obfuscationTerm"></param>
        /// <param name="padLeft"></param>
        /// <param name="padRight"></param>
        /// <returns></returns>
        public static string Obfuscate(this string s, char obfuscationSymbol = '*', string obfuscationTerm = null, int padLeft = 0, int padRight = 0)
        {
            if (string.IsNullOrEmpty(s)) return s;

            if (padLeft > 0 && padLeft > s.Length)
                padLeft = s.Length;

            if (padRight > 0 && padRight > s.Length - padLeft)
                padRight = s.Length - padLeft;

            if(padLeft + padRight < s.Length)
            {
                int len = s.Length;                
                return 
                    s.Substring(0, padLeft) 
                    +
                    (
                    obfuscationTerm.IsNull() 
                    ? new string(obfuscationSymbol, len - padLeft - padRight) 
                    : obfuscationTerm
                    )
                    + s.Substring(len - padRight);
            }

            return s;
        }

        /// <summary>
        /// Validates Croatian personal identification number (OIB)
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsValidOIB(this String s)
        {
            if (s.IsNullOrEmpty() || s.Length != 11) return false;

            long b;
            if (!long.TryParse(s, out b)) return false;

            int a = 10;
            for (int i = 0; i < 10; i++)
            {
                a = a + Convert.ToInt32(s.Substring(i, 1));
                a = a % 10;
                if (a == 0) a = 10;
                a *= 2;
                a = a % 11;
            }
            int controlNumber = 11 - a;
            if (controlNumber == 10) controlNumber = 0;

            return controlNumber == Convert.ToInt32(s.Substring(10, 1));
        }

        public static string RemoveNewLines(this string s)
        {
            return s.Replace("\n", "").Replace("\r", "");
        }

        /// <summary>
        /// Transform single string character to char. 
        /// <para>Performing operation on string with greater length then 1 will result with exception.</para>
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static char ToChar(this string s)
        {
            if (string.IsNullOrEmpty(s))
                throw new ArgumentException("String can not be null or empty");

            var chars = s.ToCharArray();

            if (chars.Length > 1)
                throw new ArgumentException("String must be single character for this operation");

            return chars[0];
        }
    }
}
