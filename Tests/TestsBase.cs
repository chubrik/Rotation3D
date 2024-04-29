namespace Rotation3D.Tests;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rotation3D.Double;
using System.Diagnostics;
using System.Numerics;

public abstract class TestsBase
{
    protected const int IterationCount = Constants.TestIterationCount;

    private static bool _isFirstInitialize = true;

    [TestInitialize]
    public void Initialize()
    {
        if (_isFirstInitialize)
        {
            _isFirstInitialize = false;
            DoubleConstants.SelfCheck();
            Constants.SelfCheck();
        }

        GC.Collect();
        GC.WaitForPendingFinalizers();
    }

    protected static Result<TSrc, TTest> Test<TSrc, TTest>(
        Func<TSrc> createSrc,
        Func<TSrc, TTest, double> compare,
        Func<TSrc, TTest> calcTest)
    {
        var sw = Stopwatch.StartNew();

        long initializeTime;
        long calcCustomTime;
        long finalizeTime;
        var listSrc = new TSrc[IterationCount];
        var listTest = new TTest[IterationCount];

        for (int i = 0; i < IterationCount; i++)
            listSrc[i] = createSrc();

        initializeTime = sw.ElapsedMilliseconds;

        sw = Stopwatch.StartNew();
        for (int i = 0; i < IterationCount; i++)
            listTest[i] = calcTest(listSrc[i]);
        calcCustomTime = sw.ElapsedMilliseconds;

        sw = Stopwatch.StartNew();

        var sumDiff_D = 0.0;
        var maxDiff_D = 0.0;

        TSrc maxDiffSrc = listSrc[0];
        TTest maxDiffRes = listTest[0];

        for (int i = 0; i < IterationCount; i++)
        {
            var srcItem = listSrc[i];
            var diff_D = compare(srcItem, listTest[i]);

            sumDiff_D += diff_D;

            if (maxDiff_D < diff_D)
            {
                maxDiff_D = diff_D;
                maxDiffSrc = listSrc[i];
                maxDiffRes = listTest[i];
            }
        }

        var avgDiffCustom = (float)(sumDiff_D / IterationCount);

        finalizeTime = sw.ElapsedMilliseconds;

        Console.WriteLine($"                 Iterations:  {IterationCount:N}");
        Console.WriteLine($"                 Initialize:  {initializeTime,4} ms");
        Console.WriteLine($"Calc: {calcCustomTime,4} ms    Finalize:    {finalizeTime,4} ms");
        Console.WriteLine();

        Console.WriteLine($"Diff:  max: {((float)maxDiff_D).Stringify(","),-13}  avg: {avgDiffCustom.Stringify()}");
        Console.WriteLine();

        Console.WriteLine("== Worst for test ==");
        Console.WriteLine($"Source:  {"",12}   {Stringify(maxDiffSrc)}");
        Console.WriteLine($"Test:    {((float)maxDiff_D).Stringify()}   {Stringify(maxDiffRes)}");

        return new Result<TSrc, TTest>(
            avgDiff: avgDiffCustom,
            maxDiff: (float)maxDiff_D
        );
    }

    protected static ResultAB<TSrc, TTest> TestAB<TSrc, TTest, TExact>(
        Func<TSrc> createSrc,
        Func<TExact, TTest, double> compare,
        Func<TSrc, TExact> calcExact,
        Func<TSrc, TTest> calcTestA,
        Func<TSrc, TTest> calcTestB)
    {
        var sw = Stopwatch.StartNew();

        long initializeTime;
        long calcATime;
        long calcBTime;
        long finalizeTime;
        var listSrc = new TSrc[IterationCount];
        var listTestA = new TTest[IterationCount];
        var listTestB = new TTest[IterationCount];

        for (int i = 0; i < IterationCount; i++)
            listSrc[i] = createSrc();

        initializeTime = sw.ElapsedMilliseconds;

        sw = Stopwatch.StartNew();
        for (int i = 0; i < IterationCount; i++)
            listTestA[i] = calcTestA(listSrc[i]);
        calcATime = sw.ElapsedMilliseconds;

        sw = Stopwatch.StartNew();
        for (int i = 0; i < IterationCount; i++)
            listTestB[i] = calcTestB(listSrc[i]);
        calcBTime = sw.ElapsedMilliseconds;

        sw = Stopwatch.StartNew();

        var maxDiffA_D = 0.0;
        var maxDiffB_D = 0.0;

        var sumDiffA_D = 0.0;
        var sumDiffB_D = 0.0;

        TSrc maxDiffBSrc = listSrc[0];
        TExact maxDiffBResExact = calcExact(listSrc[0]);
        TTest maxDiffBResA = listTestA[0];
        TTest maxDiffBRes = listTestB[0];
        var maxDiffBResAValue_D = 0.0;

        for (int i = 0; i < IterationCount; i++)
        {
            var srcItem = listSrc[i];
            var itemExact = calcExact(srcItem);
            var diffA_D = compare(itemExact, listTestA[i]);
            var diffB_D = compare(itemExact, listTestB[i]);

            sumDiffA_D += diffA_D;
            sumDiffB_D += diffB_D;

            if (maxDiffA_D < diffA_D)
                maxDiffA_D = diffA_D;

            if (maxDiffB_D < diffB_D)
            {
                maxDiffB_D = diffB_D;
                maxDiffBSrc = listSrc[i];
                maxDiffBResExact = itemExact;
                maxDiffBResA = listTestA[i];
                maxDiffBRes = listTestB[i];
                maxDiffBResAValue_D = diffA_D;
            }
        }

        var avgDiffA = (float)(sumDiffA_D / IterationCount);
        var avgDiffB = (float)(sumDiffB_D / IterationCount);

        finalizeTime = sw.ElapsedMilliseconds;

        var calculationTime = calcATime + calcBTime;
        Console.WriteLine($"Calc:    {calculationTime,4} ms    Iterations:  {IterationCount:N}");
        Console.WriteLine($"Calc A:  {calcATime,4} ms    Initialize:  {initializeTime,4} ms");
        Console.WriteLine($"Calc B:  {calcBTime,4} ms    Finalize:    {finalizeTime,4} ms");
        Console.WriteLine();

        Console.WriteLine($"Diff A:  max: {((float)maxDiffA_D).Stringify(","),-13}  avg: {avgDiffA.Stringify()}");
        Console.WriteLine($"Diff B:  max: {((float)maxDiffB_D).Stringify(","),-13}  avg: {avgDiffB.Stringify()}");
        Console.WriteLine();

        Console.WriteLine("== Worst for test B ==");
        Console.WriteLine($"Source:  {"",12}   {Stringify(maxDiffBSrc)}");
        Console.WriteLine($"Exact:   {0,-12}   {Stringify(maxDiffBResExact)}");
        Console.WriteLine($"Test A:  {((float)maxDiffBResAValue_D).Stringify()}   {Stringify(maxDiffBResA)}");
        Console.WriteLine($"Test B:  {((float)maxDiffB_D).Stringify()}   {Stringify(maxDiffBRes)}");

        return new ResultAB<TSrc, TTest>(
            avgDiffA: avgDiffA,
            maxDiffA: (float)maxDiffA_D,
            avgDiffB: avgDiffB,
            maxDiffB: (float)maxDiffB_D
        );
    }

    private static string Stringify(object? obj)
    {
        if (obj is AxisAngle axisAngle)
            return axisAngle.Stringify();

        if (obj is EulerAngles eulerAngles)
            return eulerAngles.Stringify();

        if (obj is Matrix4x4 matrix)
            return matrix.Stringify();

        if (obj is Quaternion quaternion)
            return quaternion.Stringify();

        if (obj is DoubleAxisAngle doubleAxisAngle)
            return doubleAxisAngle.ToSystem().Stringify();

        if (obj is DoubleEulerAngles doubleEulerAngles)
            return doubleEulerAngles.ToSystem().Stringify();

        if (obj is DoubleMatrix4x4 doubleMatrix)
            return doubleMatrix.ToSystem().Stringify();

        if (obj is DoubleQuaternion doubleQuaternion)
            return doubleQuaternion.ToSystem().Stringify();

        throw new InvalidOperationException();
    }

    protected sealed class Result<TSrc, TRes>
    {
        public float AvgDiff { get; }
        public float MaxDiff { get; }

        public Result(
            float avgDiff,
            float maxDiff)
        {
            AvgDiff = avgDiff;
            MaxDiff = maxDiff;
        }
    }

    protected sealed class ResultAB<TSrc, TRes>
    {
        public float AvgDiffA { get; }
        public float MaxDiffA { get; }
        public float AvgDiffB { get; }
        public float MaxDiffB { get; }

        public ResultAB(
            float avgDiffA,
            float maxDiffA,
            float avgDiffB,
            float maxDiffB)
        {
            AvgDiffA = avgDiffA;
            MaxDiffA = maxDiffA;
            AvgDiffB = avgDiffB;
            MaxDiffB = maxDiffB;
        }
    }
}
