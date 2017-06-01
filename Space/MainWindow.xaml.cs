using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace Space
{
     public partial class MainWindow : Window
     {
        private static double mPX;
        private static double mPY;
        private static double lastMPX;
        private static double lastMPY;
        private static List<Note> notes = new List<Note>();
        private static List<NoteData> notesData = new List<NoteData>();
        private static List<long> rtbStreamStartPosition = new List<long>();
        private static int i = 0;
        public static bool isLeftMouseButtonClicked = false;
        public static bool isMiddleMouseButtonClicked = false;
        public static double scrollLevel = 10;
        MemoryStream ms;

        public MainWindow()
        {
            InitializeComponent();
            load();
        }

        void exit_Click(object sender, RoutedEventArgs e)
        {
            save();
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

        void save()
        {
            notesData.Clear();
            rtbStreamStartPosition.Clear();
            FileStream fs = new FileStream("richTextBoxes.bin", FileMode.Create, FileAccess.Write);
            ms = new MemoryStream();
            TextRange range;
            long streamPositionSum = 0;
            foreach (Note n in notes)
            {
                notesData.Add(new NoteData(n.id, (double)n.GetValue(Canvas.LeftProperty), (double)n.GetValue(Canvas.TopProperty), n.Width, n.Height, n.Background.ToString()));

                range = new TextRange(n.rtb.Document.ContentStart, n.rtb.Document.ContentEnd);

                ms = new MemoryStream();
                range.Save(ms, DataFormats.XamlPackage);

                if(n.id == 0)
                {
                    rtbStreamStartPosition.Add(0);
                    streamPositionSum += ms.Length;
                }
                else
                {
                    rtbStreamStartPosition.Add(streamPositionSum);
                    streamPositionSum += ms.Length;
                }

                ms.Position = 0;
                ms.CopyTo(fs);

                ms.Close();
            }
            fs.Close();          

            IFormatter formatter = new BinaryFormatter();
            fs = new FileStream("notesData.bin",
                                        FileMode.Create,
                                        FileAccess.Write, FileShare.None);
            formatter.Serialize(fs, notesData);
            fs.Close();
            fs = new FileStream("rtbStreamStartPositions.bin",
                                        FileMode.Create,
                                        FileAccess.Write, FileShare.None);
            formatter.Serialize(fs, rtbStreamStartPosition);
            fs.Close();

        }

        void load()
        {
            if (File.Exists("richTextBoxes.bin") && File.Exists("notesData.bin"))
            {
                IFormatter formatter = new BinaryFormatter();
                Stream fs = new FileStream("notesData.bin",
                                          FileMode.Open,
                                          FileAccess.Read,
                                          FileShare.Read);
                notesData = (List<NoteData>)formatter.Deserialize(fs);
                fs.Close();
                fs = new FileStream("rtbStreamStartPositions.bin",
                                          FileMode.Open,
                                          FileAccess.Read,
                                          FileShare.Read);
                rtbStreamStartPosition = (List<long>)formatter.Deserialize(fs);
                fs.Close();

                foreach (NoteData n in notesData)
                {
                    notes.Add(new Note(i, n.X, n.Y, n.Width, n.Height));
                    myCanvas.Children.Add(notes[i]);
                    i++;
                }
                
                TextRange range;
                for (int x = i-1; x>-1; x--)
                {
                    ms = new MemoryStream();
                    range = new TextRange(notes[x].rtb.Document.ContentStart,
                                    notes[x].rtb.Document.ContentEnd);

                    fs = new FileStream("richTextBoxes.bin", FileMode.OpenOrCreate, FileAccess.ReadWrite);
                    fs.Position = rtbStreamStartPosition[x];

                    ms.Position = 0;
                    fs.CopyTo(ms);

                    if (x > 0)
                    {
                        fs.SetLength(rtbStreamStartPosition[x]);

                    }

                    range.Load(ms, DataFormats.XamlPackage);
                    ms.Close();
                    fs.Close();
                }
            }
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
                isLeftMouseButtonClicked = false;
            }
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                // this.DragMove();
                isLeftMouseButtonClicked = true;
            }

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
                notes.Add(new Note(i, mPX, mPY, 150, 150));             
                myCanvas.Children.Add(notes[i]);
                i++;
                //Info.Opacity = 0.0;
            }
        }

        private void DragBar_DragDelta(object sender, DragDeltaEventArgs e)
        {
            Application.Current.MainWindow.SetValue(LeftProperty, Application.Current.MainWindow.Left + e.HorizontalChange);
            Application.Current.MainWindow.SetValue(TopProperty, Application.Current.MainWindow.Top + e.VerticalChange);
        }

        private void DragBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Application.Current.MainWindow.DragMove();
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
    }
}
