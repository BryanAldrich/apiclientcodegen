﻿using System;
using ChristianHelle.DeveloperTools.CodeGenerators.ApiClient.Core;
using ChristianHelle.DeveloperTools.CodeGenerators.ApiClient.Core.Extensions;
using ChristianHelle.DeveloperTools.CodeGenerators.ApiClient.Core.Generators;
using ChristianHelle.DeveloperTools.CodeGenerators.ApiClient.Core.Generators.AutoRest;
using ChristianHelle.DeveloperTools.CodeGenerators.ApiClient.Core.Generators.NSwag;
using ChristianHelle.DeveloperTools.CodeGenerators.ApiClient.Core.Generators.OpenApi;
using ChristianHelle.DeveloperTools.CodeGenerators.ApiClient.Core.Generators.Swagger;
using ChristianHelle.DeveloperTools.CodeGenerators.ApiClient.Core.Installer;
using ChristianHelle.DeveloperTools.CodeGenerators.ApiClient.Core.Logging;
using ChristianHelle.DeveloperTools.CodeGenerators.ApiClient.Core.Options;
using ChristianHelle.DeveloperTools.CodeGenerators.ApiClient.Core.Options.AutoRest;
using ChristianHelle.DeveloperTools.CodeGenerators.ApiClient.Core.Options.General;
using ChristianHelle.DeveloperTools.CodeGenerators.ApiClient.Core.Options.NSwag;
using ChristianHelle.DeveloperTools.CodeGenerators.ApiClient.Core.Options.OpenApiGenerator;
using ChristianHelle.DeveloperTools.CodeGenerators.ApiClient.Options;
using ChristianHelle.DeveloperTools.CodeGenerators.ApiClient.Options.AutoRest;
using ChristianHelle.DeveloperTools.CodeGenerators.ApiClient.Options.General;
using ChristianHelle.DeveloperTools.CodeGenerators.ApiClient.Options.NSwag;
using ChristianHelle.DeveloperTools.CodeGenerators.ApiClient.Options.OpenApiGenerator;
using OpenApiDocumentFactory =
    ChristianHelle.DeveloperTools.CodeGenerators.ApiClient.Generators.NSwag.OpenApiDocumentFactory;

namespace ChristianHelle.DeveloperTools.CodeGenerators.ApiClient.Generators
{
    public class CodeGeneratorFactory : ICodeGeneratorFactory
    {
        private readonly IDependencyInstaller dependencyInstaller;
        private readonly IRemoteLogger remoteLogger;
        private readonly IOpenApiDocumentFactory documentFactory;
        private readonly IProcessLauncher processLauncher;
        private readonly IOptionsFactory optionsFactory;

        public CodeGeneratorFactory(
            IOptionsFactory optionsFactory = null,
            IProcessLauncher processLauncher = null,
            IOpenApiDocumentFactory documentFactory = null,
            IRemoteLogger remoteLogger = null,
            IDependencyInstaller dependencyInstaller = null)
        {
            this.remoteLogger = remoteLogger ?? Logger.Instance;
            this.optionsFactory = optionsFactory ?? new OptionsFactory();
            this.processLauncher = processLauncher ?? new ProcessLauncher();
            this.documentFactory = documentFactory ?? new OpenApiDocumentFactory();
            this.dependencyInstaller = dependencyInstaller ??
                                       new DependencyInstaller(
                                           new NpmInstaller(this.processLauncher),
                                           new FileDownloader(new WebDownloader()));
        }

        public ICodeGenerator Create(
            string defaultNamespace,
            string inputFileContents,
            string inputFilePath,
            SupportedLanguage language,
            SupportedCodeGenerator generator)
        {
            remoteLogger.TrackFeatureUsage(generator.GetName());

            switch (generator)
            {
                case SupportedCodeGenerator.AutoRest:
                    return new AutoRestCSharpCodeGenerator(
                        inputFilePath,
                        defaultNamespace,
                        optionsFactory.Create<IAutoRestOptions, AutoRestOptionsPage>(),
                        processLauncher,
                        documentFactory,
                        dependencyInstaller);

                case SupportedCodeGenerator.NSwag:
                    return new NSwagCSharpCodeGenerator(
                        inputFilePath,
                        new OpenApiDocumentFactory(),
                        new NSwagCodeGeneratorSettingsFactory(
                            defaultNamespace,
                            optionsFactory.Create<INSwagOptions, NSwagOptionsPage>()));

                case SupportedCodeGenerator.Swagger:
                    return new SwaggerCSharpCodeGenerator(
                        inputFilePath,
                        defaultNamespace,
                        optionsFactory.Create<IGeneralOptions, GeneralOptionPage>(),
                        processLauncher,
                        dependencyInstaller);

                case SupportedCodeGenerator.OpenApi:
                    return new OpenApiCSharpCodeGenerator(
                        inputFilePath,
                        defaultNamespace,
                        optionsFactory.Create<IGeneralOptions, GeneralOptionPage>(),
                        optionsFactory.Create<IOpenApiGeneratorOptions, OpenApiGeneratorOptionsPage>(),
                        processLauncher,
                        dependencyInstaller);

                default:
                    throw new NotSupportedException();
            }
        }
    }
}