namespace OxyPlotTestGraph
{
    using System;
    using OxyPlot;
    using OxyPlot.Series;

    public partial class MainWindow
    {
        public MainWindow()
        {
            this.InitializeComponent();
            this.mainmodel.ActualController.UnbindAll();
            this.mainmodel.ActualController.BindMouseDown(OxyMouseButton.Left, PlotCommands.Track);
            MainViewModel.MainTimer.Enabled = true;
        }
    }

    public class MainViewModel
    {
        uint counter = 1;
        LineSeries graph;
        OxyPlot.Axes.LinearAxis x, y;
        static public System.Timers.Timer MainTimer = new System.Timers.Timer();
        System.Diagnostics.PerformanceCounter cpuload = new System.Diagnostics.PerformanceCounter("Processor", "% Processor Time", "_Total");

        public MainViewModel()
        {
            this.MyModel = new PlotModel { Title = "", PlotAreaBorderColor = OxyColor.FromRgb(255, 127, 0), PlotAreaBorderThickness = new OxyThickness(1.2), };
            graph = new LineSeries();
            graph.TrackerFormatString = "{1}: {2:0.##}" + Environment.NewLine + "{3}: {4:0.##}";
            this.MyModel.Series.Add(graph);

            x = new OxyPlot.Axes.LinearAxis()
            {   // x axis
                Position = OxyPlot.Axes.AxisPosition.Bottom,
                Title = "Time",
                IsAxisVisible = false,
                Maximum = 60,
                Minimum = 1,
            };
            this.MyModel.Axes.Add(x);

            y = new OxyPlot.Axes.LinearAxis()
            {   // y axis
                Position = OxyPlot.Axes.AxisPosition.Left,
                Title = "CPU Load",
                IsAxisVisible = false,
                Maximum = 100,
                Minimum = 0,
            };
            this.MyModel.Axes.Add(y);

            graph.StrokeThickness = 1.55;
            graph.Color = OxyColor.FromRgb(0, 255, 255);

            MainTimer.Interval = 1000;
            MainTimer.Elapsed += MainTimer_Elapsed;
        }

        void MainTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            graph.Points.Add(new DataPoint(counter, cpuload.NextValue()));
            counter++;
            if (counter >= 62) {
                x.Pan(x.Transform(-1 + x.Offset));
            }
            this.MyModel.InvalidatePlot(true);
        }

        public PlotModel MyModel { get; private set; }
    }
}