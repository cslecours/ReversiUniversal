using System;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace ReversiUniversal.Views
{
    class GridCreator
    {
        public Brush LineBrush { get; set; }
        public int LineThickness { get; set; }
        public double EllipsePaddingPercentage { get; set; }

        public GridCreator()
        {
            LineBrush = new SolidColorBrush(Windows.UI.Colors.Black);
            LineThickness = 1;
            EllipsePaddingPercentage = 0.075;
        }

        public void SetupGrid(Grid grid, int cols, int rows)
        {
            grid.ColumnDefinitions.Clear();
            grid.RowDefinitions.Clear();
            grid.Children.Clear();

            for (int i = 0; i < cols; i++)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition());

                if (i > 0)
                {
                    var l = CreateLine(cols, rows);
                    l.Y1 = i * grid.Width / 8;
                    l.X1 = 0;
                    l.Y2 = i * grid.Width / 8;
                    l.X2 = grid.Height;
                    grid.Children.Add(l);
                }
            }

            for (int i = 0; i < rows; i++)
            {
                grid.RowDefinitions.Add(new RowDefinition());

                if (i > 0)
                {
                    var l = CreateLine(cols, rows);
                    l.X1 = i * grid.Height / 8;
                    l.Y1 = 0;
                    l.X2 = i * grid.Height / 8;
                    l.Y2 = grid.Width;
                    grid.Children.Add(l);
                }
            }
        }


        public void FillWithEllipseWithEvent(Grid g, Action<object, TappedRoutedEventArgs> tapped, Action<object, PointerRoutedEventArgs> pointerEntered, Action<object, PointerRoutedEventArgs> pointerExited)
        {
            int cols = g.ColumnDefinitions.Count();
            int rows = g.RowDefinitions.Count();

            for (int i = 0; i < cols; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    Ellipse el = new Ellipse();
                    el.Tapped += new TappedEventHandler(tapped);
                    el.PointerEntered += new PointerEventHandler(pointerEntered);
                    el.PointerExited += new PointerEventHandler(pointerExited);
                    Grid.SetColumn(el, i);
                    Grid.SetRow(el, j);

                    el.Margin = new Thickness((g.Width / cols) * EllipsePaddingPercentage);
                    g.Children.Add(el);
                }
            }
        }

        private Line CreateLine(int colspan, int rowspan)
        {
            var l = new Line()
            {
                Stroke = LineBrush,
                StrokeThickness = LineThickness
            };
            Grid.SetRowSpan(l, rowspan);
            Grid.SetColumnSpan(l, colspan);

            return l;
        }
    }
}
