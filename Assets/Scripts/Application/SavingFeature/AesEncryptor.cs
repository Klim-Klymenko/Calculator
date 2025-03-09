using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using JetBrains.Annotations;

namespace Application.SavingFeature
{
    [UsedImplicitly]
    internal class AesEncryptor
    {
        private const string Password = "DFg%KzaUpf@k#H*FaJ8s";
        private static readonly byte[] _salt = { 0x52, 0x41, 0x16, 0x79, 0x86, 0x64, 0x97, 0x22 };
        
        internal string Encrypt(string input, string password = null, byte[] salt = null)
        {
            password ??= Password;
            salt ??= _salt;

            byte[] convertedInput = Encoding.UTF8.GetBytes(input);
            byte[] encryptedInput = Encrypt(convertedInput, password, salt);
            
            return Convert.ToBase64String(encryptedInput);
        }
        
        internal byte[] Encrypt(byte[] input, string password = null, byte[] salt = null)
        {
            password ??= Password;
            salt ??= _salt;

            using Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(password, salt);

            using MemoryStream ms = new MemoryStream();
            using Aes aes = Aes.Create();
            
            aes.Key = pdb.GetBytes(aes.KeySize / 8);
            aes.IV = pdb.GetBytes(aes.BlockSize / 8);

            using CryptoStream cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write);
            
            cs.Write(input, 0, input.Length);
            cs.Close();

            return ms.ToArray();
        }
        
        internal string Decrypt(string input, string password = null, byte[] salt = null)
        {
            password ??= Password;
            salt ??= _salt;

            byte[] convertedInput = Convert.FromBase64String(input);
            byte[] decryptedInput = Decrypt(convertedInput, password, salt);
            
            return Encoding.UTF8.GetString(decryptedInput);
        }
        
        internal byte[] Decrypt(byte[] input, string password = null, byte[] salt = null)
        {
            password ??= Password;
            salt ??= _salt;
            
            using Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(password, salt);
            using MemoryStream ms = new MemoryStream();
            using Aes aes = Aes.Create();
            
            aes.Key = pdb.GetBytes(aes.KeySize / 8);
            aes.IV = pdb.GetBytes(aes.BlockSize / 8);
            
            using CryptoStream cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write);
            
            cs.Write(input, 0, input.Length);
            cs.Close();
            
            return ms.ToArray();
        }
    }
}