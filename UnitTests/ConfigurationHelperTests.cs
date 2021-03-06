
//https://stackoverflow.com/questions/39791634/read-appsettings-json-values-in-net-core-test-project
namespace UnitTests
{
    using Project1;
    using NUnit.Framework;
    using Microsoft.Extensions.Configuration;
    using System.Linq;
    using System;
    using FluentAssertions;
    using System.Collections.Generic;
    
    public class Tests
    {
        IConfiguration configuration;

        IConfigurationHelper sut;

        [SetUp]
        public void Setup()
        {
            configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            sut = new ConfigurationHelper(configuration);
        }

        [Test]
        public void ShouldGetKeyAndUriHost()
        {
            // Arrange
            const string expectedKey = "ServerOne";
            var expectedUri = new Uri("https://localhost.com");
            
            // Act
            var results =
                sut.GetUriFromConfigurationSection(new string[] { 
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
                sut.GetUriFromConfigurationSection(new string[] {
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
                sut.GetUriFromConfigurationSection(new string[] {
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
                sut.GetUriFromConfigurationSection(new string[] {
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
            // Arrange
            sut = new ConfigurationHelper(null);

            // Act
            var result = sut.GetUriFromConfigurationSection(null);
            
            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<Dictionary<string, string>>();
            result.Any().Should().BeFalse();
        }

        [Test]
        public void ShouldGetEmptyDictionaryWhenNullConfiguration()
        {
            // Act
            var result = sut.GetUriFromConfigurationSection(null);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<Dictionary<string, string>>();
            result.Any().Should().BeFalse();
        }

        [Test]
        public void ShouldGetEmptyDictionaryWhenEmptyList()
        {
            // Act
            var result = sut.GetUriFromConfigurationSection(new string[0]);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<Dictionary<string, string>>();
            result.Any().Should().BeFalse();
        }
    }
}