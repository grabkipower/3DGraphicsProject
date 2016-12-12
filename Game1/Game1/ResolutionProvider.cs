using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1
{
    public static class ResolutionProvider
    {
        public static Rozmiar duzy = new Rozmiar() { x = 1920, y = 1080 };
        public static Rozmiar sredni = new Rozmiar() { x = 1000, y = 600 };
        public static Rozmiar maly = new Rozmiar() { x = 500, y = 300 };

    }
    public class Rozmiar
    {
        public int x { get; set; }
        public int y { get; set; }

        public override string ToString()
        {
            return $"{x} x {y}";
        }
    }
}
