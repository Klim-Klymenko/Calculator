using System.Collections.Generic;
using System.IO;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Unity.Plastic.Newtonsoft.Json;

namespace Application.SavingFeature
{
    [UsedImplicitly]
    internal sealed class Repository : IRepository
    {
        private const string FileName = "LocalState.json";
        private static readonly string _filePath = $"{UnityEngine.Application.persistentDataPath}/{FileName}";
        
        private Dictionary<string, string> _state;
        
        private readonly AesEncryptor _encryptor;

        internal Repository(AesEncryptor encryptor)
        {
            _encryptor = encryptor;
        }

        void IRepository.SetData<T>(T data)
        {
            string serializedData = JsonConvert.SerializeObject(data);
         
            string key = typeof(T).Name;
            _state[key] = serializedData;
        }

        bool IRepository.TryGetData<T>(out T data)
        {
            string key = typeof(T).Name;
            
            if (_state.TryGetValue(key, out string serializedData))
            {
                data = JsonConvert.DeserializeObject<T>(serializedData);
                return true;
            }
            
            data = default;
            return false;
        }

        async UniTask IRepository.SaveState()
        {
            string serializedState = JsonConvert.SerializeObject(_state);
            string encryptedState = _encryptor.Encrypt(serializedState);
       
            await File.WriteAllTextAsync(_filePath, encryptedState);
        }

        async UniTask IRepository.LoadState()
        {
            if (File.Exists(_filePath))
            {
                string encryptedState = await File.ReadAllTextAsync(_filePath);
                string decryptedData = _encryptor.Decrypt(encryptedState);
                
                _state = JsonConvert.DeserializeObject<Dictionary<string, string>>(decryptedData);
                return;
            }
            
            _state = new Dictionary<string, string>();
        }
    }
}