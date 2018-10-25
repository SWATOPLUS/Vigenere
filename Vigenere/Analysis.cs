using System.Collections.Generic;
using System.Linq;

namespace Vigenere
{
    public static class Analysis
    {
        public static int CalculateKeyLength(string encryptedText)
        {
            var allL = Enumerable.Range(1, 10).ToArray();
            var allDistances = allL.Select(l => new List<int>()).ToArray();
            foreach (var l in allL)
            {
                foreach (var distance in Enumerable.Range(1, 81))
                {
                    foreach (var index in Enumerable.Range(0, encryptedText.Length))
                    {
                        var isDuplicated = true;
                        foreach (var i in Enumerable.Range(0, l))
                        {
                            if (index + i + distance >= encryptedText.Length || encryptedText[index + i] != encryptedText[index + i + distance])
                            {
                                isDuplicated = false;
                            }
                        }
                        if (isDuplicated)
                        {
                            allDistances[l - 1].Add(distance);
                        }
                    }
                }
            }
            var advancedDistances = new List<int>();

            foreach (var i in Enumerable.Range(0, allDistances.Length))
            {
                if (allDistances[i].Count > 0)
                {
                    if (allDistances[i][0] == 1 && i < 2)
                    {
                        continue;
                    }
                    foreach (var j in Enumerable.Range(1, allDistances[i].Count - 1))
                    {
                        if (j < allDistances[i].Count - 1 && allDistances[i][j] == allDistances[i][j + 1])
                        {
                            advancedDistances.Add(allDistances[i][j]);
                        }
                    }
                }
            }

            return advancedDistances.Aggregate(Gcd);
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
