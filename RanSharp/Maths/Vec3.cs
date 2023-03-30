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
        #region  Properties
        /// <summary>X value, only available if <cref see="Length"/> >= 1.</summary>
        public readonly T X;
        /// <summary>Y value, only available if <cref see="Length"/> >= 2.</summary>
        public readonly T Y;
        /// <summary>Z value, only available if <cref see="Length"/> >= 3.</summary>
        public readonly T Z;
        /// <summary>The length of this <see cref="Vec3{T}"/>.</summary>
        public readonly int Length => 3;
        private readonly double Epsilon = 1e-9;
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new <see cref="Vec3{T}"/> from the given unamed <see cref="Tuple{T, T, T}"/> tuple.
        /// </summary>
        public Vec3((T, T, T) tuple) { X = tuple.Item1; Y = tuple.Item2; Z = tuple.Item3; }
        /// <summary>
        /// Creates a new <see cref="Vec3{T}"/> from the given x, y and z values.
        /// </summary>
        public Vec3(T x, T y, T z) { X = x; Y = y; Z = z; }
        /// <summary>
        /// Creates a new <see cref="Vec3{T}"/> from the given <see cref="List{T}"/> of values.
        /// </summary>
        public Vec3(List<T> values) { if (values.Count < 3) throw new ArgumentException("Needs 3 Arguments!"); X = values[0]; Y = values[1]; Z = values[2]; }
        /// <summary>
        /// Creates a new <see cref="Vec3{T}"/> from the given <see cref="IEnumerable{T}"/> of values.
        /// </summary>
        public Vec3(IEnumerable<T> values)
        {
            if (null == values) throw new ArgumentNullException(nameof(values));
            T[] arr = values.ToArray();
            if (arr.Length < 3) throw new ArgumentException("Needs 3 Arguments!");
            X = arr[0]; Y = arr[1]; Z = arr[2];
        }
        /// <summary>
        /// Creates a new <see cref="Vec3{T}"/> where all values are set to 0.
        /// </summary>
        public Vec3() { X = Y = Z = T.Zero; }
        /// <summary>
        /// Creates a new <see cref="Vec3{T}"/> from the given params <typeparamref name="T"/>[] of values.
        /// </summary>
        public Vec3(params T[] values) { if (values.Length < 3) throw new ArgumentException("Needs 3 Arguments!"); X = values[0]; Y = values[1]; Z = values[2]; }
        /// <summary>
        /// Creates a new <see cref="Vec3{T}"/> where all values are set to the given <typeparamref name="T"/> value.
        /// </summary>
        public Vec3(T initValue) { X = Y = Z = initValue; }
        /// <summary>
        /// Creates a new <see cref="Vec3{T}"/> from the given <see cref="Vec3{T}"/>.
        /// </summary>
        public Vec3(Vec3<T> other) { X=other.X; Y=other.Y; Z = other.Z; }
        /// <summary>
        /// Creates a new <see cref="Vec3{T}"/> from the given <see cref="ReadOnlySpan{T}"/> of values.
        /// </summary>
        public Vec3(ReadOnlySpan<T> values) { if (values.Length < 3) throw new ArgumentException("Needs 3 Arguments!"); X = values[0]; Y = values[1]; Z = values[2]; }
        /// <summary>
        /// Creates a new <see cref="Vec3{T}"/> from the given <see cref="Span{T}"/> of values.
        /// </summary>
        public Vec3(Span<T> values) { if (values.Length < 3) throw new ArgumentException("Needs 3 Arguments!"); X = values[0]; Y = values[1]; Z = values[2]; }
        #endregion

        #region Converters
        /// <summary>Implicitly converts an unamed <see cref="Tuple{T, T, T}"/> to a <see cref="Vec3{T}"/>.</summary>
        public static implicit operator Vec3<T>((T, T, T) tuple) => new(tuple);
        /// <summary>Implicitly converts a <typeparamref name="T"/>[] to a <see cref="Vec3{T}"/>.</summary>
        public static implicit operator Vec3<T>(T[] values) => new(values);
        /// <summary>Implicitly converts a <see cref="List{T}"/> to a <see cref="Vec3{T}"/>.</summary>
        public static implicit operator Vec3<T>(List<T> values) => new(values);
        /// <summary>Implicitly converts a <see cref="Span{T}"/> to a <see cref="Vec3{T}"/>.</summary>
        public static implicit operator Vec3<T>(Span<T> values) => new(values);
        /// <summary>Implicitly converts a <see cref="ReadOnlySpan{T}"/> to a <see cref="Vec3{T}"/>.</summary>
        public static implicit operator Vec3<T>(ReadOnlySpan<T> values) => new(values);
        /// <summary>Implicitly converts a <see cref="Vec3{T}"/> to an unamed <see cref="Tuple{T, T, T}"/>.</summary>
        public static implicit operator (T, T, T)(Vec3<T> vec) => (vec.X, vec.Y, vec.Z);
        /// <summary>Implicitly converts a <see cref="Vec3{T}"/> to a <typeparamref name="T"/>[].</summary>
        public static implicit operator T[](Vec3<T> vec) => new[] { vec.X, vec.Y, vec.Z };
        /// <summary>Implicitly converts a <see cref="Vec3{T}"/> to a <see cref="List{T}"/>.</summary>
        public static implicit operator List<T>(Vec3<T> vec) => new() { vec.X, vec.Y, vec.Z };
        /// <summary>Implicitly converts a <see cref="Vec3{T}"/> to a <see cref="Span{T}"/>.</summary>
        public static implicit operator Span<T>(Vec3<T> vec) => new(new[] { vec.X, vec.Y, vec.Z });
        /// <summary>Implicitly converts a <see cref="Vec3{T}"/> to a <see cref="ReadOnlySpan{T}"/>.</summary>
        public static implicit operator ReadOnlySpan<T>(Vec3<T> vec) => new(new[] { vec.X, vec.Y, vec.Z });
        #endregion
        
        #region Public Methods
        /// <summary>
        /// Returns the maximum value of the <see cref="Vec3{T}"/>.
        /// </summary>
        public T Max() => X > Y ? (X > Z ? X : Z) : (Y > Z ? Y : Z);
        /// <summary>
        /// Returns the minimum value of the <see cref="Vec3{T}"/>.
        /// </summary>
        public T Min() => X < Y ? (X < Z ? X : Z) : (Y < Z ? Y : Z);
        /// <summary>
        /// Returns the sum of all values of the <see cref="Vec3{T}"/>.
        /// </summary>
        public T Sum() => X + Y + Z;
        /// <summary>
        /// Returns the magnitude squared of the <see cref="Vec3{T}"/>.
        /// </summary>
        public T Mag2() => X * X + Y * Y + Z * Z;
        /// <summary>
        /// Returns the magnitude of the <see cref="Vec3{T}"/>.
        /// </summary>
        public T Mag() => Calc<T>.Calc1(Mag2(), Math.Sqrt);
        /// <summary>
        /// The readonly indexer. Gets the value at the given index.
        /// </summary>
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
        /// <summary>
        /// Returns the normalized <see cref="Vec3{T}"/>.
        /// </summary>
        public Vec3<T> Normalized() => this / Mag();
        /// <summary>
        /// Returns a new <see cref="Vec3{T}"/> with the absolute values of the current <see cref="Vec3{T}"/>.
        /// </summary>
        public Vec3<T> Abs() => new(X < T.Zero ? -X : X, Y < T.Zero ? -Y : Y, Z < T.Zero ? -Z : Z);
        /// <summary>
        /// Returns true if the <see cref="Vec3{T}"/> is near the given <see cref="Vec3{T}"/> to the precision of <see cref="Epsilon"/>.
        /// </summary>
        public bool Near(Vec3<T> vec, double epsilon = 1e-9) 
        {
            double e = epsilon == Epsilon ? Epsilon : epsilon;
            return Calc<T>.Near(vec.X, X, e) && Calc<T>.Near(vec.Y, Y, e) && Calc<T>.Near(vec.Z, Z, e);
        }   
        /// <summary>
        /// Returns true if all values of the <see cref="Vec3{T}"/> are true for the given <see cref="Func{T, TResult}"/> where TResult is <see cref="bool"/>.
        /// </summary>
        public bool TrueForAll(Func<T, bool> func) => func(X) && func(Y) && func(Z);
        /// <summary>
        /// Returns true if any value of the <see cref="Vec3{T}"/> is true for the given <see cref="Func{T, TResult}"/> where TResult is <see cref="bool"/>.
        /// </summary>
        public bool TrueForAny(Func<T, bool> func) => func(X) || func(Y) || func(Z);
        #endregion
        
        #region Override Methods
        /// <summary>
        /// Returns a string representation of the <see cref="Vec3{T}"/>.
        /// </summary>
        public override string ToString() => $"({X}, {Y}, {Z})";
        /// <summary>
        /// Returns true if the <see cref="Vec3{T}"/> is equal to the given <see cref="object"/>.
        /// </summary>
        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            if (obj is null) return false;
            if (obj is not Vec3<T>) return false;
            Vec3<T> vec = (Vec3<T>)obj;
            return X == vec.X && Y == vec.Y && Z == vec.Z;
        }
        /// <summary>
        /// Returns the hash code of the <see cref="Vec3{T}"/>.
        /// </summary>
        public override int GetHashCode() => HashCode.Combine(X.GetHashCode(), Y.GetHashCode(), Z.GetHashCode());
        #endregion
        
        #region Static Methods
        /// <summary>
        /// Returns a new <see cref="Vec3{T}"/> with all values set to zero.
        /// </summary>
        public static Vec3<T> Zero() => new();
        /// <summary>
        /// Returns a new <see cref="Vec3{T}"/> with all values set to one.
        /// </summary>
        public static Vec3<T> One() => new(T.One);
        /// <summary>
        /// Returns a new unit <see cref="Vec3{T}"/> at a given axis.
        /// </summary>
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
        /// <summary>
        /// Returns a new unit <see cref="Vec3{T}"/> at a given <see cref="Axis"/>.
        /// </summary>
        public static Vec3<T> Unit(Axis axis) => Unit((int)axis);
        #endregion

        #region Operators
        /// <summary>
        /// Absolute value of a
        /// </summary>
        public static Vec3<T> operator +(Vec3<T> a) => a.Abs();
        /// <summary>
        /// Negation of a
        /// </summary>
        public static Vec3<T> operator -(Vec3<T> a) => new(-a.X, -a.Y, -a.Z);
        /// <summary>
        /// Elementwise increment of a
        /// </summary>
        public static Vec3<T> operator ++(Vec3<T> a) => new(a.X + T.One, a.Y + T.One, a.Z + T.One);
        /// <summary>
        /// Elementwise decrement of a
        /// </summary>
        public static Vec3<T> operator --(Vec3<T> a) => new(a.X - T.One, a.Y - T.One, a.Z - T.One);
        /// <summary>
        /// Normalized value of a
        /// </summary>
        public static Vec3<T> operator ~(Vec3<T> a) => a.Normalized();
        /// <summary>
        /// Elementwise addition of a and b
        /// </summary>
        public static Vec3<T> operator +(Vec3<T> a, Vec3<T> b) => new(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        /// <summary>
        /// Elementwise addition of a and scalar b
        /// </summary>
        public static Vec3<T> operator +(Vec3<T> a, T b) => new(a.X + b, a.Y + b, a.Z + b);
        /// <summary>
        /// Elementwise addition of b and scalar a
        /// </summary>
        public static Vec3<T> operator +(T a, Vec3<T> b) => new(a + b.X, a + b.Y, a + b.Z);
        /// <summary>
        /// Elementwise subtraction of a and b
        /// </summary>
        public static Vec3<T> operator -(Vec3<T> a, Vec3<T> b) => new(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        /// <summary>
        /// Elementwise subtraction of a and scalar b
        /// </summary>
        public static Vec3<T> operator -(Vec3<T> a, T b) => new(a.X - b, a.Y - b, a.Z - b);
        /// <summary>
        /// Elementwise subtraction of b and scalar a
        /// </summary>
        public static Vec3<T> operator -(T a, Vec3<T> b) => new(a - b.X, a - b.Y, a - b.Z);
        /// <summary>
        /// Elementwise multiplication of a and scalar b
        /// </summary>
        public static Vec3<T> operator *(Vec3<T> a, T b) => new(a.X * b, a.Y * b, a.Z * b);
        /// <summary>
        /// Elementwise multiplication of b and scalar a
        /// </summary>
        public static Vec3<T> operator *(T a, Vec3<T> b) => new(a * b.X, a * b.Y, a * b.Z);
        /// <summary>
        /// Elementwise division of a and scalar b
        /// </summary>
        public static Vec3<T> operator /(Vec3<T> a, T b) => new(a.X / b, a.Y / b, a.Z / b);
        /// <summary>
        /// Elementwise division of b and scalar a
        /// </summary>
        public static Vec3<T> operator /(T a, Vec3<T> b) => new(a / b.X, a / b.Y, a / b.Z);
        /// <summary>
        /// Elementwise a to the power of scalar b
        /// </summary>
        public static Vec3<T> operator ^(Vec3<T> a, T b) => new(Calc<T>.Calc2(a.X, b, Math.Pow), Calc<T>.Calc2(a.Y, b, Math.Pow), Calc<T>.Calc2(a.Z, b, Math.Pow));
        /// <summary>
        /// Elementwise b to the power of scalar a
        /// </summary>
        public static Vec3<T> operator ^(T a, Vec3<T> b) => new(Calc<T>.Calc2(a, b.X, Math.Pow), Calc<T>.Calc2(a, b.Y, Math.Pow), Calc<T>.Calc2(a, b.Z, Math.Pow));
        /// <summary>
        /// Dot product of a and b
        /// </summary>
        public static T operator *(Vec3<T> a, Vec3<T> b) => a.X * b.X + a.Y * b.Y + a.Z * b.Z;
        /// <summary>
        /// Angle between a and b
        /// </summary>
        public static T operator /(Vec3<T> a, Vec3<T> b) => Calc<T>.Calc1(a * b / (a.Mag() * b.Mag()), Math.Acos);
        /// <summary>
        /// Cross product of a and b
        /// </summary>
        public static Vec3<T> operator %(Vec3<T> a, Vec3<T> b) => new(a[1] * b[2] - a[2] * b[1], a[2] * b[0] - a[0] * b[2], a[0] * b[1] - a[1] * b[0]);
        /// <summary>
        /// Elementwise equality of a and b
        /// </summary>
        public static bool operator ==(Vec3<T> a, Vec3<T> b) => a.Equals(b);
        /// <summary>
        /// Elementwise inequality of a and b
        /// </summary>
        public static bool operator !=(Vec3<T> a, Vec3<T> b) => !a.Equals(b);
        /// <summary>
        /// Returns true if the magnitude of a is greater than the magnitude of b
        /// </summary>
        public static bool operator >(Vec3<T> a, Vec3<T> b) => a.Mag2() > b.Mag2();
        /// <summary>
        /// Returns true if the magnitude of a is less than the magnitude of b
        /// </summary>
        public static bool operator <(Vec3<T> a, Vec3<T> b) => a.Mag2() < b.Mag2();
        #endregion
    }
}