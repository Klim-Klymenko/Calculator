using System.Collections.Generic;
using System.IO;
using System.Linq;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Unity.Plastic.Newtonsoft.Json;
using UnityEngine;

namespace Application.SavingFeature
{
    [UsedImplicitly]
    internal sealed class Repository : IRepository
    {
        private const int MaxAllowedKeysPerRequest = 10;
        
        private const string FileName = "LocalState.json";
        private static readonly string _filePath = $"{UnityEngine.Application.persistentDataPath}/{FileName}";
        
        private Dictionary<string, string> _state;
        private List<string> _nonCompulsoryDataKeys;

        private readonly Client _client;
        private readonly AesEncryptor _encryptor;

        internal Repository(Client client, AesEncryptor encryptor)
        {
            _client = client;
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
          
            if (_state.TryGetValue(key, out string encryptedData))
            {
                data = JsonConvert.DeserializeObject<T>(encryptedData);
                return true;
            }
            
            data = default;
            return false;
        }

        async UniTask IRepository.SaveState()
        {
            uint saveTime = TryGetSaveTime(_state, out uint result) ? result : default;
            saveTime++;
            
            SaveTimeData saveTimeData = new()
            {
                SaveTime = saveTime
            };

            const string saveTimeKey = nameof(SaveTimeData);
            _state[saveTimeKey] = JsonConvert.SerializeObject(saveTimeData);

            string serializedState = JsonConvert.SerializeObject(_state);
            string encryptedState = _encryptor.Encrypt(serializedState);
            
            await File.WriteAllTextAsync(_filePath, encryptedState);

            string[] keys = _state.Keys.ToArray();
            int keysCount = keys.Length;
            
            if (keysCount <= MaxAllowedKeysPerRequest)
            {
                await _client.TrySetClientData(_state);
                return;
            }
            
            string[] values = _state.Values.ToArray();
            
            int splitsCount = Mathf.CeilToInt((float) keysCount / MaxAllowedKeysPerRequest);
            Dictionary<string, string>[] splittedStatesCollection = new Dictionary<string, string>[splitsCount];

            for (int i = 0; i < splitsCount; i++)
            {
                Dictionary<string, string> partialState = new();

                int remainingKeys = Mathf.Abs(i * MaxAllowedKeysPerRequest - keysCount);
                int iterationsCount = Mathf.Min(remainingKeys, MaxAllowedKeysPerRequest);
                
                for (int j = 0; j < iterationsCount; j++)
                {
                    int index = i * MaxAllowedKeysPerRequest + j;
                    partialState.Add(keys[index], values[index]);
                }
                
                splittedStatesCollection[i] = partialState;
            }

            for (int i = 0; i < splitsCount; i++)
            {
                await _client.TrySetClientData(splittedStatesCollection[i]);
            }
        }

        async UniTask IRepository.LoadState()
        {
            uint localSaveTime = 0;
            uint remoteSaveTime = 0;
            
            if (TryGetLocalState(out Dictionary<string, string> localState))
                localSaveTime = TryGetSaveTime(localState, out uint saveTime) ? saveTime : localSaveTime;
            
            (bool success, Dictionary<string, string> remoteState) = await _client.TryGetClientData();
            
            if (success) 
                remoteSaveTime = TryGetSaveTime(remoteState, out uint saveTime) ? saveTime : remoteSaveTime;

            if (localSaveTime >= remoteSaveTime && localState != null)
            {
                _state = localState;
                return;
            }
            
            _state = remoteState;
        }

#if UNITY_EDITOR
        public async UniTask ResetState()
        {
            File.Delete(_filePath);

            List<string> keys = new()
            {
                "CharacterStatsData",
                "ExperienceData",
                "LevelData",
                "SaveTimeData",
                "MoneyStorageData",
                "PermanentUpgradesLevelData",
                "PriceInfoData",
                "ProgressData",
                "UpgradesData",
                "WeaponStatsData"
            };
            
            await _client.TryRemoveClientData(keys);
        }
#endif        
        
        private bool TryGetLocalState(out Dictionary<string, string> state)
        {
            if (File.Exists(_filePath))
            {
                string encryptedState = File.ReadAllText(_filePath);
                string decryptedData = _encryptor.Decrypt(encryptedState);
                
                state = JsonConvert.DeserializeObject<Dictionary<string, string>>(decryptedData);
                return true;
            }
            
            state = null;
            return false;
        }
        
        private bool TryGetSaveTime(Dictionary<string, string> state, out uint saveTime)
        {
            const string key = nameof(SaveTimeData);
            
            if (state.TryGetValue(key, out string encryptedSaveTimeData))
            {
                SaveTimeData saveTimeData = JsonConvert.DeserializeObject<SaveTimeData>(encryptedSaveTimeData);
                saveTime = saveTimeData.SaveTime;
                
                return true;
            }
            
            saveTime = 0;
            return false;
        }
    }
}