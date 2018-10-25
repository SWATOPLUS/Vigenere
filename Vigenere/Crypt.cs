using System.Linq;

namespace Vigenere
{
    public static class Crypt
    {
        public static char EncryptCaesarOffset(int offset, int key, char[] alphabet)
        {
            return OffsetToChar(offset + key, alphabet);
        }

        public static char DecryptCaesarOffset(int offset, int key, char[] alphabet)
        {
            return OffsetToChar(offset - key, alphabet);
        }

        public static char EncryptCaesarLetter(char letter, char key, char[] alphabet)
        {
            return EncryptCaesarOffset(CharToOffset(letter, alphabet), CharToOffset(key, alphabet), alphabet);
        }

        public static char DecryptCaesarLetter(char letter, char key, char[] alphabet)
        {
            return DecryptCaesarOffset(CharToOffset(letter, alphabet), CharToOffset(key, alphabet), alphabet);
        }

        public static char OffsetToChar(int offset, char[] alphabet)
        {
            var length = alphabet.Length;

            return (char)((offset % length + length) % length + alphabet.First());
        }

        public static int CharToOffset(char letter, char[] alphabet)
        {
            return letter - alphabet.First();
        }

        public static string EncryptCaesarText(string text, char key, char[] alphabet)
        {
            return text.Select(x => EncryptCaesarLetter(x, key, alphabet)).Sum();
        }

        public static string DecryptCaesarText(string text, char key, char[] alphabet)
        {
            return text.Select(x => DecryptCaesarLetter(x, key, alphabet)).Sum();
        }

        public static string EncryptVigenereText(string text, string key, char[] alphabet)
        {
            return text.Select((x, i) => EncryptCaesarLetter(x, key[i % key.Length], alphabet)).Sum();
        }

        public static string DecryptVigenereText(string text, string key, char[] alphabet)
        {
            return text.Select((x, i) => DecryptCaesarLetter(x, key[i % key.Length], alphabet)).Sum();
        }
    }
}
