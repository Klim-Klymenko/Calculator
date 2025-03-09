namespace Common
{
    public interface IPool<T> where T : class
    {
        T Get();
        void Put(T obj);
    }
}