using System;
using System.IO;
using System.Linq;
using DiagramaUML.Scripts;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class Program
{
    public static void Main(string[] args)
    {
       Main1 main = new Main1();
    }
}


/*
// Caminho da pasta onde estão os seus arquivos .cs
string diretorioProjeto = @"D:\Documentos\Estudos Aleatorios\DiagramaUML\PastaDeBusca\";

if (!Directory.Exists(diretorioProjeto))
{
    Console.WriteLine("Diretório não encontrado. Altere o caminho no código.");
    return;
}

string[] arquivosCs = Directory.GetFiles(diretorioProjeto, "*.cs", SearchOption.AllDirectories);

// 1. Carrega as árvores sintáticas e cria a compilação cruzada
var syntaxTrees = arquivosCs.Select(path => CSharpSyntaxTree.ParseText(File.ReadAllText(path), path: path)).ToList();
var references = new[] { MetadataReference.CreateFromFile(typeof(object).Assembly.Location) };
var compilation = CSharpCompilation.Create("AnaliseCompletaProjeto", syntaxTrees, references);

// 2. Analisa cada arquivo .cs encontrado
foreach (var tree in syntaxTrees)
{
    var root = tree.GetRoot();
    var semanticModel = compilation.GetSemanticModel(tree);

    Console.WriteLine($"\n==================================================");
    Console.WriteLine($"ARQUIVO: {Path.GetFileName(tree.FilePath)}");
    Console.WriteLine($"==================================================");

    // 3. Varre as Classes
    var classes = root.DescendantNodes().OfType<ClassDeclarationSyntax>();
    foreach (var cls in classes)
    {
        Console.WriteLine($"\n[CLASSE] {cls.Identifier.Text}");

        // 4. Campos (Fields) da Classe
        var campos = cls.Members.OfType<FieldDeclarationSyntax>();
        foreach (var campo in campos)
        {
            string tipoCampo = campo.Declaration.Type.ToString();
            foreach (var variavel in campo.Declaration.Variables)
            {
                Console.WriteLine($"  ├── [Campo] {tipoCampo} {variavel.Identifier.Text}");
            }
        }

        // 5. Métodos da Classe
        var metodos = cls.Members.OfType<MethodDeclarationSyntax>();
        foreach (var metodo in metodos)
        {
            Console.WriteLine($"  ├── [MÉTODO] {metodo.ReturnType} {metodo.Identifier.Text}(...)");

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
                        Console.WriteLine($"  │    ├── [Var Local] {tipoVar} {v.Identifier.Text}");
                    }
                }
            }

            // 5.2. Instanciações ('new') dentro do Método
            var instancias = metodo.DescendantNodes().OfType<ObjectCreationExpressionSyntax>();
            foreach (var inst in instancias)
            {
                Console.WriteLine($"  │    ├── [Instanciação / New] Objeto do tipo: {inst.Type}");
            }
        }
    }

    // 6. Referências e Chamadas (Mapeia o uso de métodos e classes de outros arquivos)
    Console.WriteLine($"\n  --- Referências e Chamadas Identificadas Neste Arquivo ---");
    var invocacoes = root.DescendantNodes().OfType<InvocationExpressionSyntax>();
    foreach (var invocacao in invocacoes)
    {
        var simboloInfo = semanticModel.GetSymbolInfo(invocacao);
        if (simboloInfo.Symbol is IMethodSymbol metodoSimbolo)
        {
            Console.WriteLine($"  -> Chama: '{metodoSimbolo.Name}' (pertencente à classe '{metodoSimbolo.ContainingType.Name}')");
        }
    }
}
*/
