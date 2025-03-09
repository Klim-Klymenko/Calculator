using System.Collections.Generic;

namespace Common.CreationFeature
{
    public abstract class BasePool<T> : IPool<T>
    where T : class
    {
        private readonly List<T> _objects = new();
        
        private protected void Reserve(int poolSize)
        {
            for (int i = 0; i < poolSize; i++)
            {
                T obj = Instantiate();
                
                _objects.Add(obj);
                SetActive(obj, false);
            }
        }

        public T Get()
        {
            T obj = _objects.Count > 0 ? _objects[^1] : Instantiate();
            
            SetActive(obj, true);
            _objects.Remove(obj);
            
            return obj;
        }
        
        public void Put(T obj)
        {
            SetActive(obj, false);
            _objects.Add(obj);
            
            OnObjectPut(obj);
        }
        
        private protected abstract T Instantiate();
        private protected abstract void SetActive(T obj, bool active);
        
        private protected virtual void OnObjectPut(T obj) { }
    }
}