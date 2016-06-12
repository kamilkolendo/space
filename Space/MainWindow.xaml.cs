using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

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
        // KOLEKCJA //
        private static List<Page> pages = new List<Page>();
        private int i = 0;
        public static bool doesSpecialBoxExists = false;
        public static string wynik = "Nic";
        public static bool cosSieStalo = false;

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

        void special_Click(object sender, RoutedEventArgs e)
        {
            if (!doesSpecialBoxExists)
            {
                SpecialPage special = new SpecialPage();
                myCanvas.Children.Add(special);
                doesSpecialBoxExists = true;
                Info.Opacity = 0.0;
            }
        }

        void clear_Click(object sender, RoutedEventArgs e)
        {
            // 6 - number of elements on canvass other than pages
            myCanvas.Children.RemoveRange(6, myCanvas.Children.Count-6);
            doesSpecialBoxExists = false;
            pages.Clear();
            i = 0;
            Info.Opacity = 1.0;
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            showMousePosition();
        }

        public void showMousePosition()
        {
            mPX = Mouse.GetPosition(this).X;
            mPY = Mouse.GetPosition(this).Y;
        }

        public void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Right)
            {
                GenerateControls();
                Info.Opacity = 0.0;
            }
        }

        public void GenerateControls()
        {
            pages.Add(new Page(i, mPX, mPY));

            myCanvas.Children.Add(pages[i]);
            i++;

            if (cosSieStalo)
            {
                Info2.Text = "Woah";
            }
            else
            {
                Info2.Text = "Blep";
            }
        }



        private void Enter_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(Info2.Text);
        }
    }
}
