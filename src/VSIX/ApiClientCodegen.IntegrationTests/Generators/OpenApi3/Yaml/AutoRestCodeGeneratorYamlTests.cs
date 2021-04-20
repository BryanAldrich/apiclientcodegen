﻿using System;
using ApiClientCodeGen.Tests.Common.Fixtures.OpenApi3.Yaml;
using ChristianHelle.DeveloperTools.CodeGenerators.ApiClient.Core;
using ChristianHelle.DeveloperTools.CodeGenerators.ApiClient.Core.Generators;
using FluentAssertions;
using Moq;
using Xunit;

namespace ChristianHelle.DeveloperTools.CodeGenerators.ApiClient.IntegrationTests.Generators.OpenApi3.Yaml
{
    [Trait("Category", "SkipWhenLiveUnitTesting")]
    public class AutoRestCodeGeneratorYamlTests : IClassFixture<AutoRestCodeGeneratorFixture>
    {
        private readonly AutoRestCodeGeneratorFixture fixture;

        public AutoRestCodeGeneratorYamlTests(AutoRestCodeGeneratorFixture fixture)
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
    }
}
