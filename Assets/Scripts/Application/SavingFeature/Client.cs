using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

namespace Application.SavingFeature
{
    [UsedImplicitly]
    public sealed class Client
    {
        private string _sessionToken;
        private string _playFabId;
        private string _entityToken;
        private EntityKey _entity;

        public bool IsLoggedIn { get; private set; }
        
        private readonly PlayFabClientInstanceAPI _clientAPI;
        private readonly string _titleId;
        private readonly string _secretKey;

        internal Client(PlayFabClientInstanceAPI clientAPI, string titleId, string secretKey)
        {
            _clientAPI = clientAPI;
            _titleId = titleId;
            _secretKey = secretKey;
        }

        public async UniTask<bool> LogIn()
        {
            string deviceId = SystemInfo.deviceUniqueIdentifier;
            UniTaskCompletionSource<bool> invoked = new();

            LoginWithCustomIDRequest request = new LoginWithCustomIDRequest
            {
                TitleId = _titleId,
                CustomId = deviceId,
                CreateAccount = true,
                PlayerSecret = _secretKey
            };
            
            _clientAPI.LoginWithCustomID(request, response =>
            {
                _playFabId = response.PlayFabId;
                _sessionToken = response.SessionTicket;
                _entityToken = response.EntityToken.EntityToken;
                _entity = response.EntityToken.Entity;
                invoked.TrySetResult(true);
            }, error =>
            {
                Debug.LogError("Error: " + error.GenerateErrorReport());
                invoked.TrySetResult(false);
            });

            await invoked.Task;
            IsLoggedIn = _clientAPI.IsClientLoggedIn();
            
           return IsLoggedIn;
        }

        public async UniTask<(bool, Dictionary<string, string>)> TryGetClientData()
        {
            if (!IsLoggedIn)
                return (false, null);
            
            Dictionary<string, UserDataRecord> responseData = null;
            UniTaskCompletionSource<bool> success = new();

            GetUserDataRequest request = new();
            
            _clientAPI.GetUserData(request, response =>
            {
                responseData = response.Data;
                success.TrySetResult(true);
            }, error =>
            {
                Debug.LogError("Error: " + error.GenerateErrorReport());
                success.TrySetResult(false);
            });

            UniTask<bool> successTask = success.Task;
            await successTask;
           
            if (!successTask.GetAwaiter().GetResult())
                return (false, null);
            
            Dictionary<string, string> data = new();
                
            foreach (KeyValuePair<string, UserDataRecord> pairResponseData in responseData)
            {
                data.Add(pairResponseData.Key, pairResponseData.Value.Value);
            }

            return (true, data);
        }

        public async UniTask<(bool, Dictionary<string, string>)> TryGetClientData(List<string> dataKeys)
        {
            if (!IsLoggedIn)
                return (false, null);
            
            Dictionary<string, UserDataRecord> responseData = null;
            UniTaskCompletionSource<bool> success = new();
            
            GetUserDataRequest request = new GetUserDataRequest
            {
                Keys = dataKeys
            };
            
            _clientAPI.GetUserData(request, response =>
            {
                responseData = response.Data;
                success.TrySetResult(true);
            }, error =>
            {
                Debug.LogError("Error: " + error.GenerateErrorReport());
                success.TrySetResult(false);
            });

            UniTask<bool> successTask = success.Task;
            await successTask;

            if (!successTask.GetAwaiter().GetResult())
                return (false, null);
            
            Dictionary<string, string> data = new();
                
            foreach (KeyValuePair<string, UserDataRecord> pairResponseData in responseData)
            {
                data.Add(pairResponseData.Key, pairResponseData.Value.Value);
            }

            return (true, data);
        }

        public async UniTask<bool> TrySetClientData(Dictionary<string, string> data)
        {
            if (!IsLoggedIn)
                return false;
            
            UniTaskCompletionSource<bool> success = new();
            
            UpdateUserDataRequest request = new UpdateUserDataRequest
            {
                Data = data
            };
            
            _clientAPI.UpdateUserData(request, response =>
            {
                success.TrySetResult(true);
            }, error =>
            {
                Debug.LogError("Error: " + error.GenerateErrorReport());
                success.TrySetResult(false);
            });
            
            return await success.Task;
        }
        
        public async UniTask<bool> TryRemoveClientData(List<string> keysToRemove)
        {
            if (!IsLoggedIn)
                return false;
            
            UniTaskCompletionSource<bool> success = new();
            
            UpdateUserDataRequest request = new UpdateUserDataRequest
            {
                KeysToRemove = keysToRemove,
            };
            
            _clientAPI.UpdateUserData(request, response =>
            {
                success.TrySetResult(true);
            }, error =>
            {
                Debug.LogError("Error: " + error.GenerateErrorReport());
                success.TrySetResult(false);
            });
            
            return await success.Task;
        }
        
        public async UniTask<bool> TrySetAndRemoveClientData(Dictionary<string, string> data, List<string> keysToRemove)
        {
            if (!IsLoggedIn)
                return false;
            
            UniTaskCompletionSource<bool> success = new();
            
            UpdateUserDataRequest request = new UpdateUserDataRequest
            {
                Data = data,
                KeysToRemove = keysToRemove,
            };
            
            _clientAPI.UpdateUserData(request, response =>
            {
                success.TrySetResult(true);
            }, error =>
            {
                Debug.LogError("Error: " + error.GenerateErrorReport());
                success.TrySetResult(false);
            });
            
            return await success.Task;
        }
    }
}