using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Extensions
{
    public static class StringExtension
    {
        /// <summary>
        ///     Replace single quotes (') by acute accent (´)
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string ReplaceSingleQuotes(this string text)
        {
            return text.Replace("'", "´");
        }

        /// <summary>
        ///     Extract only letters (A-Z, a-z) and numbers (0-9).  Latinn characters are ignored too
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string ExtractLettersAndNumbers(this string text)
        {
            var regex = new Regex("[^0-9a-zA-Z]");
            return regex.Replace(text, String.Empty);
        }

        /// <summary>
        ///     Extract only number (0-9)
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string ExtractNumbers(this string text)
        {
            var regex = new Regex("[^0-9]");
            return regex.Replace(text, String.Empty);
        }

        /// <summary>
        ///     Remove accents of the word replacing by same letter without accent
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string RemoveAccent(this string text)
        {
            if (string.IsNullOrEmpty(text))
                return String.Empty;
            byte[] bytes = Encoding.GetEncoding("ISO-8859-8").GetBytes(text);
            return Encoding.UTF8.GetString(bytes);
        }

        /// <summary>
        ///     Remove special characters
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string RemoveSpecialCharacters(this string text)
        {
            const string chars = "~`!@#$%^&*()-+=[]{}\\|;:\",.<>/?";
            string value = chars.Aggregate(text,
                (current, c) => current.Replace(c.ToString(CultureInfo.InvariantCulture), String.Empty));
            return value;
        }

        /// <summary>
        ///     Returns a new string that remove a double white spaces in this intance
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string RemoveDoubleWhiteSpace(this string text)
        {
            var regex = new Regex(@"\s+");
            return regex.Replace(text, " ");
        }

        /// <summary>
        ///     Returns a new string that remove a conjunction (de, da, e, etc) in this intance
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string RemoveConjunctions(this string text)
        {
            var regex =
                new Regex(
                    @"(\W|^)e\s|\sda\s|\sde\s|\sdi\s|\sdo\s|\sdu\s|\sdas\s|\sdes\s|\sdis\s|\sdos\s|\sdus(\W|$)",
                    RegexOptions.IgnoreCase);
            return regex.Replace(text, " ");
        }

        /// <summary>
        ///     Returns a new string that remove word of the only one letter
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string RemoveOneLetter(this string text)
        {
            return text.Split(' ').Where(v => v.Length > 1).Aggregate((current, v) => current + " " + v);
        }

		public static string Truncate(this string value, int maxLength)
		{
			if (string.IsNullOrEmpty(value)) return value;
			return value.Length <= maxLength ? value : value.Substring(0, maxLength);
		}

		/// <summary>
		///     Returns a new string that right-aligns the characters in this intance by padding them with spaces on the left
		///     especified Unicode character, for a specified total length.
		///     If the length of string is greater then totalWidth, it is truncated
		/// </summary>
		/// <param name="text"></param>
		/// <param name="totalWidth">
		///     The number of characters in the resulting string, equal to the number of original characters
		///     plus any additional padding characters
		/// </param>
		/// <param name="paddingChar">A unicode padding character</param>
		/// <returns></returns>
		public static string PadLeftTrunc(this string text, int totalWidth, char paddingChar)
        {
            string value = text;
            if (value.Length > totalWidth)
                value = value.Substring(value.Length - totalWidth, totalWidth);
            value = value.PadLeft(totalWidth, paddingChar);
            return value;
        }

        /// <summary>
        ///     Returns a new string that left-aligns the characters in this intance by padding them with spaces on the right, for
        ///     a specified total length.
        ///     If the length of string is greater then totalWidth, it is truncated
        /// </summary>
        /// <param name="text"></param>
        /// <param name="totalWidth">
        ///     The number of characters in the resulting string, equal to the number of original characters
        ///     plus any additional padding characters
        /// </param>
        /// <returns></returns>
        public static string PadRightTrunc(this string text, int totalWidth)
        {
            string value = text;
            if (value.Length > totalWidth)
                value = value.Substring(0, totalWidth);
            value = value.PadRight(totalWidth);
            return value;
        }

        /// <summary>
        ///     Returns a new string that left-aligns the characters in this intance by padding them with spaces on the right
        ///     especified Unicode character, for a specified total length.
        ///     If the length of string is greater then totalWidth, it is truncated
        /// </summary>
        /// <param name="text"></param>
        /// <param name="totalWidth">
        ///     The number of characters in the resulting string, equal to the number of original characters
        ///     plus any additional padding characters
        /// </param>
        /// <param name="paddingChar">A unicode padding character</param>
        /// <returns></returns>
        public static string PadRightTrunc(this string text, int totalWidth, char paddingChar)
        {
            string value = text;
            if (value.Length > totalWidth)
                value = value.Substring(0, totalWidth);
            value = value.PadRight(totalWidth, paddingChar);
            return value;
        }

        /// <summary>
        ///     Captalize the first character each word into a string.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="separator">Char separator of the words</param>
        /// <param name="firstWord"></param>
        /// <returns></returns>
        public static string Captalize(this string text, bool firstWord = false, char separator = ' ')
        {
            if (string.IsNullOrWhiteSpace(text))
                return text;
            //
            string value;

            if (firstWord)
            {
                value = text.Trim().ToLower();
                value = char.ToUpper(value[0]) + (value.Length > 1 ? value.Substring(1) : null);
            }
            else
            {
                value = text.Trim()
                    .ToLower()
                    .Split(separator)
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .Aggregate("", (current, s) => current + (char.ToUpper(s[0]) + s.Substring(1) + separator));
                value = value.Remove(value.Length - 1);
            }
            //
            return value;
        }

        /// <summary>
        ///     Captalize the first character each word into a string.  Excludes special words (de, da, etc) of captalize
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string CaptalizeFromName(this string text)
        {
            //
            if (string.IsNullOrWhiteSpace(@text))
                return text;

            //
            string value = text;
            value = value.Captalize(false, '\'');
            value = value.Captalize();

            // Adjuste special words to lower case
            var excludeWords = new[] { "E", "Da", "De", "Di", "Do", "Du", "Das", "Des", "Dis", "Dos", "Dus" };

            //
            return excludeWords.Aggregate(value,
                (current, word) => current.Replace(string.Format(" {0} ", word), string.Format(" {0} ", word.ToLower())));
        }
		public static string UnformatCpfCnpj(this string document)
		{
			return document.Replace(".", string.Empty).Replace("-", string.Empty).Replace("/", string.Empty);
		}


		/// <summary>
		///     Returns a new string formated how CPF or CNPJ.  The type of formatting is choice according length of the
		///     characters.
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		public static string FormatCpfCnpj(this string text)
        {
            //
            if (string.IsNullOrWhiteSpace(text))
                return text;
            // Get only numbers
            long value;
            long.TryParse(ExtractNumbers(text), out value);
            if (value == 0)
                return String.Empty;
            // Determines whether CPF or CNPJ according to size
            if (value.ToString(CultureInfo.InvariantCulture).Length <= 11)
                return String.Format(@"{0:000\.000\.000\-00}", value);
            return String.Format(@"{0:00\.000\.000\/0000\-00}", value);
        }

        /// <summary>
        ///     Returns a new string formated how CPF or CNPJ.  The type of formatting is choice according length of the
        ///     characters.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string FormatZipCode(this string text)
        {
            //
            if (string.IsNullOrWhiteSpace(text))
                return text;
            // Get only numbers
            long value;
            long.TryParse(ExtractNumbers(text), out value);
            if (value == 0)
                return String.Empty;
            return String.Format(@"{0:00000\-000}", value);
        }

        /// <summary>
        ///     Returns a new string formated how phone number.  The type of formatting is choice according length of the
        ///     characters.
        /// </summary>
        /// <param name="text">Unformated Phone</param>
        /// <returns></returns>
        public static string FormatPhone(this string text)
        {
            //
            if (string.IsNullOrWhiteSpace(text))
                return text;


            string ddi = null;
            string ddd = null;
            string numberBlock1;
            string numberBlock2 = null;

            // Extract only numbers, remove white spaces and left zeros
            var value = text.ExtractNumbers().Trim().TrimStart('0');

            // formats the text according to their size
            switch (value.Length)
            {
                case 7: //IIIFFFF => III-FFFF
                    {
                        numberBlock1 = value.Substring(0, 3); // III
                        numberBlock2 = value.Substring(3, 4); // FFFF
                    }
                    break;
                case 8: //IIIIFFFF => IIII-FFFF
                    {
                        numberBlock1 = value.Substring(0, 4);
                        numberBlock2 = value.Substring(4, 4);
                    }
                    break;
                case 9: //DDIIIFFFF => (DD)III-FFFF | CIIIIFFFF => CIIII-FFFF
                    {
                        string block = value.Substring(2, 7).TrimStart('0'); // Can't be initialized by '0'

                        if (block.Length == 7)
                        {
                            //DDIIIFFFF => (DD)III-FFFF
                            ddd = value.Substring(0, 2);
                            numberBlock1 = block.Substring(0, 3);
                            numberBlock2 = block.Substring(3, 4);
                        }
                        else
                        {
                            //IIIIIFFFF => IIIII-FFFF
                            ddd = value.Substring(0, 2);
                            numberBlock1 = block.Substring(0, block.Length - 4);
                            numberBlock2 = block.Substring(block.Length - 4, 4);
                        }
                    }
                    break;
                case 10: //DDIIIIFFFF => (DD)IIII-FFFF | PDDIIIFFFF => +P (DD)III-FFFF
                case 11: //DDCIIIIFFFF => (DD)CIIII-FFFF | PDDIIIIFFFF => +P (DD)IIII-FFFF
                    {
                        #region DDI one digit Exceptions

                        //DDI COUNTRY
                        //1	Anguila
                        //1	Antígua e Barbuda
                        //1	Bahamas
                        //1	Barbados
                        //1	Bermudas
                        //1	Canadá
                        //1	Dominica
                        //1	Estados Unidos
                        //1	Granada
                        //1	Ilhas Caimão
                        //1	Ilhas Marianas do Norte
                        //1	Ilhas Virgens Americanas
                        //1	Ilhas Virgens Britânicas
                        //1	Jamaica
                        //1	Montserrat
                        //1	Porto Rico
                        //1	República Dominicana
                        //1	Samoa Americana
                        //1	Santa Lúcia
                        //1	São Cristóvão e Neves
                        //1	São Vicente e Granadinas
                        //1	Trinidad e Tobago
                        //1	Turcas e Caicos
                        //7	Casaquistão
                        //7	Rússia

                        #endregion

                        // If DDD contais '0' and any country one digit exception and the block number is initialized by any char diferent '0'
                        if ((value[0] == '0' || value[1] == '0') && (value[0] == '1' || value[0] == '7') && value[3] != '0')
                        {
                            //PDDIIIIFFFF
                            ddd = value.Substring(1, 2); //DD

                            numberBlock1 = value.Substring(3, value.Length - 4); // CIIII | IIII | III
                            numberBlock2 = value.Substring((value.Length - 4), 4); // FFFF
                        }
                        else
                        {
                            ddd = value.Substring(0, 2); //DD

                            // Remove '0' on left side
                            string numberBlock = value.Substring(2, value.Length - 2).TrimStart('0');
                            // IIIIFFFF | IIIFFFF | CIIIIFFFF

                            numberBlock1 = numberBlock.Substring(0, numberBlock.Length - 4); // IIII | III | CIIII
                            numberBlock2 = numberBlock.Substring((numberBlock.Length - 4), 4); // FFFF | FFFF | FFFF
                        }
                    }
                    break;
                case 12: //PPDDIIIIFFFF => +PP (DD)IIII-FFFF
                case 13: //PPDDCIIIIFFFF => +PP (DD)CIIII-FFFF
                case 14: //PPPDDCIIIIFFFF => +PPP (DD)CIIII-FFFF
                    {
                        string block;
                        switch (value.Length)
                        {
                            case 12:
                            case 13:
                                {
                                    ddi = value.Substring(0, 2);
                                    ddd = value.Substring(2, 2);
                                    // Remove '0' on left side
                                    block = value.Substring(4).TrimStart('0');
                                    break;
                                }
                            default:
                                {
                                    ddi = value.Substring(0, 3);
                                    ddd = value.Substring(3, 2);
                                    // Remove '0' on left side
                                    block = value.Substring(5).TrimStart('0');
                                    break;
                                }
                        }

                        // IIIIFFFF | IIIFFFF | CIIIIFFFF

                        numberBlock1 = block.Substring(0, block.Length - 4); // IIII | III | CIIII
                        numberBlock2 = block.Substring((block.Length - 4), 4); // FFFF | FFFF | FFFF
                    }
                    break;
                default:
                    {
                        if (value.Length > 4) //CIIII => IIII
                        {
                            numberBlock1 = value.Substring(0, value.Length - 4);
                            numberBlock2 = value.Substring(value.Length - 4, 3);
                        }
                        else
                        {
                            numberBlock1 = value;
                        }
                        break;
                    }
            }


            return string.Format(@"{0}{1}{2}{3}{4}{5}{6}",
                !string.IsNullOrWhiteSpace(ddi) ? "+" + ddi : null,
                !string.IsNullOrWhiteSpace(ddd) ? "(" : null, // "(" | null
                ddd,
                !string.IsNullOrWhiteSpace(ddd) ? ")" : null, // ")" | null
                numberBlock1,
                (!string.IsNullOrWhiteSpace(numberBlock1) && !string.IsNullOrWhiteSpace(numberBlock2)) ? "-" : null,
                // "-" | null, 
                numberBlock2);
        }

        /// <summary>
        ///     Returns a new string formated how date.  The type of formatting is choice according length of the characters.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static string FormatDate(this string text, string format)
        {
            //
            if (string.IsNullOrWhiteSpace(text))
                return text;
            if (DateTimeFormatInfo.CurrentInfo == null)
                throw new Exception("");
            //
            string defaultFormat = "{0:" + string.Format("dd{0}MM{0}yyyy", DateTimeFormatInfo.CurrentInfo.DateSeparator) +
                                   "}";
            string dateSeparator = DateTimeFormatInfo.CurrentInfo.DateSeparator;
            //
            DateTime date;
            string value = "";
            // Check if date has a separator
            if (text.Contains(dateSeparator))
            {
                // Try parse text to a valid date. if is sucessful we will format the date
                if (DateTime.TryParse(text, out date))
                    value = string.Format(defaultFormat, date);
            }
            else
            {
                switch (text.Length)
                {
                    case 4:
                        {
                            value = string.Format("{0}{1}{2}{3}{4}", text.Substring(0, 2), dateSeparator,
                                text.Substring(2, 2), dateSeparator, DateTime.Now.Year);
                            break;
                        }
                    case 6:
                        {
                            value = string.Format("{0}{1}{2}{3}{4}", text.Substring(0, 2), dateSeparator,
                                text.Substring(2, 2), dateSeparator, text.Substring(4, 2));
                            break;
                        }
                    case 8:
                        {
                            value = string.Format("{0}{1}{2}{3}{4}", text.Substring(0, 2), dateSeparator,
                                text.Substring(2, 2), dateSeparator, text.Substring(4, 4));
                            break;
                        }
                    default:
                        {
                            value = "";
                            break;
                        }
                }
            }

            //
            if (!string.IsNullOrWhiteSpace(value))
            {
                format = "{0:" + format + "}";
                value = DateTime.TryParse(value, out date) ? string.Format(format, date) : "";
            }

            //
            return value;
        }

        /// <summary>
        ///     Case insensitive replace
        /// </summary>
        /// <param name="original">Original string for search</param>
        /// <param name="pattern">String will be repleced</param>
        /// <param name="replacement">String for replacement</param>
        /// <returns>the new string</returns>
        public static string InsensitiveReplace(this string original, string pattern, string replacement)
        {
            int count = 0, position0 = 0, position1;

            string upperString = original.ToUpper();
            string upperPattern = pattern.ToUpper();
            int inc = (original.Length / pattern.Length) * (replacement.Length - pattern.Length);
            var chars = new char[original.Length + Math.Max(0, inc)];
            while ((position1 = upperString.IndexOf(upperPattern, position0, StringComparison.Ordinal)) != -1)
            {
                for (int i = position0; i < position1; ++i)
                    chars[count++] = original[i];
                for (int i = 0; i < replacement.Length; ++i)
                    chars[count++] = replacement[i];
                position0 = position1 + pattern.Length;
            }

            if (position0 == 0)
                return original;

            for (int i = position0; i < original.Length; ++i)
                chars[count++] = original[i];

            return new string(chars, 0, count);
        }

        /// <summary>
        /// Encrypt text
        /// </summary>
        /// <param name="text">Text to be a encrypt</param>
        /// <param name="password">Password used for encrypt</param>
        /// <returns></returns>
        public static string Encrypt(this string text, string password = "")
        {
            var crypt = new Crypt(password);
            return crypt.Encrypt(text);
        }

        /// <summary>
        /// Decrypt text
        /// </summary>
        /// <param name="text">Text to be a decrypt</param>
        /// <param name="password">Password used for decrypt</param>
        /// <returns></returns>
        public static string Decrypt(this string text, string password = "")
        {
            var crypt = new Crypt(password);
            return crypt.Decrypt(text);
        }

        public static string ExtractDomainFromEmail(this string text)
        {
            if (!text.Contains("@"))
                throw new Exception(string.Format("Couldn't extract the domain from email '{0}'", text));
            //
            return text.Split('@')[1];
        }

        public static string ToLowerCamelCase(this string text, bool removeSpecialCharacters = true)
        {
            var splittedPhrase = removeSpecialCharacters ? text.Split(' ', '-', '.') : new[] { text };

            var sb = new StringBuilder();

            foreach (var splittedPhraseChars in splittedPhrase.Select(s => s.ToCharArray()))
            {
                if (splittedPhraseChars.Length > 0)
                    splittedPhraseChars[0] = ((new String(splittedPhraseChars[0], 1)).ToLower().ToCharArray())[0];
                sb.Append(new String(splittedPhraseChars));
            }
            return sb.ToString();
        }

        public static string ToUpperCamelCase(this string text, bool removeSpecialCharacters = true)
        {
            var splittedPhrase = removeSpecialCharacters ? text.Split(' ', '-', '.') : new[] { text };

            var sb = new StringBuilder();

            foreach (var splittedPhraseChars in splittedPhrase.Select(s => s.ToCharArray()))
            {
                if (splittedPhraseChars.Length > 0)
                    splittedPhraseChars[0] = ((new String(splittedPhraseChars[0], 1)).ToUpper().ToCharArray())[0];
                sb.Append(new String(splittedPhraseChars));
            }
            return sb.ToString();
        }

        public static bool IsUrl(this string text)
        {
            var r = new Regex(@"((([A-Za-z]{3,9}:(?:\/\/)?)(?:[\-;:&=\+\$,\w]+@)?[A-Za-z0-9\.\-]+|(?:www\.|[\-;:&=\+\$,\w]+@)[A-Za-z0-9\.\-]+)((?:\/[\+~%\/\.\w\-_]*)?\??(?:[\-\+=&;%@\.\w_]*)#?(?:[\.\!\/\\\w]*))?)");
            var m = r.Match(text);
            return m.Success;
        }

        /// <summary>
        /// Returns the number of steps required to transform the source string
        /// into the target string.
        /// </summary>
        public static int ComputeLevenshteinDistance(this string source, string target)
        {
            if ((source == null) || (target == null)) return 0;
            if ((source.Length == 0) || (target.Length == 0)) return 0;
            if (source == target) return source.Length;

            var sourceWordCount = source.Length;
            var targetWordCount = target.Length;

            // Step 1
            if (sourceWordCount == 0)
                return targetWordCount;

            if (targetWordCount == 0)
                return sourceWordCount;

            var distance = new int[sourceWordCount + 1, targetWordCount + 1];

            // Step 2
            for (var i = 0; i <= sourceWordCount; distance[i, 0] = i++) { }
            for (var j = 0; j <= targetWordCount; distance[0, j] = j++) { }


            for (var i = 1; i <= sourceWordCount; i++)
            {
                for (var j = 1; j <= targetWordCount; j++)
                {
                    // Step 3
                    int cost = (target[j - 1] == source[i - 1]) ? 0 : 1;

                    // Step 4
                    distance[i, j] = Math.Min(
                                        Math.Min(distance[i - 1, j] + 1, distance[i, j - 1] + 1),
                                        distance[i - 1, j - 1] + cost);
                }
            }

            return distance[sourceWordCount, targetWordCount];
        }

        /// <summary> 
        /// Calculate percentage similarity of two strings
        /// <param name="source">Source String to Compare with</param>
        /// <param name="target">Targeted String to Compare</param>
        /// <returns>Return Similarity between two strings from 0 to 1.0</returns>
        /// </summary>
        public static double CalculateSimilarity(this string source, string target)
        {
            if ((source == null) || (target == null)) return 0.0;
            if ((source.Length == 0) || (target.Length == 0)) return 0.0;
            if (source == target) return 1.0;

            var stepsToSame = ComputeLevenshteinDistance(source, target);

            // ReSharper disable RedundantCast
            return (1.0 - ((double)stepsToSame / (double)Math.Max(source.Length, target.Length)));
            // ReSharper restore RedundantCast
        }

        public static string ToBase64(this string plainText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }

        public static string FromBase64(this string base64EncodedData)
        {
            var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public static bool IsTime(this string texto)
        {
            return TimeSpan.TryParse(texto, out TimeSpan result);
        }

        public static string TitleCase(this string text)
        {
            CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
            TextInfo textInfo = cultureInfo.TextInfo;

            return textInfo.ToTitleCase(text);
        }

        public static bool IsNullOrEmpyOrWhiteSpace(this string text)
        {
            return string.IsNullOrEmpty(text) || string.IsNullOrWhiteSpace(text);
        }
    }
}