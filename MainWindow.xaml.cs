using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DiagramGenerator.BLL;

namespace DiagramGenerator.GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DiagramService _diagramService;
        ObservableCollection<BLL.Point> pointsList = new ObservableCollection<BLL.Point>();

        public MainWindow()
        {
            InitializeComponent();
            _diagramService = new DiagramService();
        }



        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            // Clear canvas to avoid overlapping drawings
            canvas.Children.Clear();

            // Read divisions and draw axes
            int xDiv = int.Parse(xDivisionsTextBox.Text);
            int yDiv = int.Parse(yDivisionsTextBox.Text);
            DrawAxes(canvas, xDiv, yDiv * 100); // multiply yDiv by 100 as each division is 100 units

            // Read x and y values
            double xValue = double.Parse(xInput.Text);
            double yValue = double.Parse(yInput.Text);

            // Add to points list
            pointsList.Add(new BLL.Point(xValue, yValue));

            DrawLine(canvas, pointsList);
            DrawAxes(canvas, xDiv, yDiv); // Redraw axes to ensure they are on top
        }

        private void DrawLine(Canvas canvas, ObservableCollection<BLL.Point> points)
        {
            Polyline polyline = new Polyline
            {
                Stroke = Brushes.Blue, // Blue color for the graph line
                StrokeThickness = 2
            };

            double maxX = points.Max(p => p.X);
            double maxY = points.Max(p => p.Y) * 100;

            foreach (var point in points)
            {
                polyline.Points.Add(new System.Windows.Point(point.X * canvas.ActualWidth / maxX,
                    canvas.ActualHeight - (point.Y * canvas.ActualHeight / maxY)));
            }

            canvas.Children.Add(polyline);
        }

        /**
        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            double x, y;

            if (double.TryParse(xInput.Text, out x) && double.TryParse(yInput.Text, out y))
            {
                _diagramService.AddPoint(new BLL.Point { X = x, Y = y });
                RefreshDiagramDisplay();
            }
            else
            {
                MessageBox.Show("Please enter valid numbers for X and Y.");
            }
        }*/


        private void DrawAxes(Canvas canvas, int divisionsX, int divisionsY)
        {
            const double margin = 10;
            double xmin = margin;
            double xmax = 300 - margin;
            double ymin = margin;
            double ymax = 300 - margin;
            const double step = 10;

            // Make the X axis.
            GeometryGroup xaxis_geom = new GeometryGroup();
            xaxis_geom.Children.Add(new LineGeometry(
                new System.Windows.Point(0, ymax), new System.Windows.Point(canvas.Width, ymax)));
            for (double x = xmin + step;
                x <= canvas.Width - step; x += step)
            {
                xaxis_geom.Children.Add(new LineGeometry(
                    new System.Windows.Point(x, ymax - margin / 2),
                    new System.Windows.Point(x, ymax + margin / 2)));
            }

            Path xaxis_path = new Path();
            xaxis_path.StrokeThickness = 1;
            xaxis_path.Stroke = Brushes.Black;
            xaxis_path.Data = xaxis_geom;

            canvas.Children.Add(xaxis_path);

            // Make the Y ayis.
            GeometryGroup yaxis_geom = new GeometryGroup();
            yaxis_geom.Children.Add(new LineGeometry(
                new System.Windows.Point(xmin, 0), new System.Windows.Point(xmin, canvas.Height)));
            for (double y = step; y <= canvas.Height - step; y += step)
            {
                yaxis_geom.Children.Add(new LineGeometry(
                    new System.Windows.Point(xmin - margin / 2, y),
                    new System.Windows.Point(xmin + margin / 2, y)));
            }

            Path yaxis_path = new Path();
            yaxis_path.StrokeThickness = 1;
            yaxis_path.Stroke = Brushes.Black;
            yaxis_path.Data = yaxis_geom;

            canvas.Children.Add(yaxis_path);

            // Make some data sets.
            Brush[] brushes = { Brushes.Red, Brushes.Green, Brushes.Blue };
            Random rand = new Random();
            for (int data_set = 0; data_set < 3; data_set++)
            {
                int last_y = rand.Next((int)ymin, (int)ymax);

                PointCollection points = new PointCollection();
                for (double x = xmin; x <= xmax; x += step)
                {
                    last_y = rand.Next(last_y - 10, last_y + 10);
                    if (last_y < ymin) last_y = (int)ymin;
                    if (last_y > ymax) last_y = (int)ymax;
                    points.Add(new System.Windows.Point(x, last_y));
                }

                Polyline polyline = new Polyline();
                polyline.StrokeThickness = 1;
                polyline.Stroke = brushes[data_set];
                polyline.Points = points;

                canvas.Children.Add(polyline);
            }
        }







        private void SortXButton_Click(object sender, RoutedEventArgs e)
        {
            _diagramService.SortInXDirection();
            RefreshDiagramDisplay();
        }

        private void SortYButton_Click(object sender, RoutedEventArgs e)
        {
            _diagramService.SortInYDirection();
            RefreshDiagramDisplay();
        }

        private void RefreshDiagramDisplay()
        {
            pointsListBox.ItemsSource = null;
            pointsListBox.ItemsSource = _diagramService.GetPoints();
        }
    }
}
