﻿using System;
using ApiClientCodeGen.Tests.Common.Build;
using ApiClientCodeGen.Tests.Common.Fixtures;
using ChristianHelle.DeveloperTools.CodeGenerators.ApiClient.Core;
using ChristianHelle.DeveloperTools.CodeGenerators.ApiClient.Core.Generators;
using FluentAssertions;
using Moq;
using Xunit;

namespace ChristianHelle.DeveloperTools.CodeGenerators.ApiClient.IntegrationTests.Generators
{
    [Trait("Category", "SkipWhenLiveUnitTesting")]
    public class AutoRestCodeGeneratorTests : IClassFixture<AutoRestCodeGeneratorFixture>
    {
        private readonly AutoRestCodeGeneratorFixture fixture;

        public AutoRestCodeGeneratorTests(AutoRestCodeGeneratorFixture fixture)
        {
            this.fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
        }
        
        [SkippableFact(typeof(ProcessLaunchException))]
        public void AutoRest_CSharp_Generated_Code_NotNullOrWhitespace()
            => fixture.Code.Should().NotBeNullOrWhiteSpace();

        [SkippableFact(typeof(ProcessLaunchException))]
        public void AutoRest_CSharp_Reports_Progres()
            => fixture.ProgressReporterMock.Verify(
                c => c.Progress(It.IsAny<uint>(), It.IsAny<uint>()), 
                Times.AtLeastOnce);

        [SkippableFact(typeof(ProcessLaunchException))]
        public void Reads_AddCredentials_From_Options() 
            => fixture.OptionsMock.Verify(c => c.AddCredentials, Times.AtLeastOnce);

        [SkippableFact(typeof(ProcessLaunchException))]
        public void Reads_ClientSideValidation_From_Options() 
            => fixture.OptionsMock.Verify(c => c.ClientSideValidation, Times.AtLeastOnce);

        [SkippableFact(typeof(ProcessLaunchException))]
        public void Reads_OverrideClientName_From_Options() 
            => fixture.OptionsMock.Verify(c => c.OverrideClientName, Times.AtLeastOnce);

        [SkippableFact(typeof(ProcessLaunchException))]
        public void Reads_SyncMethods_From_Options() 
            => fixture.OptionsMock.Verify(c => c.SyncMethods, Times.AtLeastOnce);

        [SkippableFact(typeof(ProcessLaunchException))]
        public void Reads_UseDateTimeOffset_From_Options() 
            => fixture.OptionsMock.Verify(c => c.UseDateTimeOffset, Times.AtLeastOnce);

        [SkippableFact(typeof(ProcessLaunchException))]
        public void Reads_UseInternalConstructors_From_Options() 
            => fixture.OptionsMock.Verify(c => c.UseInternalConstructors, Times.AtLeastOnce);

        [SkippableFact(typeof(ProcessLaunchException))]
        public void GeneratedCode_Can_Build_In_NetCoreApp()
            => BuildHelper.BuildCSharp(
                ProjectTypes.DotNetCoreApp,
                fixture.Code,
                SupportedCodeGenerator.AutoRest);

        [SkippableFact(typeof(ProcessLaunchException))]
        public void GeneratedCode_Can_Build_In_NetStandardLibrary()
            => BuildHelper.BuildCSharp(
                ProjectTypes.DotNetStandardLibrary,
                fixture.Code,
                SupportedCodeGenerator.AutoRest);
    }
}
