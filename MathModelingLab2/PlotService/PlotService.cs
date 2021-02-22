﻿using System;
using System.Collections.Generic;
using System.Linq;
using GemBox.Spreadsheet;
using ScottPlot;

namespace MathModelingLab2.PlotService
{
    public class PlotService
    {
        // public void CompareWellBeingWithPrediction()
        // {
        //     
        //
        //     var labels = Enumerable.Range(0, PredictionLength).Select(i =>
        //         CurrentDate.AddDays(-i)).ToList();
        //
        //     var xs = labels.Select(x => x.ToOADate()).ToArray();
        //
        //     var plotsEmotional = new List<PlotLine>
        //     {
        //         new PlotLine("EmotionalPrediction", xs, labels.Select(GetEmotionCoefficientByDate).ToArray()),
        //         new PlotLine("EmotionalWell-being", xs, emotionalStates.ToArray())
        //     };
        //
        //     var plotsPhysical = new List<PlotLine>
        //     {
        //         new PlotLine("PhysicalPrediction", xs, labels.Select(GetPhysicalCoefficientByDate).ToArray()),
        //         new PlotLine("PhysicalWell-being", xs, physicalStates.ToArray())
        //     };
        //
        //     var plotsIntellectual = new List<PlotLine>
        //     {
        //         new PlotLine("IntellectualPrediction", xs, labels.Select(GetIntellectualCoefficientByDate).ToArray()),
        //         new PlotLine("IntellectualWell-being", xs, intellectualStates.ToArray())
        //     };
        //
        //     MakePlot($"EmotionalComparison{Name}", plotsEmotional);
        //     MakePlot($"PhysicalComparison{Name}", plotsPhysical);
        //     MakePlot($"IntellectualComparison{Name}", plotsIntellectual);
        // }
        
        private static void MakePlot(string fileName, List<PlotLine> plotLines)
        {
            var plt = new Plot(800, 600);

            plotLines.ForEach(plotLine => plt.AddScatter(plotLine.XDots, plotLine.YDots, label: plotLine.Label));

            plt.Legend();
            plt.XAxis.TickLabelStyle(rotation: 45);
            plt.XAxis.ManualTickSpacing(1, ScottPlot.Ticks.DateTimeUnit.Day);
            plt.XAxis.SetSizeLimit(50);

            plt.SaveFig($"{fileName}.png");
        }
    }
}