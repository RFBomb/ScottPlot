﻿namespace ScottPlot.DataSources;

/// <summary>
/// This data source manages X/Y points as a collection of coordinates
/// </summary>
public class ScatterSourceCoordinatesArray(Coordinates[] coordinates) : IScatterSource, IDataSource, IGetNearest
{
    private readonly Coordinates[] Coordinates = coordinates;
    private bool? isSorted;

    public int MinRenderIndex { get; set; } = 0;
    public int MaxRenderIndex { get; set; } = coordinates.Length - 1;

    bool IDataSource.PreferCoordinates => true;
    int IDataSource.Length => Coordinates.Length;
    bool IDataSource.IsSorted => isSorted ??= Coordinates.IsAscending(BinarySearchComparer.Instance);

    public IReadOnlyList<Coordinates> GetScatterPoints()
    {
        return Coordinates
            .Skip(MinRenderIndex)
            .Take(this.GetRenderIndexCount())
            .ToList();
    }

    public AxisLimits GetLimits()
    {
        ExpandingAxisLimits limits = new();
        limits
            .Expand(Coordinates
            .Skip(MinRenderIndex)
            .Take(this.GetRenderIndexCount()));
        return limits.AxisLimits;
    }

    public CoordinateRange GetLimitsX()
    {
        return GetLimits().Rect.XRange;
    }

    public CoordinateRange GetLimitsY()
    {
        return GetLimits().Rect.YRange;
    }

    public DataPoint GetNearest(Coordinates mouseLocation, RenderDetails renderInfo, float maxDistance = 15)
        => DataSourceUtilities.GetNearest(this, mouseLocation, renderInfo, maxDistance);
    

    public DataPoint GetNearestX(Coordinates mouseLocation, RenderDetails renderInfo, float maxDistance = 15)
        => DataSourceUtilities.GetNearestX(this, mouseLocation, renderInfo, maxDistance);

    public DataPoint GetNearest(Coordinates mouseLocation, RenderDetails renderInfo, float maxDistance, IXAxis? xAxis, IYAxis? yAxis)
        => DataSourceUtilities.GetNearest(this, mouseLocation, renderInfo, maxDistance, xAxis, yAxis);

    public DataPoint GetNearestX(Coordinates mouseLocation, RenderDetails renderInfo, float maxDistance, IXAxis? xAxis)
        => DataSourceUtilities.GetNearestX(this, mouseLocation, renderInfo, maxDistance, xAxis);

    int IDataSource.GetXClosestIndex(Coordinates mouseLocation) => DataSourceUtilities.GetClosestIndex(Coordinates, mouseLocation, new IndexRange(MinRenderIndex, MaxRenderIndex));
    Coordinates IDataSource.GetCoordinate(int index) => Coordinates[index];
    Coordinates IDataSource.GetCoordinateScaled(int index) => Coordinates[index];
    double IDataSource.GetX(int index) => Coordinates[index].X;
    double IDataSource.GetY(int index) => Coordinates[index].Y;
    double IDataSource.GetXScaled(int index) => Coordinates[index].X;
    double IDataSource.GetYScaled(int index) => Coordinates[index].Y;
}
