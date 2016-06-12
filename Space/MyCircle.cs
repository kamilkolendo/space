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
    class MyCircle : IColorShape
    {
        private string color;

        public FrameworkElementFactory GetShape(string color)
        {
            this.color = color;
            var col = (Color)ColorConverter.ConvertFromString(color);
            var brush = new SolidColorBrush(col);
            var fecCircle = new FrameworkElementFactory(typeof(Ellipse));
            fecCircle.SetValue(Ellipse.FillProperty, brush);
            fecCircle.SetValue(Ellipse.WidthProperty, 30.0);
            fecCircle.SetValue(Ellipse.HeightProperty, 30.0);

            return fecCircle;
        }

        public string GetColor()
        {
            return this.color;
        }

        private void IAmTheCircle()
        {
            //dummy method showing that this class is different from MyCircle
            //although sharing the same Interface
        }
    }
}
