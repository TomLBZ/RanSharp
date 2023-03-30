using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using RanSharp.Maths;

namespace RanSharp.Performance
{
    public static class Loop
    {
        #region Easy Loop
        public static void Do(int count, Action action)
        {
            for (int i = 0; i < count; i++) action();
        }
        public static void Do(int count, Action<int> action)
        {
            for (int i = 0; i < count; i++) action(i);
        }
        #endregion
    }
    public static class Loop<T>
    {
        #region On List
        public static List<T> Composite(List<T>? a, List<T>? b, Func<T, T, T> func)
        {
            if (null == a || null == b) throw new ArgumentNullException(nameof(a));
            if (a.Count != b.Count) throw new ArgumentException("Arguments are not of equal length!");
            List<T> result = new(a.Count);
            ReadOnlySpan<T> spana = CollectionsMarshal.AsSpan(a);
            ReadOnlySpan<T> spanb = CollectionsMarshal.AsSpan(b);
            for (int i = 0; i < spana.Length; i++)
                result.Add(func(spana[i], spanb[i]));
            return result;
        }
        public static void CompositeInPlace(List<T>? a, List<T>? b, Func<T, T, T> func)
        {
            if (null == a || null == b) throw new ArgumentNullException(nameof(a));
            if (a.Count != b.Count) throw new ArgumentException("Arguments are not of equal length!");
            Span<T> spana = CollectionsMarshal.AsSpan(a);
            ReadOnlySpan<T> spanb = CollectionsMarshal.AsSpan(b);
            for (int i = 0; i < spana.Length; i++) spana[i] = func(spana[i], spanb[i]);
        }
        public static List<T> Combine(List<T>? a, T? b, Func<T, T, T> func)
        {
            if (null == a || null == b) throw new ArgumentNullException(nameof(a));
            List<T> result = new(a.Count);
            ReadOnlySpan<T> spana = CollectionsMarshal.AsSpan(a);
            for (int i = 0; i < spana.Length; i++)
                result.Add(func(spana[i], b));
            return result;
        }
        public static void CombineInPlace(List<T>? a, T? b, Func<T, T, T> func)
        {
            if (null == a || null == b) throw new ArgumentNullException(nameof(a));
            Span<T> spana = CollectionsMarshal.AsSpan(a);
            for (int i = 0; i < spana.Length; i++) spana[i] = func(spana[i], b);
        }
        public static List<T> Map(List<T>? a, Func<T, T> func)
        {
            if (null == a) throw new ArgumentNullException(nameof(a));
            List<T> result = new(a.Count);
            ReadOnlySpan<T> spana = CollectionsMarshal.AsSpan(a);
            for (int i = 0; i < spana.Length; i++)
                result.Add(func(spana[i]));
            return result;
        }
        public static void MapInPlace(List<T>? a, Func<T, T> func)
        {
            if (null == a) throw new ArgumentNullException(nameof(a));
            Span<T> spana = CollectionsMarshal.AsSpan(a);
            for (int i = 0; i < spana.Length; i++) spana[i] = func(spana[i]);
        }
        public static List<T> ReMap(List<T>? a, Func<int, T> func)
        {
            if (null == a) throw new ArgumentNullException(nameof(a));
            List<T> result = new(a.Count);
            for (int i = 0; i < a.Count; i++)
                result.Add(func(i));
            return result;
        }
        public static void ReMapInPlace(List<T>? a, Func<int, T> func)
        {
            if (null == a) throw new ArgumentNullException(nameof(a));
            Span<T> spana = CollectionsMarshal.AsSpan(a);
            for (int i = 0; i < spana.Length; i++) spana[i] = func(i);
        }
        public static T Accumulate(List<T> a, T acc, Func<T, T, T> func)
        {
            ReadOnlySpan<T> span = CollectionsMarshal.AsSpan(a);
            for (int i = 0; i < span.Length; i++) acc = func(acc, span[i]);
            return acc;
        }
        public static void Apply(List<T>? a, Action<T> action)
        {
            if (null == a) throw new ArgumentNullException(nameof(a));
            ReadOnlySpan<T> spana = CollectionsMarshal.AsSpan(a);
            for (int i = 0; i < spana.Length; i++)
                action(spana[i]);
        }
        public static bool TrueForAll(List<T> a, Func<T, bool> func)
        {
            ReadOnlySpan<T> span = CollectionsMarshal.AsSpan(a);
            for (int i = 0; i < span.Length; i++)
                if (!func(span[i])) return false;
            return true;
        }
        public static bool TrueForAny(List<T> a, Func<T, bool> func)
        {
            ReadOnlySpan<T> span = CollectionsMarshal.AsSpan(a);
            for (int i = 0; i < span.Length; i++)
                if (func(span[i])) return true;
            return false;
        }
        #endregion

        #region On FastList
        public static FastList<T> Composite(FastList<T>? a, FastList<T>? b, Func<T, T, T> func)
        {
            if (null == a || null == b) throw new ArgumentNullException(nameof(a));
            if (a.Count != b.Count) throw new ArgumentException("Arguments are not of equal length!");
            FastList<T> result = new(a.Count);
            ReadOnlySpan<T> spana = a.ItemsUnsafe.AsSpan(0, a.Count);
            ReadOnlySpan<T> spanb = b.ItemsUnsafe.AsSpan(0, b.Count);
            Span<T> spanr = result.ItemsUnsafe.AsSpan(); // full span
            for (int i = 0; i < spana.Length; i++)
                spanr[i] = func(spana[i], spanb[i]);
            result.UpdateCountUnsafe(a.Count);
            return result;
        }
        public static void CompositeInPlace(FastList<T>? a, FastList<T>? b, Func<T, T, T> func)
        {
            if (null == a || null == b) throw new ArgumentNullException(nameof(a));
            if (a.Count != b.Count) throw new ArgumentException("Arguments are not of equal length!");
            Span<T> spana = a.ItemsUnsafe.AsSpan(0, a.Count);
            ReadOnlySpan<T> spanb = b.ItemsUnsafe.AsSpan(0, b.Count);
            for (int i = 0; i < spana.Length; i++) spana[i] = func(spana[i], spanb[i]);
        }
        public static FastList<T> Combine(FastList<T>? a, T? b, Func<T, T, T> func)
        {
            if (null == a || null == b) throw new ArgumentNullException(nameof(a));
            FastList<T> result = new(a.Count);
            ReadOnlySpan<T> spana = a.ItemsUnsafe.AsSpan(0, a.Count);
            Span<T> spanr = result.ItemsUnsafe.AsSpan(); // full span
            for (int i = 0; i < spana.Length; i++)
                spanr[i] = func(spana[i], b);
            result.UpdateCountUnsafe(a.Count);
            return result;
        }
        public static void CombineInPlace(FastList<T>? a, T? b, Func<T, T, T> func)
        {
            if (null == a || null == b) throw new ArgumentNullException(nameof(a));
            Span<T> spana = a.ItemsUnsafe.AsSpan(0, a.Count);
            for (int i = 0; i < spana.Length; i++) spana[i] = func(spana[i], b);
        }
        public static FastList<T> Map(FastList<T>? a, Func<T, T> func)
        {
            if (null == a) throw new ArgumentNullException(nameof(a));
            FastList<T> result = new(a.Count);
            ReadOnlySpan<T> spana = a.ItemsUnsafe.AsSpan(0, a.Count);
            Span<T> spanr = result.ItemsUnsafe.AsSpan(); // full span
            for (int i = 0; i < spana.Length; i++)
                spanr[i] = func(spana[i]);
            result.UpdateCountUnsafe(a.Count);
            return result;
        }
        public static void MapInPlace(FastList<T>? a, Func<T, T> func)
        {
            if (null == a) throw new ArgumentNullException(nameof(a));
            Span<T> spana = a.ItemsUnsafe.AsSpan(0, a.Count);
            for (int i = 0; i < spana.Length; i++) spana[i] = func(spana[i]);
        }
        public static FastList<T> ReMap(FastList<T>? a, Func<int, T> func)
        {
            if (null == a) throw new ArgumentNullException(nameof(a));
            FastList<T> result = new(a.Count);
            Span<T> spanr = result.ItemsUnsafe.AsSpan(); // full span
            for (int i = 0; i < spanr.Length; i++)
                spanr[i] = func(i);
            result.UpdateCountUnsafe(a.Count);
            return result;
        }
        public static void ReMapInPlace(FastList<T>? a, Func<int, T> func)
        {
            if (null == a) throw new ArgumentNullException(nameof(a));
            Span<T> spana = a.ItemsUnsafe.AsSpan(0, a.Count);
            for (int i = 0; i < spana.Length; i++) spana[i] = func(i);
        }
        public static void Apply(FastList<T>? a, Action<T> action)
        {
            if (null == a) throw new ArgumentNullException(nameof(a));
            ReadOnlySpan<T> spana = a.ItemsUnsafe.AsSpan(0, a.Count);
            for (int i = 0; i < spana.Length; i++)
                action(spana[i]);
        }
        public static T Accumulate(FastList<T> a, T acc, Func<T, T, T> func)
        {
            ReadOnlySpan<T> span = a.ItemsUnsafe.AsSpan(0, a.Count);
            for (int i = 0; i < span.Length; i++) acc = func(acc, span[i]);
            return acc;
        }
        public static bool TrueForAll(FastList<T> a, Func<T, bool> func)
        {
            ReadOnlySpan<T> span = a.ItemsUnsafe.AsSpan(0, a.Count);
            for (int i = 0; i < span.Length; i++)
                if (!func(span[i])) return false;
            return true;
        }
        public static bool TrueForAny(FastList<T> a, Func<T, bool> func)
        {
            ReadOnlySpan<T> span = a.ItemsUnsafe.AsSpan(0, a.Count);
            for (int i = 0; i < span.Length; i++)
                if (func(span[i])) return true;
            return false;
        }
        #endregion

        #region On Array
        public static T[] Composite(T[]? a, T[]? b, Func<T, T, T> func)
        {
            if (null == a || null == b) throw new ArgumentNullException(nameof(a));
            if (a.Length != b.Length) throw new ArgumentException("Arguments are not of equal length!");
            T[] result = new T[a.Length];
            ReadOnlySpan<T> spana = a.AsSpan();
            ReadOnlySpan<T> spanb = b.AsSpan();
            Span<T> spanr = result.AsSpan();
            for (int i = 0; i < spana.Length; i++)
                spanr[i] = func(spana[i], spanb[i]);
            return result;
        }
        public static void CompositeInPlace(T[]? a, T[]? b, Func<T, T, T> func)
        {
            if (null == a || null == b) throw new ArgumentNullException(nameof(a));
            if (a.Length != b.Length) throw new ArgumentException("Arguments are not of equal length!");
            Span<T> spana = a.AsSpan();
            ReadOnlySpan<T> spanb = b.AsSpan();
            for (int i = 0; i < spana.Length; i++) spana[i] = func(spana[i], spanb[i]);
        }
        public static T[] Combine(T[]? a, T? b, Func<T, T, T> func)
        {
            if (null == a || null == b) throw new ArgumentNullException(nameof(a));
            T[] result = new T[a.Length];
            ReadOnlySpan<T> spana = a.AsSpan();
            Span<T> spanr = result.AsSpan();
            for (int i = 0; i < spana.Length; i++)
                spanr[i] = func(spana[i], b);
            return result;
        }
        public static void CombineInPlace(T[]? a, T? b, Func<T, T, T> func)
        {
            if (null == a || null == b) throw new ArgumentNullException(nameof(a));
            Span<T> spana = a.AsSpan();
            for (int i = 0; i < spana.Length; i++) spana[i] = func(spana[i], b);
        }
        public static T[] Map(T[]? a, Func<T, T> func)
        {
            if (null == a) throw new ArgumentNullException(nameof(a));
            T[] result = new T[a.Length];
            ReadOnlySpan<T> spana = a.AsSpan();
            Span<T> spanr = result.AsSpan();
            for (int i = 0; i < spana.Length; i++)
                spanr[i] = func(spana[i]);
            return result;
        }
        public static void MapInPlace(T[]? a, Func<T, T> func)
        {
            if (null == a) throw new ArgumentNullException(nameof(a));
            Span<T> spana = a.AsSpan();
            for (int i = 0; i < spana.Length; i++) spana[i] = func(spana[i]);
        }
        public static T[] ReMap(T[]? a, Func<int, T> func)
        {
            if (null == a) throw new ArgumentNullException(nameof(a));
            T[] result = new T[a.Length];
            Span<T> spanr = result.AsSpan();
            for (int i = 0; i < spanr.Length; i++)
                spanr[i] = func(i);
            return result;
        }
        public static void ReMapInPlace(T[]? a, Func<int, T> func)
        {
            if (null == a) throw new ArgumentNullException(nameof(a));
            Span<T> spana = a.AsSpan();
            for (int i = 0; i < spana.Length; i++) spana[i] = func(i);
        }
        public static void Apply(T[]? a, Action<T> action)
        {
            if (null == a) throw new ArgumentNullException(nameof(a));
            ReadOnlySpan<T> spana = a.AsSpan();
            for (int i = 0; i < spana.Length; i++)
                action(spana[i]);
        }
        public static T Accumulate(T[] a, T acc, Func<T, T, T> func)
        {
            ReadOnlySpan<T> span = a.AsSpan();
            for (int i = 0; i < span.Length; i++) acc = func(acc, span[i]);
            return acc;
        }
        public static bool TrueForAll(T[] a, Func<T, bool> func)
        {
            ReadOnlySpan<T> span = a.AsSpan();
            for (int i = 0; i < span.Length; i++)
                if (!func(span[i])) return false;
            return true;
        }
        public static bool TrueForAny(T[] a, Func<T, bool> func)
        {
            ReadOnlySpan<T> span = a.AsSpan();
            for (int i = 0; i < span.Length; i++)
                if (func(span[i])) return true;
            return false;
        }
        #endregion

        #region Print Extensions
        public static string ReadableString(T[] a) => $"({string.Join(", ", a)})";
        public static string ReadableString(T[][] a)
        {
            StringBuilder sb = new('[');
            for (int i = 0; i < a.Length; i++)
            {
                sb.Append(ReadableString(a[i]));
                if (i < a.Length - 1) sb.Append("; ");
            }
            sb.Append(']');
            return sb.ToString();
        }
        #endregion
    }

    public static class NumericLoop<T> where T: struct, INumber<T>
    {
        #region On ArrVector
        public static ArrVector<T> Composite(ArrVector<T> a, ArrVector<T> b, Func<T, T, T> func)
        {
            if (a.Length != b.Length) throw new ArgumentException("Argument are not of equal length!");
            ReadOnlySpan<T> spana = a;
            ReadOnlySpan<T> spanb = b;
            ArrVector<T> arrVector = new(a.Length);
            Span<T> spanv = arrVector;
            for (int i = 0; i < spana.Length; i++) spanv[i] = func(spana[i], spanb[i]);
            return arrVector;
        }
        public static void CompositeInPlace(ArrVector<T> a, ArrVector<T> b, Func<T, T, T> func)
        {
            if (a.Length != b.Length) throw new ArgumentException("Argument are not of equal length!");
            Span<T> spana = a;
            ReadOnlySpan<T> spanb = b;
            for (int i = 0; i < spana.Length; i++) spana[i] = func(spana[i], spanb[i]);
        }
        public static ArrVector<T> Combine(ArrVector<T> a, T b, Func<T, T, T> func)
        {
            ArrVector<T> result = new(a.Length);
            ReadOnlySpan<T> spana = a;
            Span<T> spanv = result;
            for (int i = 0; i < spana.Length; i++) spanv[i] = func(spana[i], b);
            return result;
        }
        public static void CombineInPlace(ArrVector<T> a, T b, Func<T, T, T> func)
        {
            Span<T> spana = a;
            for (int i = 0; i < spana.Length; i++) spana[i] = func(spana[i], b);
        }
        public static ArrVector<T> Map(ArrVector<T> a, Func<T, T> func)
        {
            ArrVector<T> result = new(a.Length);
            ReadOnlySpan<T> spana = a;
            Span<T> spanv = result;
            for (int i = 0; i < spana.Length; i++) spanv[i] = func(spana[i]);
            return result;
        }
        public static void MapInPlace(ArrVector<T> a, Func<T, T> func)
        {
            Span<T> spana = a;
            for (int i = 0; i < spana.Length; i++) spana[i] = func(spana[i]);
        }
        public static ArrVector<T> ReMap(ArrVector<T> a, Func<int, T> func)
        {
            ArrVector<T> result = new(a.Length);
            Span<T> spanv = result;
            for (int i = 0; i < spanv.Length; i++) spanv[i] = func(i);
            return result;
        }
        public static void ReMapInPlace(ArrVector<T> a, Func<int, T> func)
        {
            Span<T> spana = a;
            for (int i = 0; i < spana.Length; i++) spana[i] = func(i);
        }
        public static T Accumulate(ArrVector<T> a, T acc, Func<T, T, T> func)
        {
            ReadOnlySpan<T> span = a;
            for (int i = 0; i < span.Length; i++) acc = func(acc, span[i]);
            return acc;
        }
        public static void Apply(ArrVector<T> a, Action<T> action)
        {
            ReadOnlySpan<T> spana = a;
            for (int i = 0; i < spana.Length; i++) action(spana[i]);
        }
        public static bool TrueForAll(ArrVector<T> a, Func<T, bool> func)
        {
            ReadOnlySpan<T> span = a;
            for (int i = 0; i < span.Length; i++)
                if (!func(span[i])) return false;
            return true;
        }
        public static bool TrueForAny(ArrVector<T> a, Func<T, bool> func)
        {
            ReadOnlySpan<T> span = a;
            for (int i = 0; i < span.Length; i++)
                if (func(span[i])) return true;
            return false;
        }
        #endregion
    }
}