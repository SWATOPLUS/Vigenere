using System;
using System.Collections.Generic;
using System.Linq;

namespace Vigenere
{
    public static class Analysis
    {
        public static int CalculateKeyLength(string encryptedText)
        {
            var gramLength = 3;

            var allDistances = new List<int>();

            foreach (var distance in Enumerable.Range(gramLength, Math.Min(200, encryptedText.Length - 2 * gramLength)))
            {
                foreach (var index in Enumerable.Range(0, encryptedText.Length - distance - gramLength))
                {
                    var aSub = encryptedText.Substring(index, gramLength);
                    var bSub = encryptedText.Substring(index + distance, gramLength);

                    if (aSub == bSub)
                    {
                        allDistances.Add(distance);
                    }
                }
            }

            var countBarrier = (int) Math.Floor(Math.Sqrt(encryptedText.Length) / 10.0);

            var advancedDistances = allDistances.FilterByOccurs(x => x > countBarrier);

            var gcd = advancedDistances.DefaultIfEmpty(1).Aggregate(Gcd);

            return gcd;
        }



        public static int Gcd(int a, int b)
        {
            while (a != 0 && b != 0)
            {
                if (a > b)
                {
                    a %= b;
                }
                else
                {
                    b %= a;
                }
            }

            return a == 0 ? b : a;
        }

        public static string BuildKey(char[] alphabet, string encryptedText, int keyLength, char modeLetter)
        {
            return encryptedText
                .Select((x, i) => (letter: x, group: i % keyLength))
                .GroupBy(x => x.group)
                .Select(x => x.Select(y => y.letter))
                .Select(x => x.Mode() - modeLetter)
                .Select(x => Crypt.OffsetToChar(x, alphabet))
                .Sum();
        }

        public static string BuildKeyByPearson(char[] alphabet, string encryptedText, int keyLength,
            IDictionary<char, double> alphabetFreq)
        {
            return encryptedText
                .Select((x, i) => (letter: x, group: i % keyLength))
                .GroupBy(x => x.group)
                .Select(x => x.Select(y => Crypt.CharToOffset(y.letter, alphabet)))
                .Select(x => GetKeyOffset(x, alphabetFreq))
                .Select(x => Crypt.OffsetToChar(x, alphabet))
                .Sum();
        }

        private static int GetKeyOffset(IEnumerable<int> text, IDictionary<char, double> alphabetFreq)
        {
            var practicalFreq = text.ToCountArray(0, alphabetFreq.Count).ToFrequencyArray();
            var theoryFreq = alphabetFreq.OrderBy(x => x.Key).Select(x => x.Value).ToArray();

            return Enumerable.Range(0, practicalFreq.Length)
                .Select(shift => practicalFreq.Shift(shift))
                .Select((x, i) => (shift: i, chi: CalcChiSquare(x.ToArray(), theoryFreq)))
                .OrderBy(x => x.chi)
                .Select(x => x.shift)
                .First();

        }

        private static double CalcChiSquare(IList<double> practical, IList<double> theoretical)
        {
            var len = Math.Min(practical.Count, theoretical.Count);
            var value = 0.0;
            for (var i = 0; i < len; i++)
            {
                value += Math.Pow(practical[i] - theoretical[i], 2) / theoretical[i];
            }

            return value;
        }
    }
}
