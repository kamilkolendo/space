using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Space
{
    // DZIEDZICZENIE //
    public class Page : Thumb
    {
        private int id;

        public Page() { }

        public Page(int i, double mPX, double mPY)
        {
            id = i;
            this.Name = "box" + i.ToString();
            this.SetValue(Canvas.LeftProperty, mPX);
            this.SetValue(Canvas.TopProperty, mPY);
            this.Width = 100;
            this.Height = 100;

            // POLIMORFIZM //
            var fecThumbN = CreateDirectionThumb('N');
            var fecThumbE = CreateDirectionThumb('E');
            var fecThumbW = CreateDirectionThumb('W');
            var fecThumbS = CreateDirectionThumb('S');
            var fecThumbNW = CreateDirectionThumb('N', 'W');
            var fecThumbNE = CreateDirectionThumb('N', 'E');
            var fecThumbSW = CreateDirectionThumb('S', 'W');
            var fecThumbSE = CreateDirectionThumb('S', 'E');

            //Thumb in which the draggable rectangle is stored
            var fecRecThumb = new FrameworkElementFactory(typeof(Thumb));
            fecRecThumb.AddHandler(DragDeltaEvent, new DragDeltaEventHandler(Thumb_DragDelta));
            fecRecThumb.SetValue(FrameworkElement.HeightProperty, 15.0);
            fecRecThumb.SetValue(FrameworkElement.VerticalAlignmentProperty, VerticalAlignment.Top);

            //Draggable rectangle
            var fecRectangle = new FrameworkElementFactory(typeof(System.Windows.Shapes.Rectangle));
            fecRectangle.SetValue(System.Windows.Shapes.Rectangle.FillProperty, Brushes.Khaki);

            //Thumb in which the detroyer button is stored
            var fecDestructionThumb = new FrameworkElementFactory(typeof(Thumb));
            fecDestructionThumb.AddHandler(PreviewMouseLeftButtonUpEvent, new MouseButtonEventHandler(PageDestroy));

            //Button which destroys Page on click
            var fecDestroyButton = new FrameworkElementFactory(typeof(TextBlock));
            fecDestroyButton.SetValue(FrameworkElement.VerticalAlignmentProperty, VerticalAlignment.Top);
            fecDestroyButton.SetValue(FrameworkElement.HorizontalAlignmentProperty, HorizontalAlignment.Right);
            fecDestroyButton.SetValue(TextBlock.HeightProperty, 22.0);
            fecDestroyButton.SetValue(TextBlock.WidthProperty, 12.0);
            fecDestroyButton.SetValue(TextBlock.FontSizeProperty, 11.0);
            fecDestroyButton.SetValue(TextBlock.FontWeightProperty, FontWeights.DemiBold);
            TextBlock tb = new TextBlock();
            tb.Text = "X";
            fecDestroyButton.SetValue(TextBlock.TextProperty, tb.Text);

            //TextBox
            var fecRichTextBox = new FrameworkElementFactory(typeof(RichTextBox));
            fecRichTextBox.SetValue(RichTextBox.BackgroundProperty, Brushes.Khaki);
            fecRichTextBox.SetValue(FrameworkElement.MarginProperty, new Thickness(0, 15, 0, 0));

            //Grid in which all elements are stored
            var fecGrid = new FrameworkElementFactory(typeof(Grid));
            fecGrid.AppendChild(fecRichTextBox);
            fecGrid.AppendChild(fecRecThumb);
            fecGrid.AppendChild(fecThumbN);
            fecGrid.AppendChild(fecThumbE);
            fecGrid.AppendChild(fecThumbW);
            fecGrid.AppendChild(fecThumbS);
            fecGrid.AppendChild(fecThumbNW);
            fecGrid.AppendChild(fecThumbNE);
            fecGrid.AppendChild(fecThumbSW);
            fecGrid.AppendChild(fecThumbSE);
            fecGrid.AppendChild(fecDestructionThumb);

            //DestroyButton Thumb Control
            ControlTemplate destroyCtrl = new ControlTemplate();
            destroyCtrl.VisualTree = fecDestroyButton;
            fecDestructionThumb.SetValue(TemplateProperty, destroyCtrl);

            //Rectangle Thumb Control
            ControlTemplate rectCtrl = new ControlTemplate();
            rectCtrl.VisualTree = fecRectangle;
            fecRecThumb.SetValue(TemplateProperty, rectCtrl);

            //Root Thumb Control
            ControlTemplate rootCtrl = new ControlTemplate();
            rootCtrl.VisualTree = fecGrid;

            this.Template = rootCtrl;
        }

        // PROPERCJE //
        public int ID { get; set; }

        public void PageDestroy(object sender, MouseButtonEventArgs e)
        {
            UIElement thumb = e.Source as UIElement;
            UIElement rootThumb = FindVisualTreeRoot(thumb) as UIElement;

            rootThumb.IsEnabled = false;
            rootThumb.Visibility = Visibility.Hidden;
            if (rootThumb.GetType() == typeof(SpecialPage))
            {
                MainWindow.doesSpecialBoxExists = false;
            }
        }

        public void Thumb_DragDelta(object sender, DragDeltaEventArgs e)
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
            while (current.GetType() != typeof(Page) && current.GetType() != typeof(SpecialPage));

            return current;
        }

        private FrameworkElementFactory CreateDirectionThumb(char direction)
        {
            var fec = new FrameworkElementFactory(typeof(Thumb));

            switch (direction)
            {
                case 'N':          
                    fec.AddHandler(DragDeltaEvent, new DragDeltaEventHandler(Thumb_ResizeN));
                    fec.SetValue(FrameworkElement.HeightProperty, 5.0);
                    fec.SetValue(FrameworkElement.CursorProperty, Cursors.SizeNS);
                    fec.SetValue(FrameworkElement.OpacityProperty, 0.0);
                    fec.SetValue(FrameworkElement.MarginProperty, new Thickness(0, -4, 0, 0));
                    fec.SetValue(FrameworkElement.VerticalAlignmentProperty, VerticalAlignment.Top);
                    fec.SetValue(FrameworkElement.HorizontalAlignmentProperty, HorizontalAlignment.Stretch);
                    break;

                case 'W':
                    fec.AddHandler(DragDeltaEvent, new DragDeltaEventHandler(Thumb_ResizeW));
                    fec.SetValue(FrameworkElement.WidthProperty, 5.0);
                    fec.SetValue(FrameworkElement.CursorProperty, Cursors.SizeWE);
                    fec.SetValue(FrameworkElement.OpacityProperty, 0.0);
                    fec.SetValue(FrameworkElement.MarginProperty, new Thickness(-4, 0, 0, 0));
                    fec.SetValue(FrameworkElement.VerticalAlignmentProperty, VerticalAlignment.Stretch);
                    fec.SetValue(FrameworkElement.HorizontalAlignmentProperty, HorizontalAlignment.Left);
                    break;

                case 'E':
                    fec.AddHandler(DragDeltaEvent, new DragDeltaEventHandler(Thumb_ResizeE));
                    fec.SetValue(FrameworkElement.WidthProperty, 5.0);
                    fec.SetValue(FrameworkElement.CursorProperty, Cursors.SizeWE);
                    fec.SetValue(FrameworkElement.OpacityProperty, 0.0);
                    fec.SetValue(FrameworkElement.MarginProperty, new Thickness(0, 0, -4, 0));
                    fec.SetValue(FrameworkElement.VerticalAlignmentProperty, VerticalAlignment.Stretch);
                    fec.SetValue(FrameworkElement.HorizontalAlignmentProperty, HorizontalAlignment.Right);
                    break;

                case 'S':
                    fec.AddHandler(DragDeltaEvent, new DragDeltaEventHandler(Thumb_ResizeS));
                    fec.SetValue(FrameworkElement.HeightProperty, 5.0);
                    fec.SetValue(FrameworkElement.CursorProperty, Cursors.SizeNS);
                    fec.SetValue(FrameworkElement.OpacityProperty, 0.0);
                    fec.SetValue(FrameworkElement.MarginProperty, new Thickness(0, 0, 0, -4));
                    fec.SetValue(FrameworkElement.VerticalAlignmentProperty, VerticalAlignment.Bottom);
                    fec.SetValue(FrameworkElement.HorizontalAlignmentProperty, HorizontalAlignment.Stretch);
                    break;

                default:
                    break;

            }
            return fec;
        }

        private FrameworkElementFactory CreateDirectionThumb(char direction1, char direction2)
        {
            var fec = new FrameworkElementFactory(typeof(Thumb));

            switch (direction1)
            {
                case 'N':
                    if (direction2 == 'W')
                    {
                        fec.AddHandler(DragDeltaEvent, new DragDeltaEventHandler(Thumb_ResizeNW));
                        fec.SetValue(FrameworkElement.HeightProperty, 7.0);
                        fec.SetValue(FrameworkElement.WidthProperty, 7.0);
                        fec.SetValue(FrameworkElement.CursorProperty, Cursors.SizeNWSE);
                        fec.SetValue(FrameworkElement.OpacityProperty, 0.0);
                        fec.SetValue(FrameworkElement.MarginProperty, new Thickness(-6, -6, 0, 0));
                        fec.SetValue(FrameworkElement.VerticalAlignmentProperty, VerticalAlignment.Top);
                        fec.SetValue(FrameworkElement.HorizontalAlignmentProperty, HorizontalAlignment.Left);
                    }
                    else
                    {
                        fec.AddHandler(DragDeltaEvent, new DragDeltaEventHandler(Thumb_ResizeNE));
                        fec.SetValue(FrameworkElement.HeightProperty, 7.0);
                        fec.SetValue(FrameworkElement.WidthProperty, 7.0);
                        fec.SetValue(FrameworkElement.CursorProperty, Cursors.SizeNESW);
                        fec.SetValue(FrameworkElement.OpacityProperty, 0.0);
                        fec.SetValue(FrameworkElement.MarginProperty, new Thickness(0, -6, -6, 0));
                        fec.SetValue(FrameworkElement.VerticalAlignmentProperty, VerticalAlignment.Top);
                        fec.SetValue(FrameworkElement.HorizontalAlignmentProperty, HorizontalAlignment.Right);
                    }
                    break;

                case 'S':
                    if (direction2 == 'W')
                    {
                        fec.AddHandler(DragDeltaEvent, new DragDeltaEventHandler(Thumb_ResizeSW));
                        fec.SetValue(FrameworkElement.HeightProperty, 7.0);
                        fec.SetValue(FrameworkElement.WidthProperty, 7.0);
                        fec.SetValue(FrameworkElement.CursorProperty, Cursors.SizeNESW);
                        fec.SetValue(FrameworkElement.OpacityProperty, 0.0);
                        fec.SetValue(FrameworkElement.MarginProperty, new Thickness(-6, 0, 0, -6));
                        fec.SetValue(FrameworkElement.VerticalAlignmentProperty, VerticalAlignment.Bottom);
                        fec.SetValue(FrameworkElement.HorizontalAlignmentProperty, HorizontalAlignment.Left);
                    }
                    else
                    {
                        fec.AddHandler(DragDeltaEvent, new DragDeltaEventHandler(Thumb_ResizeSE));
                        fec.SetValue(FrameworkElement.HeightProperty, 7.0);
                        fec.SetValue(FrameworkElement.WidthProperty, 7.0);
                        fec.SetValue(FrameworkElement.CursorProperty, Cursors.SizeNWSE);
                        fec.SetValue(FrameworkElement.OpacityProperty, 0.0);
                        fec.SetValue(FrameworkElement.MarginProperty, new Thickness(0, 0, -6, -6));
                        fec.SetValue(FrameworkElement.VerticalAlignmentProperty, VerticalAlignment.Bottom);
                        fec.SetValue(FrameworkElement.HorizontalAlignmentProperty, HorizontalAlignment.Right);
                    }
                    break;

                default:
                    break;

            }
            return fec;
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
