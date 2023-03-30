using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using RanSharp.Performance;

namespace RanSharp.Maths
{
    /// <summary>
    /// Fixed-sized matrix3 struct with special reloaded operators and optimized loop speeds.
    /// <br/>Absolute value (Abs(a)) =========== +a
    /// <br/>Normalized value (a.Normalized()): === ~a
    /// <br/>Matrix product (a*b) ============= a * b
    /// </summary>
    /// <typeparam name="D"></typeparam>
    public readonly struct Mat3<D> where D : struct, INumber<D>
    {
        #region Properties
        /// <summary>
        /// The first row of the matrix.
        /// </summary>
        public readonly Vec3<D> R1;
        /// <summary>
        /// The second row of the matrix.
        /// </summary>
        public readonly Vec3<D> R2;
        /// <summary>
        /// The third row of the matrix.
        /// </summary>
        public readonly Vec3<D> R3;
        /// <summary>
        /// The default epsilon value for the Near() method.
        /// </summary>
        public readonly double Epsilon = 1e-9;
        #endregion
        
        #region Constructors
        // Constructors based on a list of values
        /// <summary>
        /// Creates a new matrix with the specified array.
        /// </summary>
        public Mat3(params D[] arr)
        {
            if (null == arr || arr.Length == 0) R1 = R2 = R3 = Vec3<D>.Zero();
            else if (arr.Length == 1) R1 = R2 = R3 = new(arr[0]);
            else if (arr.Length == 3) R1 = R2 = R3 = new(arr);
            else if (arr.Length < 9) throw new ArgumentException("Must have 1, 3 or at least 9 parameters!");
            else { R1 = new(arr[0], arr[1], arr[2]); R2 = new(arr[3], arr[4], arr[5]); R3 = new(arr[6], arr[7], arr[8]); }
        }
        /// <summary>
        /// Creates a new matrix with the specified <see cref="IEnumerable{D}"/>.
        /// </summary>
        public Mat3(IEnumerable<D> arr) : this(arr.ToArray()) { }
        /// <summary>
        /// Creates a new matrix with the specified <see cref="IList{D}"/>.
        /// </summary>
        public Mat3(IList<D> lst) : this(lst.ToArray()) { }
        /// <summary>
        /// Creates a new matrix with the specified <see cref="List{D}"/>.
        /// </summary>
        public Mat3(List<D> lst) : this(lst.ToArray()) { }
        /// <summary>
        /// Creates a new matrix with the specified <see cref="FastList{D}"/>.
        /// </summary>
        public Mat3(FastList<D> lst) : this(lst.ItemsUnsafe) { }
        /// <summary>
        /// Creates a new matrix with the specified <see cref="ArrVector{D}"/>.
        /// </summary>
        public Mat3(ArrVector<D> vec) : this((D[])vec) { }
        // Constructors based on a list of list of values
        /// <summary>
        /// Creates a new matrix with the specified array of arrays.
        /// </summary>
        public Mat3(params D[][] arr)
        {
            if (null == arr || arr.Length == 0) R1 = R2 = R3 = Vec3<D>.Zero();
            else if (arr.Length == 1)
            {
                D[] row = arr[0];
                if (null == row || row.Length == 0) R1 = R2 = R3 = Vec3<D>.Zero();
                else if (row.Length == 1) R1 = R2 = R3 = new(row[0]);
                else if (row.Length == 3) R1 = R2 = R3 = new(row);
                else if (row.Length < 9) throw new ArgumentException("Must have 1, 3 or at least 9 parameters!");
                else { R1 = new(row[0], row[1], row[2]); R2 = new(row[3], row[4], row[5]); R3 = new(row[6], row[7], row[8]); }
            }
            else if (arr.Length < 3) throw new ArgumentException("Must have 1 or at least 3 rows!");
            else { R1 = new(arr[0][0], arr[0][1], arr[0][2]); R2 = new(arr[1][0], arr[1][1], arr[1][2]); R3 = new(arr[2][0], arr[2][1], arr[2][2]); }
        }
        /// <summary>
        /// Creates a new matrix with the specified <see cref="IEnumerable{D[]}"/>.
        /// </summary>
        public Mat3(IEnumerable<D[]> arr) : this(arr.ToArray()) { }
        public Mat3(IList<D[]> arr) : this(arr.ToArray()) { }
        public Mat3(List<D[]> arr) : this(arr.ToArray()) { }
        public Mat3(FastList<D[]> arr) : this(arr.ItemsUnsafe) { }
        // Constructors based on a list of DataType that contains a row of values
        /// <summary>
        /// Creates a new matrix with the specified array of <see cref="Vec3{D}"/>.
        /// </summary>
        public Mat3(params Vec3<D>[] arr)
        {
            if (null == arr || arr.Length == 0) R1 = R2 = R3 = Vec3<D>.Zero();
            else if (arr.Length == 1) R1 = R2 = R3 = arr[0];
            else if (arr.Length < 3) throw new ArgumentException("Must have 1 or at least 3 rows!");
            else { R1 = arr[0]; R2 = arr[1]; R3 = arr[2]; }
        }
        /// <summary>
        /// Creates a new matrix with the specified <see cref="IEnumerable{Vec3{D}}"/>.
        /// </summary>
        public Mat3(IEnumerable<Vec3<D>> arr) : this(arr.ToArray()) { }
        public Mat3(IList<Vec3<D>> arr) : this(arr.ToArray()) { }
        public Mat3(List<Vec3<D>> arr) : this(arr.ToArray()) { }
        public Mat3(FastList<Vec3<D>> arr) : this(arr.ItemsUnsafe) { }
        /// <summary>
        /// Creates a new matrix with the specified params array of <see cref="(D,D,D)){D}"/>.
        /// </summary>
        public Mat3(params (D, D, D)[] arr)
        {
            if (null == arr || arr.Length == 0) R1 = R2 = R3 = Vec3<D>.Zero();
            else if (arr.Length == 1) R1 = R2 = R3 = arr[0];
            else if (arr.Length < 3) throw new ArgumentException("Must have 1 or at least 3 rows!");
            else { R1 = arr[0]; R2 = arr[1]; R3 = arr[2]; }
        }
        /// <summary>
        /// Creates a new matrix with the specified <see cref="IEnumerable{(D,D,D)}"/>.
        /// </summary>
        public Mat3(IEnumerable<(D, D, D)> arr) : this(arr.ToArray()) { }
        public Mat3(IList<(D, D, D)> arr) : this(arr.ToArray()) { }
        public Mat3(List<(D, D, D)> arr) : this(arr.ToArray()) { }
        public Mat3(FastList<(D, D, D)> arr) : this(arr.ItemsUnsafe) { }
        // Constructor based on a Datatype that contains the whole matrix data
        /// <summary>
        /// Creates a new matrix with the specified <see cref="(D, D, D, D, D, D, D, D, D)"/>.
        /// </summary>
        public Mat3((D, D, D, D, D, D, D, D, D) tuple) { R1 = new(tuple.Item1, tuple.Item2, tuple.Item3); R2 = new(tuple.Item4, tuple.Item5, tuple.Item6); R3 = new(tuple.Item7, tuple.Item8, tuple.Item9); }
        /// <summary>
        /// Creates a new matrix with the specified 2d array of <see cref="D"/>.
        /// </summary>
        public Mat3(D[,] arr)
        {
            if (null == arr) R1 = R2 = R3 = Vec3<D>.Zero();
            else if (arr.Rank != 2) throw new ArgumentException("Must be an 2-d matrix!");
            else if (arr.GetLength(0) != 3 || arr.GetLength(1) != 3) throw new ArgumentException("Must be a 3x3 matrix!");
            else { R1 = new(arr[0, 0], arr[0, 1], arr[0, 2]); R2 = new(arr[1, 0], arr[1, 1], arr[1, 2]); R3 = new(arr[2, 0], arr[2, 1], arr[2, 2]); }
        }
        #endregion

        #region Public Override Methods
        /// <summary>
        /// Returns a string that represents the matrix.
        /// </summary>
        public override string ToString() => $"[{R1}; {R2}; {R3}]";
        /// <summary>
        /// Returns a hash code for the matrix.
        /// </summary>
        public override int GetHashCode() => HashCode.Combine(R1.GetHashCode(), R2.GetHashCode(), R3.GetHashCode());
        /// <summary>
        /// Determines whether the specified object is equal to the current matrix.
        /// </summary>
        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            if (obj is null) return false;
            if (obj is not Mat3<D>) return false;
            Mat3<D> mat = (Mat3<D>)obj;
            return R1 == mat.R1 && R2 == mat.R2 && R3 == mat.R3;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// The indexer of the matrix. Returns the value at the specified row and column.
        /// </summary>
        public D this[int row, int col]
        {
            get => row switch
            {
                0 => R1[col],
                1 => R2[col],
                2 => R3[col],
                _ => throw new ArgumentOutOfRangeException(nameof(row)),
            };
        }
        /// <summary>
        /// The indexer of the matrix. Returns the row of values at the specified row number.
        /// </summary>
        public D this[int index]
        {
            get => index switch
            {
                0 => R1[0],
                1 => R1[1],
                2 => R1[2],
                3 => R2[0],
                4 => R2[1],
                5 => R2[2],
                6 => R3[0],
                7 => R3[1],
                8 => R3[2],
                _ => throw new ArgumentOutOfRangeException(nameof(index)),
            };
        }
        /// <summary>
        /// Returns the transpose of the matrix.
        /// </summary>
        public Mat3<D> T() => new(R1[0], R2[0], R3[0], R1[1], R2[1], R3[1], R1[2], R2[2], R3[2]);
        /// <summary>
        /// Returns the cofactor of the matrix at the provided row and column.
        /// </summary>
        public D Cofactor(int r, int c) => (3 * r + c) switch
        {
            0 => R2[1] * R3[2] - R2[2] * R3[1],
            1 => R2[2] * R3[0] - R2[0] * R3[2],
            2 => R2[0] * R3[1] - R2[1] * R3[0],
            3 => R1[2] * R3[1] - R1[1] * R3[2],
            4 => R1[0] * R3[2] - R1[2] * R3[0],
            5 => R1[1] * R3[0] - R1[0] * R3[1],
            6 => R1[1] * R2[2] - R1[2] * R2[1],
            7 => R1[2] * R2[0] - R1[0] * R2[2],
            8 => R1[0] * R2[1] - R1[1] * R2[0],
            _ => throw new ArgumentOutOfRangeException(nameof(r))
        };
        /// <summary>
        /// Returns the determinant of the matrix.
        /// </summary>
        public D Det() => R1[0] * Cofactor(0, 0) + R1[1] * Cofactor(0, 1) + R1[2] * Cofactor(0, 2);
        /// <summary>
        /// Returns the adjoint of the matrix.
        /// </summary>
        public Mat3<D> Adj() => new(Cofactor(0, 0), Cofactor(1, 0), Cofactor(2, 0), 
            Cofactor(0, 1), Cofactor(1, 1), Cofactor(2, 1), Cofactor(0, 2), Cofactor(1, 2), Cofactor(2, 2));
        /// <summary>
        /// Returns the inverse of the matrix.
        /// </summary>
        public Mat3<D> Inv()
        {
            D det = Det();
            if (det == D.Zero) throw new DivideByZeroException(nameof(det));
            return Adj() / det;
        }
        /// <summary>
        /// Returns a new matrix where each value is the absolute value of the original value.
        /// </summary>
        public Mat3<D> Abs() => new(R1.Abs(), R2.Abs(), R3.Abs());
        /// <summary>
        /// Returns the nomalized matrix.
        /// </summary>
        public Mat3<D> Normalized()
        {
            D det = Det();
            if (det == D.Zero) throw new DivideByZeroException(nameof(det));
            return this / det;
        }
        /// <summary>
        /// Returns the matrix as a 2D array.
        /// </summary>
        public D[,] To2dArray() => new D[3, 3] { { R1[0], R1[1], R1[2] }, { R2[0], R2[1], R2[2] }, { R3[0], R3[1], R3[2] } };
        /// <summary>
        /// Returns the matrix as a 1D array.
        /// </summary>
        public D[] To1dArray() => new D[] { R1[0], R1[1], R1[2], R2[0], R2[1], R2[2], R3[0], R3[1], R3[2] };
        /// <summary>
        /// Returns the matrix as an array of rows.
        /// </summary>
        public Vec3<D>[] Rows() => new Vec3<D>[] { R1, R2, R3 };
        /// <summary>
        /// Returns the matrix as an array of columns.
        /// </summary>
        public Vec3<D>[] Columns() => new Vec3<D>[] { new(R1[0], R2[0], R3[0]), new(R1[1], R2[1], R3[1]), new(R1[2], R2[2], R3[2]) };
        /// <summary>
        /// Returns true if the matrix is equal to the other matrix at the precision of the Epsilon value, which is 1e-9 by default.
        /// </summary>
        public bool Near(Mat3<D> mat) => R1.Near(mat.R1, Epsilon) && R2.Near(mat.R2, Epsilon) && R3.Near(mat.R3, Epsilon);
        #endregion

        #region Public Static Methods
        /// <summary>
        /// Returns a matrix with all values set to zero.
        /// </summary>
        public static Mat3<D> Zeroes() => new(D.Zero);
        /// <summary>
        /// Returns a matrix with all values set to one.
        /// </summary>
        public static Mat3<D> Ones() => new(D.One);
        /// <summary>
        /// Returns the 3x3 Identity matrix.
        /// </summary>
        public static Mat3<D> I() => new(D.One, D.Zero, D.Zero, D.Zero, D.One, D.Zero, D.Zero, D.Zero, D.One);
        /// <summary>
        /// Returns a 3x3 Upper Triangular matrix.
        /// </summary>
        public static Mat3<D> UpperTrig() => new(D.One, D.One, D.One, D.Zero, D.One, D.One, D.Zero, D.Zero, D.One);
        /// <summary>
        /// Returns a 3x3 Lower Triangular matrix.
        /// </summary>
        public static Mat3<D> LowerTrig() => new(D.One, D.Zero, D.Zero, D.One, D.One, D.Zero, D.One, D.One, D.One);
        /// <summary>
        /// Returns a 3x3 Rotation matrix around the given <see cref="Axis"/> by the given angle.
        /// </summary>
        public static Mat3<D> Rot(Axis axis, D angle)
        {
            D sin = Calc<D>.Calc1(angle, Math.Sin);
            D cos = Calc<D>.Calc1(angle, Math.Cos);
            return axis switch
            {
                Axis.X => new(D.One, D.Zero, D.Zero, D.Zero, cos, -sin, D.Zero, sin, cos),
                Axis.Y => new(cos, D.Zero, sin, D.Zero, D.One, D.Zero, -sin, D.Zero, cos),
                Axis.Z => new(cos, -sin, D.Zero, sin, cos, D.Zero, D.Zero, D.Zero, D.One),
                _ => throw new InvalidOperationException("Only X, Y and Z are possible!")
            };
        }
        #endregion

        #region Converters
        /// <summary>
        /// Returns a new matrix with the given array of values.
        /// </summary>
        public static implicit operator Mat3<D>(D[] arr) => new(arr);
        /// <summary>
        /// Returns an array of the values in the matrix.
        /// </summary>
        public static implicit operator D[](Mat3<D> mat) => mat.To1dArray();
        /// <summary>
        /// Returns a new matrix with the given 2d array of values.
        /// </summary>
        public static implicit operator Mat3<D>(D[,] arr) => new(arr);
        /// <summary>
        /// Returns a 2d array of the values in the matrix.
        /// </summary>
        public static implicit operator D[,](Mat3<D> mat) => mat.To2dArray();
        /// <summary>
        /// Returns a new matrix with the given array of rows.
        /// </summary>
        public static implicit operator Mat3<D>(Vec3<D>[] arr) => new(arr);
        /// <summary>
        /// Returns an array of the rows in the matrix.
        /// </summary>
        public static implicit operator Vec3<D>[](Mat3<D> mat) => mat.Rows();
        #endregion

        #region Operators
        /// <summary>
        /// Absolute value of a
        /// </summary>
        public static Mat3<D> operator +(Mat3<D> a) => a.Abs();
        /// <summary>
        /// Negation of a
        /// </summary>
        public static Mat3<D> operator -(Mat3<D> a) => new(-a.R1, -a.R2, -a.R3);
        /// <summary>
        /// Elementwise increment of a
        /// </summary>
        public static Mat3<D> operator ++(Mat3<D> a) => new(a.R1 + D.One, a.R2 + D.One, a.R3 + D.One);
        /// <summary>
        /// Elementwise decrement of a
        /// </summary>
        public static Mat3<D> operator --(Mat3<D> a) => new(a.R1 - D.One, a.R2 - D.One, a.R3 - D.One);
        /// <summary>
        /// Normalized a
        /// </summary>
        public static Mat3<D> operator ~(Mat3<D> a) => a.Normalized();
        /// <summary>
        /// Elementwise addition of a and b
        /// </summary>
        public static Mat3<D> operator +(Mat3<D> a, Mat3<D> b) => new(a.R1 + b.R1, a.R2 + b.R2, a.R3 + b.R3);
        /// <summary>
        /// Elementwise addition of a and scalar b
        /// </summary>
        public static Mat3<D> operator +(Mat3<D> a, D b) => new(a.R1 + b, a.R2 + b, a.R3 + b);
        /// <summary>
        /// Elementwise addition of b and scalar a
        /// </summary>
        public static Mat3<D> operator +(D a, Mat3<D> b) => new(a + b.R1, a + b.R2, a + b.R3);
        /// <summary>
        /// Elementwise subtraction of a and b
        /// </summary>
        public static Mat3<D> operator -(Mat3<D> a, Mat3<D> b) => new(a.R1 - b.R1, a.R2 - b.R2, a.R3 - b.R3);
        /// <summary>
        /// Elementwise subtraction of a and scalar b
        /// </summary>
        public static Mat3<D> operator -(Mat3<D> a, D b) => new(a.R1 - b, a.R2 - b, a.R3 - b);
        /// <summary>
        /// Elementwise subtraction of b and scalar a
        /// </summary>
        public static Mat3<D> operator -(D a, Mat3<D> b) => new(a - b.R1, a - b.R2, a - b.R3);
        /// <summary>
        /// Elementwise multiplication of a and scalar b
        /// </summary>
        public static Mat3<D> operator *(Mat3<D> a, D b) => new(a.R1 * b, a.R2 * b, a.R3 * b);
        /// <summary>
        /// Elementwise multiplication of b and scalar a
        /// </summary>
        public static Mat3<D> operator *(D a, Mat3<D> b) => new(a * b.R1, a * b.R2, a * b.R3);
        /// <summary>
        /// Matrix division a / b = a * b^-1
        /// </summary>
        public static Mat3<D> operator /(Mat3<D> a, Mat3<D> b) => a * b.Inv();
        /// <summary>
        /// Elementwise division of a and scalar b
        /// </summary>
        public static Mat3<D> operator /(Mat3<D> a, D b) => new(a.R1 / b, a.R2 / b, a.R3 / b);
        /// <summary>
        /// Elementwise division of b and scalar a
        /// </summary>
        public static Mat3<D> operator /(D a, Mat3<D> b) => new(a / b.R1, a / b.R2, a / b.R3);
        /// <summary>
        /// Matrix multiplication of a and b
        /// </summary>
        public static Mat3<D> operator *(Mat3<D> a, Mat3<D> b)
        {
            Vec3<D>[] c = b.Columns();
            return new(a.R1 * c[0], a.R1 * c[1], a.R1 * c[2], a.R2 * c[0], a.R2 * c[1], a.R2 * c[2], a.R3 * c[0], a.R3 * c[1], a.R3 * c[2]);
        }
        /// <summary>
        /// Multiplication of matrix a and vector b
        /// </summary>
        public static Vec3<D> operator *(Mat3<D> a, Vec3<D> b) => new(a.R1 * b, a.R2 * b, a.R3 * b);
        /// <summary>
        /// Multiplication of vector a and matrix b
        /// </summary>
        public static Vec3<D> operator *(Vec3<D> a, Mat3<D> b)
        {
            Vec3<D>[] c = b.Columns();
            return new(a * c[0], a * c[1], a * c[2]);
        }
        /// <summary>
        /// Elementwise equality of a and b
        /// </summary>
        public static bool operator ==(Mat3<D> a, Mat3<D> b) => a.Equals(b);
        /// <summary>
        /// Elementwise inequality of a and b
        /// </summary>
        public static bool operator !=(Mat3<D> a, Mat3<D> b) => !a.Equals(b);
        /// <summary>
        /// Returns true if the determinant of a is greater than the determinant of b
        /// </summary>
        public static bool operator >(Mat3<D> a, Mat3<D> b) => a.Det() > b.Det();
        /// <summary>
        /// Returns true if the determinant of a is less than the determinant of b
        /// </summary>
        public static bool operator <(Mat3<D> a, Mat3<D> b) => a.Det() < b.Det();
        #endregion
    }
}
