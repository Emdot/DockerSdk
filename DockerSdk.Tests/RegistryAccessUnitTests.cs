﻿using DockerSdk.Registries;
using DockerSdk.Registries.Dto;
using FluentAssertions;
using Xunit;

namespace DockerSdk.Tests
{
    // These tests can run in parallel with other tests.
    public class RegistryAccessUnitTests
    {
        [Fact]
        public void AddAnonymous_AddsEmptyAuthObject()
        {
            var access = new RegistryAccess(null!);

            access.AddAnonymous("abc");

            access.TryGetAuthObject("abc", out AuthConfig? actual).Should().BeTrue(); ;
            actual.Should().BeEquivalentTo(new AuthConfig { ServerAddress = "abc" });
        }

        [Fact]
        public void AddBasicAuth_AddsUserPasswordAuthObject()
        {
            var access = new RegistryAccess(null!);

            access.AddBasicAuth("abc", "donald", "duck");

            access.TryGetAuthObject("abc", out AuthConfig? actual).Should().BeTrue(); ;
            actual.Should().BeEquivalentTo(new AuthConfig { ServerAddress = "abc", Username = "donald", Password = "duck" });
        }

        [Fact]
        public void AddBasicAuth_ToAnonymousRegistry_ConvertsEntryToBasicAuth()
        {
            var access = new RegistryAccess(null!);

            access.AddBasicAuth("docker.io", "donald", "duck");

            access.TryGetAuthObject("docker.io", out AuthConfig? actual).Should().BeTrue(); ;
            actual.Should().BeEquivalentTo(new AuthConfig { ServerAddress = "docker.io", Username = "donald", Password = "duck" });
        }

        [Fact]
        public void AddIdentityToken_AddsIdentityTokenAuthObject()
        {
            var access = new RegistryAccess(null!);

            access.AddIdentityToken("abc", "123-456-789");

            access.TryGetAuthObject("abc", out AuthConfig? actual).Should().BeTrue(); ;
            actual.Should().BeEquivalentTo(new AuthConfig { ServerAddress = "abc", IdentityToken = "123-456-789" });
        }

        [Fact]
        public void Clear_PreservesBuiltIns()
        {
            var access = new RegistryAccess(null!);

            access.Clear();

            access.Registries.Should().Contain(new RegistryReference("docker.io"));
        }

        [Theory]
        [InlineData("test", "docker.io")]
        [InlineData("example.com/test", "example.com")]
        [InlineData("example:123/test", "example:123")]
        [InlineData("example/test:123", "docker.io")]
        [InlineData("example.com", "docker.io")]
        [InlineData("example.com:123", "docker.io")]
        [InlineData("example.com/test:0123", "example.com")]
        [InlineData("example.com:0123/test", "example.com:123")]
        [InlineData("Example/test", "example")]
        public void GetRegistryName_ProducesExpectedOutput(string imageName, string expected)
        {
            var access = new RegistryAccess(null!);

            var actual = access.GetRegistryName(imageName);

            actual.ToString().Should().Be(expected);
        }

        [Fact]
        public void Remove_RemovesEntry()
        {
            var access = new RegistryAccess(null!);

            access.Remove("docker.io");

            access.TryGetAuthObject("docker.io", out _).Should().BeFalse();
        }

        [Fact]
        public void TryGetAuthObject_UnknownRegistry_ReturnsFalse()
        {
            var access = new RegistryAccess(null!);

            var result = access.TryGetAuthObject("example.com", out _);

            result.Should().BeFalse();
        }
    }
}
