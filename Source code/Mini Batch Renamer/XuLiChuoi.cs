using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WpfApp1
{
    public partial class XuLiChuoi
    {
        public static string Replace(string sourceString, string oldString, string newString)
        {
            string destinationString = sourceString.Replace(oldString, newString);
            return destinationString;
        }

        public static string ToUpper(string sourceString)
        {
            string destinationString = sourceString.ToUpper();
            return destinationString;
        }

        public static string ToLower(string sourceString)
        {
            string destinationString = sourceString.ToLower();
            return destinationString;
        }

        public static string ConvertFirstLetterOfEachWordToUpper(string sourceString)
        {
            string destinationString = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(sourceString.ToLower());
            return destinationString;
        }

        public static string Trim(string sourceString)
        {
            string destinationString = sourceString.Trim();
            return destinationString;
        }

        public static string AllowOnlyOneSpaceBetweenWords(string sourceString)
        {
            string destinationString;
            Regex regex = new Regex(@"[ ]{2,}", RegexOptions.None);
            destinationString = regex.Replace(sourceString, @" ");
            return destinationString;
        }

        public static string MoveISBNFromFrontToEndOfString(string sourceString, int lenISBN)
        {
            string destinationString;
            string isbn = sourceString.Substring(0, lenISBN);
            destinationString = sourceString.Remove(0, lenISBN);
            destinationString = destinationString.Trim();
            destinationString = destinationString + " " + isbn;
            return destinationString;
        }

        public static string MoveISBNFromEndToFrontOfString(string sourceString, int lenISBN)
        {
            string destinationString;
            string isbn = sourceString.Substring(sourceString.Length - lenISBN, lenISBN);
            destinationString = sourceString.Remove(sourceString.Length - lenISBN, lenISBN);
            destinationString = destinationString.Trim();
            destinationString = isbn + " " + destinationString;
            return destinationString;
        }

        public static string CreateUniqueString()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
