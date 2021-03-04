using System;
using FluentAssertions;
using Moq;
using Xunit;

namespace iHeartLinks.Core.Tests
{
    public sealed class HypermediaServiceExtensionTests
    {
        private const string TestHref = "https://iheartlinks.example.com";

        private readonly Mock<IHypermediaDocument> mockDocument;
        private readonly Mock<IHypermediaService> mockSut;
        private readonly IHypermediaService sut;

        public HypermediaServiceExtensionTests()
        {
            mockDocument = new Mock<IHypermediaDocument>();

            mockSut = new Mock<IHypermediaService>();
            mockSut.Setup(x => x.GetLink()).Returns(new Link(TestHref));

            sut = mockSut.Object;
        }

        [Fact]
        public void AddSelfShouldThrowArgumentNullExceptionWhenServiceIsNull()
        {
            Func<IHypermediaBuilder<IHypermediaDocument>> func = () => default(IHypermediaService).AddSelf(mockDocument.Object);

            func.Should().Throw<ArgumentNullException>().Which.ParamName.Should().Be("service");

            mockSut.Verify(x => x.GetLink(), Times.Never);

            mockDocument.Verify(x => x.AddLink(It.IsAny<string>(), It.IsAny<Link>()), Times.Never);
        }

        [Fact]
        public void AddSelfShouldThrowArgumentNullExceptionWhenDocumentIsNull()
        {
            Func<IHypermediaBuilder<IHypermediaDocument>> func = () => sut.AddSelf(default(IHypermediaDocument));

            func.Should().Throw<ArgumentNullException>().Which.ParamName.Should().Be("document");

            mockSut.Verify(x => x.GetLink(), Times.Never);

            mockDocument.Verify(x => x.AddLink(It.IsAny<string>(), It.IsAny<Link>()), Times.Never);
        }

        [Fact]
        public void AddSelfShouldInvokeHypermediaServiceGetLinkMethod()
        {
            sut.AddSelf(mockDocument.Object);

            mockSut.Verify(x => x.GetLink(), Times.Once);
        }

        [Fact]
        public void AddSelfShouldInvokeDocumentAddLinkMethod()
        {
            sut.AddSelf(mockDocument.Object);

            mockDocument.Verify(x =>
                x.AddLink(
                    It.Is<string>(y => y == "self"),
                    It.Is<Link>(y => y.Href == TestHref)),
                Times.Once);
        }

        [Fact]
        public void AddSelfShouldReturnHypermediaBuilder()
        {
            var result = sut.AddSelf(mockDocument.Object);

            result.Should().NotBeNull();
        }

        [Fact]
        public void PrepareShouldThrowArgumentNullExceptionWhenServiceIsNull()
        {
            Func<IHypermediaBuilder<IHypermediaDocument>> func = () => default(IHypermediaService).Prepare(mockDocument.Object);

            func.Should().Throw<ArgumentNullException>().Which.ParamName.Should().Be("service");
        }

        [Fact]
        public void PrepareShouldThrowArgumentNullExceptionWhenDocumentIsNull()
        {
            Func<IHypermediaBuilder<IHypermediaDocument>> func = () => sut.Prepare(default(IHypermediaDocument));

            func.Should().Throw<ArgumentNullException>().Which.ParamName.Should().Be("document");
        }

        [Fact]
        public void PrepareShouldReturnHypermediaBuilder()
        {
            var result = sut.Prepare(mockDocument.Object);

            result.Should().NotBeNull();
        }
    }
}
