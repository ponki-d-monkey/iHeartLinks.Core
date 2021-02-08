using System;
using FluentAssertions;
using Moq;
using Xunit;

namespace iHeartLinks.Core.Tests
{
    public sealed class HypermediaBuilderTests
    {
        private readonly Mock<IHypermediaDocument> mockDocument;
        private readonly HypermediaBuilder<IHypermediaDocument> sut;

        public HypermediaBuilderTests()
        {
            mockDocument = new Mock<IHypermediaDocument>();
            sut = new HypermediaBuilder<IHypermediaDocument>(Mock.Of<IHypermediaService>(), mockDocument.Object);
        }

        [Fact]
        public void CtorShouldThrowArgumentNullExceptionWhenServiceIsNull()
        {
            Action action = () => new HypermediaBuilder<IHypermediaDocument>(null, Mock.Of<IHypermediaDocument>());

            action.Should().Throw<ArgumentNullException>().Which.ParamName.Should().Be("service");
        }

        [Fact]
        public void CtorShouldThrowArgumentNullExceptionWhenDocumentIsNull()
        {
            Action action = () => new HypermediaBuilder<IHypermediaDocument>(Mock.Of<IHypermediaService>(), null);

            action.Should().Throw<ArgumentNullException>().Which.ParamName.Should().Be("document");
        }

        [Fact]
        public void CtorShouldInitializeProperties()
        {
            sut.Service.Should().NotBeNull();
            sut.Document.Should().NotBeNull();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void AddLinkShouldThrowArgumentExceptionWhenRelIsNullOrWhitespace(string rel)
        {
            Func<IHypermediaBuilder<IHypermediaDocument>> func = () => sut.AddLink(rel, CreateValidLink());

            var exception = func.Should().Throw<ArgumentException>().Which;
            exception.Message.Should().Be("Parameter 'rel' must not be null or empty.");
            exception.ParamName.Should().BeNull();
        }

        [Fact]
        public void AddLinkShouldThrowArgumentNullExceptionWhenLinkIsNull()
        {
            Func<IHypermediaBuilder<IHypermediaDocument>> func = () => sut.AddLink("link", null);

            func.Should().Throw<ArgumentNullException>().Which.ParamName.Should().Be("link");
        }

        [Fact]
        public void AddLinkShouldInvokeDocumentAddLinkMethod()
        {
            sut.AddLink("link", CreateValidLink());

            mockDocument.Verify(x => x.AddLink(It.Is<string>(y => y == "link"), It.Is<Link>(y => y != null)), Times.Once);
        }

        [Fact]
        public void AddLinkShouldReturnSameInstanceOfHypermediaBuilder()
        {
            var result = sut.AddLink("link", CreateValidLink());

            result.Should().BeSameAs(sut);
        }

        private Link CreateValidLink()
        {
            return new Link("https://iheartlinks.example.com");
        }
    }
}
