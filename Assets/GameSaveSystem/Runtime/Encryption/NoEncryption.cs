using UnityEngine;

namespace GameSaveSystem.Encryption
{
    [CreateAssetMenu(
        fileName = "AES Encrypter",
        menuName = "Game Save System/Encryption/None")]
    public class NoEncryption : Encrypter
    {
        public override string Encrypt(string _)
        {
            return _;
        }

        public override string Decrypt(string _)
        {
            return _;
        }
    }
}
