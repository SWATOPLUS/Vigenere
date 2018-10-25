using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Vigenere
{
    public static class Analysis
    {
        public static int CalculateKeyLength(string encryptedText)
        {
            var gramLength = 3;

            var allDistances = new List<int>();

            foreach (var distance in Enumerable.Range(gramLength, Math.Min(200, encryptedText.Length)))
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

            var advancedDistances = allDistances.FilterByOccurs(o => o > countBarrier);

            var gcd = advancedDistances.Aggregate(Gcd);

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
    }
}
