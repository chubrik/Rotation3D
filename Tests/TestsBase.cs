namespace Rotation3D.Tests;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rotation3D.Double;
using System.Diagnostics;

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
    }

    protected static Result<TSrc, TRes> Prepare<TSrc, TRes>(
        Func<TSrc> create,
        Func<TSrc, string> srcToString,
        Func<TRes, string> resToString,
        Func<TSrc, TRes, TRes, float> compare,
        Func<TSrc, TRes> calcExact,
        Func<TSrc, TRes> calcSystem,
        Func<TSrc, TRes> calcCustom)
    {
        var sw = Stopwatch.StartNew();

        long initializeTime;
        long calcSystemTime;
        long calcCustomTime;
        long finalizeTime;
        var srcSystem = new TSrc[IterationCount];
        var resSystem = new TRes[IterationCount];
        var resCustom = new TRes[IterationCount];
        TSrc srcItem;

        for (int i = 0; i < IterationCount; i++)
            srcSystem[i] = create();

        initializeTime = sw.ElapsedMilliseconds;

        sw = Stopwatch.StartNew();
        for (int i = 0; i < IterationCount; i++)
            resSystem[i] = calcSystem(srcSystem[i]);
        calcSystemTime = sw.ElapsedMilliseconds;

        sw = Stopwatch.StartNew();
        for (int i = 0; i < IterationCount; i++)
            resCustom[i] = calcCustom(srcSystem[i]);
        calcCustomTime = sw.ElapsedMilliseconds;

        sw = Stopwatch.StartNew();

        var maxDiffSystem = 0f;
        var maxDiffCustom = 0f;

        var sumDiffSystem_D = 0.0;
        var sumDiffCustom_D = 0.0;

        TSrc maxDiffCustomSrc = srcSystem[0];
        TRes maxDiffCustomResDouble = calcExact(srcSystem[0]);
        TRes maxDiffCustomResSystem = resSystem[0];
        TRes maxDiffCustomRes = resCustom[0];
        var maxDiffCustomSystem = 0f;

        for (int i = 0; i < IterationCount; i++)
        {
            srcItem = srcSystem[i];
            var itemDouble = calcExact(srcItem);
            var diffSystem = compare(srcItem, itemDouble, resSystem[i]);
            var diffCustom = compare(srcItem, itemDouble, resCustom[i]);

            sumDiffSystem_D += diffSystem;
            sumDiffCustom_D += diffCustom;

            if (maxDiffSystem < diffSystem)
                maxDiffSystem = diffSystem;

            if (maxDiffCustom < diffCustom)
            {
                maxDiffCustom = diffCustom;
                maxDiffCustomSrc = srcSystem[i];
                maxDiffCustomResDouble = itemDouble;
                maxDiffCustomResSystem = resSystem[i];
                maxDiffCustomRes = resCustom[i];
                maxDiffCustomSystem = diffSystem;
            }
        }

        var avgDiffSystem = (float)(sumDiffSystem_D / IterationCount);
        var avgDiffCustom = (float)(sumDiffCustom_D / IterationCount);

        finalizeTime = sw.ElapsedMilliseconds;

        var calculationTime = calcSystemTime + calcCustomTime;
        Console.WriteLine($"Calculation: {calculationTime,4} ms    Iterations:  {IterationCount:N}");
        Console.WriteLine($"Calc system: {calcSystemTime,4} ms    Initialize:  {initializeTime,4} ms");
        Console.WriteLine($"Calc custom: {calcCustomTime,4} ms    Finalize:    {finalizeTime,4} ms");
        Console.WriteLine();

        Console.WriteLine($"System diff:  max: {maxDiffSystem.Stringify(","),-13}  avg: {avgDiffSystem.Stringify()}");
        Console.WriteLine($"Custom diff:  max: {maxDiffCustom.Stringify(","),-13}  avg: {avgDiffCustom.Stringify()}");
        Console.WriteLine();

        Console.WriteLine("== Worst for custom ==");
        Console.WriteLine($"Source:  {"",12}   {srcToString(maxDiffCustomSrc)}");
        Console.WriteLine($"Double:  {0,-12}   {resToString(maxDiffCustomResDouble)}");
        Console.WriteLine($"System:  {maxDiffCustomSystem.Stringify()}   {resToString(maxDiffCustomResSystem)}");
        Console.WriteLine($"Custom:  {maxDiffCustom.Stringify()}   {resToString(maxDiffCustomRes)}");

        return new Result<TSrc, TRes>(
            avgDiffSystem: avgDiffSystem,
            maxDiffSystem: maxDiffSystem,
            avgDiffCustom: avgDiffCustom,
            maxDiffCustom: maxDiffCustom
        );
    }

    protected static Result<TSrc, TRes> Prepare<TSrc, TRes>(
        Func<TSrc> create,
        Func<TSrc, string> srcToString,
        Func<TRes, string> resToString,
        Func<TSrc, TRes, float> compare,
        Func<TSrc, TRes> calcCustom)
    {
        var sw = Stopwatch.StartNew();

        long initializeTime;
        long calcCustomTime;
        long finalizeTime;
        var srcSystem = new TSrc[IterationCount];
        var resCustom = new TRes[IterationCount];
        TSrc srcItem;

        for (int i = 0; i < IterationCount; i++)
            srcSystem[i] = create();

        initializeTime = sw.ElapsedMilliseconds;

        sw = Stopwatch.StartNew();
        for (int i = 0; i < IterationCount; i++)
            resCustom[i] = calcCustom(srcSystem[i]);
        calcCustomTime = sw.ElapsedMilliseconds;

        sw = Stopwatch.StartNew();

        var sumDiffCustom = 0f;
        var maxDiffCustom = 0f;

        TSrc maxDiffCustomSrc = srcSystem[0];
        TRes maxDiffCustomRes = resCustom[0];

        for (int i = 0; i < IterationCount; i++)
        {
            srcItem = srcSystem[i];
            var diffCustom = compare(srcItem, resCustom[i]);

            sumDiffCustom += diffCustom;

            if (maxDiffCustom < diffCustom)
            {
                maxDiffCustom = diffCustom;
                maxDiffCustomSrc = srcSystem[i];
                maxDiffCustomRes = resCustom[i];
            }
        }

        var avgDiffCustom = sumDiffCustom / IterationCount;

        finalizeTime = sw.ElapsedMilliseconds;

        Console.WriteLine($"                        Iterations:  {IterationCount:N}");
        Console.WriteLine($"                        Initialize:  {initializeTime,4} ms");
        Console.WriteLine($"Calc custom: {calcCustomTime,4} ms    Finalize:    {finalizeTime,4} ms");
        Console.WriteLine();

        Console.WriteLine($"Custom diff:  max: {maxDiffCustom.Stringify(","),-13}  avg: {avgDiffCustom.Stringify()}");
        Console.WriteLine();

        Console.WriteLine("== Worst for custom ==");
        Console.WriteLine($"Source:  {"",12}   {srcToString(maxDiffCustomSrc)}");
        Console.WriteLine($"Custom:  {maxDiffCustom.Stringify()}   {resToString(maxDiffCustomRes)}");

        return new Result<TSrc, TRes>(
            avgDiffSystem: float.NaN,
            maxDiffSystem: float.NaN,
            avgDiffCustom: avgDiffCustom,
            maxDiffCustom: maxDiffCustom
        );
    }

    protected sealed class Result<TSrc, TRes>
    {
        public float AvgDiffSystem { get; }
        public float MaxDiffSystem { get; }
        public float AvgDiffCustom { get; }
        public float MaxDiffCustom { get; }

        public Result(
            float avgDiffSystem,
            float maxDiffSystem,
            float avgDiffCustom,
            float maxDiffCustom)
        {
            AvgDiffSystem = avgDiffSystem;
            MaxDiffSystem = maxDiffSystem;
            AvgDiffCustom = avgDiffCustom;
            MaxDiffCustom = maxDiffCustom;
        }
    }
}
