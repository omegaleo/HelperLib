using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace OmegaLeo.HelperLib.CommentGenerator;

class Program
{
    static int Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("Usage: dotnet run <directory-path>");
            Console.WriteLine("Generates triple-slash comments from DocumentationAttribute in C# files.");
            return 1;
        }

        var directory = args[0];
        if (!Directory.Exists(directory))
        {
            Console.Error.WriteLine($"Error: Directory '{directory}' does not exist.");
            return 1;
        }

        var csFiles = Directory.GetFiles(directory, "*.cs", SearchOption.AllDirectories)
            .Where(f => !f.Contains("/obj/") && !f.Contains("\\obj\\") &&
                       !f.Contains("/bin/") && !f.Contains("\\bin\\"))
            .ToList();

        Console.WriteLine($"[CommentGenerator] Processing {csFiles.Count} C# files in {directory}...");

        int modifiedCount = 0;
        foreach (var filePath in csFiles)
        {
            if (ProcessFile(filePath))
            {
                modifiedCount++;
            }
        }

        Console.WriteLine($"[CommentGenerator] ✅ Complete! Modified {modifiedCount} files.");
        return 0;
    }

    static bool ProcessFile(string filePath)
    {
        try
        {
            var code = File.ReadAllText(filePath);
            var tree = CSharpSyntaxTree.ParseText(code);
            var root = tree.GetRoot();

            var rewriter = new DocumentationCommentRewriter();
            var newRoot = rewriter.Visit(root);

            if (rewriter.Modified)
            {
                File.WriteAllText(filePath, newRoot.ToFullString());
                Console.WriteLine($"  ✓ {Path.GetFileName(filePath)}");
                return true;
            }

            return false;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"  ✗ Error processing {Path.GetFileName(filePath)}: {ex.Message}");
            return false;
        }
    }
}

class DocumentationCommentRewriter : CSharpSyntaxRewriter
{
    public bool Modified { get; private set; }

    public override SyntaxNode? VisitClassDeclaration(ClassDeclarationSyntax node)
    {
        node = (ClassDeclarationSyntax)base.VisitClassDeclaration(node)!;
        return AddDocumentationComment(node);
    }

    public override SyntaxNode? VisitStructDeclaration(StructDeclarationSyntax node)
    {
        node = (StructDeclarationSyntax)base.VisitStructDeclaration(node)!;
        return AddDocumentationComment(node);
    }

    public override SyntaxNode? VisitMethodDeclaration(MethodDeclarationSyntax node)
    {
        node = (MethodDeclarationSyntax)base.VisitMethodDeclaration(node)!;
        return AddDocumentationComment(node);
    }

    public override SyntaxNode? VisitPropertyDeclaration(PropertyDeclarationSyntax node)
    {
        node = (PropertyDeclarationSyntax)base.VisitPropertyDeclaration(node)!;
        return AddDocumentationComment(node);
    }

    private T AddDocumentationComment<T>(T node) where T : MemberDeclarationSyntax
    {
        // Check if already has /// comments
        if (HasXmlDocumentation(node))
        {
            return node;
        }

        // Find DocumentationAttribute
        var docAttr = node.AttributeLists
            .SelectMany(al => al.Attributes)
            .FirstOrDefault(a => a.Name.ToString().Contains("Documentation"));

        if (docAttr == null)
        {
            return node;
        }

        // Extract attribute arguments
        var args = docAttr.ArgumentList?.Arguments;
        if (args == null || args.Value.Count < 2)
        {
            return node;
        }

        // Get description (2nd argument - index 1)
        var description = GetStringLiteral(args.Value[1].Expression);
        
        if (string.IsNullOrWhiteSpace(description))
        {
            return node;
        }

        // Get args array (3rd argument - index 2) if present
        string[]? paramDescriptions = null;
        if (args.Value.Count >= 3)
        {
            paramDescriptions = GetStringArray(args.Value[2].Expression);
        }

        // Generate XML documentation comment text
        var commentText = GenerateXmlCommentText(node, description, paramDescriptions);
        
        // Add the comment as leading trivia
        var existingLeadingTrivia = node.GetLeadingTrivia();
        var newTrivia = SyntaxFactory.ParseLeadingTrivia(commentText);
        var combinedTrivia = existingLeadingTrivia.AddRange(newTrivia);
        
        Modified = true;
        return node.WithLeadingTrivia(combinedTrivia);
    }

    private bool HasXmlDocumentation(SyntaxNode node)
    {
        return node.GetLeadingTrivia()
            .Any(t => t.IsKind(SyntaxKind.SingleLineDocumentationCommentTrivia) ||
                     t.IsKind(SyntaxKind.MultiLineDocumentationCommentTrivia));
    }

    private string? GetStringLiteral(ExpressionSyntax expr)
    {
        if (expr is LiteralExpressionSyntax literal && 
            literal.IsKind(SyntaxKind.StringLiteralExpression))
        {
            return literal.Token.ValueText;
        }
        
        // Handle verbatim strings @"..."
        var text = expr.ToString();
        if (text.StartsWith("@\"") && text.EndsWith("\""))
        {
            return text.Substring(2, text.Length - 3);
        }

        return null;
    }

    private string[]? GetStringArray(ExpressionSyntax expr)
    {
        if (expr.ToString() == "null")
        {
            return null;
        }

        if (expr is ImplicitArrayCreationExpressionSyntax implicitArray)
        {
            return implicitArray.Initializer.Expressions
                .Select(e => GetStringLiteral(e))
                .Where(s => s != null)
                .ToArray()!;
        }

        if (expr is ArrayCreationExpressionSyntax arrayExpr && arrayExpr.Initializer != null)
        {
            return arrayExpr.Initializer.Expressions
                .Select(e => GetStringLiteral(e))
                .Where(s => s != null)
                .ToArray()!;
        }

        return null;
    }

    private string GenerateXmlCommentText(
        MemberDeclarationSyntax node,
        string description,
        string[]? paramDescriptions)
    {
        var sb = new StringBuilder();
        var indent = GetIndentation(node);

        // Add <summary>
        sb.AppendLine($"{indent}/// <summary>");
        sb.AppendLine($"{indent}/// {description}");
        sb.AppendLine($"{indent}/// </summary>");

        // Add <typeparam> for generic types
        if (node is TypeDeclarationSyntax typeDecl && typeDecl.TypeParameterList != null)
        {
            foreach (var typeParam in typeDecl.TypeParameterList.Parameters)
            {
                sb.AppendLine($"{indent}/// <typeparam name=\"{typeParam.Identifier.Text}\">The {typeParam.Identifier.Text} type parameter</typeparam>");
            }
        }

        // Add <param> tags for methods
        if (node is MethodDeclarationSyntax method)
        {
            var parameters = method.ParameterList.Parameters;
            foreach (var param in parameters)
            {
                var paramDesc = GetParameterDescription(param.Identifier.Text, paramDescriptions);
                sb.AppendLine($"{indent}/// <param name=\"{param.Identifier.Text}\">{paramDesc}</param>");
            }

            // Add <returns> if not void
            if (!method.ReturnType.ToString().Contains("void"))
            {
                sb.AppendLine($"{indent}/// <returns>The return value</returns>");
            }
        }

        return sb.ToString();
    }

    private string GetIndentation(SyntaxNode node)
    {
        var trivia = node.GetLeadingTrivia();
        var whitespace = trivia.LastOrDefault(t => t.IsKind(SyntaxKind.WhitespaceTrivia));
        return whitespace.ToString();
    }

    private string GetParameterDescription(string paramName, string[]? paramDescriptions)
    {
        if (paramDescriptions == null)
        {
            return $"The {paramName} parameter";
        }

        foreach (var desc in paramDescriptions)
        {
            if (desc.StartsWith($"{paramName}:", StringComparison.OrdinalIgnoreCase))
            {
                return desc.Substring(paramName.Length + 1).Trim();
            }
        }

        return $"The {paramName} parameter";
    }
}
