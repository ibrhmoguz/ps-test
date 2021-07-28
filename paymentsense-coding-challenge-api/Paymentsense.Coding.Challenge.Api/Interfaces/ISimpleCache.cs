namespace Paymentsense.Coding.Challenge.Api.Interfaces
{
    public interface ISimpleCache<T>
    {
        T Get(object key);
        void Set(object key, T entry);
    }
}
