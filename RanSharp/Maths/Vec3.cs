using RanSharp.Performance;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace RanSharp.Maths
{
    /// <summary>
    /// Fixed-size vector3 struct with special reloaded operators and optimized loop speeds.
    /// <br/>Absolute value (Abs(a)) =========== +a
    /// <br/>Normalized value (a.Normalized()): === ~a
    /// <br/>Dot product (a·b) =============== a * b
    /// <br/>Angle between ((a·b)/(|a|*|b|)) ======== a / b
    /// <br/>Cross product (a X b) ============ a % b
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public readonly struct Vec3<T> : IVect<T> where T : struct, INumber<T>
    {
        public readonly T X;
        public readonly T Y;
        public readonly T Z;
        public readonly int Length => 3;
        public Vec3((T, T, T) tuple) { X = tuple.Item1; Y = tuple.Item2; Z = tuple.Item3; }
        public Vec3(T x, T y, T z) { X = x; Y = y; Z = z; }
        public Vec3(List<T> values) { if (values.Count < 3) throw new ArgumentException("Needs 3 Arguments!"); X = values[0]; Y = values[1]; Z = values[2]; }
        public Vec3(IEnumerable<T> values)
        {
            if (null == values) throw new ArgumentNullException(nameof(values));
            T[] arr = values.ToArray();
            if (arr.Length < 3) throw new ArgumentException("Needs 3 Arguments!");
            X = arr[0]; Y = arr[1]; Z = arr[2];
        }
        public Vec3() { X = Y = Z = T.Zero; }
        public Vec3(params T[] values) { if (values.Length < 3) throw new ArgumentException("Needs 3 Arguments!"); X = values[0]; Y = values[1]; Z = values[2]; }
        public Vec3(T initValue) { X = Y = Z = initValue; }
        public Vec3(Vec3<T> other) { X=other.X; Y=other.Y; Z = other.Z; }
        public Vec3(ReadOnlySpan<T> values) { if (values.Length < 3) throw new ArgumentException("Needs 3 Arguments!"); X = values[0]; Y = values[1]; Z = values[2]; }
        public Vec3(Span<T> values) { if (values.Length < 3) throw new ArgumentException("Needs 3 Arguments!"); X = values[0]; Y = values[1]; Z = values[2]; }
        public static implicit operator Vec3<T>((T, T, T) tuple) => new(tuple);
        public static implicit operator Vec3<T>(T[] values) => new(values);
        public static implicit operator Vec3<T>(List<T> values) => new(values);
        public static implicit operator Vec3<T>(Span<T> values) => new(values);
        public static implicit operator Vec3<T>(ReadOnlySpan<T> values) => new(values);
        public static implicit operator (T, T, T)(Vec3<T> vec) => (vec.X, vec.Y, vec.Z);
        public static implicit operator T[](Vec3<T> vec) => new[] { vec.X, vec.Y, vec.Z };
        public static implicit operator List<T>(Vec3<T> vec) => new() { vec.X, vec.Y, vec.Z };
        public static implicit operator Span<T>(Vec3<T> vec) => new(new[] { vec.X, vec.Y, vec.Z });
        public static implicit operator ReadOnlySpan<T>(Vec3<T> vec) => new(new[] { vec.X, vec.Y, vec.Z });
        public T Max() => X > Y ? (X > Z ? X : Z) : (Y > Z ? Y : Z);
        public T Min() => X < Y ? (X < Z ? X : Z) : (Y < Z ? Y : Z);
        public T Sum() => X + Y + Z;
        public T Mag2() => X * X + Y * Y + Z * Z;
        public T Mag() => Calc<T>.Calc1(Mag2(), Math.Sqrt);
        public T this[int index]
        {
            get => index switch
            {
                0 => X,
                1 => Y,
                2 => Z,
                _ => throw new ArgumentOutOfRangeException(nameof(index))
            };
        }
        public Vec3<T> Normalized() => this / Mag();
        public Vec3<T> Abs() => new(X < T.Zero ? -X : X, Y < T.Zero ? -Y : Y, Z < T.Zero ? -Z : Z);
        public override string ToString() => $"({X}, {Y}, {Z})";
        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            if (obj is null) return false;
            if (obj is not Vec3<T>) return false;
            Vec3<T> vec = (Vec3<T>)obj;
            return X == vec.X && Y == vec.Y && Z == vec.Z;
        }
        public override int GetHashCode() => HashCode.Combine(X.GetHashCode(), Y.GetHashCode(), Z.GetHashCode());
        public bool Near(Vec3<T> vec, double epsilon = 1e-9) =>
            Calc<T>.Near(vec.X, X, epsilon) && Calc<T>.Near(vec.Y, Y, epsilon) && Calc<T>.Near(vec.Z, Z, epsilon);
        public bool TrueForAll(Func<T, bool> func) => func(X) && func(Y) && func(Z);
        public bool TrueForAny(Func<T, bool> func) => func(X) || func(Y) || func(Z);
        public static Vec3<T> Zero() => new();
        public static Vec3<T> One() => new(T.One);
        public static Vec3<T> Unit(int axis)
        {
            return axis switch
            {
                0 => new(T.One, T.Zero, T.Zero),
                1 => new(T.Zero, T.One, T.Zero),
                2 => new(T.Zero, T.Zero, T.One),
                _ => throw new ArgumentOutOfRangeException(nameof(axis))
            };
        }
        public static Vec3<T> Unit(Axis axis) => Unit((int)axis);

        #region Operators
        /// <summary>
        /// Absolute value of a
        /// </summary>
        /// <param name="a">Vec3 a</param>
        /// <returns>Vec3 abs(a)</returns>
        public static Vec3<T> operator +(Vec3<T> a) => a.Abs();
        public static Vec3<T> operator -(Vec3<T> a) => new(-a.X, -a.Y, -a.Z);
        public static Vec3<T> operator ++(Vec3<T> a) => new(a.X + T.One, a.Y + T.One, a.Z + T.One);
        public static Vec3<T> operator --(Vec3<T> a) => new(a.X - T.One, a.Y - T.One, a.Z - T.One);
        /// <summary>
        /// Normalized value of a
        /// </summary>
        /// <param name="a">Vec3 a</param>
        /// <returns>Vec3 a.Normalized()</returns>
        public static Vec3<T> operator ~(Vec3<T> a) => a.Normalized();
        public static Vec3<T> operator +(Vec3<T> a, Vec3<T> b) => new(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        public static Vec3<T> operator +(Vec3<T> a, T b) => new(a.X + b, a.Y + b, a.Z + b);
        public static Vec3<T> operator +(T a, Vec3<T> b) => new(a + b.X, a + b.Y, a + b.Z);
        public static Vec3<T> operator -(Vec3<T> a, Vec3<T> b) => new(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        public static Vec3<T> operator -(Vec3<T> a, T b) => new(a.X - b, a.Y - b, a.Z - b);
        public static Vec3<T> operator -(T a, Vec3<T> b) => new(a - b.X, a - b.Y, a - b.Z);
        public static Vec3<T> operator *(Vec3<T> a, T b) => new(a.X * b, a.Y * b, a.Z * b);
        public static Vec3<T> operator *(T a, Vec3<T> b) => new(a * b.X, a * b.Y, a * b.Z);
        public static Vec3<T> operator /(Vec3<T> a, T b) => new(a.X / b, a.Y / b, a.Z / b);
        public static Vec3<T> operator /(T a, Vec3<T> b) => new(a / b.X, a / b.Y, a / b.Z);
        public static Vec3<T> operator ^(Vec3<T> a, T b) => new(Calc<T>.Calc2(a.X, b, Math.Pow), Calc<T>.Calc2(a.Y, b, Math.Pow), Calc<T>.Calc2(a.Z, b, Math.Pow));
        public static Vec3<T> operator ^(T a, Vec3<T> b) => new(Calc<T>.Calc2(a, b.X, Math.Pow), Calc<T>.Calc2(a, b.Y, Math.Pow), Calc<T>.Calc2(a, b.Z, Math.Pow));
        /// <summary>
        /// Dot product of a and b
        /// </summary>
        /// <param name="a">Vec3 a</param>
        /// <param name="b">Vec3 b</param>
        /// <returns>The dot product a·b of type T</returns>
        public static T operator *(Vec3<T> a, Vec3<T> b) => a.X * b.X + a.Y * b.Y + a.Z * b.Z;
        /// <summary>
        /// Angle between a and b
        /// </summary>
        /// <param name="a">Vec3 a</param>
        /// <param name="b">Vec3 b</param>
        /// <returns>(a·b)/(|a|*|b|)</returns>
        public static T operator /(Vec3<T> a, Vec3<T> b) => Calc<T>.Calc1(a * b / (a.Mag() * b.Mag()), Math.Acos);
        /// <summary>
        /// Cross product of a and b
        /// </summary>
        /// <param name="a">Vec3 a</param>
        /// <param name="b">Vec3 b</param>
        /// <returns>The cross product a X b of type T</returns>
        public static Vec3<T> operator %(Vec3<T> a, Vec3<T> b) => new(a[1] * b[2] - a[2] * b[1], a[2] * b[0] - a[0] * b[2], a[0] * b[1] - a[1] * b[0]);
        public static bool operator ==(Vec3<T> a, Vec3<T> b) => a.Equals(b);
        public static bool operator !=(Vec3<T> a, Vec3<T> b) => !a.Equals(b);
        public static bool operator >(Vec3<T> a, Vec3<T> b) => a.Mag2() > b.Mag2();
        public static bool operator <(Vec3<T> a, Vec3<T> b) => a.Mag2() < b.Mag2();
        #endregion
    }
}