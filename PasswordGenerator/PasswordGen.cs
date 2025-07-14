using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;

namespace PasswordGenerator
{
    public class PasswordGen
    {
        public int pwLength;
        public int maxPwLength;
        public int numWords;
        public int lengthOfWords;
        public int minLengthOfWord;
        public int maxLengthOfWord;
        public bool manageLengthOfWords;
        public bool lowercaseChars;
        public bool uppercaseChars;
        public bool numbersIncluded;
        public bool symbolsIncluded;
        public bool excludeSimilarChars;
        public bool excludeAmbigChars;
        public bool useDictionaryWords;
        public bool capitalizeFirstLetter;
        public List<string> passwords;
        private string CapitalizeFirstLetter(string str)
        {
            if (string.IsNullOrEmpty(str))
                return string.Empty;

            char firstLetter = Char.ToUpper(str[0]);
            if (str.Length == 1)
                return firstLetter.ToString();
            else
                return firstLetter.ToString() + str.Substring(1);
        }

        private void AddPasswordToList(string password)
        {
            if (password.Length > maxPwLength)
            {
                password = password.Substring(0, maxPwLength);
            }
            passwords.Add(password);
        }

        private string GenerateCharacterPool()
        {
            const string lowercase = "abcdefghijklmnopqrstuvwxyz";
            const string uppercase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string numbers = "0123456789";
            const string symbols = "!@#$%^&*()-_=+[]{};:'\",.<>/?`~";
            const string similarChars = "il1Lo0O";
            const string ambiguousChars = "{}[]()/\\'\"`~,;:.<>";

            StringBuilder preExclusionPool = new StringBuilder(); // we will need to build the character pool before we randomly pick letters for password
            string finalPool;

            if (lowercaseChars)
                preExclusionPool.Append(lowercase);
            if (uppercaseChars)
                preExclusionPool.Append(uppercase);
            if (numbersIncluded)
                preExclusionPool.Append(numbers);
            if (symbolsIncluded)
                preExclusionPool.Append(symbols);

            finalPool = preExclusionPool.ToString();

            if (excludeSimilarChars)
                // we can use .Where() IEnumerable as a filter to get rid of specific chars
                finalPool = new string(finalPool.Where(c => !similarChars.Contains(c)).ToArray());
            if (excludeAmbigChars)
                finalPool = new string(finalPool.Where(c => !ambiguousChars.Contains(c)).ToArray());

            if (finalPool == "")
                return string.Empty;

            return finalPool;

        }

        public void GeneratePassword()
        {
            string finalPool = GenerateCharacterPool();
            StringBuilder newPassword = new StringBuilder();
            RandomNumberGenerator rng = RandomNumberGenerator.Create();
            byte[] randomByte = new byte[1]; // max possible length is 255

            for (int i = 0; i < pwLength; i++)
            {
                rng.GetBytes(randomByte);
                uint randomNumber = randomByte[0];

                // A % B, 0 <= R < B
                // remainder will always be within 0 to finalPool.Length - 1 so index is guaranteed to be within bounds
                int index = (int)(randomNumber % (uint)finalPool.Length);
                newPassword.Append(finalPool[index]);
            }

            AddPasswordToList(newPassword.ToString());
        }

        public string GeneratePassword(bool returnAsString)
        {
            string finalPool = GenerateCharacterPool();
            StringBuilder newPassword = new StringBuilder();
            RandomNumberGenerator rng = RandomNumberGenerator.Create();
            byte[] randomByte = new byte[1]; // max possible length is 255

            for (int i = 0; i < pwLength; i++)
            {
                rng.GetBytes(randomByte);
                uint randomNumber = randomByte[0];
                int index = (int)(randomNumber % (uint)finalPool.Length);
                newPassword.Append(finalPool[index]);
            }

            return newPassword.ToString();
        }


        public void GeneratePasswordWithDictionary()
        {
            string finalPool = GenerateCharacterPool();
            List<string> words = new List<string>();
            string[] lines = Properties.Resources.words_alpha.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);

            if (manageLengthOfWords)
                lines = lines.Where(s => s.Length == lengthOfWords).ToArray();

            RandomNumberGenerator rng = RandomNumberGenerator.Create();
            byte[] randomBytes = new byte[4]; // max supported lines for our dictionary is 4,294,967,295

            for (int i = 0; i < numWords; i++)
            {
                rng.GetBytes(randomBytes);
                uint randomNumber = BitConverter.ToUInt32(randomBytes, 0);
                int index = (int)(randomNumber % (uint)lines.Length);
                string newWord = lines[index];
                if (capitalizeFirstLetter)
                {
                    newWord = CapitalizeFirstLetter(newWord);
                }

                words.Add(newWord);
            }

            string newPassword = string.Join("", words);
            if (finalPool.Length > 0)
            {
                newPassword += GeneratePassword(true);
            }

            AddPasswordToList(newPassword);
        }

        public PasswordGen()
        {
            passwords = new List<string>();
        }
    }
}
