using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Vigenere.Cli
{
    internal class Program
    {
        private const string PureFileName = "pure.txt";
        private const string EncryptedFileName = "encrypted.txt";
        private const string DecryptedFileName = "decrypted.txt";

        public static void Main(string[] args)
        {
            CryptAndAnalyze();
        }

        public static readonly IDictionary<char, double> AlphabetFreq =
            new Dictionary<char, double>
            {
                {'A', 0.08167},
                {'B', 0.01492},
                {'C', 0.02782},
                {'D', 0.04253},
                {'E', 0.12702},
                {'F', 0.0228},
                {'G', 0.02015},
                {'H', 0.06094},
                {'I', 0.06966},
                {'J', 0.00153},
                {'K', 0.00772},
                {'L', 0.04025},
                {'M', 0.02406},
                {'N', 0.06749},
                {'O', 0.07507},
                {'P', 0.01929},
                {'Q', 0.00095},
                {'R', 0.05987},
                {'S', 0.06327},
                {'T', 0.09056},
                {'U', 0.02758},
                {'V', 0.00978},
                {'W', 0.0236},
                {'X', 0.0015},
                {'Y', 0.01974},
                {'Z', 0.00074}
            };

        public static readonly char ModeLetter = AlphabetFreq
            .OrderByDescending(x => x.Value)
            .Select(x => x.Key)
            .First();

        public static readonly char[] Alphabet = AlphabetFreq.Keys.ToArray();

        public static void CryptAndAnalyze()
        {
            var key = string.Join("", Alphabet.GetRandomElements(4));

            Console.WriteLine($"Crypt key is {key}");

            var text = File.ReadAllText(PureFileName);

            var encryptedText = Crypt.EncryptVigenereText(text, key, Alphabet);
            File.WriteAllText(EncryptedFileName, encryptedText);

            var keyLength = Analysis.CalculateKeyLength(encryptedText);

            Console.WriteLine($"Key length is {keyLength}");

            var hackedKey = Analysis.BuildKey(Alphabet, encryptedText, keyLength, ModeLetter);
            Console.WriteLine($"Decrypt key is {hackedKey}");

            var decryptedText = Crypt.DecryptVigenereText(encryptedText, hackedKey, Alphabet);
            File.WriteAllText(DecryptedFileName, decryptedText);
        }
    }
}
