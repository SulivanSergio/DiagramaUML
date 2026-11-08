using DiagramaUML.Scripts.ErrosDebug;
using DiagramaUML.Scripts.Graph;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace DiagramaUML.Scripts.Varredura
{
    public class BuscaArquivos
    {
        public string diretorioProjeto = "";
        string[] arquivosCs;
        List<SyntaxTree> syntaxTrees = new List<SyntaxTree>();
        PortableExecutableReference[] references;
        CSharpCompilation compilation;
        public BuscaArquivos()
        {
            Console.WriteLine("Digite o comando 'help' descobrir os comandos");
            diretorioProjeto = @"D:\Documentos\Estudos Aleatorios\Diagrama Uml estilo Git\PastaDeBusca\";
            LoddingFiles();
        }
        public void LoddingFiles()
        {
            // Caminho da pasta onde estão os seus arquivos .cs
            

            if (!Directory.Exists(diretorioProjeto))
            {
                IError error = new Error();
                error.Erro("DIRETORIO NÃO ENCONTRADO");
                return;
            }

            arquivosCs = Directory.GetFiles(diretorioProjeto, "*.cs", SearchOption.AllDirectories);

            // 1. Carrega as árvores sintáticas e cria a compilação cruzada
            syntaxTrees = arquivosCs.Select(path => CSharpSyntaxTree.ParseText(File.ReadAllText(path), path: path)).ToList();
            references = new[] { MetadataReference.CreateFromFile(typeof(object).Assembly.Location) };
            compilation = CSharpCompilation.Create("AnaliseCompletaProjeto", syntaxTrees, references);
        }
        public void ShowAllClasses()
        {
            for (int i = 0; i < syntaxTrees.Count; i++)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"{Path.GetFileName(syntaxTrees[i].FilePath)}");
            }
            Console.ResetColor();
            Console.WriteLine($"\n{syntaxTrees.Count} arquivos encontrados\n");
        }
        public void ShowAllClassesCol()
        {
            for (int i = 0; i < syntaxTrees.Count; i++)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write($"{Path.GetFileName(syntaxTrees[i].FilePath)}   ");
            }
            Console.ResetColor();
            Console.WriteLine($"\n\n{syntaxTrees.Count} arquivos encontrados\n");
        }

        public void ShowClasse(string nome)
        {
            for (int i = 0; i < syntaxTrees.Count; i++)
            {
                if ((nome + ".cs") == Path.GetFileName(syntaxTrees[i].FilePath))
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"{Path.GetFileName(syntaxTrees[i].FilePath)}\n");
                    return;
                }
            }
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"classe não encontrada\n");
            Console.ResetColor();
        }

        public void ModificaDiretorio(string diretorio)
        {
            
            diretorioProjeto = diretorio;
            LoddingFiles();
        }

        private SyntaxTree BuscaClasseNode(string nome)
        {
            for (int i = 0; i < syntaxTrees.Count; i++)
            {
                if ((nome + ".cs") == Path.GetFileName(syntaxTrees[i].FilePath))
                {
                    return syntaxTrees[i];
                }
            }            
            return null;
        }
        public void ShowReferenciaClasse(string nome)
        {
            if(BuscaClasseNode(nome) == null || nome == null)
            {
                IError error = new Error();
                error.Erro("CLASSE NÃO ENCONTRADA");
                return;
            }
            var root = BuscaClasseNode(nome).GetRoot();
            var semanticModel = compilation.GetSemanticModel(BuscaClasseNode(nome));

            // 6. Referências e Chamadas (Mapeia o uso de métodos e classes de outros arquivos)
            var invocacoes = root.DescendantNodes().OfType<InvocationExpressionSyntax>();
            foreach (var invocacao in invocacoes)
            {
                var simboloInfo = semanticModel.GetSymbolInfo(invocacao);
                if (simboloInfo.Symbol is IMethodSymbol metodoSimbolo)
                {
                    var linhaInfo = BuscaClasseNode(nome).GetLineSpan(invocacao.Span);
                    int numeroLinha = linhaInfo.StartLinePosition.Line + 1;

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write($" -> ");
                    Console.ResetColor();
                    Console.Write($"Chama:");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write($" {numeroLinha}");
                    Console.ForegroundColor = ConsoleColor.Green;                    
                    Console.Write($" {metodoSimbolo.Name}()");
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write($" --> ");
                    Console.ResetColor();
                    Console.Write($"pertencente à classe ");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write($" {metodoSimbolo.ContainingType.Name}\n");
                    
                }
            }
            Console.ResetColor();
            Console.WriteLine();
        }

        public void ShowVariavelClasse(string nome)
        {
            if (BuscaClasseNode(nome) == null || nome == null)
            {
                IError error = new Error();
                error.Erro("CLASSE NÃO ENCONTRADA");
                return;
            }
            var root = BuscaClasseNode(nome).GetRoot();
            var classes = root.DescendantNodes().OfType<ClassDeclarationSyntax>();
            foreach (var cls in classes)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\n{cls.Identifier.Text}");

                // 4. Campos (Fields) da Classe
                var campos = cls.Members.OfType<FieldDeclarationSyntax>();
                foreach (var campo in campos)
                {                    
                    string tipoCampo = campo.Declaration.Type.ToString();
                    foreach (var variavel in campo.Declaration.Variables)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write($"  ├── ");
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.Write($" {campo.Modifiers}");
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.Write($" {tipoCampo}");
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write($" {variavel.Identifier.Text} \n");
                    }
                }
                Console.WriteLine();
                
                
            }



        }

        public void ShowMetodoClasse(string nome)
        {
            if (BuscaClasseNode(nome) == null || nome == null)
            {
                IError error = new Error();
                error.Erro("CLASSE NÃO ENCONTRADA");
                return;
            }
            var root = BuscaClasseNode(nome).GetRoot();
            var classes = root.DescendantNodes().OfType<ClassDeclarationSyntax>();
            foreach (var cls in classes)
            {
                
                Console.WriteLine($"\n{cls.Identifier.Text}");                

                // 5. Métodos da Classe
                var metodos = cls.Members.OfType<MethodDeclarationSyntax>();
                foreach (var metodo in metodos)
                {
                    var linhaInfo = BuscaClasseNode(nome).GetLineSpan(metodo.Span);
                    int numeroLinha = linhaInfo.StartLinePosition.Line + 1;
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write($"  ├── ");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write($" {numeroLinha}");
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.Write($" {metodo.Modifiers}");
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write($" {metodo.ReturnType}");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write($" {metodo.Identifier.Text} \n");
                    
                }
            }

            Console.WriteLine();
        }

        public void ShowReset()
        {
            LoddingFiles();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Atualizado!");
            Console.WriteLine();
            Console.ResetColor();
        }
        public void ShowHelp()
        {
            Console.WriteLine("all\t\t\t\t-> mostra as classes\r\nallcol\t\t\t\t-> mostra as classes em colunas\r\nclasse nomeDaClasse\t\t-> mostra a classe encontrada\r\nref nomeDaClasse\t\t-> todas as referencias da classe\r\nvar nomeDaClasse\t\t-> variaveis da classe\r\nmet nomeDaClasse\t\t-> metodos da classe\r\nvm nomeDaClasse\t\t\t-> variaveis e metodos da Classe\r\ncd nomeDaPasta\\\t\t\t-> troca caminho da varredura\r\nreset\t\t\t\t-> recarrega a pasta\r\nhelp\t\t\t\t-> mostra os comandos\r\nrefi nomeDaClasse\t\t-> compara para ver se tem instancias iguais ao nome classe\r\nrefm nomeDaClasse\t\t-> todas as referencias da classe em todas as classes");
            Console.ResetColor();
        }

        public void ShowInstancias(string nome)
        {
            if (BuscaClasseNode(nome) == null || nome == null)
            {
                IError error = new Error();
                error.Erro("CLASSE 1 NÃO ENCONTRADA");
                return;
            }
            var rootOrigin = BuscaClasseNode(nome).GetRoot();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\n{nome}");
            foreach (var tree in syntaxTrees)
            {
                var root = tree.GetRoot();
                var semanticModel = compilation.GetSemanticModel(tree);

                // 3. Varre as Classes
                var classes = root.DescendantNodes().OfType<ClassDeclarationSyntax>();
                var classeOrigin = rootOrigin.DescendantNodes().OfType<ClassDeclarationSyntax>();

                foreach (var clsOrigin in classeOrigin)
                {

                    foreach (var cls in classes)
                    {


                        // 4. Campos (Fields) da Classe
                        var campos = cls.Members.OfType<FieldDeclarationSyntax>();
                        foreach (var campo in campos)
                        {
                            string tipoCampo = campo.Declaration.Type.ToString();
                            foreach (var variavel in campo.Declaration.Variables)
                            {

                                if (tipoCampo == clsOrigin.Identifier.Text)
                                {

                                    Console.ForegroundColor = ConsoleColor.Green;
                                    Console.Write($" -> ");
                                    Console.ResetColor();
                                    Console.Write($"Chama:");
                                    Console.ForegroundColor = ConsoleColor.Magenta;
                                    Console.Write($" {campo.Modifiers}");
                                    Console.ForegroundColor = ConsoleColor.Green;
                                    Console.Write($" {tipoCampo}");
                                    Console.Write($" {variavel.Identifier.Text}");
                                    Console.ForegroundColor = ConsoleColor.Blue;
                                    Console.Write($" --> ");
                                    Console.ResetColor();
                                    Console.Write($"pertencente à classe ");
                                    Console.ForegroundColor = ConsoleColor.Green;
                                    Console.Write($" {cls.Identifier.Text}\n");
                                }
                            }
                        }
                        var metodos = cls.Members.OfType<MethodDeclarationSyntax>();
                        foreach (var metodo in metodos)
                        {
                            // 5.1. Variáveis Locais dentro do Método
                            var variaveisLocais = metodo.DescendantNodes().OfType<VariableDeclarationSyntax>();
                            foreach (var varLocal in variaveisLocais)
                            {
                                // Garante que é uma variável local declarada dentro do escopo do método
                                if (varLocal.Parent is LocalDeclarationStatementSyntax)
                                {
                                    string tipoVar = varLocal.Type.ToString();
                                    foreach (var v in varLocal.Variables)
                                    {
                                        if (tipoVar == clsOrigin.Identifier.Text)
                                        {
                                            Console.ForegroundColor = ConsoleColor.Green;
                                            Console.Write($" -> ");                                            
                                            Console.ResetColor();
                                            Console.Write($"Chama:");
                                            Console.ForegroundColor = ConsoleColor.Magenta;
                                            Console.Write($" Local");
                                            Console.ForegroundColor = ConsoleColor.Green;
                                            Console.Write($" {tipoVar}");
                                            Console.Write($" {v.Identifier.Text}");
                                            Console.ForegroundColor = ConsoleColor.Blue;
                                            Console.Write($" --> ");
                                            Console.ResetColor();
                                            Console.Write($"pertencente à classe ");
                                            Console.ForegroundColor = ConsoleColor.Green;
                                            Console.Write($" {cls.Identifier.Text}\n");
                                        }
                                    }
                                }
                            }
                        }
                    }

                }
            }
            Console.WriteLine();
        }

        public void ShowReferenciaTodasClasses(string nome)
        {
            if (BuscaClasseNode(nome) == null || nome == null)
            {
                IError error = new Error();
                error.Erro("CLASSE NÃO ENCONTRADA");
                return;
            }
            
            foreach (var tree in syntaxTrees)
            {
                var root = tree.GetRoot();
                var semanticModel = compilation.GetSemanticModel(tree);
                var classes = root.DescendantNodes().OfType<ClassDeclarationSyntax>();
                foreach (var cls in classes)
                {

                    // 6. Referências e Chamadas (Mapeia o uso de métodos e classes de outros arquivos)
                    var invocacoes = root.DescendantNodes().OfType<InvocationExpressionSyntax>();
                    foreach (var invocacao in invocacoes)
                    {
                        var simboloInfo = semanticModel.GetSymbolInfo(invocacao);
                        if (simboloInfo.Symbol is IMethodSymbol metodoSimbolo)
                        {
                            if (metodoSimbolo.ContainingType.Name == nome)
                            {
                                
                                var linhaInfo = tree.GetLineSpan(invocacao.Span);
                                int numeroLinha = linhaInfo.StartLinePosition.Line + 1;                            
                                                                
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.Write($" -> ");
                                Console.ResetColor();
                                Console.Write($"Chama:");
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Console.Write($" {numeroLinha}");
                                Console.ForegroundColor = ConsoleColor.Magenta;
                                Console.Write($" {cls.Identifier.Text}");
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.Write($" {metodoSimbolo.Name}()");
                                Console.ForegroundColor = ConsoleColor.Blue;
                                Console.Write($" --> ");
                                Console.ResetColor();
                                Console.Write($"pertencente à classe ");
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.Write($" {metodoSimbolo.ContainingType.Name}\n");
                                
                            }

                        }
                    }
                }
            }
            Console.ResetColor();
            Console.WriteLine();
        }

        
    }
    
}
