﻿using FluentAssertions;
using ScottPlot.DataSources;

namespace ScottPlotTests.UnitTests;

internal class IDataSourceTests
{
    // Data that tests should use when creating the IDataSource

    #region < Setup >

    public static readonly double[] Xs = Generate.Consecutive(51, Period, 0);
    public static readonly double[] Ys = Generate.Sin(51);
    public static readonly Coordinates[] Cs = Xs.Zip(Ys).Select(pair => new Coordinates(pair.First, pair.Second)).ToArray();
    
    public static readonly int[] GenericXs = Xs.Select(i => (int)i).ToArray();
    public static readonly float[] GenericYs = Ys.Select(i => (float)i).ToArray();

    public const int PlotWidth = 600;
    public const int PlotHeight = 400;

    public const double Period = 1;
    public const int IndexToCheck = 20;
    public static readonly int Length = Xs.Length;
    public static readonly double XOffSet = (double)Random.Shared.Next(1, 50);
    public static readonly double YOffSet = (double)Random.Shared.Next(51, 100);
    public static readonly double XScaling = 3;
    public static readonly double YScaling = 3;

    public static readonly Coordinates MouseLocation = new(Period * IndexToCheck, Ys[IndexToCheck]);

    private static double GetValidScaling()
    {
        double i = Random.Shared.NextDouble();
        while (i is 0d or 1d) 
        {
            i = Random.Shared.NextDouble();
        }
        return i * 3;
    }

    private static Plot GetPlot()
    {
        Plot plot = new Plot();
        plot.Axes.Top.IsVisible = false;
        plot.Axes.Right.IsVisible = false;
        plot.Axes.Bottom.IsVisible = false;
        plot.Axes.Left.IsVisible = false;
        plot.HideLegend();
        return plot;
    }

    #endregion

    #region < GetNearest | GetNearestFast >

    [Test]
    public void Test_IsAscending()
    {
        Assert.That(Xs.IsAscending(null), Is.True);
        Assert.That(Ys.IsAscending(null), Is.False);
    }

    [TestCase(false)]
    [TestCase(true)]
    [Test]
    public void Test_DataSourceUtility_GetNearest(bool ignoreOffsetsAndScaling)
    {
        Plot plot = new Plot();
        var dataSource = ignoreOffsetsAndScaling
            ? new SignalXYSourceDoubleArray(Xs, Ys)
            : new SignalXYSourceDoubleArray(Xs, Ys)
            {
                XOffset = XOffSet,
                YOffset = YOffSet,
                XScale = XScaling,
                YScale = YScaling
            };
        plot.Add.SignalXY(dataSource);
        plot.RenderInMemory(PlotWidth, PlotHeight);
        var mouseLoc = ignoreOffsetsAndScaling ? MouseLocation : DataSourceUtilities.ScaleCoordinate(MouseLocation, XScaling, XOffSet, YScaling, YOffSet);

        var nearest = DataSourceUtilities.GetNearest(dataSource, mouseLoc, plot.LastRender, 100);
        Assert.That(nearest.Index, Is.EqualTo(IndexToCheck));
        nearest.X.Should().Be(mouseLoc.X);
        nearest.Y.Should().BeApproximately(mouseLoc.Y, .001);
    }

    [TestCase(false)]
    [TestCase(true)]
    [Test]
    public void Test_DataSourceUtility_GetNearestFast(bool ignoreOffsetsAndScaling)
    {
        Plot plot = new Plot();
        var dataSource = ignoreOffsetsAndScaling 
            ? new SignalXYSourceDoubleArray(Xs, Ys)
            : new SignalXYSourceDoubleArray(Xs, Ys)
            {
                XOffset = XOffSet,
                YOffset = YOffSet,
                XScale = XScaling,
                YScale = YScaling
            };
        var signal = plot.Add.SignalXY(dataSource);
        plot.RenderInMemory(PlotWidth, PlotHeight);
        var mouseLoc = ignoreOffsetsAndScaling ? MouseLocation : DataSourceUtilities.ScaleCoordinate(MouseLocation, XScaling, XOffSet, YScaling, YOffSet);

        var nearest = DataSourceUtilities.GetNearestFast(dataSource, mouseLoc, plot.LastRender, 100);
        Assert.That(nearest.Index, Is.EqualTo(IndexToCheck));
        nearest.X.Should().Be(mouseLoc.X);
        nearest.Y.Should().BeApproximately(mouseLoc.Y, .001);

        //Native function should also be using this method
        nearest = signal.GetNearest(mouseLoc, plot.LastRender, 100);
        Assert.That(nearest.Index, Is.EqualTo(IndexToCheck));
        nearest.X.Should().Be(mouseLoc.X);
        nearest.Y.Should().BeApproximately(mouseLoc.Y, .001);
    }

    #endregion

    #region #region < GetNearestX | GetNearestXFast >

    [TestCase(false)]
    [TestCase(true)]
    [Test]
    public void Test_DataSourceUtility_GetNearestX(bool ignoreOffsetsAndScaling)
    {
        Plot plot = new Plot();
        var dataSource = ignoreOffsetsAndScaling
            ? new SignalXYSourceDoubleArray(Xs, Ys)
            : new SignalXYSourceDoubleArray(Xs, Ys)
            {
                XOffset = XOffSet,
                YOffset = YOffSet,
                XScale = XScaling,
                YScale = YScaling
            };
        plot.Add.SignalXY(dataSource);
        plot.RenderInMemory(PlotWidth, PlotHeight);
        var mouseLoc = ignoreOffsetsAndScaling ? MouseLocation : DataSourceUtilities.ScaleCoordinate(MouseLocation, XScaling, XOffSet, YScaling, YOffSet);

        var nearest = DataSourceUtilities.GetNearestX(dataSource, mouseLoc, plot.LastRender, 100);
        Assert.That(nearest.Index, Is.EqualTo(IndexToCheck));
        nearest.X.Should().Be(mouseLoc.X);
        nearest.Y.Should().BeApproximately(mouseLoc.Y, .001);
    }

    [TestCase(false)]
    [TestCase(true)]
    [Test]
    public void Test_DataSourceUtility_GetNearestXFast(bool ignoreOffsetsAndScaling)
    {
        Plot plot = new Plot();
        var dataSource = ignoreOffsetsAndScaling
            ? new SignalXYSourceDoubleArray(Xs, Ys)
            : new SignalXYSourceDoubleArray(Xs, Ys)
            {
                XOffset = XOffSet,
                YOffset = YOffSet,
                XScale = XScaling,
                YScale = YScaling
            };
        var signal = plot.Add.SignalXY(dataSource);
        plot.RenderInMemory(PlotWidth, PlotHeight);
        var mouseLoc = ignoreOffsetsAndScaling ? MouseLocation : DataSourceUtilities.ScaleCoordinate(MouseLocation, XScaling, XOffSet, YScaling, YOffSet);

        var nearest = DataSourceUtilities.GetNearestXFast(dataSource, mouseLoc, plot.LastRender, 100);
        Assert.That(nearest.Index, Is.EqualTo(IndexToCheck));
        nearest.X.Should().Be(mouseLoc.X);
        nearest.Y.Should().BeApproximately(mouseLoc.Y, .001);

        //Native function should also be using this method
        nearest = signal.GetNearestX(mouseLoc, plot.LastRender, 100);
        Assert.That(nearest.Index, Is.EqualTo(IndexToCheck));
        nearest.X.Should().Be(mouseLoc.X);
        nearest.Y.Should().BeApproximately(mouseLoc.Y, .001);
    }

    #endregion

    [TestCase(false)]
    [TestCase(true)]
    [Test]
    public void Test_CoordinateDataSource(bool scaling)
    {
        // Test IDataSource
        var ds = new CoordinateDataSource(Cs);
        if (scaling)
        {
            ds.YOffset = YOffSet; 
            ds.XOffset = XOffSet; 
            ds.XScale = XScaling; 
            ds.YScale = YScaling;
        }
        Test_IDataSource(ds, scaling ? DataSourceUtilities.ScaleCoordinate(MouseLocation, XScaling, XOffSet, YScaling, YOffSet) : MouseLocation);
    }

    [TestCase(true)][TestCase(false)][Test] public void Test_SignalSourceDouble(bool scaling) => TestSignal(new SignalSourceDouble(Ys, Period), scaling);
    [TestCase(true)][TestCase(false)][Test] public void Test_SignalSourceGenericArray(bool scaling) => TestSignal(new SignalSourceGenericArray<float>(GenericYs, Period), scaling);
    [TestCase(true)][TestCase(false)][Test] public void Test_SignalSourceGenericList(bool scaling) => TestSignal(new SignalSourceGenericList<float>(GenericYs, Period), scaling);
    [TestCase(true)][TestCase(false)][Test] public void Test_FastSignalSourceDouble(bool scaling) => TestSignal(new FastSignalSourceDouble(Ys, Period), scaling);

    private static void TestSignal<T>(T signalSource, bool scaling) where T : ISignalSource, IDataSource
    {
        var mouse = MouseLocation;
        if (scaling)
        {
            signalSource.XOffset = XOffSet;
            signalSource.YOffset = YOffSet;
            signalSource.YScale = YScaling;
            mouse = DataSourceUtilities.ScaleCoordinate(MouseLocation, 1, XOffSet, YScaling, YOffSet);
        }
        Console.Write($" - Testing {signalSource.GetType().Name} as {nameof(IDataSource)} ");
        Test_IDataSource(signalSource, mouse);
        Console.WriteLine($"- Passed");

        var plot = GetPlot();

        var signal = plot.Add.Signal(signalSource);
        plot.RenderInMemory(PlotWidth, PlotHeight);

        if (signalSource is IGetNearest gn)
        {
            Console.Write($" - Testing {signalSource.GetType().Name} as {nameof(IGetNearest)} ");
            Test_IGetNearest(gn, plot, mouse);
            Console.WriteLine($"- Passed");
        }

        if (signal is IDataSource ds)
        {
            Console.Write($" - Testing {signal.GetType().Name} as {nameof(IDataSource)} ");
            Test_IDataSource(ds, mouse);
            Console.WriteLine($"- Passed");
        }

        Console.Write($" - Testing Signal as {nameof(IGetNearest)}");
        Test_IGetNearest(signal, plot, mouse);
        Console.WriteLine($"- Passed");
    }


    [TestCase(false, false)]
    [TestCase(false, true)]
    [TestCase(true, true)]    
    [TestCase(true, false)]
    [Test] 
    public void Test_SignalXYSourceDoubleArray(bool rotation, bool scaling)
        => TestSignalXY(new SignalXYSourceDoubleArray(Xs, Ys) { Rotated = rotation }, rotation, scaling);

    [TestCase(false, false)]
    [TestCase(false, true)]
    [TestCase(true, true)]
    [TestCase(true, false)]
    [Test] public void Test_SignalXYSourceGenericArray(bool rotation, bool scaling) 
        => TestSignalXY(new SignalXYSourceGenericArray<int, float>(GenericXs, GenericYs) { Rotated = rotation}, rotation, scaling);

    private static void TestSignalXY<T>(T signalXYSource, bool rotation, bool scaling) where T: ISignalXYSource, IDataSource
    {
        var mouse = MouseLocation;
        if (scaling)
        {
            signalXYSource.XOffset = XOffSet;
            signalXYSource.YOffset = YOffSet;
            signalXYSource.XScale = XScaling;
            signalXYSource.YScale = YScaling;
            mouse = DataSourceUtilities.ScaleCoordinate(MouseLocation, XScaling, XOffSet, YScaling, YOffSet);
        }
        if (rotation) mouse = new Coordinates(mouse.Y, mouse.X);

        Console.Write($" - Testing {signalXYSource.GetType().Name} as {nameof(IDataSource)} ");
        Test_IDataSource(signalXYSource, mouse, rotation);
        Console.WriteLine($"- Passed");

        var plot = GetPlot();

        var signal = plot.Add.SignalXY(signalXYSource);

        plot.RenderInMemory(PlotWidth, PlotHeight);

        if (signalXYSource is IGetNearest gn)
        {
            Console.Write($" - Testing {signalXYSource.GetType().Name} as {nameof(IGetNearest)} ");
            Test_IGetNearest(gn, plot, mouse, rotation);
            Console.WriteLine($"- Passed");
        }

        if (signal is IDataSource ds)
        {
            Console.Write($" - Testing {signal.GetType().Name} as {nameof(IDataSource)} ");
            Test_IDataSource(ds, mouse, rotation);
            Console.WriteLine($"- Passed");
        }

        Console.Write($" - Testing SignalXY as {nameof(IGetNearest)}");
        Test_IGetNearest(signal, plot, mouse, rotation);
        Console.WriteLine($"- Passed");
    }

    [TestCase(true)][TestCase(false)][Test] public void Test_ScatterSourceDoubleArray(bool scaling) => TestScatter(new ScatterSourceDoubleArray(Xs, Ys), scaling);
    [TestCase(true)][TestCase(false)][Test] public void Test_ScatterSourceGenericArray(bool scaling) => TestScatter(new ScatterSourceGenericArray<int, float>(GenericXs, GenericYs), scaling);
    [TestCase(true)][TestCase(false)][Test] public void Test_ScatterSourceGenericList(bool scaling) => TestScatter(new ScatterSourceGenericList<int, float>(GenericXs.ToList(), GenericYs.ToList()), scaling);
    [TestCase(true)][TestCase(false)][Test] public void Test_ScatterSourceCoordinatesArray(bool scaling) => TestScatter(new ScatterSourceCoordinatesArray(Cs), scaling);
    [TestCase(true)][TestCase(false)][Test] public void Test_ScatterSourceCoordinatesList(bool scaling) => TestScatter(new ScatterSourceCoordinatesList(Cs.ToList()), scaling);

    private static void TestScatter<T>(T scatterSource, bool scaling) where T : IScatterSource, IDataSource
    {
        Console.Write($" - Testing {scatterSource.GetType().Name} as {nameof(IDataSource)} ");
        var mouse = MouseLocation;
        Test_IDataSource(scatterSource, mouse);
        Console.WriteLine($"- Passed");

        var plot = GetPlot();

        var scatter = plot.Add.Scatter(scatterSource);
        
        if (scaling)
        {
            scatter.OffsetX = XOffSet;
            scatter.OffsetY = YOffSet;
            scatter.ScaleX = XScaling;
            scatter.ScaleY = YScaling;
            mouse = DataSourceUtilities.ScaleCoordinate(MouseLocation, XScaling, XOffSet, YScaling, YOffSet);
        }

        Console.Write($" - Testing Scatter as {nameof(IDataSource)}");
        Test_IDataSource(scatter, mouse);
        Console.WriteLine($"- Passed");

        plot.RenderInMemory(PlotWidth, PlotHeight);

        if (scatterSource is IGetNearest gn)
        {
            Console.Write($" - Testing {scatterSource.GetType().Name} as {nameof(IGetNearest)} ");
            Test_IGetNearest(gn, plot, MouseLocation); // unscaled mouse location is required because Scatter has scaling details, IScatterSource does not
            Console.WriteLine($"- Passed");
        }

        Console.Write($" - Testing Scatter as {nameof(IGetNearest)}");
        Test_IGetNearest(scatter, plot, mouse);
        Console.WriteLine($"- Passed");
    }

    private static void Test_IDataSource(IDataSource dataSource, Coordinates mouseLocation, bool isRotated = false)
    {
        Assert.That(dataSource, Is.Not.Null);
        
        // If using the supplied dataset, all are sorted!
        Assert.That(dataSource.IsSorted, Is.True);

        double expectedX = isRotated ? Cs[IndexToCheck].Y : Cs[IndexToCheck].X;
        double expectedY = isRotated ? Cs[IndexToCheck].X : Cs[IndexToCheck].Y;

        // Get Coordinate
        var coordinate = dataSource.GetCoordinate(IndexToCheck);
        coordinate.X.Should().BeApproximately(expectedX, .001, $"{nameof(IDataSource)}.{nameof(IDataSource.GetCoordinate)} returned a different X result than coordinate at same index");
        coordinate.Y.Should().BeApproximately(expectedY, .001, $"{nameof(IDataSource)}.{nameof(IDataSource.GetCoordinate)} returned a different Y result than coordinate at same index");

        // Get XY
        Assert.That(dataSource.GetX(IndexToCheck), Is.EqualTo(coordinate.X), $"{nameof(IDataSource)}.{nameof(IDataSource.GetX)} returned a different result than coordinate at same index");
        Assert.That(dataSource.GetY(IndexToCheck), Is.EqualTo(coordinate.Y), $"{nameof(IDataSource)}.{nameof(IDataSource.GetY)} returned a different result than coordinate at same index");

        // GetXY Scaled
        var scaledCoordinate = dataSource.GetCoordinateScaled(IndexToCheck); // cannot check that scaling occurred, only that all values return same result
        Assert.That( dataSource.GetXScaled(IndexToCheck), Is.EqualTo(scaledCoordinate.X), $"{nameof(IDataSource)}.{nameof(IDataSource.GetXScaled)} returned a different result than the scaled coordinate at same index");
        Assert.That(dataSource.GetYScaled(IndexToCheck), Is.EqualTo(scaledCoordinate.Y), $"{nameof(IDataSource)}.{nameof(IDataSource.GetYScaled)} returned a different result than scaled coordinate at same index");

        // GetClosestIndex
        Assert.That(dataSource.GetXClosestIndex(mouseLocation), Is.EqualTo(IndexToCheck), $"{nameof(IDataSource)}.{nameof(IDataSource.GetXClosestIndex)} returned an unexpected value");
    }

    private static void Test_IGetNearest(IGetNearest dataSource, Plot plot, Coordinates mouseLocation, bool isRotated = false)
    {
        Assert.That(dataSource, Is.Not.Null);
        Assert.That(plot, Is.Not.Null);

        float maxDistance = 100;

        var nearestX = dataSource.GetNearestX(mouseLocation, plot.LastRender, maxDistance);
        Assert.That(nearestX.Index, Is.EqualTo(IndexToCheck), $"{dataSource.GetType().Name} failed to get correct index using {nameof(IGetNearest)}.{nameof(IGetNearest.GetNearestX)}");
        nearestX.X.Should().BeApproximately(mouseLocation.X, .001);
        nearestX.Y.Should().BeApproximately(mouseLocation.Y, .001);
        
        var nearest = dataSource.GetNearest(mouseLocation, plot.LastRender, maxDistance);
        Assert.That(nearest.Index, Is.EqualTo(IndexToCheck), $"{dataSource.GetType().Name} failed to get correct index using {nameof(IGetNearest)}.{nameof(IGetNearest.GetNearest)}");
        nearest.X.Should().BeApproximately(mouseLocation.X, .001);
        nearest.Y.Should().BeApproximately(mouseLocation.Y, .001);

    }

    
}

