using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CharacterCopy.Kata.Dependencies;
using CharCopyKata;
using NSubstitute;
using NUnit.Framework;

namespace CharCopyKataTests
{
    [TestFixture]
    public class TestCopier
    {
        [Test]
        public void Copy_GivenFirstCharIsNewLine_ShouldWriteNothing()
        {
            // Arrange
            var source = CreateSource('\n');
            var destination = CreateDestination();

            var copier = CreateCopier(source, destination);
            // Act
            copier.Copy();
            // Assert
            destination.DidNotReceive().WriteChar(Arg.Any<char>());
        }

        [TestCase('a')]
        [TestCase('x')]
        [TestCase('y')]
        public void Copy_GivenOneCharBeforeNewLine_ShouldWriteThatChar(char c)
        {
            // Arrange
            var source = CreateSource(c, '\n');
            var destination = CreateDestination();

            var copier = CreateCopier(source, destination);
            // Act
            copier.Copy();
            // Assert
            destination.Received(1).WriteChar(c);
            destination.Received(1).WriteChar(Arg.Any<char>());
        }

        [TestCase('a', 'a', 'a')]
        [TestCase('x', 'y', 'z')]
        [TestCase('1', '2', '3')]
        public void Copy_GivenManyCharsBeforeNewLine_ShouldWriteThoseChars(char c1, char c2, char c3)
        {
            // Arrange
            var source = CreateSource(c1, c2, c3, '\n');
            var destination = CreateDestination();

            var copier = CreateCopier(source, destination);
            // Act
            copier.Copy();
            // Assert
            Received.InOrder(() =>
            {
                destination.WriteChar(c1);
                destination.WriteChar(c2);
                destination.WriteChar(c3);
            });
            
            destination.Received(3).WriteChar(Arg.Any<char>());
        }

        [TestCase('b', 'b', 'b')]
        [TestCase('f', 't', 'h')]
        [TestCase('5', '7', '9')]
        public void Copy_GivenCharAfterNewLine_ShouldOnlyWriteCharsBeforeNewLine(char c1, char c2, char c3)
        {
            // Arrange
            var source = CreateSource(c1, c2, '\n', c3);
            var destination = CreateDestination();

            var copier = CreateCopier(source, destination);
            // Act
            copier.Copy();
            // Assert
            Received.InOrder(() =>
            {
                destination.WriteChar(c1);
                destination.WriteChar(c2);
            });

            destination.Received(2).WriteChar(Arg.Any<char>());
        }

        private static Copier CreateCopier(ISource source, IDestination destination)
        {
            return new Copier(source, destination);
        }

        private static IDestination CreateDestination()
        {
            return Substitute.For<IDestination>();
        }

        private static ISource CreateSource(char c, params char[] otherChars)
        {
            var source = Substitute.For<ISource>();
            source.ReadChar().Returns(c, otherChars);

            return source;
        }
    }
}
