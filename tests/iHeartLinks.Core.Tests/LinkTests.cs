using System;
using FluentAssertions;
using Xunit;

namespace iHeartLinks.Core.Tests
{
    public sealed class LinkTests
    {
        [Fact]
        public void CtorShouldInstantiateObjectWithHref()
        {
            var href = "https://iheartlinks.example.com";
            var link = new Link(href);

            link.Href.Should().Be(href);
            link.Method.Should().BeNull();
        }

        [Fact]
        public void CtorShouldInstantiateObjectWithMethod()
        {
            var href = "https://iheartlinks.example.com";
            var method = "GET";
            var link = new Link(href, method);

            link.Href.Should().Be(href);
            link.Method.Should().Be(method);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void CtorShouldThrowArgumentExceptionWhen(string href)
        {
            Action action = () => new Link(href);

            var exception = action.Should().Throw<ArgumentException>().Which;
            exception.Message.Should().Be("Parameter 'href' must not be null or empty.");
            exception.ParamName.Should().BeNull();
        }

        [Theory]
        [InlineData(null, "GET")]
        [InlineData("", "GET")]
        [InlineData(" ", "GET")]
        public void CtorWithMethodShouldThrowArgumentExceptionWhen(string href, string method)
        {
            Action action = () => new Link(href, method);

            var exception = action.Should().Throw<ArgumentException>().Which;
            exception.Message.Should().Be("Parameter 'href' must not be null or empty.");
            exception.ParamName.Should().BeNull();
        }
    }
}
