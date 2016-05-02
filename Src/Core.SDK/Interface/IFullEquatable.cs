

namespace Core.SDK.Interface
{
    /// <summary>
    /// Полное сравнение объектов, в отличие от IEquatable, которое для сравнения может использовать только, напрмиер Id 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IFullEquatable<T>
    {
        bool FullEquals(T other);
    }
}
