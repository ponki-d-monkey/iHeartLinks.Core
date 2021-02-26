using System;
using FluentAssertions;
using Moq;
using Xunit;

namespace iHeartLinks.Core.Tests
{
    public sealed class HypermediaBuilderExtensionTests
    {
        private const string TestRel = "link";
        private const string TestHref = "https://iheartlinks.example.com";

        private readonly Mock<IHypermediaBuilder<IHypermediaDocument>> mockSut;
        private readonly IHypermediaBuilder<IHypermediaDocument> sut;

        public HypermediaBuilderExtensionTests()
        {
            mockSut = new Mock<IHypermediaBuilder<IHypermediaDocument>>();
            sut = mockSut.Object;
        }

        [Fact]
        public void AddLinkShouldThrowArgumentNullExceptionWhenBuilderIsNull()
        {
            Func<IHypermediaBuilder<IHypermediaDocument>> func = () => default(IHypermediaBuilder<IHypermediaDocument>).AddLink(TestRel, TestHref);

            func.Should().Throw<ArgumentNullException>().Which.ParamName.Should().Be("builder");

            mockSut.Verify(x => x.AddLink(It.IsAny<string>(), It.IsAny<Link>()), Times.Never);
        }

        [Fact]
        public void AddLinkShouldInvokeHypermediaBuilderAddLinkMethod()
        {
            sut.AddLink(TestRel, TestHref);

            mockSut.Verify(x => 
                x.AddLink(
                    It.Is<string>(y => y == TestRel), 
                    It.Is<Link>(y => y.Href == TestHref)), 
                Times.Once);
        }

        [Fact]
        public void AddLinkShouldReturnSameInstanceOfHypermediaBuilder()
        {
            var result = sut.AddLink(TestRel, TestHref);

            result.Should().BeSameAs(sut);
        }

        [Fact]
        public void AddLinksToChildShouldThrowArgumentNullExceptionWhenBuilderIsNull()
        {
            Func<IHypermediaBuilder<IHypermediaDocument>> func = () => default(IHypermediaBuilder<IHypermediaDocument>).AddLinksToChild((doc, svc) => { });

            func.Should().Throw<ArgumentNullException>().Which.ParamName.Should().Be("builder");

            mockSut.Verify(x => x.Document, Times.Never);
            mockSut.Verify(x => x.Service, Times.Never);
        }

        [Fact]
        public void AddLinksToChildShouldThrowArgumentNullExceptionWhenChildHandlerIsNull()
        {
            Func<IHypermediaBuilder<IHypermediaDocument>> func = () => sut.AddLinksToChild(null);

            func.Should().Throw<ArgumentNullException>().Which.ParamName.Should().Be("childHandler");

            mockSut.Verify(x => x.Document, Times.Never);
            mockSut.Verify(x => x.Service, Times.Never);
        }

        [Fact]
        public void AddLinksToChildShouldPassDocumentToChildHandler()
        {
            // Arrange
            mockSut.Setup(x => x.Document).Returns(Mock.Of<IHypermediaDocument>());

            // Act
            sut.AddLinksToChild(HandleChild);

            // Assert
            mockSut.Verify(x => x.Document, Times.Once);

            void HandleChild(IHypermediaDocument document, IHypermediaService service)
            {
                document.Should().NotBeNull();
            }
        }

        [Fact]
        public void AddLinksToChildShouldPassServiceToChildHandler()
        {
            // Arrange
            mockSut.Setup(x => x.Service).Returns(Mock.Of<IHypermediaService>());

            // Act
            sut.AddLinksToChild(HandleChild);

            // Assert
            mockSut.Verify(x => x.Service, Times.Once);

            void HandleChild(IHypermediaDocument document, IHypermediaService service)
            {
                service.Should().NotBeNull();
            }
        }

        [Fact]
        public void AddLinksToChildShouldReturnSameInstanceOfHypermediaBuilder()
        {
            var result = sut.AddLinksToChild((doc, svc) => { });

            result.Should().BeSameAs(sut);
        }

        [Fact]
        public void AddLinksPerConditionShouldThrowArgumentNullExceptionWhenBuilderIsNull()
        {
            Func<IHypermediaBuilder<IHypermediaDocument>> func = () => default(IHypermediaBuilder<IHypermediaDocument>).AddLinksPerCondition(doc => true, builder => { });

            func.Should().Throw<ArgumentNullException>().Which.ParamName.Should().Be("builder");
        }

        [Fact]
        public void AddLinksPerConditionShouldThrowArgumentNullExceptionWhenConditionHandlerIsNull()
        {
            Func<IHypermediaBuilder<IHypermediaDocument>> func = () => sut.AddLinksPerCondition(null, builder => { });

            func.Should().Throw<ArgumentNullException>().Which.ParamName.Should().Be("conditionHandler");
        }

        [Fact]
        public void AddLinksPerConditionShouldThrowArgumentNullExceptionWhenBuilderHandlerIsNull()
        {
            Func<IHypermediaBuilder<IHypermediaDocument>> func = () => sut.AddLinksPerCondition(doc => true, null);

            func.Should().Throw<ArgumentNullException>().Which.ParamName.Should().Be("builderHandler");
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void AddLinksPerConditionShouldPassDocumentToConditionHandlerWhen(bool conditionHandlerResult)
        {
            // Arrange
            mockSut.Setup(x => x.Document).Returns(Mock.Of<IHypermediaDocument>());

            // Act
            sut.AddLinksPerCondition(HandleCondition, builder => { });

            // Assert
            mockSut.Verify(x => x.Document, Times.Once);

            bool HandleCondition(IHypermediaDocument document)
            {
                document.Should().NotBeNull();

                return conditionHandlerResult;
            }
        }

        [Fact]
        public void AddLinksPerConditionShouldInvokeBuilderHandlerWhenConditionHandlerResultIsTrue()
        {
            sut.AddLinksPerCondition(doc => true, HandleBuilder);

            mockSut.Verify(x =>
                x.AddLink(
                    It.Is<string>(y => y == TestRel),
                    It.Is<Link>(y => y.Href == TestHref)),
                Times.Once);

            void HandleBuilder(IHypermediaBuilder<IHypermediaDocument> builder)
            {
                // In order to test that the "BuilderHandler" was invoked,
                // we need to call a mock method inside that we can verify.
                builder.AddLink(TestRel, TestHref);
            }
        }

        [Fact]
        public void AddLinksPerConditionShouldNotInvokeBuilderHandlerWhenConditionHandlerResultIsFalse()
        {
            sut.AddLinksPerCondition(doc => false, HandleBuilder);

            mockSut.Verify(x =>
                x.AddLink(
                    It.IsAny<string>(),
                    It.IsAny<Link>()),
                Times.Never);

            void HandleBuilder(IHypermediaBuilder<IHypermediaDocument> builder)
            {
                // In order to test that the "BuilderHandler" was invoked,
                // we need to call a mock method inside that we can verify.
                builder.AddLink(TestRel, TestHref);
            }
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void AddLinksPerConditionShouldReturnSameInstanceOfHypermediaBuilderWhen(bool conditionHandlerResult)
        {
            var result = sut.AddLinksPerCondition(doc => conditionHandlerResult, builder => { });

            result.Should().BeSameAs(sut);
        }
    }
}
