using DiagramaUML.Scripts.ClassesComando;
using DiagramaUML.Scripts.Varredura;
using System.Diagnostics;

namespace DiagramaUML.Scripts
{
    public class Main1
    {
        BuscaArquivos busca;
        Comandos comandos;
        bool rodando = true;
        public Main1() 
        {
            Console.Title = "UML";
            busca = new BuscaArquivos();
            comandos = new Comandos();

            CapturaComandos();
        }
        private void CapturaComandos()
        {
            while (rodando)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(busca.diretorioProjeto);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("> ");
                Console.ResetColor();
                string comando = Console.ReadLine();
                comandos.EscolheComandos(busca,comando);
            }
        }
    }
}
