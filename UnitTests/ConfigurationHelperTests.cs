using Project1;
using NUnit.Framework;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System;
using FluentAssertions;
using FluentAssertions.Common;
using System.Collections.Generic;

//https://stackoverflow.com/questions/39791634/read-appsettings-json-values-in-net-core-test-project
namespace UnitTests
{
    public class Tests
    {
        IConfiguration configuration;

        [OneTimeSetUp]
        public void Setup()
        {
            configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
        }

        [Test]
        public void ShouldGetKeyAndUriHost()
        {
            // Arrange
            const string expectedKey = "ServerOne";
            var expectedUri = new Uri("https://localhost.com");
            
            // Act
            var results = 
                ConfigurationHelper.GetUriFromConfigurationSection(configuration, new string[] { 
                    "KafkaClients" 
                });

            // Assert
            results.Count().Should().Be(2);
            results.First().Key.Should().Be(expectedKey);
            results.First().Value.Should().Be(expectedUri.Host);
        }

        [Test]
        public void ShouldNotContainUrlOrUri()
        {
            // Arrange
            // Act
            var results =
                ConfigurationHelper.GetUriFromConfigurationSection(configuration, new string[] {
                    "MicroservicesConfig"
                });

            // Assert
            results.Count.Should().Be(6);
            foreach (var item in results)
            {
                item.Key.ToLower()
                    .Should().NotContain("uri")
                    .And.NotContain("url");
            }
        }

        [Test]
        public void ShouldGetOneResultWhenSameHost()
        {
            // Arrange
            var expectedUri = new Uri("https://localhost.com");

            // Act
            var results =
                ConfigurationHelper.GetUriFromConfigurationSection(configuration, new string[] {
                    "SameHostUri",
                    "SameHostUri2"
                });

            // Assert
            results.Count.Should().Be(1);
            results.First().Value.Should().Be(expectedUri.Host);
        }

        [Test]
        public void ShouldGetArrayValues()
        {
            // Arrange
            var expectedValue0 = "ArrayValues0";
            var expectedValue1 = "ArrayValues1";
            var expectedValue2 = "ArrayValues2";

            // Act
            var results =
                ConfigurationHelper.GetUriFromConfigurationSection(configuration, new string[] {
                    "SectionWithArrays"
                });

            // Assert
            results.Count.Should().Be(3);
            results.Keys.Contains(expectedValue0).Should().BeTrue();
            results.Keys.Contains(expectedValue1).Should().BeTrue();
            results.Keys.Contains(expectedValue2).Should().BeTrue();
        }

        [Test]
        public void ShouldGetEmptyDictionaryWhenNullInput()
        {
            // Act
            var result = ConfigurationHelper.GetUriFromConfigurationSection(null, null);
            
            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<Dictionary<string, string>>();
            result.Any().Should().BeFalse();
        }

        [Test]
        public void ShouldGetEmptyDictionaryWhenNullConfiguration()
        {
            // Act
            var result = ConfigurationHelper.GetUriFromConfigurationSection(configuration, null);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<Dictionary<string, string>>();
            result.Any().Should().BeFalse();
        }

        [Test]
        public void ShouldGetEmptyDictionaryWhenEmptyList()
        {
            // Act
            var result = ConfigurationHelper.GetUriFromConfigurationSection(configuration, new string[0]);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<Dictionary<string, string>>();
            result.Any().Should().BeFalse();
        }
    }
}