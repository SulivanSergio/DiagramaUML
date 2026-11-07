using DiagramaUML.Scripts.ErrosDebug;
using DiagramaUML.Scripts.Varredura;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiagramaUML.Scripts.ClassesComando
{
    public class Comandos
    {

        public void EscolheComandos(BuscaArquivos busca,string texto)
        {
            
            string[] comandos = texto.Split(" ");
            try
            {
                switch (comandos[0])
                {
                    case "all":
                        busca.ShowAllClasses();
                        return;
                        break;
                    case "allcol":
                        busca.ShowAllClassesCol();
                        return;
                        break;
                    case "classe":
                        busca.ShowClasse(comandos[1]);
                        break;
                    case "ref":
                        busca.ShowReferenciaClasse(comandos[1]);
                        break;
                    case "var":
                        busca.ShowVariavelClasse(comandos[1]);
                        break;
                    case "met":
                        busca.ShowMetodoClasse(comandos[1]);
                        break;
                    case "vm":
                        busca.ShowVariavelClasse(comandos[1]);
                        busca.ShowMetodoClasse(comandos[1]);
                        break;
                    case "reset":
                        busca.ShowReset();
                        break;
                    case "help":
                        busca.ShowHelp();
                        break;
                    case "refi":
                        busca.ShowInstancias(comandos[1]);
                        break;
                    case "refm":
                        busca.ShowReferenciaTodasClasses(comandos[1]);
                        break;
                    case "cd":
                        string[] caminho = texto.Split("cd ");
                        busca.ModificaDiretorio(caminho[1]);
                        return;

                    default:
                        IError error = new Error();
                        error.Erro();
                        break;
                }
            }
            catch (Exception ex)
            {
                IError error = new Error();
                error.Erro();
                //error.Erro(ex.ToString());
            }
            
        }
    }
}
