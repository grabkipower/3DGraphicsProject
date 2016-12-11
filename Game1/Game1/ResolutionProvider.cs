using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1
{
    public static class ResolutionProvider
    {
        public static Rozmiar duzy = new Rozmiar() { x = 1900, y = 1200 };
        public static Rozmiar sredni = new Rozmiar() { x = 1000, y = 600 };

    }
    public class Rozmiar
    {
        public int x { get; set; }
        public int y { get; set; }
    }
}
