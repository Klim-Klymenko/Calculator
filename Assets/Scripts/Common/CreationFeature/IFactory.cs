namespace Common.CreationFeature
{
    public interface IFactory<out T> where T : class
    {
        T Create();
    }
    
    public interface IFactory<out TR, in TArg> where TR : class
    {
        TR Create(TArg arg);
    }
}