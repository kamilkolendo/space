using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Space
{
    class SpecialPage : Page
    {
        enum Colors { Czerwony, Zielony, Niebieski, Zolty, Pomaranczowy, Rozowy};

        private int choice;
        private string result;
        private string choice2;

        public SpecialPage()
        {
            this.Name = "specialBox";
            double cWidth = 800;
            double cHeight = 600;
            this.Width = 220;
            this.Height = 340;
            this.SetValue(Canvas.LeftProperty, cWidth/2 - this.Width/2);
            this.SetValue(Canvas.TopProperty, cHeight/2 - this.Height/2);
            
            //First Text
            var fecTextBlock1 = new FrameworkElementFactory(typeof(TextBlock));
            fecTextBlock1.SetValue(FrameworkElement.MarginProperty, new Thickness(5, 25, 0, 0));
            fecTextBlock1.SetValue(FrameworkElement.VerticalAlignmentProperty, VerticalAlignment.Top);
            fecTextBlock1.SetValue(FrameworkElement.WidthProperty, 200.0);
            fecTextBlock1.SetValue(TextBlock.ForegroundProperty, Brushes.White);
            TextBlock tb = new TextBlock();
            tb.Text = "    Powiedz mi, która z kolorowych\r" +
                      "     figur podoba Ci sie najbardziej,\r" +
                      " a ja dzięki zdziwiającej mocy enuma,\r" +
                      "      interfejsu i obsługi wyjątków\r" +
                      "    powiem Ci, jakiego jest koloru!!!\r\r" +
                      "   1)       2)       3)       4)       5)       6)";
            fecTextBlock1.SetValue(TextBlock.TextProperty, tb.Text);

            //Shapes
            MyRectangle redRect = new MyRectangle();
            FrameworkElementFactory fecRedRect = redRect.GetShape("Red");
            fecRedRect.SetValue(FrameworkElement.MarginProperty, new Thickness(-165, 0, 0, 0));

            MyCircle greenCirc = new MyCircle();
            FrameworkElementFactory fecGreenCirc = greenCirc.GetShape("Green");
            fecGreenCirc.SetValue(FrameworkElement.MarginProperty, new Thickness(-100, 0, 0, 0));

            MyRectangle blueRect = new MyRectangle();
            FrameworkElementFactory fecBlueRect = blueRect.GetShape("Blue");
            fecBlueRect.SetValue(FrameworkElement.MarginProperty, new Thickness(-35, 0, 0, 0));

            MyCircle yellowCirc = new MyCircle();
            FrameworkElementFactory fecYellowCirc = yellowCirc.GetShape("Yellow");
            fecYellowCirc.SetValue(FrameworkElement.MarginProperty, new Thickness(32, 0, 0, 0));

            MyCircle orangeCirc = new MyCircle();
            FrameworkElementFactory fecOrangeCirc = orangeCirc.GetShape("Orange");
            fecOrangeCirc.SetValue(FrameworkElement.MarginProperty, new Thickness(98, 0, 0, 0));

            MyRectangle pinkRect = new MyRectangle();
            FrameworkElementFactory fecPinkRect = pinkRect.GetShape("Pink");
            fecPinkRect.SetValue(FrameworkElement.MarginProperty, new Thickness(163, 0, 0, 0));

            //Second Text
            var fecTextBlock2 = new FrameworkElementFactory(typeof(TextBlock));
            fecTextBlock2.SetValue(FrameworkElement.MarginProperty, new Thickness(75, 200, 0, 0));
            fecTextBlock2.SetValue(FrameworkElement.VerticalAlignmentProperty, VerticalAlignment.Top);
            fecTextBlock2.SetValue(FrameworkElement.WidthProperty, 200.0);
            fecTextBlock2.SetValue(TextBlock.ForegroundProperty, Brushes.White);
            tb = new TextBlock();
            tb.Text = "Podaj numer:";
            fecTextBlock2.SetValue(TextBlock.TextProperty, tb.Text);

            //Input TextBox
            var fecInputTextBox = new FrameworkElementFactory(typeof(TextBox));
            fecInputTextBox.AddHandler(TextBox.TextChangedEvent, new TextChangedEventHandler(InputTextCheck));
            fecInputTextBox.SetValue(FrameworkElement.MarginProperty, new Thickness(0, 225, 0, 0));
            fecInputTextBox.SetValue(FrameworkElement.VerticalAlignmentProperty, VerticalAlignment.Top);
            fecInputTextBox.SetValue(FrameworkElement.WidthProperty, 12.0);
            fecInputTextBox.SetValue(TextBox.NameProperty, "InputBox");

            //Button
            var fecButton = new FrameworkElementFactory(typeof(Button));
            fecButton.AddHandler(PreviewMouseLeftButtonUpEvent, new MouseButtonEventHandler(ButtonClick));
            fecButton.SetValue(FrameworkElement.MarginProperty, new Thickness(0, 298, 0, 0));
            fecButton.SetValue(FrameworkElement.VerticalAlignmentProperty, VerticalAlignment.Top);
            fecButton.SetValue(FrameworkElement.WidthProperty, 70.0);
            fecButton.SetValue(Button.ContentProperty, "Sprawdź to!");

            //Third Text
            var fecTextBlock3 = new FrameworkElementFactory(typeof(TextBlock));
            fecTextBlock3.SetValue(FrameworkElement.MarginProperty, new Thickness(0, 250, 0, 0));
            fecTextBlock3.SetValue(FrameworkElement.VerticalAlignmentProperty, VerticalAlignment.Top);
            fecTextBlock3.SetValue(FrameworkElement.WidthProperty, 200.0);
            fecTextBlock3.SetValue(TextBlock.ForegroundProperty, Brushes.White);
            tb = new TextBlock();
            tb.Text = "     a kolor Twojej figury toooooo...\r" +
                      "         ...TUDUDUDUDUDUDUM... \r\r";
            fecTextBlock3.SetValue(TextBlock.TextProperty, tb.Text);

            //Thumb in which the draggable rectangle is stored
            var fecRecThumb = new FrameworkElementFactory(typeof(Thumb));
            fecRecThumb.AddHandler(DragDeltaEvent, new DragDeltaEventHandler(Thumb_DragDelta));

            //Draggable rectangle
            var fecRectangle = new FrameworkElementFactory(typeof(System.Windows.Shapes.Rectangle));
            fecRectangle.SetValue(Shape.FillProperty, Brushes.DarkRed);

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
            fecDestroyButton.SetValue(TextBlock.ForegroundProperty, Brushes.White);
            tb = new TextBlock();
            tb.Text = "X";
            fecDestroyButton.SetValue(TextBlock.TextProperty, tb.Text);

            //Grid in which all elements are stored
            var fecGrid = new FrameworkElementFactory(typeof(Grid));
            fecGrid.AppendChild(fecRecThumb);
            fecGrid.AppendChild(fecTextBlock1);
            fecGrid.AppendChild(fecTextBlock2);
            fecGrid.AppendChild(fecTextBlock3);
            fecGrid.AppendChild(fecRedRect);
            fecGrid.AppendChild(fecGreenCirc);
            fecGrid.AppendChild(fecBlueRect);
            fecGrid.AppendChild(fecYellowCirc);
            fecGrid.AppendChild(fecOrangeCirc);
            fecGrid.AppendChild(fecPinkRect);
            fecGrid.AppendChild(fecInputTextBox);
            fecGrid.AppendChild(fecButton);
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

        private void InputTextCheck(object sender, TextChangedEventArgs e)
        {
            //choice2 = e.
        }

        public void ButtonClick(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show(choice2);
        }



    }
}
