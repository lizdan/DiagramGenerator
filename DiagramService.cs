using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace DiagramGenerator.BLL
{
    public class DiagramService
    {
        private List<Point> _points = new List<Point>();
        private List<Value> values = new List<Value>();

        // Add a point to the list
        public void AddPoint(Point point)
        {
            if (point.X < 0 || point.Y < 0)
                throw new ArgumentException("Only positive values are allowed for x and y coordinates.");

            _points.Add(point);
        }

        // Sort points in x direction
        public List<Point> SortInXDirection()
        {
            return _points.OrderBy(p => p.X).ToList();
        }

        // Sort points in y direction
        public List<Point> SortInYDirection()
        {
            return _points.OrderBy(p => p.Y).ToList();
        }

        // Calculate scale for x-axis
        public double CalculateScaleX(double panelWidth, double maxValueX, double margin = 0)
        {
            if (panelWidth <= 0 || maxValueX <= 0)
                throw new ArgumentException("Panel width and max x-value should be positive.");

            return (panelWidth - 2 * margin) / maxValueX;
        }

        // Calculate scale for y-axis
        public double CalculateScaleY(double panelHeight, double maxValueY, double margin = 0)
        {
            if (panelHeight <= 0 || maxValueY <= 0)
                throw new ArgumentException("Panel height and max y-value should be positive.");

            return (panelHeight - 2 * margin) / maxValueY;
        }

        // Get all points
        public List<Point> GetPoints()
        {
            return _points;
        }

        // Clear all points
        public void ClearPoints()
        {
            _points.Clear();
        }
    }
}
