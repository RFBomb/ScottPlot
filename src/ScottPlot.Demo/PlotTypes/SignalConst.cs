﻿using ScottPlot.Drawing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ScottPlot.Demo.PlotTypes
{
    class SignalConst
    {
        public class RandomWalk_5millionPoints_SignalConst : PlotDemo, IPlotDemo
        {
            public string name { get; } = "5 Million Points";
            public string description { get; } = "SignalConst plots pre-processes data to render much faster than Signal plots. Pre-processing takes a little time up-front and requires 4x the memory of Signal.";

            public void Render(Plot plt)
            {
                Random rand = new Random(0);
                int pointCount = 1_000_000;
                int lineCount = 5;

                for (int i = 0; i < lineCount; i++)
                    plt.PlotSignalConst(DataGen.RandomWalk(rand, pointCount));
            }
        }

        // TODO: need an example showing how to update data

        public class PlotGradientFillAboveAndBelowRange : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Gradient Fill Above and Below";
            public string description { get; } = "Plot a range of values using gradient fill above and below.";

            public void Render(Plot plt)
            {
                Random rand = new Random(0);
                double[] data = DataGen.RandomWalk(rand, 100_000);

                plt.Style(Style.Gray1);
                plt.Colorset(Colorset.OneHalfDark);

                var sig = plt.PlotSignalConst(data);
                sig.MinRenderIndex = 4000;
                sig.MaxRenderIndex = 5000;
                sig.MarkerSize = 0;
                sig.Color = Color.Black;
                sig.FillType = FillType.FillAboveAndBelow;
                sig.FillColor1 = Color.FromArgb(255, 44, 160, 44); // Green
                sig.GradientFillColor1 = Color.Transparent;
                sig.FillColor2 = Color.FromArgb(255, 214, 39, 40); // Red
                sig.GradientFillColor2 = Color.Transparent;
                sig.Baseline = -35;

                plt.Title($"SignalConst displaying {data.Length} values");
                plt.YLabel("Value");
                plt.XLabel("Array Index");
                plt.AxisAutoX(margin: 0);
            }
        }
    }
}
