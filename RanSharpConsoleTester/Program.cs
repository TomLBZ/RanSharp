using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using RanSharp.Performance;
using RanSharp.Maths;
using Benchmarking;

BenchmarkType btype = BenchmarkType.None;
switch (btype)
{
    case BenchmarkType.None:
        // fixed 3d types
        Mat3<double> mzero = new();
        Mat3<double> mi = Mat3<double>.I();
        Mat3<double> m = new(2, 4, 1, 3, 5, 6, 5, 7, 8);
        var invm = m.Inv();
        var product = m * invm;
        bool mult_success = (product - mi).Near(mzero);
        string mult_str = "Multiplication " + (mult_success ? "Success" : "Failure");
        Vec3<double> vzero = new();
        Vec3<double> x = Vec3<double>.Unit(Axis.X);
        Mat3<double> rot = Mat3<double>.Rot(Axis.Z, Math.PI / 2);
        var y_frontrot = rot * x;
        var y_backrot = x * rot;
        bool rot_success = (y_frontrot + y_backrot).Near(vzero);
        string rot_str = "Rotation " + (rot_success ? "Success" : $"Failure: Got {y_frontrot} and {y_backrot}");
        Console.WriteLine($"Fixed Length test:\n{mult_str}\n{rot_str}");
        // generic nd types
        Matrix<double> M_zero = new(3, 3);
        Matrix<double> M_i = Matrix<double>.I(3);
        Matrix<double> M = new(new double[,] { { 6, 3, 4 }, { 5, 6, 7 }, { 6, 2, 4 } });
        var invM = M.Inv();
        var productM = M * invM;
        bool multM_success = (productM - M_i).Near(M_zero);
        string multM_str = "Multiplication " + (multM_success ? "Success" : "Failure");
        ArrVector<double> V_zero = new(3);
        ArrVector<double> X = ArrVector<double>.Unit(3, Axis.X);
        Matrix<double> Rot = Matrix<double>.Rot3D(Axis.Z, Math.PI / 2);
        var Y_frontrot = Rot * X;
        var Y_backrot = X * Rot;
        bool rotM_success = (Y_frontrot + Y_backrot).Near(V_zero);
        string rotM_str = "Rotation " + (rotM_success ? "Success" : $"Failure: Got {Y_frontrot} and {Y_backrot}");
        Console.WriteLine($"Generic Length test:\n{multM_str}\n{rotM_str}");
        break;
    case BenchmarkType.LargeDataTest:
        _ = BenchmarkRunner.Run<LargeDataTester>();
        break;
    case BenchmarkType.SmallDataTest:
        _ = BenchmarkRunner.Run<SmallDataTester>();
        break;
    case BenchmarkType.LoopSpeedTest:
        _ = BenchmarkRunner.Run<LoopSpeedTester>();
        break;
    default:
        break;
}

namespace Benchmarking
{
    public enum BenchmarkType
    {
        None,
        LargeDataTest,
        SmallDataTest,
        LoopSpeedTest
    }

    [MemoryDiagnoser]
    [SimpleJob(RuntimeMoniker.NativeAot70)]
    public class SmallDataTester
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        private ArrVector<double>[] dataA;
        private Vec3<double>[] dataB;
        private double[][] dataC;
        private List<double>[] dataD;
        private FastList<double>[] dataE;
        private readonly ArrVector<double> zero3ArrVec = ArrVector<double>.Zero(3);
        private readonly Vec3<double> zero3Vec = Vec3<double>.Zero();
        private readonly double[] zero3Arr = new double[3];
        private readonly List<double> zero3List = new() { 0, 0, 0 };
        private readonly FastList<double> zero3FList = new() { 0, 0, 0 };
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        [Params((int)1e3, (int)1e6)]
        public int N;

        [GlobalSetup]
        public void Setup()
        {
            dataA = new ArrVector<double>[N];
            dataB = new Vec3<double>[N];
            dataC = new double[N][];
            dataD = new List<double>[N];
            dataE = new FastList<double>[N];
            Random rnd = new(42);
            double a, b, c;
            for (int i = 0; i < N; i++)
            {
                a = rnd.NextDouble();
                b = rnd.NextDouble();
                c = rnd.NextDouble();
                dataA[i] = new(a, b, c);
                dataB[i] = new(a, b, c);
                dataC[i] = new double[] { a, b, c };
                dataD[i] = new List<double> { a, b, c };
                dataE[i] = new FastList<double> { a, b, c };
            }
        }
        [Benchmark]
        public Vec3<double> Vec3Test() => Loop<Vec3<double>>.Accumulate(dataB, zero3Vec, (a, b) => a + b);
        [Benchmark]
        public double[] ArrTest() => Loop<double[]>.Accumulate(dataC, zero3Arr, (a, b) => a.Composite(b, (a, b) => a + b));
        [Benchmark]
        public ArrVector<double> ArrVecTest() => Loop<ArrVector<double>>.Accumulate(dataA, zero3ArrVec, (a, b) => a + b);
        [Benchmark]
        public List<double> ListTest() => Loop<List<double>>.Accumulate(dataD, zero3List, (a, b) => a.Composite(b, (a, b) => a + b));
        [Benchmark]
        public FastList<double> FListTest() => Loop<FastList<double>>.Accumulate(dataE, zero3FList, (a, b) => a.Composite(b, (a, b) => a + b));
    }

    [MemoryDiagnoser]
    [SimpleJob(RuntimeMoniker.NativeAot70)]
    public class LargeDataTester
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        private ArrVector<double> dataA;
        private double[] dataB;
        private List<double> dataC;
        private FastList<double> dataD;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        [Params((int)1e3,(int)1e6)]
        public int N;

        [GlobalSetup]
        public void Setup()
        {
            Random rnd = new(42);
            dataA = new(N, i => rnd.Next());
            dataB = new double[N];
            dataC = new(N);
            dataD = new(N, i => rnd.Next());
            Loop.Do(N, i => { 
                double r = rnd.Next();
                dataB[i] = r;
                dataC.Add(r);
            });
        }
        [Benchmark]
        public double[] ArrTest() => dataB.Composite(dataB, (a, b) => a * b - a + b);
        [Benchmark]
        public ArrVector<double> ArrVecTest() => dataA.Composite(dataA, (a, b) => a * b - a + b);
        [Benchmark]
        public List<double> ListTest() => dataC.Composite(dataC, (a, b) => a * b - a + b);
        [Benchmark]
        public FastList<double> FListTest() => dataD.Composite(dataD, (a, b) => a * b - a + b);
    }

    [MemoryDiagnoser]
    [SimpleJob(RuntimeMoniker.NativeAot70)]
    public class LoopSpeedTester
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        private double[] dataA;
        private double[] dataB;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        private readonly Random rnd = new();
        [Params((int)1e7)]
        public int N;

        [GlobalSetup]
        public void Setup()
        {
            dataA = new double[N];
            dataB = new double[N];
            Loop.Do(N, i =>
            {
                dataA[i] = rnd.Next();
                dataB[i] = rnd.Next();
            });
        }
        public double Mutate(double input)
        {
            // implement some algorithm that takes up huge instruction count:
            input *= rnd.NextDouble();
            input /= input.GetHashCode().ToString().Length;
            input *= Math.Log(input) * Math.Sqrt(input);
            return Math.Pow(input, input.ToString().Length);
        }
        [Benchmark]
        public double[] ArrayLoopTest() // baseline
        {
            double[] newarr = new double[N];
            for (int i = 0; i < N; i++) newarr[i] = Mutate(dataA[i] * dataB[i]);
            return newarr;
        }
        [Benchmark] // hope for the best
        public double[] ArrayCompositeTest() => dataA.Composite(dataB, (a, b) => Mutate(a * b));
        [Benchmark]
        public double[] ArrayModifyInPlaceTest() // should be the fastest
        {
            for (int i = 0; i < dataA.Length; i++) dataA[i] = Mutate(dataA[i] * dataB[i]);
            return dataA;
        }
        [Benchmark] // hope for the best
        public double[] ArrayCompositeInPlaceTest()
        {
            dataA.CompositeInPlace(dataB, (a, b) => Mutate(a * b));
            return dataA;
        }

    }
}