using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Space
{
    public class Note : Grid
    {
        public int id;
        public RichTextBox rtb = new RichTextBox();

        public Note() { }

        public Note(int i, double mPX, double mPY, double Width, double Height)
        {
            this.id = i;
            this.Name = "note" + i.ToString();
            this.SetValue(Canvas.LeftProperty, mPX);
            this.SetValue(Canvas.TopProperty, mPY);
            this.Width = Width;
            this.Height = Height;
            this.Background = new SolidColorBrush(Colors.Khaki);

            ColumnDefinition col1 = new ColumnDefinition();
            col1.Width = new GridLength(3);
            ColumnDefinition col2 = new ColumnDefinition();
            ColumnDefinition col3 = new ColumnDefinition();
            col3.Width = new GridLength(3);
            this.ColumnDefinitions.Add(col1);
            this.ColumnDefinitions.Add(col2);
            this.ColumnDefinitions.Add(col3);

            RowDefinition row1 = new RowDefinition();
            row1.Height = new GridLength(3);
            RowDefinition row2 = new RowDefinition();
            row2.Height = new GridLength(14);
            RowDefinition row3 = new RowDefinition();
            RowDefinition row4 = new RowDefinition();
            row4.Height = new GridLength(3);
            this.RowDefinitions.Add(row1);
            this.RowDefinitions.Add(row2);
            this.RowDefinitions.Add(row3);
            this.RowDefinitions.Add(row4);

            Thumb thumbNW = new Thumb();
            thumbNW.Cursor = Cursors.SizeNWSE;
            thumbNW.Opacity = 0;
            thumbNW.DragDelta += new DragDeltaEventHandler(Thumb_ResizeNW);
            Grid.SetRow(thumbNW, 0);
            Grid.SetColumn(thumbNW, 0);
            this.Children.Add(thumbNW);

            Thumb thumbN = new Thumb();
            thumbN.Cursor = Cursors.SizeNS;
            thumbN.Opacity = 0;
            thumbN.DragDelta += new DragDeltaEventHandler(Thumb_ResizeN);
            Grid.SetRow(thumbN, 0);
            Grid.SetColumn(thumbN, 1);
            this.Children.Add(thumbN);

            Thumb thumbNE = new Thumb();
            thumbNE.Cursor = Cursors.SizeNESW;
            thumbNE.Opacity = 0;
            thumbNE.DragDelta += new DragDeltaEventHandler(Thumb_ResizeNE);
            Grid.SetRow(thumbNE, 0);
            Grid.SetColumn(thumbNE, 2);
            this.Children.Add(thumbNE);

            Thumb thumbW = new Thumb();
            thumbW.Cursor = Cursors.SizeWE;
            thumbW.Opacity = 0;
            thumbW.DragDelta += new DragDeltaEventHandler(Thumb_ResizeW);
            Grid.SetRow(thumbW, 1);
            Grid.SetColumn(thumbW, 0);
            this.Children.Add(thumbW);

            Thumb thumbE = new Thumb();
            thumbE.Cursor = Cursors.SizeWE;
            thumbE.Opacity = 0;
            thumbE.DragDelta += new DragDeltaEventHandler(Thumb_ResizeE);
            Grid.SetRow(thumbE, 1);
            Grid.SetColumn(thumbE, 2);
            this.Children.Add(thumbE);

            Thumb thumbW2 = new Thumb();
            thumbW2.Cursor = Cursors.SizeWE;
            thumbW2.Opacity = 0;
            thumbW2.DragDelta += new DragDeltaEventHandler(Thumb_ResizeW);
            Grid.SetRow(thumbW2, 2);
            Grid.SetColumn(thumbW2, 0);
            this.Children.Add(thumbW2);

            Thumb thumbE2 = new Thumb();
            thumbE2.Cursor = Cursors.SizeWE;
            thumbE2.Opacity = 0;
            thumbE2.DragDelta += new DragDeltaEventHandler(Thumb_ResizeE);
            Grid.SetRow(thumbE2, 2);
            Grid.SetColumn(thumbE2, 2);
            this.Children.Add(thumbE2);

            Thumb thumbSW = new Thumb();
            thumbSW.Cursor = Cursors.SizeNESW;
            thumbSW.Opacity = 0;
            thumbSW.DragDelta += new DragDeltaEventHandler(Thumb_ResizeSW);
            Grid.SetRow(thumbSW, 3);
            Grid.SetColumn(thumbSW, 0);
            this.Children.Add(thumbSW);

            Thumb thumbS = new Thumb();
            thumbS.Cursor = Cursors.SizeNS;
            thumbS.Opacity = 0;
            thumbS.DragDelta += new DragDeltaEventHandler(Thumb_ResizeS);
            Grid.SetRow(thumbS, 3);
            Grid.SetColumn(thumbS, 1);
            this.Children.Add(thumbS);

            Thumb thumbSE = new Thumb();
            thumbSE.Cursor = Cursors.SizeNWSE;
            thumbSE.Opacity = 0;
            thumbSE.DragDelta += new DragDeltaEventHandler(Thumb_ResizeSE);
            Grid.SetRow(thumbSE, 3);
            Grid.SetColumn(thumbSE, 2);
            this.Children.Add(thumbSE);

            Grid namePanel = new Grid();
            ColumnDefinition np_col1 = new ColumnDefinition();
            ColumnDefinition np_col2 = new ColumnDefinition();
            np_col2.Width = new GridLength(14);
            namePanel.ColumnDefinitions.Add(np_col1);
            namePanel.ColumnDefinitions.Add(np_col2);
            Grid.SetRow(namePanel, 1);
            Grid.SetColumn(namePanel, 1);
            this.Children.Add(namePanel);

            Button destroyButton = new Button();
            destroyButton.PreviewMouseLeftButtonUp += new MouseButtonEventHandler(PageDestroy);
            destroyButton.Content = "X";
            destroyButton.BorderThickness = new Thickness(0);
            destroyButton.FontSize = 10;
            destroyButton.FontWeight = FontWeights.DemiBold;
            destroyButton.Background = new SolidColorBrush(Colors.Khaki);
            Grid.SetRow(destroyButton, 0);
            Grid.SetColumn(destroyButton, 1);
            namePanel.Children.Add(destroyButton);

            Thumb thumbDrag = new Thumb();
            thumbDrag.DragDelta += new DragDeltaEventHandler(Thumb_DragDelta);
            thumbDrag.Opacity = 0;
            Grid.SetRow(thumbDrag, 0);
            Grid.SetColumn(thumbDrag, 0);
            namePanel.Children.Add(thumbDrag);

            rtb = new RichTextBox();
            rtb.Background = new SolidColorBrush(Colors.Khaki);
            rtb.BorderThickness = new Thickness(0);
            Style noSpaceStyle = new Style(typeof(Paragraph));
            noSpaceStyle.Setters.Add(new Setter(Paragraph.MarginProperty, new Thickness(0)));
            rtb.Resources.Add(typeof(Paragraph), noSpaceStyle);
            Grid.SetRow(rtb, 2);
            Grid.SetColumn(rtb, 1);
            this.Children.Add(rtb);
            
        }

        DependencyObject FindVisualTreeRoot(DependencyObject initial)
        {
            DependencyObject current = initial;

            do
            {
                current = VisualTreeHelper.GetParent(current);
            }
            while (current.GetType() != typeof(Note));

            return current;
        }

        public void PageDestroy(object sender, MouseButtonEventArgs e)
        {
            UIElement thumb = e.Source as UIElement;
            UIElement root = FindVisualTreeRoot(thumb) as UIElement;

            root.IsEnabled = false;
            root.Visibility = Visibility.Hidden;
        }

        private void Thumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            UIElement thumb = e.Source as UIElement;
            UIElement root = FindVisualTreeRoot(thumb) as UIElement;

            Canvas.SetLeft(root, Canvas.GetLeft(root) + e.HorizontalChange);
            Canvas.SetTop(root, Canvas.GetTop(root) + e.VerticalChange);
        }

        private void Thumb_ResizeE(object sender, DragDeltaEventArgs e)
        {
            UIElement thumb = e.Source as UIElement;
            UIElement root = FindVisualTreeRoot(thumb) as UIElement;

            double newSize = Convert.ToDouble(root.GetValue(WidthProperty)) + e.HorizontalChange;
            if (newSize >= 50)
            {
                root.SetValue(WidthProperty, newSize);
            }
        }
        private void Thumb_ResizeW(object sender, DragDeltaEventArgs e)
        {
            UIElement thumb = e.Source as UIElement;
            UIElement root = FindVisualTreeRoot(thumb) as UIElement;

            double newSize = Convert.ToDouble(root.GetValue(WidthProperty)) - e.HorizontalChange;
            if (newSize >= 50)
            {
                root.SetValue(WidthProperty, newSize);
                Canvas.SetLeft(root, Canvas.GetLeft(root) + e.HorizontalChange);
            }
        }

        private void Thumb_ResizeS(object sender, DragDeltaEventArgs e)
        {
            UIElement thumb = e.Source as UIElement;
            UIElement root = FindVisualTreeRoot(thumb) as UIElement;

            double newSize = Convert.ToDouble(root.GetValue(HeightProperty)) + e.VerticalChange;
            if (newSize >= 50)
            {
                root.SetValue(HeightProperty, newSize);
            }

        }

        private void Thumb_ResizeN(object sender, DragDeltaEventArgs e)
        {
            UIElement thumb = e.Source as UIElement;
            UIElement root = FindVisualTreeRoot(thumb) as UIElement;

            double newSize = Convert.ToDouble(root.GetValue(HeightProperty)) - e.VerticalChange;
            if (newSize >= 50)
            {
                root.SetValue(HeightProperty, newSize);
                Canvas.SetTop(root, Canvas.GetTop(root) + e.VerticalChange);
            }
        }

        private void Thumb_ResizeSE(object sender, DragDeltaEventArgs e)
        {
            UIElement thumb = e.Source as UIElement;
            UIElement root = FindVisualTreeRoot(thumb) as UIElement;

            double newWidth = Convert.ToDouble(root.GetValue(WidthProperty)) + e.HorizontalChange;
            double newHeight = Convert.ToDouble(root.GetValue(HeightProperty)) + e.VerticalChange;
            if (newWidth >= 50)
            {
                root.SetValue(WidthProperty, newWidth);
            }
            if (newHeight >= 50)
            {
                root.SetValue(HeightProperty, newHeight);
            }
        }

        private void Thumb_ResizeSW(object sender, DragDeltaEventArgs e)
        {
            UIElement thumb = e.Source as UIElement;
            UIElement root = FindVisualTreeRoot(thumb) as UIElement;

            double newWidth = Convert.ToDouble(root.GetValue(WidthProperty)) - e.HorizontalChange;
            double newHeight = Convert.ToDouble(root.GetValue(HeightProperty)) + e.VerticalChange;
            if (newWidth >= 50)
            {
                root.SetValue(WidthProperty, newWidth);
                Canvas.SetLeft(root, Canvas.GetLeft(root) + e.HorizontalChange);
            }
            if (newHeight >= 50)
            {
                root.SetValue(HeightProperty, newHeight);
            }
        }

        private void Thumb_ResizeNE(object sender, DragDeltaEventArgs e)
        {
            UIElement thumb = e.Source as UIElement;
            UIElement root = FindVisualTreeRoot(thumb) as UIElement;

            double newWidth = Convert.ToDouble(root.GetValue(WidthProperty)) + e.HorizontalChange;
            double newHeight = Convert.ToDouble(root.GetValue(HeightProperty)) - e.VerticalChange;
            if (newWidth >= 50)
            {
                root.SetValue(WidthProperty, newWidth);
            }
            if (newHeight >= 50)
            {
                root.SetValue(HeightProperty, newHeight);
                Canvas.SetTop(root, Canvas.GetTop(root) + e.VerticalChange);
            }
        }

        private void Thumb_ResizeNW(object sender, DragDeltaEventArgs e)
        {
            UIElement thumb = e.Source as UIElement;
            UIElement root = FindVisualTreeRoot(thumb) as UIElement;

            double newWidth = Convert.ToDouble(root.GetValue(WidthProperty)) - e.HorizontalChange;
            double newHeight = Convert.ToDouble(root.GetValue(HeightProperty)) - e.VerticalChange;
            if (newWidth >= 50)
            {
                root.SetValue(WidthProperty, newWidth);
                Canvas.SetLeft(root, Canvas.GetLeft(root) + e.HorizontalChange);
            }
            if (newHeight >= 50)
            {
                root.SetValue(HeightProperty, newHeight);
                Canvas.SetTop(root, Canvas.GetTop(root) + e.VerticalChange);
            }
        }
    }
}
