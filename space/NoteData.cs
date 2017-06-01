using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace Space
{
    [Serializable]
    public class NoteData
    {
        public int id;
        public double X;
        public double Y;
        public double Width;
        public double Height;
        public string Background;

        public NoteData() { }

        public NoteData(int id, double X, double Y, double Width, double Height, string Background)
        {
            this.id = id;
            this.X = X;
            this.Y = Y;
            this.Width = Width;
            this.Height = Height;
            this.Background = Background;
        }
    }
}
