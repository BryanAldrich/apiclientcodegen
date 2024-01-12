﻿using Refitter.Core;
using System.Diagnostics.CodeAnalysis;
using Rapicgen.Core.Options.Refitter;
using System.Threading.Tasks;

namespace Rapicgen.Core.Generators.Refitter;

public class RefitterCodeGenerator : ICodeGenerator
{
    private readonly string swaggerFile;
    private readonly string defaultNamespace;
    private readonly IRefitterOptions options;

    public RefitterCodeGenerator(
        string swaggerFile,
        string defaultNamespace,
        IRefitterOptions options)
    {
        this.swaggerFile = swaggerFile;
        this.defaultNamespace = defaultNamespace;
        this.options = options;
    }

    [SuppressMessage(
        "Usage",
        "VSTHRD002:Avoid problematic synchronous waits",
        Justification = "This is run from a Visual Studio CustomTool")]
    public string GenerateCode(IProgressReporter? pGenerateProgress)
    {
        pGenerateProgress?.Progress(10);
        var generator = Task.Run(() => RefitGenerator.CreateAsync(
            new RefitGeneratorSettings
            {
                OpenApiPath = swaggerFile,
                Namespace = defaultNamespace,
                AddAutoGeneratedHeader = options.AddAutoGeneratedHeader,
                GenerateContracts = options.GenerateContracts,
                GenerateXmlDocCodeComments = options.GenerateXmlDocCodeComments,
                ReturnIApiResponse = options.ReturnIApiResponse,
                UseCancellationTokens = options.UseCancellationTokens,
                GenerateOperationHeaders = options.GenerateHeaderParameters,
            }))
            .GetAwaiter()
            .GetResult();

        pGenerateProgress?.Progress(50);
        var code = generator.Generate();

        pGenerateProgress?.Progress(90);
        return GeneratedCode.PrefixAutogeneratedCodeHeader(
            code,
            "Refitter",
            "v0.9.4");
    }
}