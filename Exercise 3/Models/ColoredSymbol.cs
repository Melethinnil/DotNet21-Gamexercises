using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseWorker.Models
{
    internal class ColoredSymbol
    {
        public char Symbol { get; private set; }
        public ConsoleColor Color { get; private set; }

        public ColoredSymbol(char symbol, ConsoleColor color)
        {
            Symbol = symbol;
            Color = color;
        }
    }
}
