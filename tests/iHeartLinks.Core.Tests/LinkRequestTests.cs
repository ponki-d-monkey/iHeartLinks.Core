using System;
using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace iHeartLinks.Core.Tests
{
    public sealed class LinkRequestTests
    {
        private const string TestKey = "TestKey";
        private const int TestValue = 1;
        private const string TestNonExistingKey = "TestNonExistingKey";

        private readonly LinkRequest sut;

        public LinkRequestTests()
        {
            sut = new LinkRequest(new Dictionary<string, object>
            {
                { TestKey, TestValue }
            });
        }

        public static IEnumerable<object[]> InvalidRequestParameters = new List<object[]>
        {
            new[] { default(IDictionary<string, object>) },
            new[] { new Dictionary<string, object>() }
        };

        [Theory]
        [MemberData(nameof(InvalidRequestParameters))]
        public void CtorShouldThrowArgumentExceptionWhenRequestParametersIs(IDictionary<string, object> requestParameters)
        {
            Action action = () => new LinkRequest(requestParameters);

            var exception = action.Should().Throw<ArgumentException>().Which;
            exception.Message.Should().Be("Parameter 'requestParameters' must not be null or empty.");
            exception.ParamName.Should().BeNull();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void ContainsKeyShouldThrowArgumentExceptionWhenKeyIs(string key)
        {
            Func<bool> func = () => sut.ContainsKey(key);

            var exception = func.Should().Throw<ArgumentException>().Which;
            exception.Message.Should().Be("Parameter 'key' must not be null or empty.");
            exception.ParamName.Should().BeNull();
        }

        [Fact]
        public void ContainsKeyShouldReturnTrueWhenKeyExists()
        {
            var result = sut.ContainsKey(TestKey);

            result.Should().BeTrue();
        }

        [Fact]
        public void ContainsKeyShouldReturnFalseWhenKeyDoesNotExist()
        {
            var result = sut.ContainsKey(TestNonExistingKey);

            result.Should().BeFalse();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void GetValueOrDefaultShouldThrowArgumentExceptionWhenKeyIs(string key)
        {
            Func<object> func = () => sut.GetValueOrDefault(key);

            var exception = func.Should().Throw<ArgumentException>().Which;
            exception.Message.Should().Be("Parameter 'key' must not be null or empty.");
            exception.ParamName.Should().BeNull();
        }

        [Fact]
        public void GetValueOrDefaultShouldReturnValueWhenValueExists()
        {
            var result = sut.GetValueOrDefault(TestKey);

            result.Should().Be(TestValue);
        }

        [Fact]
        public void GetValueOrDefaultShouldReturnNullWhenValueDoesNotExist()
        {
            var result = sut.GetValueOrDefault(TestNonExistingKey);

            result.Should().BeNull();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void GetValueOrDefaultWithTypeShouldThrowArgumentExceptionWhenKeyIs(string key)
        {
            Func<object> func = () => sut.GetValueOrDefault<int>(key);

            var exception = func.Should().Throw<ArgumentException>().Which;
            exception.Message.Should().Be("Parameter 'key' must not be null or empty.");
            exception.ParamName.Should().BeNull();
        }

        [Fact]
        public void GetValueOrDefaultWithTypeShouldReturnValueWhenValueExists()
        {
            var result = sut.GetValueOrDefault<int>(TestKey);

            result.Should().Be(TestValue);
        }

        [Fact]
        public void GetValueOrDefaultWithTypeShouldReturnDefaultWhenValueDoesNotExist()
        {
            var result = sut.GetValueOrDefault<int>(TestNonExistingKey);

            result.Should().Be(default);
        }

        [Fact]
        public void GetValueOrDefaultWithTypeShouldThrowInvalidCastExceptionWhenValueAndTypeDoNotMatch()
        {
            Func<string> func = () => sut.GetValueOrDefault<string>(TestKey);

            func.Should().Throw<InvalidCastException>();
        }
    }
}
