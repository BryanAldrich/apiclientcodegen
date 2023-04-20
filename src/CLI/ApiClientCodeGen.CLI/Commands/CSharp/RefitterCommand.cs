﻿using McMaster.Extensions.CommandLineUtils;
using Rapicgen.Core;
using Rapicgen.Core.Generators;
using Rapicgen.Core.Generators.Refitter;
using Rapicgen.Core.Logging;
using Rapicgen.Core.Options.Refitter;

namespace Rapicgen.CLI.Commands.CSharp;

[Command("refitter", Description = "Refitter (v0.4.1)")]
public class RefitterCommand : CodeGeneratorCommand, IRefitterOptions
{
    private readonly IRefitterOptions options;

    public RefitterCommand(
        IConsoleOutput console,
        IProgressReporter? progressReporter,
        IRefitterOptions options)
        : base(console, progressReporter)
    {
        this.options = options;
    }

    public override ICodeGenerator CreateGenerator() =>
        new RefitterCodeGenerator(SwaggerFile, DefaultNamespace, options);
    
    [Option(
        LongName = "generateContracts",
        Description = "Set this to FALSE to skip generating the contract types (default: TRUE)")]
    public bool GenerateContracts
    {
        get => options.GenerateContracts;
        set => options.GenerateContracts = value;
    }

    [Option(
        LongName = "generateXmlDocCodeComments",
        Description = "Set this to FALSE to skip generating XML doc style code comments (default: TRUE)")]
    public bool GenerateXmlDocCodeComments
    {
        get => options.GenerateXmlDocCodeComments;
        set => options.GenerateXmlDocCodeComments = value;
    }

    [Option(
        LongName = "autoGeneratedHeader",
        Description = "Set this to FALSE to skip generating <auto-generated> headers in C# files (default: TRUE)")]
    public bool AddAutoGeneratedHeader
    {
        get => options.AddAutoGeneratedHeader;
        set => options.AddAutoGeneratedHeader = value;
    }

    [Option(
        LongName = "returnApiResponse",
        Description = "Set this to TRUE to wrap the returned the contract types in IApiResponse<T> (default: FALSE)")]
    public bool ReturnIApiResponse
    {
        get => options.ReturnIApiResponse;
        set => options.ReturnIApiResponse = value;
    }
}