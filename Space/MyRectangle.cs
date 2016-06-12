using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Space
{
    class MyRectangle : IColorShape
    {
        private string color;

        public FrameworkElementFactory GetShape(string color)
        {
            this.color = color;
            var col = (Color)ColorConverter.ConvertFromString(color);
            var brush = new SolidColorBrush(col);
            var fecRectangle = new FrameworkElementFactory(typeof(Rectangle));
            fecRectangle.SetValue(Rectangle.FillProperty, brush);
            fecRectangle.SetValue(FrameworkElement.WidthProperty, 30.0);
            fecRectangle.SetValue(FrameworkElement.HeightProperty, 30.0);

            return fecRectangle;
        }

        public string GetColor()
        {
            return this.color;
        }

        private void IAmTheRectanle()
        {
            //dummy method showing that this class is different from MyCircle
            //although sharing the same Interface
        }

    }
}
