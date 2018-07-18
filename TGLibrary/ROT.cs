using System;

namespace TGLibrary {
    public static class ROT {
        public static string Encrypt(string DataToEncrypt) {
            return Crypter(Config.DefaultRotation, DataToEncrypt);
        }
        public static string Encrypt(int rotation, string DataToEncrypt) {
            return Crypter(rotation, DataToEncrypt);
        }

        public static string Decrypt(string DataToDecrypt) {
            return Crypter(-Config.DefaultRotation, DataToDecrypt);
        }
        public static string Decrypt(int rotation, string DataToDecrypt) {
            return Crypter(-rotation, DataToDecrypt);
        }

        private static string Crypter(int rotation, string DataToCrypt) {
            char[] dataArray = DataToCrypt.ToCharArray();
            int cipher = Characters.Alphanumeric.Length + rotation;
            string output = "";
            for (int i = 0; i < dataArray.Length; i++) {
                // Current character in Data
                char current = dataArray[i];
                // Char 'rotation' letters [+ AFTER / - BEFORE]'current'
                char rotated = Characters.Alphanumeric[cipher % (Characters.Alphanumeric.Length)];
                for (int letter = 0; letter < Characters.Alphanumeric.Length; letter++) {
                    // If data is equal to the current letter
                    if (current == Characters.Alphanumeric[letter]) {
                        // Replace that letter with the rotated letter
                        dataArray[i] = rotated;
                        output = new string(dataArray);
                        // Letter found, next data.
                        break;
                    }
                    // Next rotated letter, modulo to keep within bounds of Characters
                    rotated = Characters.Alphanumeric[((cipher + letter + 1) % (Characters.Alphanumeric.Length))];
                }
            }
            return new string(dataArray);
        }
    }
}