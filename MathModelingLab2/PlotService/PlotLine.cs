namespace MathModelingLab2.PlotService
{
    public class PlotLine
    {
        public string Label { get; set; }
        public double[] XDots { get; set; }
        public double[] YDots { get; set; }
        
        public PlotLine(string label, double[] xDots, double[] yDots)
        {
            Label = label;
            XDots = xDots;
            YDots = yDots;
        } 
    }
}