﻿namespace Rotation3D.Tests;

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

    protected static Result<TSrc, TRes> Prepare<TSrc, TSrcDouble, TRes, TResDouble>(
        Func<TSrc> create,
        Func<TSrc, TSrcDouble> toDouble,
        Func<TResDouble, TRes> fromDouble,
        Func<TSrc, TRes, TRes, float> compare,
        Func<TSrc, string> srcToString,
        Func<TRes, string> resToString,
        Func<TSrcDouble, TResDouble> calcDouble,
        Func<TSrc, TRes> calcSystem,
        Func<TSrc, TRes> calcCustom)
    {
        var sw = Stopwatch.StartNew();

        long initializeTime;
        long calcDoubleTime;
        long calcSystemTime;
        long calcCustomTime;
        long finalizeTime;
        var srcSystem = new TSrc[IterationCount];
        var srcDouble = new TSrcDouble[IterationCount];
        var resDouble = new TResDouble[IterationCount];
        var resSystem = new TRes[IterationCount];
        var resCustom = new TRes[IterationCount];
        TSrc srcItem;

        for (int i = 0; i < IterationCount; i++)
        {
            srcItem = create();
            srcSystem[i] = srcItem;
            srcDouble[i] = toDouble(srcItem);
        }

        initializeTime = sw.ElapsedMilliseconds;

        sw = Stopwatch.StartNew();
        for (int i = 0; i < IterationCount; i++)
            resDouble[i] = calcDouble(srcDouble[i]);
        calcDoubleTime = sw.ElapsedMilliseconds;

        sw = Stopwatch.StartNew();
        for (int i = 0; i < IterationCount; i++)
            resSystem[i] = calcSystem(srcSystem[i]);
        calcSystemTime = sw.ElapsedMilliseconds;

        sw = Stopwatch.StartNew();
        for (int i = 0; i < IterationCount; i++)
            resCustom[i] = calcCustom(srcSystem[i]);
        calcCustomTime = sw.ElapsedMilliseconds;

        sw = Stopwatch.StartNew();

        var sumDiffSystem = 0f;
        var sumDiffCustom = 0f;

        var maxDiffSystem = 0f;
        var maxDiffCustom = 0f;

        TSrc maxDiffCustomSrc = srcSystem[0];
        TRes maxDiffCustomResDouble = fromDouble(resDouble[0]);
        TRes maxDiffCustomResSystem = resSystem[0];
        TRes maxDiffCustomRes = resCustom[0];
        var maxDiffCustomSystem = 0f;

        for (int i = 0; i < IterationCount; i++)
        {
            srcItem = srcSystem[i];
            var itemDouble = fromDouble(resDouble[i]);
            var diffSystem = compare(srcItem, itemDouble, resSystem[i]);
            var diffCustom = compare(srcItem, itemDouble, resCustom[i]);

            sumDiffSystem += diffSystem;
            sumDiffCustom += diffCustom;

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

        var avgDiffSystem = sumDiffSystem / IterationCount;
        var avgDiffCustom = sumDiffCustom / IterationCount;

        finalizeTime = sw.ElapsedMilliseconds;

        var calculationTime = calcDoubleTime + calcSystemTime + calcCustomTime;
        Console.WriteLine($"Calc double: {calcDoubleTime,4} ms    Iterations:   {IterationCount:N}");
        Console.WriteLine($"Calc system: {calcSystemTime,4} ms    Initialize:   {initializeTime,4} ms");
        Console.WriteLine($"Calc custom: {calcCustomTime,4} ms    Calculation:  {calculationTime,4} ms");
        Console.WriteLine($"                        Finalize:     {finalizeTime,4} ms");
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
            avgDiffCustom: avgDiffCustom,
            maxDiffSystem: maxDiffSystem,
            maxDiffCustom: maxDiffCustom
        );
    }

    protected sealed class Result<TSrc, TRes>
    {
        public float AvgDiffSystem { get; }
        public float AvgDiffCustom { get; }
        public float MaxDiffSystem { get; }
        public float MaxDiffCustom { get; }

        public Result(
            float avgDiffSystem,
            float avgDiffCustom,
            float maxDiffSystem,
            float maxDiffCustom)
        {
            AvgDiffSystem = avgDiffSystem;
            AvgDiffCustom = avgDiffCustom;
            MaxDiffSystem = maxDiffSystem;
            MaxDiffCustom = maxDiffCustom;
        }
    }
}
