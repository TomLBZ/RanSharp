using System.Numerics;

namespace RanSharp.Maths
{
    public interface IVect<T> where T : struct, INumber<T>
    {
        T this[int i] { get; }
        int Length { get; }
        T Sum();
        T Max();
        T Min();
        T Mag2();
        T Mag();
    }
}
