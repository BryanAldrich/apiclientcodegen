﻿using System;
using System.Linq;
using ChristianHelle.DeveloperTools.CodeGenerators.ApiClient.Extensions;
using ChristianHelle.DeveloperTools.CodeGenerators.ApiClient.Options;
using Microsoft.VisualStudio.Shell.Interop;
using NSwag;
using NSwag.CodeGeneration.CSharp;

namespace ChristianHelle.DeveloperTools.CodeGenerators.ApiClient.Generators.NSwag
{
    public class NSwagCSharpCodeGenerator : ICodeGenerator
    {
        private readonly string swaggerFile;
        private readonly string defaultNamespace;
        private readonly NSwagCSharpOptions options;

        public NSwagCSharpCodeGenerator(string swaggerFile, string defaultNamespace, INSwagOptions options)
        {
            this.swaggerFile = swaggerFile ?? throw new ArgumentNullException(nameof(swaggerFile));
            this.defaultNamespace = defaultNamespace ?? throw new ArgumentNullException(nameof(defaultNamespace));
            this.options = new NSwagCSharpOptions(options ?? throw new ArgumentNullException(nameof(options)));
        }

        public string GenerateCode(IVsGeneratorProgress pGenerateProgress)
        {
            try
            {
                pGenerateProgress?.Progress(10);

                var document = OpenApiDocument
                    .FromFileAsync(swaggerFile)
                    .GetAwaiter()
                    .GetResult();

                pGenerateProgress?.Progress(20);

                var settings = new CSharpClientGeneratorSettings
                {
                    ClassName = GetClassName(document),
                    InjectHttpClient = options.InjectHttpClient,
                    GenerateClientInterfaces = options.GenerateClientInterfaces,
                    GenerateDtoTypes = options.GenerateDtoTypes,
                    UseBaseUrl = options.UseBaseUrl,
                    CSharpGeneratorSettings =
                    {
                        Namespace = defaultNamespace,
                        ClassStyle = options.ClassStyle
                    },
                };

                pGenerateProgress?.Progress(50);

                var generator = new CSharpClientGenerator(document, settings);
                return generator.GenerateFile();
            }
            finally
            {
                pGenerateProgress?.Progress(90);
            }
        }

        private static string GetClassName(OpenApiDocument document)
            => string.IsNullOrWhiteSpace(document.Info?.Title)
                ? "ApiClient"
                : $"{SanitizeTitle(document)}Client";

        private static string SanitizeTitle(OpenApiDocument document)
            => RemoveCharacters(
                document.Info.Title,
                "Swagger", " ", ".", "-");

        private static string RemoveCharacters(string source, params string[] removeChars)
            => removeChars.Aggregate(
                source,
                (current, c) => current.Replace(c, string.Empty));
    }
}
