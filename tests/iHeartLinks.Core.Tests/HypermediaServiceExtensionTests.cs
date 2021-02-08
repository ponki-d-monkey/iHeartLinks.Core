using System;
using FluentAssertions;
using Moq;
using Xunit;

namespace iHeartLinks.Core.Tests
{
    public sealed class HypermediaServiceExtensionTests
    {
        private const string TestHref = "https://iheartlinks.example.com";
        private const string TestMethod = "GET";

        private readonly Mock<IHypermediaDocument> mockDocument;
        private readonly Mock<IHypermediaService> mockSut;
        private readonly IHypermediaService sut;

        public HypermediaServiceExtensionTests()
        {
            mockDocument = new Mock<IHypermediaDocument>();

            mockSut = new Mock<IHypermediaService>();
            mockSut.Setup(x => x.GetCurrentUrl()).Returns(TestHref);
            mockSut.Setup(x => x.GetCurrentMethod()).Returns(TestMethod);

            sut = mockSut.Object;
        }

        [Fact]
        public void AddSelfShouldThrowArgumentNullExceptionWhenServiceIsNull()
        {
            Func<IHypermediaBuilder<IHypermediaDocument>> func = () => default(IHypermediaService).AddSelf(mockDocument.Object);

            func.Should().Throw<ArgumentNullException>().Which.ParamName.Should().Be("service");

            mockSut.Verify(x => x.GetCurrentUrl(), Times.Never);
            mockSut.Verify(x => x.GetCurrentMethod(), Times.Never);

            mockDocument.Verify(x => x.AddLink(It.IsAny<string>(), It.IsAny<Link>()), Times.Never);
        }

        [Fact]
        public void AddSelfShouldThrowArgumentNullExceptionWhenDocumentIsNull()
        {
            Func<IHypermediaBuilder<IHypermediaDocument>> func = () => sut.AddSelf(default(IHypermediaDocument));

            func.Should().Throw<ArgumentNullException>().Which.ParamName.Should().Be("document");

            mockSut.Verify(x => x.GetCurrentUrl(), Times.Never);
            mockSut.Verify(x => x.GetCurrentMethod(), Times.Never);

            mockDocument.Verify(x => x.AddLink(It.IsAny<string>(), It.IsAny<Link>()), Times.Never);
        }

        [Fact]
        public void AddSelfShouldInvokeHypermediaServiceGetCurrentUrlMethod()
        {
            sut.AddSelf(mockDocument.Object);

            mockSut.Verify(x => x.GetCurrentUrl(), Times.Once);
        }

        [Fact]
        public void AddSelfShouldInvokeHypermediaServiceGetCurrentMethodMethod()
        {
            sut.AddSelf(mockDocument.Object);

            mockSut.Verify(x => x.GetCurrentMethod(), Times.Once);
        }

        [Fact]
        public void AddSelfShouldInvokeDocumentAddLinkMethod()
        {
            sut.AddSelf(mockDocument.Object);

            mockDocument.Verify(x =>
                x.AddLink(
                    It.Is<string>(y => y == "self"),
                    It.Is<Link>(y => y.Href == TestHref && y.Method == TestMethod)),
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
