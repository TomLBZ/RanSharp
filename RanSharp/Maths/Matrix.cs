using System.Diagnostics.CodeAnalysis;
using RanSharp.Performance;
using System.Numerics;

namespace RanSharp.Maths
{
    /// <summary>
    /// Represents a matrix of any dimension.
    /// <br/>Special Usages:<br/>
    /// Generic Matrix Multiplication: <code>m1 * m2</code>
    /// Note that the order of the operands matters and the dimensions of the vector and the matrix must allow multiplication.
    /// </summary>
    /// <typeparam name="D"></typeparam>
    public readonly struct Matrix<D> where D : struct, INumber<D>
    {
        #region Constants
        private readonly D minValue = D.CreateSaturating(double.MinValue);
        private readonly D maxValue = D.CreateSaturating(double.MaxValue);
        #endregion

        #region Properties
        /// <summary>
        /// The row count of the Matrix.
        /// </summary>
        public readonly int RowCount;
        /// <summary>
        /// The column count of the Matrix.
        /// </summary>
        public readonly int ColumnCount;
        /// <summary>
        /// The total number of elements in the Matrix.
        /// </summary>
        public readonly int Count => RowCount * ColumnCount;
        /// <summary>
        /// The default epsilon value for Near() method.
        /// </summary>
        public readonly double Epsilon = 1e-9;
        #endregion

        #region Private States
        private readonly FastList<D[]> data;
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new Matrix with the row and column count specified.
        /// </summary>
        public Matrix(int rowCount, int columnCount)
        {
            RowCount = rowCount;
            ColumnCount = columnCount;
            data = new(rowCount, i => new D[columnCount]);
        }
        /// <summary>
        /// Creates a new Matrix with the row and column count specified and initializes all elements to the specified value.
        /// </summary>
        public Matrix(int rowCount, int columnCount, D initValue)
        {
            RowCount = rowCount;
            ColumnCount = columnCount;
            data = new(rowCount, i =>
            {
                D[] row = new D[columnCount];
                Loop<D>.MapInPlace(row, x => initValue);
                return row;
            });
        }
        /// <summary>
        /// Creates a new Matrix with the row and column count specified using data from the specified array.
        /// </summary>
        public Matrix(int rowCount, int columnCount, D[] arr)
        {
            if (null == arr || arr.Length == 0) throw new ArgumentNullException(nameof(arr));
            if (rowCount * columnCount != arr.Length) throw new IndexOutOfRangeException(nameof(rowCount));
            RowCount = rowCount;
            ColumnCount = columnCount;
            data = new();
            ReadOnlySpan<D> span = arr.AsSpan();
            for (int i = 0; i < span.Length; i += columnCount)
            {
                D[] row = new D[columnCount];
                span.Slice(i, columnCount).CopyTo(row);
                data.Add(row);
            }
        }
        /// <summary>
        /// Creates a new Matrix using data from the specified 2D array.
        /// </summary>
        public Matrix(D[,] values)
        {
            if (values is null) throw new NullReferenceException();
            if (values.Length == 0) throw new IndexOutOfRangeException();
            RowCount = values.GetLength(0);
            ColumnCount = values.GetLength(1);
            data = new();
            for (int i = 0; i < RowCount; i++)
            {
                D[] row = new D[ColumnCount];
                Span<D> span = row.AsSpan();
                for (int j = 0; j < span.Length; j++)
                    span[j] = values[i, j];
                data.Add(row);
            }
        }
        /// <summary>
        /// Creates a new Matrix using data from the specified jagged array.
        /// </summary>
        public Matrix(params D[][] rows)
        {
            if (rows is null) throw new NullReferenceException(); // D[][] must be non-null
            if (rows.Length == 0) throw new IndexOutOfRangeException(); // must contain at least 1 row
            if (null == rows[0]) throw new NullReferenceException(); // first row must be non-null
            RowCount = rows.Length;
            ColumnCount = rows[0].Length;
            data = new();
            for (int i = 0; i < RowCount; i++)
            {
                if (null == rows[i]) throw new NullReferenceException(); // if any row is null, fail immediately
                if (rows[i].Length != ColumnCount) throw new ArgumentException("Row lengths must be all equal!");
                D[] newrow = new D[ColumnCount];
                Array.Copy(rows[i], newrow, RowCount);
                data.Add(newrow);
            }
        }
        /// <summary>
        /// Creates a new Matrix using data from the given array of <see cref="IEnumerable{D}"/> representing rows.
        /// </summary>
        public Matrix(params IEnumerable<D>[] rows)
        {
            if (rows is null) throw new NullReferenceException();
            if (rows.Length == 0) throw new IndexOutOfRangeException();
            RowCount = rows.Length;
            ColumnCount = rows[0].Count();
            data = new();
            for (int i = 0; i < RowCount; i++)
            {
                if (rows[i].Count() != ColumnCount) throw new ArgumentOutOfRangeException(nameof(rows));
                data.Add(rows[i].ToArray());
            }
        }
        /// <summary>
        /// Creates a new Matrix using data from the given <see cref="IEnumerable{T}"/> of <typeparamref name="D"/>[] representing rows.
        /// </summary>
        public Matrix(IEnumerable<D[]> rows)
        {
            if (rows is null) throw new NullReferenceException();
            if (!rows.Any()) throw new IndexOutOfRangeException();
            RowCount = rows.Count();
            ColumnCount = -1;
            data = new();
            foreach (D[] row in rows)
            {
                if (null == row) throw new NullReferenceException();
                if (row.Length == 0) throw new IndexOutOfRangeException();
                if (ColumnCount == -1) ColumnCount = row.Length;
                else if (ColumnCount != row.Length) throw new IndexOutOfRangeException();
                D[] newrow = new D[ColumnCount];
                Array.Copy(row, newrow, RowCount);
                data.Add(newrow);
            }
        }
        /// <summary>
        /// Creates a new Matrix using data from the given array of <see cref="ArrVector{D}"/> representing rows.
        /// </summary>
        public Matrix(params ArrVector<D>[] rows)
        {
            if (rows is null) throw new NullReferenceException();
            if (rows.Length == 0) throw new IndexOutOfRangeException();
            RowCount = rows.Length;
            ColumnCount = rows[0].Length;
            data = new();
            for (int i = 0; i < RowCount; i++)
            {
                if (rows[i].Length != ColumnCount) throw new ArgumentOutOfRangeException(nameof(rows));
                data.Add(rows[i]); // implicit conversion returns a copy
            }
        }
        /// <summary>
        /// Creates a new Matrix using data from the given <see cref="Matrix{D}"/>.
        /// </summary>
        public Matrix(Matrix<D> mat)
        {
            if (mat.Count == 0) throw new IndexOutOfRangeException();
            RowCount = mat.RowCount;
            ColumnCount = mat.ColumnCount;
            FastList<ArrVector<D>> rows = mat.GetRows();
            data = new(RowCount, i => rows[i]);
        }
        /// <summary>
        /// Creates a new Matrix using the given row and column counts and a generator function for each element.
        /// </summary>
        public Matrix(int rowCount, int columnCount, Func<int, int, D> generator)
        {
            if (rowCount <= 0 || columnCount <= 0) throw new ArgumentException("Row and Column counts must be > 0!");
            RowCount = rowCount;
            ColumnCount = columnCount;
            data = new(rowCount);
            for (int i = 0; i < RowCount; i++)
            {
                D[] row = new D[columnCount];
                Span<D> span = row.AsSpan();
                for (int j = 0; j < ColumnCount; j++)
                    span[j] = generator(i, j);
                data.Add(row);
            }
        }
        /// <summary>
        /// Creates a new Matrix using the given row and column counts and a generator function for each row.
        /// </summary>
        public Matrix(int rowCount, int columnCount, Func<int, D> rowGenerator)
        {
            if (rowCount <= 0 || columnCount <= 0) throw new ArgumentException("Row and Column counts must be > 0!");
            RowCount = rowCount;
            ColumnCount = columnCount;
            data = new(rowCount);
            for (int i = 0; i < RowCount; i++)
            {
                D[] row = new D[columnCount];
                Span<D> span = row.AsSpan();
                for (int j = 0; j < ColumnCount; j++)
                    span[j] = rowGenerator(j);
                data.Add(row);
            }
        }
        #endregion

        #region Private Methods
        #endregion

        #region Public Methods
        /// <summary>
        /// Returns the rows of this matrix as a <see cref="FastList{T}"/> of <see cref="ArrVector{D}"/>.
        /// </summary>
        public FastList<ArrVector<D>> GetRows()
        {
            FastList<ArrVector<D>> rows = new(RowCount);
            data.Apply(i => rows.Add(i.ToArray())); // copy before adding
            return rows;
        }
        /// <summary>
        /// Returns the columns of this matrix as a <see cref="FastList{T}"/> of <see cref="ArrVector{D}"/>.
        /// </summary>
        public FastList<ArrVector<D>> GetColumns()
        {
            FastList<ArrVector<D>> cols = new();
            for (int i = 0; i < ColumnCount; i++)
            {
                D[] col = new D[RowCount];
                Span<D> span = col.AsSpan();
                for (int j = 0; j < span.Length; j++)
                    span[j] = data[j][i];
                cols.Add(col);
            }
            return cols;
        }
        /// <summary>
        /// The indexer for this matrix. Gets or sets the element at the given row and column.
        /// </summary>
        public D this[int i, int j]
        {
            get { return data[i][j]; }
            set { data[i][j] = value; }
        }
        /// <summary>
        /// The indexer for this matrix. Gets or sets the row at the given index.
        /// </summary>
        public D[] this[int row]
        {
            get { return data[row]; }
            set { data[row] = value; }
        }
        /// <summary>
        /// Composites this <see cref="Matrix{D}"/> with the given <see cref="Matrix{D}"/> using the given function. Returns a new <see cref="Matrix{D}"/> containing the result.
        /// </summary>
        public Matrix<D> Composite(Matrix<D> b, Func<D, D, D> f)
        {
            if (RowCount != b.RowCount || ColumnCount != b.ColumnCount) throw new ArgumentException("Two matrices must have the same size!");
            Matrix<D> mat = new(this);
            mat.CompositeInPlace(b, f);
            return mat;
        }
        /// <summary>
        /// Composites this <see cref="Matrix{D}"/> with the given <see cref="Matrix{D}"/> using the given function. Modifies this <see cref="Matrix{D}"/> to contain the result.
        /// </summary>
        public void CompositeInPlace(Matrix<D> b, Func<D, D, D> f)
        {
            if (RowCount != b.RowCount || ColumnCount != b.ColumnCount) throw new ArgumentException("Two matrices must have the same size!");
            for (int i = 0; i < RowCount; i++)
                Loop<D>.CompositeInPlace(data[i], b.data[i], f);
        }
        /// <summary>
        /// Combines each elements of this <see cref="Matrix{D}"/> with the given scalar <typeparamref name="D"/> using the given function. Returns a new <see cref="Matrix{D}"/> containing the result.
        /// </summary>
        public Matrix<D> Combine(D b, Func<D, D, D> f)
        {
            Matrix<D> mat = new(this);
            mat.CombineInPlace(b, f);
            return mat;
        }
        /// <summary>
        /// Combines each elements of this <see cref="Matrix{D}"/> with the given scalar <typeparamref name="D"/> using the given function. Modifies this <see cref="Matrix{D}"/> to contain the result.
        /// </summary>
        public void CombineInPlace(D b, Func<D, D, D> f) => data.Apply(row => Loop<D>.CombineInPlace(row, b, f));
        /// <summary>
        /// Maps each element of this <see cref="Matrix{D}"/> using the given function. Returns a new <see cref="Matrix{D}"/> containing the result.
        /// </summary>
        public Matrix<D> Map(Func<D, D> f)
        {
            Matrix<D> newmat = new(this);
            newmat.MapInPlace(f);
            return newmat;
        }
        /// <summary>
        /// Maps each index of this <see cref="Matrix{D}"/> using the given function. Modifies this <see cref="Matrix{D}"/> to contain the result.
        /// </summary>
        public Matrix<D> ReMap(Func<int, int, D> f)
        {
            Matrix<D> newmat = new(this);
            newmat.ReMapInPlace(f);
            return newmat;
        }
        /// <summary>
        /// Maps each element of this <see cref="Matrix{D}"/> using the given function. Modifies this <see cref="Matrix{D}"/> to contain the result.
        /// </summary>
        public void MapInPlace(Func<D, D> f) => data.Apply(arr => Loop<D>.MapInPlace(arr, f));
        /// <summary>
        /// Maps each index of this <see cref="Matrix{D}"/> using the given function. Modifies this <see cref="Matrix{D}"/> to contain the result.
        /// </summary>
        public void ReMapInPlace(Func<int, int, D> f)
        {
            for (int i = 0; i < RowCount; i++)
            {
                Span<D> span = data[i];
                for (int j = 0; j < span.Length; j++)
                    span[j] = f(i, j);
            }
        }
        /// <summary>
        /// Applies the given action using each element of this <see cref="Matrix{D}"/> as the argument. Does not modify this <see cref="Matrix{D}"/>.
        /// </summary>
        public void Apply(Action<D> action) => data.Apply(arr => Loop<D>.Apply(arr, action));
        /// <summary>
        /// Accumulates the elements of this <see cref="Matrix{D}"/> using the given function into the given accumulator. Returns the accumulated value.
        /// </summary>
        public D Accumulate(D acc, Func<D, D, D> func)
        {
            data.Apply(row => Loop<D>.Accumulate(row, acc, func));
            return acc;
        }
        /// <summary>
        /// Returns true if the given function returns true for all elements of this <see cref="Matrix{D}"/>. Returns false otherwise.
        /// </summary>
        public bool TrueForAll(Func<D, bool> func)
        {
            bool res = true;
            for (int i = 0; i < RowCount; i++)
            {
                res &= Loop<D>.TrueForAll(data[i], func);
                if (!res) break;
            }
            return res;
        }
        /// <summary>
        /// Returns true if the given function returns true for any element of this <see cref="Matrix{D}"/>. Returns false otherwise.
        /// </summary>
        public bool TrueForAny(Func<D, bool> func)
        {
            bool res = false;
            for (int i = 0; i < RowCount; i++)
            {
                res |= Loop<D>.TrueForAny(data[i], func);
                if (res) break;
            }
            return res;
        }
        /// <summary>
        /// Returns a new <see cref="Matrix{D}"/> where each element is the absolute value of the corresponding element of this <see cref="Matrix{D}"/>.
        /// </summary>
        public Matrix<D> Abs() => Map(x => x < D.Zero ? -x : x);
        /// <summary>
        /// Returns the sum of all elements of this <see cref="Matrix{D}"/>.
        /// </summary>
        public D Sum() => Accumulate(D.Zero, (acc, x) => acc + x);
        /// <summary>
        /// Returns the max value of all elements of this <see cref="Matrix{D}"/>.
        /// </summary>
        public D Max() => Accumulate(minValue, (acc, x) => x > acc ? x : acc);
        /// <summary>
        /// Returns the min value of all elements of this <see cref="Matrix{D}"/>.
        /// </summary>
        public D Min() => Accumulate(maxValue, (acc, x) => x < acc ? x : acc);
        /// <summary>
        /// Returns the transpose of this <see cref="Matrix{D}"/>.
        /// </summary>
        public Matrix<D> T()
        {
            FastList<ArrVector<D>> cols = new();
            GetColumns().ForEach(cols.Add);
            return cols;
        }
        /// <summary>
        /// Returns the inverse of this <see cref="Matrix{D}"/>.
        /// </summary>
        public Matrix<D> Inv()
        {
            D det = Det();
            if (Calc<D>.Near(det, D.Zero)) throw new DivideByZeroException(nameof(det));
            return Adj() / det;
        }
        /// <summary>
        /// Returns the determinant of this <see cref="Matrix{D}"/>.
        /// </summary>
        public D Det()
        {
            if (RowCount != ColumnCount) throw new Exception("Determinant does not exist!");
            if (RowCount == 0) throw new Exception("Matrix is empty!");
            if (RowCount == 1) return data[0][0];
            if (RowCount == 2) return data[0][0] * data[1][1] - data[0][1] * data[1][0];
            D det = D.One;
            FastList<ArrVector<D>> rows = GetRows();
            for (int c = 0; c < ColumnCount; c++)
            {
                bool isReduced = false;
                for (int r = c; r < RowCount; r++) // make minors start with 1
                {
                    if (Calc<D>.Near(rows[r][c], D.Zero)) continue; // [0,x,y,...]
                    det *= rows[r][c]; // [n,x,y,...]
                    rows[r] /= rows[r][c]; // [1,x,y,...]
                    isReduced = true;
                }
                if (!isReduced) throw new Exception("Determinant does not exist!");
                D currentMinor = rows[c][c]; // upperleft corner of the target range
                int rowNumber = c;
                while (Calc<D>.Near(currentMinor, D.Zero)) // when the cth row minor is 0
                {
                    rowNumber++;
                    currentMinor = rows[rowNumber][c]; // set to next minor
                }
                if (rowNumber != c) // means first minor is 0 and the rowNumber'th minor is 1
                {
                    (rows[c], rows[rowNumber]) = (rows[rowNumber], rows[c]); // swap rows
                    det *= -D.One; // flip the coefficient due to row swapping
                }
                for (int i = c + 1; i < RowCount; i++)
                {
                    if (Calc<D>.Near(rows[i][c], D.Zero)) continue; // minor is already 0
                    rows[i] -= rows[c]; // minus the first row
                }
            }
            return det;
        }
        /// <summary>
        /// Returns the adjoint of this <see cref="Matrix{D}"/>.
        /// </summary>
        public Matrix<D> Adj()
        {
            if (RowCount != ColumnCount) throw new Exception("Adjugate does not exist!");
            if (RowCount == 0) throw new Exception("Matrix is empty!");
            Matrix<D> res = new(this);
            for (int i = 0; i < RowCount; i++)
            {
                Span<D> span = res.data[i];
                for (int j = 0; j < span.Length; j++)
                    span[j] = Cofactor(i, j);
            }
            return res.T();
        }
        /// <summary>
        /// Returns the cofactor of this <see cref="Matrix{D}"/> at the given row and column.
        /// </summary>
        public D Cofactor(int r, int c)
        {
            FastList<ArrVector<D>> rows = GetRows();
            rows.RemoveAt(r);
            FastList<ArrVector<D>> cols = ((Matrix<D>)rows).GetColumns();
            cols.RemoveAt(c);
            D coefficient = D.CreateSaturating(Math.Pow(-1, r + c));
            return coefficient * ((Matrix<D>)cols).Det();
        }
        /// <summary>
        /// Returns the normalized version of this <see cref="Matrix{D}"/>.
        /// </summary>
        public Matrix<D> Normalized()
        {
            D det = Det();
            if (det == D.Zero) throw new DivideByZeroException(nameof(det));
            return this / det;
        }
        /// <summary>
        /// Returns true if this <see cref="Matrix{D}"/> is equal to the given <see cref="Matrix{D}"/> at the precision of <see cref="Epsilon"/>.
        /// </summary>
        public bool Near(Matrix<D> other)
        {
            if (RowCount != other.RowCount || ColumnCount != other.ColumnCount) return false;
            bool isNear = true;
            for (int i = 0; i < other.RowCount; i++)
            {
                ReadOnlySpan<D> spana = data[i];
                ReadOnlySpan<D> spanb = other.data[i];
                for (int j = 0; j < spana.Length; j++)
                {
                    isNear &= Calc<D>.Near(spana[j], spanb[j], Epsilon);
                    if (!isNear) break;
                }
                if (!isNear) break;
            }
            return isNear;
        }
        #endregion

        #region Public Override Methods
        /// <summary>
        /// Returns the string representation of this <see cref="Matrix{D}"/>.
        /// </summary>
        public override string ToString() => $"[{string.Join("; ", GetRows())}]";
        /// <summary>
        /// Returns true if this <see cref="Matrix{D}"/> is equal to the given <see cref="Matrix{D}"/>.
        /// </summary>
        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            if (obj is null) return false;
            if (obj is not Matrix<D>) return false;
            Matrix<D> mat = (Matrix<D>)obj;
            if (mat.RowCount != RowCount || mat.ColumnCount != ColumnCount) return false;
            bool equals = true;
            for (int i = 0; i < RowCount; i++)
            {
                equals &= data[i].Equals(mat.data[i]);
                if (!equals) break;
            }
            return equals;
        }
        /// <summary>
        /// Returns the hash code of this <see cref="Matrix{D}"/>.
        /// </summary>
        public override int GetHashCode() { return data.GetHashCode(); }
        #endregion

        #region Static Methods
        /// <summary>
        /// Returns the zero <see cref="Matrix{D}"/> of the given row and column count.
        /// </summary>
        public static Matrix<D> Zero(int rowCount, int columnCount) { return new(rowCount, columnCount, D.Zero); }
        /// <summary>
        /// Returns the one <see cref="Matrix{D}"/> of the given row and column count.
        /// </summary>
        public static Matrix<D> One(int rowCount, int columnCount) { return new(rowCount, columnCount, D.One); }
        /// <summary>
        /// Returns the 2D rotation matrix of the given angle.
        /// </summary>
        public static Matrix<D> Rot2D(D theta)
        {
            D sin = Calc<D>.Calc1(theta, Math.Sin);
            D cos = Calc<D>.Calc1(theta, Math.Cos);
            return new(2, 2, new D[] { cos, -sin, sin, cos });
        }
        /// <summary>
        /// Returns the 3D rotation matrix about the given <see cref="Axis"/> of the given angle.
        /// </summary>
        public static Matrix<D> Rot3D(Axis axis, D theta)
        {
            D sin = Calc<D>.Calc1(theta, Math.Sin);
            D cos = Calc<D>.Calc1(theta, Math.Cos);
            return axis switch
            {
                Axis.X => new(3, 3, new D[] { D.One, D.Zero, D.Zero, D.Zero, cos, -sin, D.Zero, sin, cos }),
                Axis.Y => new(3, 3, new D[] { cos, D.Zero, sin, D.Zero, D.One, D.Zero, -sin, D.Zero, cos }),
                Axis.Z => new(3, 3, new D[] { cos, -sin, D.Zero, sin, cos, D.Zero, D.Zero, D.Zero, D.One }),
                _ => throw new ArgumentException("Axis must be X, Y or Z!")
            };
        }
        /// <summary>
        /// Returns the identity <see cref="Matrix{D}"/> of the given row count.
        /// </summary>
        public static Matrix<D> I(int rows) => new(rows, rows, (i, j) => i == j ? D.One : D.Zero);
        /// <summary>
        /// Returns the upper triangular <see cref="Matrix{D}"/> of the given row count.
        /// </summary>
        public static Matrix<D> UpperTrig(int rows) => new(rows, rows, (i, j) => i <= j ? D.One : D.Zero);
        /// <summary>
        /// Returns the lower triangular <see cref="Matrix{D}"/> of the given row count.
        /// </summary>
        public static Matrix<D> LowerTrig(int rows) => new(rows, rows, (i, j) => i >= j ? D.One : D.Zero);
        #endregion

        #region Operators
        /// <summary>
        /// Absolute value of mat
        /// </summary>
        public static Matrix<D> operator +(Matrix<D> mat) => mat.Abs();
        /// <summary>
        /// Negation of mat
        /// </summary>
        public static Matrix<D> operator -(Matrix<D> mat) => mat.Map(x => -x);
        /// <summary>
        /// Elementwise increment of mat
        /// </summary>
        public static Matrix<D> operator ++(Matrix<D> mat) => mat.Map(x => ++x);
        /// <summary>
        /// Elementwise decrement of mat
        /// </summary>
        public static Matrix<D> operator --(Matrix<D> mat) => mat.Map(x => --x);
        /// <summary>
        /// Normalized mat
        /// </summary>
        public static Matrix<D> operator ~(Matrix<D> mat) => mat.Normalized();
        /// <summary>
        /// Elementwise addition of mat1 and mat2
        /// </summary>
        public static Matrix<D> operator +(Matrix<D> mat1, Matrix<D> mat2) => mat1.Composite(mat2, (a, b) => a + b);
        /// <summary>
        /// Elementwise addition of scalar val and mat
        /// </summary>
        public static Matrix<D> operator +(D val, Matrix<D> mat) => mat.Combine(val, (a, b) => a + b);
        /// <summary>
        /// Elementwise addition of mat and scalar val
        /// </summary>
        public static Matrix<D> operator +(Matrix<D> mat, D val) => mat.Combine(val, (a, b) => a + b);
        /// <summary>
        /// Elementwise subtraction of mat1 and mat2
        /// </summary>
        public static Matrix<D> operator -(Matrix<D> mat1, Matrix<D> mat2) => mat1.Composite(mat2, (a, b) => a - b);
        /// <summary>
        /// Elementwise subtraction of scalar val from mat
        /// </summary>
        public static Matrix<D> operator -(D val, Matrix<D> mat) => mat.Combine(val, (a, b) => a - b);
        /// <summary>
        /// Elementwise subtraction of mat and scalar val
        /// </summary>
        public static Matrix<D> operator -(Matrix<D> mat, D val) => mat.Combine(val, (a, b) => a - b);
        /// <summary>
        /// Elementwise multiplication of scalar val and mat
        /// </summary>
        public static Matrix<D> operator *(D val, Matrix<D> mat) => mat.Combine(val, (a, b) => a * b);
        /// <summary>
        /// Elementwise multiplication of mat and scalar val
        /// </summary>
        public static Matrix<D> operator *(Matrix<D> mat, D val) => mat.Combine(val, (a, b) => a * b);
        /// <summary>
        /// Matrix division mat / mat2 = mat * mat2.Inv()
        /// </summary>
        public static Matrix<D> operator /(Matrix<D> mat1, Matrix<D> mat2) => mat1 * mat2.Inv();
        /// <summary>
        /// Elementwise division of scalar val and mat
        /// </summary>
        public static Matrix<D> operator /(D val, Matrix<D> mat) => mat.Combine(val, (a, b) => a / b);
        /// <summary>
        /// Elementwise division of mat and scalar val
        /// </summary>
        public static Matrix<D> operator /(Matrix<D> mat, D val) => mat.Combine(val, (a, b) => a / b);
        /// <summary>
        /// Matrix multiplication mat1 * mat2
        /// </summary>
        public static Matrix<D> operator *(Matrix<D> mat1, Matrix<D> mat2)
        {
            if (mat1.ColumnCount != mat2.RowCount) throw new ArgumentException("The column count of mat1 must be equal to the row count of mat2!");
            FastList<ArrVector<D>> rows = mat1.GetRows();
            FastList<ArrVector<D>> cols = mat2.GetColumns();
            return new(mat1.RowCount, mat2.ColumnCount, (r, c) => rows[r] * cols[c]);
        }
        /// <summary>
        /// Vector and Matrix multiplication v * m
        /// </summary>
        public static ArrVector<D> operator *(ArrVector<D> v, Matrix<D> m)
        {
            if (v.Length != m.RowCount) throw new ArgumentException("Vector length must be equal to the matrix row count!");
            FastList<ArrVector<D>> cols = m.GetColumns();
            return new(m.ColumnCount, i => v * cols[i]);
        }
        /// <summary>
        /// Matrix and Vector multiplication m * v
        /// </summary>
        public static ArrVector<D> operator *(Matrix<D> m, ArrVector<D> v)
        {
            if (v.Length != m.ColumnCount) throw new ArgumentException("Vector length must be equal to the matrix column count!");
            FastList<ArrVector<D>> rows = m.GetRows();
            return new(m.RowCount, i => rows[i] * v);
        }
        /// <summary>
        /// Returns thue if mat1 and mat2 are equal
        /// </summary>
        public static bool operator ==(Matrix<D> mat1, Matrix<D> mat2) => mat1.Equals(mat2);
        /// <summary>
        /// Returns thue if mat1 and mat2 are not equal
        /// </summary>
        public static bool operator !=(Matrix<D> mat1, Matrix<D> mat2) => mat1.Equals(mat2);
        #endregion

        #region Converters
        /// <summary>Implicit conversion from <see cref="Matrix{D}"/> to <typeparamref name="D"/>[][]</summary>
        public static implicit operator D[][](Matrix<D> mat) => mat.data.ToArray();
        /// <summary>Implicit conversion from <typeparamref name="D"/>[][] to <see cref="Matrix{D}"/></summary>
        public static implicit operator Matrix<D>(D[][] values) => new(values);
        /// <summary>Implicit conversion from <see cref="Matrix{D}"/> to <see cref="FastList{T}"/> of <typeparamref name="D"/>[]</summary>
        public static implicit operator FastList<D[]>(Matrix<D> mat) => mat.data;
        /// <summary>Implicit conversion from <see cref="FastList{D}"/>[] to <see cref="Matrix{D}"/></summary>
        public static implicit operator Matrix<D>(FastList<D[]> list) => new(list.ToArray());
        /// <summary>Implicit conversion from <see cref="Matrix{D}"/> to <see cref="FastList{T}"/> of <see cref="ArrVector{D}"/></summary>
        public static implicit operator FastList<ArrVector<D>>(Matrix<D> mat) => mat.GetRows();
        /// <summary>Implicit conversion from <see cref="FastList{T}"/> of <see cref="ArrVector{D}"/> to <see cref="Matrix{D}"/></summary>
        public static implicit operator Matrix<D>(FastList<ArrVector<D>> list) => new(list.ToArray());
        #endregion
    }
}