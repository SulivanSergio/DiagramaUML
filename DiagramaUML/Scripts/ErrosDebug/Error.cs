using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiagramaUML.Scripts.ErrosDebug
{
    public class Error : IError
    {
        public void Erro()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("ERRO NO COMANDO UML\n");
            Console.ResetColor();
        }
        public void Erro(string texto) {

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"{texto}\n");
            Console.ResetColor();
        }
    }
}
