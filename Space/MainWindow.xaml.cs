using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;

namespace Space
{
    // INTERFEJS //
    interface IColorShape
    {
        FrameworkElementFactory GetShape(string color);
        string GetColor();
    }
   
    public partial class MainWindow : Window
    {
        private static double mPX;
        private static double mPY;
        private static double lastMPX;
        private static double lastMPY;
        // KOLEKCJA //
        private static List<Page> pages = new List<Page>();
        private int i = 0;
        public static bool doesSpecialBoxExists = false;
        public static bool isMiddleMouseButtonClicked = false;
        public static double scrollLevel = 10;

        public MainWindow()
        {
            InitializeComponent();
        }

        void exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        void maximize_Click(object sender, RoutedEventArgs e)
        {
            if (MyWindow.WindowState == WindowState.Normal)
            {
                MyWindow.WindowState = WindowState.Maximized;
            }
            else if (MyWindow.WindowState == WindowState.Maximized)
            {
                MyWindow.WindowState = WindowState.Normal;
            }
        }

        void minimize_Click(object sender, RoutedEventArgs e)
        {
            MyWindow.WindowState = WindowState.Minimized;
        }


        /*void clear_Click(object sender, RoutedEventArgs e)
        {
            // 6 - number of elements on canvass other than pages
            myCanvas.Children.RemoveRange(6, myCanvas.Children.Count-6);
            doesSpecialBoxExists = false;
            pages.Clear();
            i = 0;
            Info.Opacity = 1.0;
        }*/

        private void Window_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Middle)
            {
                isMiddleMouseButtonClicked = false;

            }
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();

            if (e.ChangedButton == MouseButton.Middle)
            {
                isMiddleMouseButtonClicked = true;
                lastMPX = Mouse.GetPosition(this).X;
                lastMPY = Mouse.GetPosition(this).Y;
            }

            if (e.ChangedButton == MouseButton.Right)
            {
                mPX = Mouse.GetPosition(this.myCanvas).X;
                mPY = Mouse.GetPosition(this.myCanvas).Y;
                pages.Add(new Page(i, mPX, mPY));
                myCanvas.Children.Add(pages[i]);
                i++;
                //Info.Opacity = 0.0;
            }
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMiddleMouseButtonClicked == true)
            {
                mPX = Mouse.GetPosition(this).X;
                mPY = Mouse.GetPosition(this).Y;
                canvasTranslate.X -= (lastMPX - mPX) * (10 / scrollLevel);
                canvasTranslate.Y -= (lastMPY - mPY) * (10 / scrollLevel);
                lastMPX = Mouse.GetPosition(this).X;
                lastMPY = Mouse.GetPosition(this).Y;
            }
        }

        private void MouseWheelHandler(object sender, MouseWheelEventArgs e)
        {
            canvasScale.CenterX = 400;
            canvasScale.CenterY = 300;
            buttonScale.CenterX = 400;
            buttonScale.CenterY = 300;

            // If the mouse wheel delta is positive, move the box up.
            if (e.Delta > 0)
            {
                canvasScale.ScaleX += 0.1;
                canvasScale.ScaleY += 0.1;
                scrollLevel++;
            }

            // If the mouse wheel delta is negative, move the box down.
            if (e.Delta < 0 && scrollLevel > 1)
            {
                canvasScale.ScaleX -= 0.1;
                canvasScale.ScaleY -= 0.1;
                scrollLevel--;
            }

        }


        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {

        }

        public void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void Thumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            UIElement thumb = e.Source as UIElement;
            UIElement rootThumb = FindVisualTreeRoot(thumb) as UIElement;

            Canvas.SetLeft(rootThumb, Canvas.GetLeft(rootThumb) + e.HorizontalChange);
            Canvas.SetTop(rootThumb, Canvas.GetTop(rootThumb) + e.VerticalChange);
        }

        DependencyObject FindVisualTreeRoot(DependencyObject initial)
        {
            DependencyObject current = initial;

            do
            {
                current = VisualTreeHelper.GetParent(current);
            }
            while (current.GetType() != typeof(Grid));

            return current;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////

        private void Window_ResizeE(object sender, DragDeltaEventArgs e)
        {
            double newSize = Convert.ToDouble(this.GetValue(WidthProperty)) + e.HorizontalChange;
            if (newSize >= 50)
            {
                this.SetValue(WidthProperty, newSize);
            }
        }

        private void Window_ResizeW(object sender, DragDeltaEventArgs e)
        {
            double newSize = Convert.ToDouble(this.GetValue(WidthProperty)) - e.HorizontalChange;
            if (newSize >= 50)
            {
                canvasTranslate.X -= e.HorizontalChange * (10 / scrollLevel);
                this.SetValue(WidthProperty, newSize);
                Application.Current.MainWindow.SetValue(LeftProperty, Application.Current.MainWindow.Left + e.HorizontalChange);
            }
        }

        private void Window_ResizeS(object sender, DragDeltaEventArgs e)
        {
            double newSize = Convert.ToDouble(this.GetValue(HeightProperty)) + e.VerticalChange;
            if (newSize >= 50)
            {
                this.SetValue(HeightProperty, newSize);
            }

        }

        private void Window_ResizeN(object sender, DragDeltaEventArgs e)
        {
            double newSize = Convert.ToDouble(this.GetValue(HeightProperty)) - e.VerticalChange;
            if (newSize >= 50)
            {
                canvasTranslate.Y -= e.VerticalChange * (10 / scrollLevel);
                this.SetValue(HeightProperty, newSize);
                Application.Current.MainWindow.SetValue(TopProperty, Application.Current.MainWindow.Top + e.VerticalChange);
            }
        }

        private void Window_ResizeSE(object sender, DragDeltaEventArgs e)
        {
            double newWidth = Convert.ToDouble(this.GetValue(WidthProperty)) + e.HorizontalChange;
            double newHeight = Convert.ToDouble(this.GetValue(HeightProperty)) + e.VerticalChange;
            if (newWidth >= 50)
            {
                this.SetValue(WidthProperty, newWidth);
            }
            if (newHeight >= 50)
            {
                this.SetValue(HeightProperty, newHeight);
            }
        }

        private void Window_ResizeSW(object sender, DragDeltaEventArgs e)
        {
            double newWidth = Convert.ToDouble(this.GetValue(WidthProperty)) - e.HorizontalChange;
            double newHeight = Convert.ToDouble(this.GetValue(HeightProperty)) + e.VerticalChange;
            if (newWidth >= 50)
            {
                canvasTranslate.X -= e.HorizontalChange * (10 / scrollLevel);
                this.SetValue(WidthProperty, newWidth);
                Application.Current.MainWindow.SetValue(LeftProperty, Application.Current.MainWindow.Left + e.HorizontalChange);
            }
            if (newHeight >= 50)
            {
                this.SetValue(HeightProperty, newHeight);
            }
        }

        private void Window_ResizeNE(object sender, DragDeltaEventArgs e)
        {
            double newWidth = Convert.ToDouble(this.GetValue(WidthProperty)) + e.HorizontalChange;
            double newHeight = Convert.ToDouble(this.GetValue(HeightProperty)) - e.VerticalChange;
            if (newWidth >= 50)
            {
                this.SetValue(WidthProperty, newWidth);
            }
            if (newHeight >= 50)
            {
                canvasTranslate.Y -= e.VerticalChange * (10 / scrollLevel);
                this.SetValue(HeightProperty, newHeight);
                Application.Current.MainWindow.SetValue(TopProperty, Application.Current.MainWindow.Top + e.VerticalChange);
            }
        }

        private void Window_ResizeNW(object sender, DragDeltaEventArgs e)
        {
            double newWidth = Convert.ToDouble(this.GetValue(WidthProperty)) - e.HorizontalChange;
            double newHeight = Convert.ToDouble(this.GetValue(HeightProperty)) - e.VerticalChange;
            if (newWidth >= 50)
            {
                canvasTranslate.X -= e.HorizontalChange * (10 / scrollLevel);
                this.SetValue(WidthProperty, newWidth);
                Application.Current.MainWindow.SetValue(LeftProperty, Application.Current.MainWindow.Left + e.HorizontalChange);
            }
            if (newHeight >= 50)
            {
                canvasTranslate.Y -= e.VerticalChange * (10 / scrollLevel);
                this.SetValue(HeightProperty, newHeight);
                Application.Current.MainWindow.SetValue(TopProperty, Application.Current.MainWindow.Top + e.VerticalChange);
            }
        }

        private void Thumb_ResizeE(object sender, DragDeltaEventArgs e)
        {
            UIElement thumb = e.Source as UIElement;
            UIElement rootThumb = FindVisualTreeRoot(thumb) as UIElement;

            double newSize = Convert.ToDouble(rootThumb.GetValue(WidthProperty)) + e.HorizontalChange;
            if (newSize >= 50)
            {
                rootThumb.SetValue(WidthProperty, newSize);
            }
        }

        private void Thumb_ResizeW(object sender, DragDeltaEventArgs e)
        {
            UIElement thumb = e.Source as UIElement;
            UIElement rootThumb = FindVisualTreeRoot(thumb) as UIElement;

            double newSize = Convert.ToDouble(rootThumb.GetValue(WidthProperty)) - e.HorizontalChange;
            if (newSize >= 50)
            {
                rootThumb.SetValue(WidthProperty, newSize);
                Canvas.SetLeft(rootThumb, Canvas.GetLeft(rootThumb) + e.HorizontalChange);
            }
        }

        private void Thumb_ResizeS(object sender, DragDeltaEventArgs e)
        {
            UIElement thumb = e.Source as UIElement;
            UIElement rootThumb = FindVisualTreeRoot(thumb) as UIElement;

            double newSize = Convert.ToDouble(rootThumb.GetValue(HeightProperty)) + e.VerticalChange;
            if (newSize >= 50)
            {
                rootThumb.SetValue(HeightProperty, newSize);
            }

        }

        private void Thumb_ResizeN(object sender, DragDeltaEventArgs e)
        {
            UIElement thumb = e.Source as UIElement;
            UIElement rootThumb = FindVisualTreeRoot(thumb) as UIElement;

            double newSize = Convert.ToDouble(rootThumb.GetValue(HeightProperty)) - e.VerticalChange;
            if (newSize >= 50)
            {
                rootThumb.SetValue(HeightProperty, newSize);
                Canvas.SetTop(rootThumb, Canvas.GetTop(rootThumb) + e.VerticalChange);
            }
        }

        private void Thumb_ResizeSE(object sender, DragDeltaEventArgs e)
        {
            UIElement thumb = e.Source as UIElement;
            UIElement rootThumb = FindVisualTreeRoot(thumb) as UIElement;

            double newWidth = Convert.ToDouble(rootThumb.GetValue(WidthProperty)) + e.HorizontalChange;
            double newHeight = Convert.ToDouble(rootThumb.GetValue(HeightProperty)) + e.VerticalChange;
            if (newWidth >= 50)
            {
                rootThumb.SetValue(WidthProperty, newWidth);
            }
            if (newHeight >= 50)
            {
                rootThumb.SetValue(HeightProperty, newHeight);
            }
        }

        private void Thumb_ResizeSW(object sender, DragDeltaEventArgs e)
        {
            UIElement thumb = e.Source as UIElement;
            UIElement rootThumb = FindVisualTreeRoot(thumb) as UIElement;

            double newWidth = Convert.ToDouble(rootThumb.GetValue(WidthProperty)) - e.HorizontalChange;
            double newHeight = Convert.ToDouble(rootThumb.GetValue(HeightProperty)) + e.VerticalChange;
            if (newWidth >= 50)
            {
                rootThumb.SetValue(WidthProperty, newWidth);
                Canvas.SetLeft(rootThumb, Canvas.GetLeft(rootThumb) + e.HorizontalChange);
            }
            if (newHeight >= 50)
            {
                rootThumb.SetValue(HeightProperty, newHeight);
            }
        }

        private void Thumb_ResizeNE(object sender, DragDeltaEventArgs e)
        {
            UIElement thumb = e.Source as UIElement;
            UIElement rootThumb = FindVisualTreeRoot(thumb) as UIElement;

            double newWidth = Convert.ToDouble(rootThumb.GetValue(WidthProperty)) + e.HorizontalChange;
            double newHeight = Convert.ToDouble(rootThumb.GetValue(HeightProperty)) - e.VerticalChange;
            if (newWidth >= 50)
            {
                rootThumb.SetValue(WidthProperty, newWidth);
            }
            if (newHeight >= 50)
            {
                rootThumb.SetValue(HeightProperty, newHeight);
                Canvas.SetTop(rootThumb, Canvas.GetTop(rootThumb) + e.VerticalChange);
            }
        }

        private void Thumb_ResizeNW(object sender, DragDeltaEventArgs e)
        {
            UIElement thumb = e.Source as UIElement;
            UIElement rootThumb = FindVisualTreeRoot(thumb) as UIElement;

            double newWidth = Convert.ToDouble(rootThumb.GetValue(WidthProperty)) - e.HorizontalChange;
            double newHeight = Convert.ToDouble(rootThumb.GetValue(HeightProperty)) - e.VerticalChange;
            if (newWidth >= 50)
            {
                rootThumb.SetValue(WidthProperty, newWidth);
                Canvas.SetLeft(rootThumb, Canvas.GetLeft(rootThumb) + e.HorizontalChange);
            }
            if (newHeight >= 50)
            {
                rootThumb.SetValue(HeightProperty, newHeight);
                Canvas.SetTop(rootThumb, Canvas.GetTop(rootThumb) + e.VerticalChange);
            }
        }
    }
}
