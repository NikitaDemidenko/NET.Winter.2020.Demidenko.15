using System;
using System.Collections.Generic;
using DoubleManipulation;
using System.Linq;
using PseudoEnumerableClassTask;
using PseudoEnumerableClassTask.Interfaces;
using Moq;
using NUnit.Framework;
using NumberExtension;
using System.Globalization;
using Notebook;

namespace PseudoEnumerableClassTask.Tests
{
    [TestFixture]
    public class EnumerableSequencesTests
    {
        private Mock<IPredicate<int>> mockPredicate;
        private Mock<ITransformer<double, string>> mockTransformer;
        private Mock<System.Collections.Generic.IComparer<string>> mockComparer;

        private IPredicate<int> predicate;
        private ITransformer<double, string> transformer;
        private System.Collections.Generic.IComparer<string> comparer;

        private Converter<double, string> converter = (double d) => d.TransformToIEEE754();
        private Converter<Note, string> noteConverter = (Note note) => note.ToString();
        private Comparison<Note> comparerDelegate = new NoteComparer().Compare; 

        [SetUp]
        public void Setup()
        {
            mockPredicate = new Mock<IPredicate<int>>();

            mockPredicate
                .Setup(p => p.IsMatch(It.Is<int>(i => new PredicateDigit { Digit = 5 }.ContainsKey(i))))
                .Returns(true);

            predicate = mockPredicate.Object;

            mockTransformer = new Mock<ITransformer<double, string>>();

            mockTransformer.Setup(t => t.Transform(It.IsAny<double>()))
                .Returns((double d) => d.TransformToIEEE754());

            transformer = mockTransformer.Object;

            mockComparer = new Mock<System.Collections.Generic.IComparer<string>>();

            mockComparer.Setup(c => c.Compare(It.IsAny<string>(), It.IsAny<string>()))
                .Returns((string lhs, string rhs) => lhs.Length.CompareTo(rhs.Length));

            comparer = mockComparer.Object;
        }

        [TestCase(55)]
        [TestCase(551)]
        [TestCase(-12551)]
        [TestCase(-90551)]
        public void IsMatchTests_Return_True(int value)
        {
            Assert.IsTrue(predicate.IsMatch(value));

            mockPredicate.Verify(p => p.IsMatch(It.IsAny<int>()), Times.Exactly(1));
        }

        [TestCase(109)]
        [TestCase(67632)]
        [TestCase(-120943)]
        [TestCase(-2113)]
        public void IsMatchTests_Return_False(int value)
        {
            Assert.IsFalse(predicate.IsMatch(value));

            mockPredicate.Verify(p => p.IsMatch(It.IsAny<int>()), Times.Exactly(1));
        }

        [Test]
        public void FilterByTests()
        {
            var source = new int[] { 12, 35, -65, 543, 23 };

            var expected = new int[] { 35, -65, 543 };

            var actual = source.FilterBy(predicate);

            CollectionAssert.AreEqual(actual, expected);

            mockPredicate.Verify(p => p.IsMatch(It.IsAny<int>()), Times.Exactly(5));
        }

        [TestCase(-255.255, "1100000001101111111010000010100011110101110000101000111101011100")]
        [TestCase(255.255, "0100000001101111111010000010100011110101110000101000111101011100")]
        [TestCase(4294967295.0, "0100000111101111111111111111111111111111111000000000000000000000")]
        [TestCase(double.MinValue, "1111111111101111111111111111111111111111111111111111111111111111")]
        [TestCase(double.MaxValue, "0111111111101111111111111111111111111111111111111111111111111111")]
        [TestCase(double.Epsilon, "0000000000000000000000000000000000000000000000000000000000000001")]
        [TestCase(double.NaN, "1111111111111000000000000000000000000000000000000000000000000000")]
        [TestCase(double.NegativeInfinity, "1111111111110000000000000000000000000000000000000000000000000000")]
        [TestCase(double.PositiveInfinity, "0111111111110000000000000000000000000000000000000000000000000000")]
        [TestCase(-0.0, "1000000000000000000000000000000000000000000000000000000000000000")]
        [TestCase(0.0, "0000000000000000000000000000000000000000000000000000000000000000")]
        public void TransformTests_Return_String(double value, string expected)
        {
            Assert.That(string.Equals(expected, transformer.Transform(value)));

            mockTransformer.Verify(t => t.Transform(It.IsAny<double>()), Times.Exactly(1));
        }

        [Test]
        public void TransformTests()
        {
            var source = new[]
            {
                -255.255,
                255.255,
                4294967295.0,
                double.MinValue,
                double.MaxValue,
                double.Epsilon,
                double.NaN,
                double.NegativeInfinity,
                double.PositiveInfinity,
                -0.0,
                0.0
            };

            var expected = new[]
            {
                "1100000001101111111010000010100011110101110000101000111101011100",
                "0100000001101111111010000010100011110101110000101000111101011100",
                "0100000111101111111111111111111111111111111000000000000000000000",
                "1111111111101111111111111111111111111111111111111111111111111111",
                "0111111111101111111111111111111111111111111111111111111111111111",
                "0000000000000000000000000000000000000000000000000000000000000001",
                "1111111111111000000000000000000000000000000000000000000000000000",
                "1111111111110000000000000000000000000000000000000000000000000000",
                "0111111111110000000000000000000000000000000000000000000000000000",
                "1000000000000000000000000000000000000000000000000000000000000000",
                "0000000000000000000000000000000000000000000000000000000000000000"
            };

            var actual = source.Transform(transformer);

            CollectionAssert.AreEqual(actual, expected);

            mockTransformer.Verify(t => t.Transform(It.IsAny<double>()), Times.Exactly(11));
        }

        [Test]
        public void OrderAccordingTo_Returns_OrderedSource()
        {
            var source = new[] { "1234567890000", "message", "asjdbjaksb", "qwe", "", "12345", "1" };
            var expected = new[] { "", "1", "qwe", "12345", "message", "asjdbjaksb", "1234567890000" };

            var actual = source.OrderAccordingTo(comparer);

            CollectionAssert.AreEqual(expected, actual);

            mockComparer.Verify(c => c.Compare(It.IsAny<string>(), It.IsAny<string>()), Times.AtLeast(10));
        }

        [Test]
        public void TypeOfTests()
        {
            var source = new object[] { 1, 2, "asndad", double.NaN, -123, -732, 2173, int.MaxValue };
            var expected = new[] { 1, 2, -123, -732, 2173, int.MaxValue };

            var actual = source.TypeOf<int>();

            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void ReverseTest()
        {
            var source = new[]
            {
                -255.255,
                255.255,
                4294967295.0,
                double.MinValue,
                double.MaxValue,
                double.Epsilon,
                double.NaN,
                double.NegativeInfinity,
                double.PositiveInfinity,
                -0.0,
                0.0,
            };

            var expected = new[]
            {
                0.0,
                -0.0,
                double.PositiveInfinity,
                double.NegativeInfinity,
                double.NaN,
                double.Epsilon,
                double.MaxValue,
                double.MinValue,
                4294967295.0,
                255.255,
                -255.255,
            };

            var actual = source.Reverse();

            CollectionAssert.AreEqual(expected, actual);
        }

        [TestFixture(new object[] { new int[] { 12, 3, 4 } }, TypeArgs = new Type[] { typeof(int) })]
        [TestFixture(new object[] { new string[] { "12", "2", "6" } }, TypeArgs = new Type[] { typeof(string) })]
        [TestFixture(new object[] { new double[] { 13.56, 17.901 } }, TypeArgs = new Type[] { typeof(double) })]
        internal class GenericTestFixture<T>
        {
            private readonly T[] expected;

            public GenericTestFixture(T[] expected) => this.expected = expected;

            [Test]
            public void TypeOfTest()
            {
                var source = new object[] { 12, 3, 4, "12", "2", 13.56, "6", 17.901 };
                var actual = source.TypeOf<T>().ToArray();
                CollectionAssert.AreEqual(actual, expected);
            }
        }

        [Test]
        public void Transform_Delegate_Tests()
        {
            var source = new[]
            {
                -255.255,
                255.255,
                4294967295.0,
                double.MinValue,
                double.MaxValue,
                double.Epsilon,
                double.NaN,
                double.NegativeInfinity,
                double.PositiveInfinity,
                -0.0,
                0.0
            };

            var expected = new[]
            {
                "1100000001101111111010000010100011110101110000101000111101011100",
                "0100000001101111111010000010100011110101110000101000111101011100",
                "0100000111101111111111111111111111111111111000000000000000000000",
                "1111111111101111111111111111111111111111111111111111111111111111",
                "0111111111101111111111111111111111111111111111111111111111111111",
                "0000000000000000000000000000000000000000000000000000000000000001",
                "1111111111111000000000000000000000000000000000000000000000000000",
                "1111111111110000000000000000000000000000000000000000000000000000",
                "0111111111110000000000000000000000000000000000000000000000000000",
                "1000000000000000000000000000000000000000000000000000000000000000",
                "0000000000000000000000000000000000000000000000000000000000000000"
            };

            var actual = source.Transform(converter);

            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void TransformNote_Delegate_Tests()
        {
            var firstNote = new Note("Albahari", "test");
            var secondNote = new Note("Skeet", "test");
            var thirdNote = new Note("Troelsen", "test");
            var fourthNote = new Note("A", "test");
            var fifthNote = new Note("B", "test");
            var sixthNote = new Note("C", "test");
            var seventhNote = new Note("D", "test");

            var source = new[]
            {
                firstNote,
                secondNote,
                thirdNote,
                fourthNote,
                fifthNote,
                sixthNote,
                seventhNote,
            };

            var expected = new[]
            {
                $"Albahari, {firstNote.TimeOfCreation.ToString(CultureInfo.InvariantCulture)}: test",
                $"Skeet, {secondNote.TimeOfCreation.ToString(CultureInfo.InvariantCulture)}: test",
                $"Troelsen, {thirdNote.TimeOfCreation.ToString(CultureInfo.InvariantCulture)}: test",
                $"A, {fourthNote.TimeOfCreation.ToString(CultureInfo.InvariantCulture)}: test",
                $"B, {fifthNote.TimeOfCreation.ToString(CultureInfo.InvariantCulture)}: test",
                $"C, {sixthNote.TimeOfCreation.ToString(CultureInfo.InvariantCulture)}: test",
                $"D, {seventhNote.TimeOfCreation.ToString(CultureInfo.InvariantCulture)}: test",
            };

            var actual = source.Transform(noteConverter);

            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void OrderAccordingTo_Delegate_Tests()
        {
            var source = new Note[] { new Note("Skeet", "test"), new Note("Albahari", "test"), new Note("Troelsen", "test") };

            var expected = new Note[] { new Note("Albahari", "test"), new Note("Skeet", "test"), new Note("Troelsen", "test") };

            var actual = source.OrderAccordingTo(comparerDelegate);

            CollectionAssert.AreEqual(expected, actual);
        }
    }
}