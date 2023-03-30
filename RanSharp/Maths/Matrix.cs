using System.Diagnostics.CodeAnalysis;
using RanSharp.Performance;
using System.Numerics;

namespace RanSharp.Maths
{
    /// <summary>
    /// Represents a matrix of dimension RxC, where R and C are dimensions.
    /// <br/>Special Usages:<br/>
    /// Generic Matrix Multiplication: <code>m1 * m2</code>
    /// Vector x Matrix: <code>v * m</code> or <code>m * v</code>, where v is a vector and m is a matrix.
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
        public readonly int RowCount;
        public readonly int ColumnCount;
        public readonly int Count => RowCount * ColumnCount;
        public readonly double Epsilon = 1e-9;
        #endregion

        #region Private States
        private readonly FastList<D[]> data;
        #endregion

        #region Constructors
        public Matrix(int rowCount, int columnCount)
        {
            RowCount = rowCount;
            ColumnCount = columnCount;
            data = new(rowCount, i => new D[columnCount]);
        }
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
        public Matrix(Matrix<D> mat)
        {
            if (mat.Count == 0) throw new IndexOutOfRangeException();
            RowCount = mat.RowCount;
            ColumnCount = mat.ColumnCount;
            FastList<ArrVector<D>> rows = mat.GetRows();
            data = new(RowCount, i => rows[i]);
        }
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
        public FastList<ArrVector<D>> GetRows()
        {
            FastList<ArrVector<D>> rows = new(RowCount);
            data.Apply(i => rows.Add(i.ToArray())); // copy before adding
            return rows;
        }
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
        public D this[int i, int j]
        {
            get { return data[i][j]; }
            set { data[i][j] = value; }
        }
        public D[] this[int row]
        {
            get { return data[row]; }
            set { data[row] = value; }
        }
        public Matrix<D> Composite(Matrix<D> b, Func<D, D, D> f)
        {
            if (RowCount != b.RowCount || ColumnCount != b.ColumnCount) throw new ArgumentException("Two matrices must have the same size!");
            Matrix<D> mat = new(this);
            mat.CompositeInPlace(b, f);
            return mat;
        }
        public void CompositeInPlace(Matrix<D> b, Func<D, D, D> f)
        {
            if (RowCount != b.RowCount || ColumnCount != b.ColumnCount) throw new ArgumentException("Two matrices must have the same size!");
            for (int i = 0; i < RowCount; i++)
                Loop<D>.CompositeInPlace(data[i], b.data[i], f);
        }
        public Matrix<D> Combine(D b, Func<D, D, D> f)
        {
            Matrix<D> mat = new(this);
            mat.CombineInPlace(b, f);
            return mat;
        }
        public void CombineInPlace(D b, Func<D, D, D> f) => data.Apply(row => Loop<D>.CombineInPlace(row, b, f));
        public Matrix<D> Map(Func<D, D> f)
        {
            Matrix<D> newmat = new(this);
            newmat.MapInPlace(f);
            return newmat;
        }
        public Matrix<D> ReMap(Func<int, int, D> f)
        {
            Matrix<D> newmat = new(this);
            newmat.ReMapInPlace(f);
            return newmat;
        }
        public void MapInPlace(Func<D, D> f) => data.Apply(arr => Loop<D>.MapInPlace(arr, f));
        public void ReMapInPlace(Func<int, int, D> f)
        {
            for (int i = 0; i < RowCount; i++)
            {
                Span<D> span = data[i];
                for (int j = 0; j < span.Length; j++)
                    span[j] = f(i, j);
            }
        }
        public void Apply(Action<D> action) => data.Apply(arr => Loop<D>.Apply(arr, action));
        public D Accumulate(D acc, Func<D, D, D> func)
        {
            data.Apply(row => Loop<D>.Accumulate(row, acc, func));
            return acc;
        }
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
        public Matrix<D> Abs() => Map(x => x < D.Zero ? -x : x);
        public D Sum() => Accumulate(D.Zero, (acc, x) => acc + x);
        public D Max() => Accumulate(minValue, (acc, x) => x > acc ? x : acc);
        public D Min() => Accumulate(maxValue, (acc, x) => x < acc ? x : acc);
        public Matrix<D> T()
        {
            FastList<ArrVector<D>> cols = new();
            GetColumns().ForEach(cols.Add);
            return cols;
        }
        public Matrix<D> Inv()
        {
            D det = Det();
            if (Calc<D>.Near(det, D.Zero)) throw new DivideByZeroException(nameof(det));
            return Adj() / det;
        }
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
        public D Cofactor(int r, int c)
        {
            FastList<ArrVector<D>> rows = GetRows();
            rows.RemoveAt(r);
            FastList<ArrVector<D>> cols = ((Matrix<D>)rows).GetColumns();
            cols.RemoveAt(c);
            D coefficient = D.CreateSaturating(Math.Pow(-1, r + c));
            return coefficient * ((Matrix<D>)cols).Det();
        }
        public Matrix<D> Normalized()
        {
            D det = Det();
            if (det == D.Zero) throw new DivideByZeroException(nameof(det));
            return this / det;
        }
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
        public override string ToString() => $"[{string.Join("; ", GetRows())}]";
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
        public override int GetHashCode()
        {
            return data.GetHashCode();
        }
        #endregion

        #region Static Methods
        public static Matrix<D> Zero(int rowCount, int columnCount) { return new(rowCount, columnCount, D.Zero); }
        public static Matrix<D> One(int rowCount, int columnCount) { return new(rowCount, columnCount, D.One); }
        public static Matrix<D> Rot2D(D theta)
        {
            D sin = Calc<D>.Calc1(theta, Math.Sin);
            D cos = Calc<D>.Calc1(theta, Math.Cos);
            return new(2, 2, new D[] { cos, -sin, sin, cos });
        }
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
        public static Matrix<D> I(int rows) => new(rows, rows, (i, j) => i == j ? D.One : D.Zero);
        public static Matrix<D> UpperTrig(int rows) => new(rows, rows, (i, j) => i <= j ? D.One : D.Zero);
        public static Matrix<D> LowerTrig(int rows) => new(rows, rows, (i, j) => i >= j ? D.One : D.Zero);
        #endregion

        #region Operators
        /// <summary>
        /// Absolute value of a
        /// </summary>
        /// <param name="a">Matrix a</param>
        /// <returns>Matrix Abs(a)</returns>
        public static Matrix<D> operator +(Matrix<D> mat) => mat.Abs();
        public static Matrix<D> operator -(Matrix<D> mat) => mat.Map(x => -x);
        public static Matrix<D> operator ++(Matrix<D> mat) => mat.Map(x => ++x);
        public static Matrix<D> operator --(Matrix<D> mat) => mat.Map(x => --x);
        /// <summary>
        /// Normalized a
        /// </summary>
        /// <param name="a">Matrix a</param>
        /// <returns>Matrix a.Normalized()</returns>
        public static Matrix<D> operator ~(Matrix<D> mat) => mat.Normalized();
        public static Matrix<D> operator +(Matrix<D> mat1, Matrix<D> mat2) => mat1.Composite(mat2, (a, b) => a + b);
        public static Matrix<D> operator +(D val, Matrix<D> mat) => mat.Combine(val, (a, b) => a + b);
        public static Matrix<D> operator +(Matrix<D> mat, D val) => mat.Combine(val, (a, b) => a + b);
        public static Matrix<D> operator -(Matrix<D> mat1, Matrix<D> mat2) => mat1.Composite(mat2, (a, b) => a - b);
        public static Matrix<D> operator -(D val, Matrix<D> mat) => mat.Combine(val, (a, b) => a - b);
        public static Matrix<D> operator -(Matrix<D> mat, D val) => mat.Combine(val, (a, b) => a - b);
        public static Matrix<D> operator *(D val, Matrix<D> mat) => mat.Combine(val, (a, b) => a * b);
        public static Matrix<D> operator *(Matrix<D> mat, D val) => mat.Combine(val, (a, b) => a * b);
        public static Matrix<D> operator /(Matrix<D> mat1, Matrix<D> mat2) => mat1 * mat2.Inv();
        public static Matrix<D> operator /(D val, Matrix<D> mat) => mat.Combine(val, (a, b) => a / b);
        public static Matrix<D> operator /(Matrix<D> mat, D val) => mat.Combine(val, (a, b) => a / b);
        /// <summary>
        /// Matrix multiplication
        /// </summary>
        /// <param name="a">Matrix a</param>
        /// <param name="b">Matrix b</param>
        /// <returns>Matrix a*b</returns>
        public static Matrix<D> operator *(Matrix<D> mat1, Matrix<D> mat2)
        {
            if (mat1.ColumnCount != mat2.RowCount) throw new ArgumentException("The column count of mat1 must be equal to the row count of mat2!");
            FastList<ArrVector<D>> rows = mat1.GetRows();
            FastList<ArrVector<D>> cols = mat2.GetColumns();
            return new(mat1.RowCount, mat2.ColumnCount, (r, c) => rows[r] * cols[c]);
        }
        /// <summary>
        /// Vector and Matrix multiplication
        /// </summary>
        /// <param name="a">ArrVector a</param>
        /// <param name="b">Matrix b</param>
        /// <returns>ArrVector a*b</returns>
        public static ArrVector<D> operator *(ArrVector<D> v, Matrix<D> m)
        {
            if (v.Length != m.RowCount) throw new ArgumentException("Vector length must be equal to the matrix row count!");
            FastList<ArrVector<D>> cols = m.GetColumns();
            return new(m.ColumnCount, i => v * cols[i]);
        }
        /// <summary>
        /// Matrix and Vector multiplication
        /// </summary>
        /// <param name="a">Matrix a</param>
        /// <param name="b">ArrVector b</param>
        /// <returns>ArrVector a*b</returns>
        public static ArrVector<D> operator *(Matrix<D> m, ArrVector<D> v)
        {
            if (v.Length != m.ColumnCount) throw new ArgumentException("Vector length must be equal to the matrix column count!");
            FastList<ArrVector<D>> rows = m.GetRows();
            return new(m.RowCount, i => rows[i] * v);
        }
        public static bool operator ==(Matrix<D> mat1, Matrix<D> mat2) => mat1.Equals(mat2);
        public static bool operator !=(Matrix<D> mat1, Matrix<D> mat2) => mat1.Equals(mat2);
        #endregion

        #region Converters
        public static implicit operator D[][](Matrix<D> mat) => mat.data.ToArray();
        public static implicit operator Matrix<D>(D[][] values) => new(values);
        public static implicit operator FastList<D[]>(Matrix<D> mat) => mat.data;
        public static implicit operator Matrix<D>(FastList<D[]> list) => new(list.ToArray());
        public static implicit operator FastList<ArrVector<D>>(Matrix<D> mat) => mat.GetRows();
        public static implicit operator Matrix<D>(FastList<ArrVector<D>> list) => new(list.ToArray());
        #endregion
    }
}