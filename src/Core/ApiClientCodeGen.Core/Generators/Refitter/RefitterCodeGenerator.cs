﻿using Refitter.Core;
using System.Diagnostics.CodeAnalysis;

namespace Rapicgen.Core.Generators.Refitter;

public class RefitterCodeGenerator : ICodeGenerator
{
    private readonly string swaggerFile;
    private readonly string defaultNamespace;

    public RefitterCodeGenerator(
        string swaggerFile,
        string defaultNamespace)
    {
        this.swaggerFile = swaggerFile;
        this.defaultNamespace = defaultNamespace;
    }

    [SuppressMessage(
        "Usage",
        "VSTHRD002:Avoid problematic synchronous waits",
        Justification = "This is run from a Visual Studio CustomTool")]
    public string GenerateCode(IProgressReporter? pGenerateProgress)
    {
        pGenerateProgress?.Progress(10);
        var generator = RefitGenerator.CreateAsync(
            new RefitGeneratorSettings
            {
                OpenApiPath = swaggerFile,
                Namespace = defaultNamespace,
                AddAutoGeneratedHeader = false
            })
            .GetAwaiter()
            .GetResult();

        pGenerateProgress?.Progress(50);
        var code = generator.Generate();

        pGenerateProgress?.Progress(90);
        return GeneratedCode.PrefixAutogeneratedCodeHeader(
            code,
            "Refitter",
            "v0.3.1");
    }
}