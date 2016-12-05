// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace Intervals.Tests
{
    using System.Linq;
    using NUnit.Framework;

    [TestFixture]
    public class IntervalTests
    {
        public static readonly bool[] Booleans = { false, true };
        public static readonly int[] Integers = { -3, -2, -1, 0, 1, 2, 3 };

        [Test]
        [Pairwise]
        public void Contains_FromEmptyInterval_ReturnsFalse(
            [ValueSource("Booleans")] bool aStartInclusive,
            [ValueSource("Booleans")] bool aEndInclusive,
            [ValueSource("Integers")] int bStart,
            [ValueSource("Booleans")] bool bStartInclusive,
            [ValueSource("Integers")] int bEnd,
            [ValueSource("Booleans")] bool bEndInclusive)
        {
            var intervalA = new IntegerInterval(1, aStartInclusive, 0, aEndInclusive);
            var intervalB = new IntegerInterval(bStart, bStartInclusive, bEnd, bEndInclusive);

            Assume.That(!intervalB.IsEmpty());

            var result = intervalA.Contains(intervalB);

            Assert.That(result, Is.False);
        }

        [Test]
        public void Contains_InDegenerateInterval_ReturnsTrue(
            [ValueSource("Booleans")] bool bStartInclusive,
            [ValueSource("Booleans")] bool bEndInclusively)
        {
            var intervalA = new IntegerInterval(0, true, 0, true);
            var intervalB = new IntegerInterval(0, bStartInclusive, 0, bEndInclusively);

            var result = intervalA.Contains(intervalB);

            Assert.That(result, Is.True);
        }

        [Test]
        [Pairwise]
        public void Contains_WithEmptyInterval_ReturnsFalse(
            [ValueSource("Integers")] int start,
            [ValueSource("Integers")] int end,
            [ValueSource("Booleans")] bool startInclusive,
            [ValueSource("Booleans")] bool endInclusive,
            [ValueSource("Integers")] int value)
        {
            Assume.That(start > end);

            var interval = new IntegerInterval(start, startInclusive, end, endInclusive);

            var result = interval.Contains(value);

            Assert.That(result, Is.False);
        }

        [Test]
        [Pairwise]
        public void Contains_WithEmptyInterval_ReturnsTrue(
            [ValueSource("Integers")] int aStart,
            [ValueSource("Booleans")] bool aStartInclusive,
            [ValueSource("Integers")] int aEnd,
            [ValueSource("Booleans")] bool aEndInclusive,
            [ValueSource("Booleans")] bool bStartInclusive,
            [ValueSource("Booleans")] bool bEndInclusive)
        {
            var intervalA = new IntegerInterval(aStart, aStartInclusive, aStart, aEndInclusive);
            var intervalB = new IntegerInterval(1, bStartInclusive, 0, bEndInclusive);

            var result = intervalA.Contains(intervalB);

            Assert.That(result, Is.True);
        }

        [Test]
        [Pairwise]
        public void Contains_WithEndAdjacentZeroIntervalExcluded_ReturnsFalse(
            [ValueSource("Booleans")] bool aStartInclusive,
            [ValueSource("Booleans")] bool bStartInclusive,
            [ValueSource("Booleans")] bool bEndInclusively)
        {
            var intervalA = new IntegerInterval(0, aStartInclusive, 3, false);
            var intervalB = new IntegerInterval(3, bStartInclusive, 3, bEndInclusively);

            Assume.That(!intervalB.IsEmpty());

            var result = intervalA.Contains(intervalB);

            Assert.That(result, Is.False);
        }

        [Test]
        [Pairwise]
        public void Contains_WithEndAdjacentZeroIntervalIncluded_ReturnsTrue(
            [ValueSource("Booleans")] bool aStartInclusive,
            [ValueSource("Booleans")] bool bStartInclusive,
            [ValueSource("Booleans")] bool bEndInclusively)
        {
            var intervalA = new IntegerInterval(0, aStartInclusive, 3, true);
            var intervalB = new IntegerInterval(3, bStartInclusive, 3, bEndInclusively);

            var result = intervalA.Contains(intervalB);

            Assert.That(result, Is.True);
        }

        [Test]
        [Pairwise]
        public void Contains_WithExclusiveEnd_ReturnsFalse(
            [ValueSource("Integers")] int start,
            [ValueSource("Integers")] int end,
            [ValueSource("Booleans")] bool startInclusive)
        {
            Assume.That(start <= end);

            var interval = new IntegerInterval(start, startInclusive, end, false);

            var result = interval.Contains(end);

            Assert.That(result, Is.False);
        }

        [Test]
        [Pairwise]
        public void Contains_WithExclusiveStart_ReturnsFalse(
            [ValueSource("Integers")] int start,
            [ValueSource("Integers")] int end,
            [ValueSource("Booleans")] bool endInclusive)
        {
            Assume.That(start <= end);

            var interval = new IntegerInterval(start, false, end, endInclusive);

            var result = interval.Contains(start);

            Assert.That(result, Is.False);
        }

        [Test]
        [Pairwise]
        public void Contains_WithFullyContainedInterval_ReturnsTrue(
            [ValueSource("Booleans")] bool aStartInclusive,
            [ValueSource("Booleans")] bool aEndInclusive,
            [ValueSource("Booleans")] bool bStartInclusive,
            [ValueSource("Booleans")] bool bEndInclusive)
        {
            var intervalA = new IntegerInterval(0, aStartInclusive, 3, aEndInclusive);
            var intervalB = new IntegerInterval(1, bStartInclusive, 2, bEndInclusive);

            var result = intervalA.Contains(intervalB);

            Assert.That(result, Is.True);
        }

        [Test]
        [Pairwise]
        public void Contains_WithIncludedValue_ReturnsTrue(
            [ValueSource("Integers")] int start,
            [ValueSource("Integers")] int end,
            [ValueSource("Integers")] int value,
            [ValueSource("Booleans")] bool startInclusive,
            [ValueSource("Booleans")] bool endInclusive)
        {
            Assume.That(start < value && value < end);

            var interval = new IntegerInterval(start, startInclusive, end, endInclusive);

            var result = interval.Contains(value);

            Assert.That(result, Is.True);
        }

        [Test]
        [Pairwise]
        public void Contains_WithInclusiveEnd_ReturnsTrue(
            [ValueSource("Integers")] int start,
            [ValueSource("Integers")] int end,
            [ValueSource("Booleans")] bool startInclusive)
        {
            Assume.That(start < end);

            var interval = new IntegerInterval(start, startInclusive, end, true);

            var result = interval.Contains(end);

            Assert.That(result, Is.True);
        }

        [Test]
        [Pairwise]
        public void Contains_WithInclusiveStart_ReturnsTrue(
            [ValueSource("Integers")] int start,
            [ValueSource("Integers")] int end,
            [ValueSource("Booleans")] bool endInclusive)
        {
            Assume.That(start < end);

            var interval = new IntegerInterval(start, true, end, endInclusive);

            var result = interval.Contains(start);

            Assert.That(result, Is.True);
        }

        [Test]
        public void Contains_WithIntervalAdjacentlyExcludedAtEnd_ReturnsTrue(
            [ValueSource("Booleans")] bool aStartInclusive,
            [ValueSource("Booleans")] bool bStartInclusive)
        {
            var intervalA = new IntegerInterval(0, aStartInclusive, 3, false);
            var intervalB = new IntegerInterval(1, bStartInclusive, 3, true);

            var result = intervalA.Contains(intervalB);

            Assert.That(result, Is.False);
        }

        [Test]
        public void Contains_WithIntervalAdjacentlyExcludedAtStart_ReturnsFalse(
            [ValueSource("Booleans")] bool aEndInclusive,
            [ValueSource("Booleans")] bool bEndInclusive)
        {
            var intervalA = new IntegerInterval(0, false, 3, aEndInclusive);
            var intervalB = new IntegerInterval(0, true, 2, bEndInclusive);

            var result = intervalA.Contains(intervalB);

            Assert.That(result, Is.False);
        }

        [Test]
        [Pairwise]
        public void Contains_WithIntervalAdjacentlyIncludedAtEnd_ReturnsTrue(
            [ValueSource("Booleans")] bool aStartInclusive,
            [ValueSource("Booleans")] bool bStartInclusive,
            [ValueSource("Booleans")] bool bEndInclusive)
        {
            var intervalA = new IntegerInterval(0, aStartInclusive, 3, true);
            var intervalB = new IntegerInterval(1, bStartInclusive, 3, bEndInclusive);

            var result = intervalA.Contains(intervalB);

            Assert.That(result, Is.True);
        }

        [Test]
        [Pairwise]
        public void Contains_WithIntervalAdjacentlyIncludedAtStart_ReturnsTrue(
            [ValueSource("Booleans")] bool aEndInclusive,
            [ValueSource("Booleans")] bool bStartInclusive,
            [ValueSource("Booleans")] bool bEndInclusive)
        {
            var intervalA = new IntegerInterval(0, true, 3, aEndInclusive);
            var intervalB = new IntegerInterval(0, bStartInclusive, 2, bEndInclusive);

            var result = intervalA.Contains(intervalB);

            Assert.That(result, Is.True);
        }

        [Test]
        [Pairwise]
        public void Contains_WithNonIntersectingInterval_ReturnsFalse(
            [ValueSource("Booleans")] bool aStartInclusive,
            [ValueSource("Booleans")] bool aEndInclusive,
            [ValueSource("Booleans")] bool bStartInclusive,
            [ValueSource("Booleans")] bool bEndInclusive)
        {
            var intervalA = new IntegerInterval(0, aStartInclusive, 1, aEndInclusive);
            var intervalB = new IntegerInterval(2, bStartInclusive, 3, bEndInclusive);

            var result = intervalA.Contains(intervalB);

            Assert.That(result, Is.False);
        }

        [Test]
        [Pairwise]
        public void Contains_WithStartAdjacentZeroIntervalExcluded_ReturnsFalse(
            [ValueSource("Booleans")] bool aEndInclusive,
            [ValueSource("Booleans")] bool bStartInclusive,
            [ValueSource("Booleans")] bool bEndInclusively)
        {
            var intervalA = new IntegerInterval(0, false, 3, aEndInclusive);
            var intervalB = new IntegerInterval(0, bStartInclusive, 0, bEndInclusively);

            Assume.That(!intervalB.IsEmpty());

            var result = intervalA.Contains(intervalB);

            Assert.That(result, Is.False);
        }

        [Test]
        [Pairwise]
        public void Contains_WithStartAdjacentZeroIntervalIncluded_ReturnsTrue(
            [ValueSource("Booleans")] bool aEndInclusive,
            [ValueSource("Booleans")] bool bStartInclusive,
            [ValueSource("Booleans")] bool bEndInclusively)
        {
            var intervalA = new IntegerInterval(0, true, 3, aEndInclusive);
            var intervalB = new IntegerInterval(0, bStartInclusive, 0, bEndInclusively);

            var result = intervalA.Contains(intervalB);

            Assert.That(result, Is.True);
        }

        [Test]
        [Pairwise]
        public void Contains_WithZeroIntervalExclusive_ReturnsFalse(
            [ValueSource("Booleans")] bool aStartInclusive,
            [ValueSource("Booleans")] bool aEndInclusive,
            [ValueSource("Booleans")] bool bStartInclusive,
            [ValueSource("Booleans")] bool bEndInclusively)
        {
            var intervalA = new IntegerInterval(0, false, 0, false);
            var intervalB = new IntegerInterval(0, bStartInclusive, 0, bEndInclusively);

            Assume.That(intervalA.IsEmpty());
            Assume.That(!intervalB.IsEmpty());

            var result = intervalA.Contains(intervalB);

            Assert.That(result, Is.False);
        }

        [Test]
        [Pairwise]
        public void Contains_WithZeroLengthAndExclusiveStartOrEnd_ReturnsFalse(
            [ValueSource("Integers")] int startAndEnd,
            [ValueSource("Booleans")] bool startInclusive,
            [ValueSource("Booleans")] bool endInclusive,
            [ValueSource("Integers")] int value)
        {
            Assume.That(!startInclusive || !endInclusive);

            var interval = new IntegerInterval(startAndEnd, startInclusive, startAndEnd, endInclusive);

            var result = interval.Contains(value);

            Assert.That(result, Is.False);
        }

        [Test]
        public void DifferenceWith_FromEmptyInterval_ReturnsNull()
        {
            var intervalA = new IntegerInterval(2, false, 2, false);
            var intervalB = new IntegerInterval(0, false, 3, false);

            var actual = intervalA.DifferenceWith(intervalB);

            Assert.That(actual, Is.Null);
        }

        [Test]
        public void DifferenceWith_WhenOtherIntervalContainedByThis_ReturnsTwoIntervalsThatDontIntersectOther()
        {
            var intervalA = new IntegerInterval(0, true, 3, true);
            var intervalB = new IntegerInterval(1, true, 2, true);

            var actual = intervalA.DifferenceWith(intervalB);

            Assert.That(actual.Count, Is.EqualTo(2));
            Assert.That(actual[0].IntersectWith(intervalB).IsEmpty());
            Assert.That(actual[1].IntersectWith(intervalB).IsEmpty());
        }

        [Test]
        public void DifferenceWith_WhenOtherOverlapsThisEnd_ReturnsNewIntervalExcludingStart()
        {
            var intervalA = new IntegerInterval(0, true, 2, true);
            var intervalB = new IntegerInterval(1, true, 3, true);

            var actual = intervalA.DifferenceWith(intervalB);

            var single = actual.Single(); // Asserts that the array contains a single entry.
            Assert.That(single.End, Is.EqualTo(intervalB.Start));
            Assert.That(single.EndInclusive, Is.False);
        }

        [Test]
        public void DifferenceWith_WhenOtherOverlapsThisStart_ReturnsNewIntervalExcludingStart()
        {
            var intervalA = new IntegerInterval(1, true, 3, true);
            var intervalB = new IntegerInterval(0, true, 2, true);

            var actual = intervalA.DifferenceWith(intervalB);

            var single = actual.Single(); // Asserts that the array contains a single entry.
            Assert.That(single.Start, Is.EqualTo(intervalB.End));
            Assert.That(single.StartInclusive, Is.False);
        }

        [Test]
        public void DifferenceWith_WhenSetIsExcluded_ReturnsEmpty()
        {
            var set = new[]
            {
                new IntegerInterval(0, true, 2, true),
                new IntegerInterval(2, false, 3, false),
            };

            var interval = new IntegerInterval(0, true, 3, true);

            var actual = set.DifferenceWith(interval);

            Assert.That(actual.IsEmpty());
        }

        [Test]
        public void DifferenceWith_WhenThisIntervalIsContainedByOtherInterval_ReturnsNull()
        {
            var intervalA = new IntegerInterval(1, false, 2, false);
            var intervalB = new IntegerInterval(0, false, 3, false);

            var actual = intervalA.DifferenceWith(intervalB);

            Assert.That(actual, Is.Null);
        }

        [Test]
        public void DifferenceWith_WhenWhollyOverlappedExceptEndPoints_ReturnsTwoDegenerateIntervalsForTheEndPoints()
        {
            var intervalA = new IntegerInterval(0, true, 3, true);
            var intervalB = new IntegerInterval(0, false, 3, false);

            var actual = intervalA.DifferenceWith(intervalB);

            Assert.That(actual.Count, Is.EqualTo(2));
            Assert.That(actual[0].Start, Is.EqualTo(actual[0].End));
            Assert.That(actual[1].Start, Is.EqualTo(actual[1].End));
        }

        [Test]
        public void DifferenceWith_WithEmptyInterval_ReturnsThisInterval()
        {
            var intervalA = new IntegerInterval(0, false, 3, false);
            var intervalB = new IntegerInterval(2, false, 2, false);

            var actual = intervalA.DifferenceWith(intervalB);

            Assert.That(actual, Is.EquivalentTo(new[] { intervalA }));
        }

        [Test]
        public void DifferenceWith_WithIntervalsIntersectingAtEndPoint_ReturnsNewIntervalExcludingEnd()
        {
            var intervalA = new IntegerInterval(0, true, 3, true);
            var intervalB = new IntegerInterval(3, true, 3, true);

            var actual = intervalA.DifferenceWith(intervalB);

            var single = actual.Single(); // Asserts that the array contains a single entry.
            Assert.That(single.End, Is.EqualTo(intervalA.End));
            Assert.That(single.EndInclusive, Is.False);
        }

        [Test]
        public void DifferenceWith_WithIntervalsIntersectingAtStartPoint_ReturnsNewIntervalExcludingStart()
        {
            var intervalA = new IntegerInterval(0, true, 3, true);
            var intervalB = new IntegerInterval(0, true, 0, true);

            var actual = intervalA.DifferenceWith(intervalB);

            var single = actual.Single(); // Asserts that the array contains a single entry.
            Assert.That(single.Start, Is.EqualTo(intervalA.Start));
            Assert.That(single.StartInclusive, Is.False);
        }

        [Test]
        [Pairwise]
        public void IntersectWith_FromEmptyInterval_ReturnsNull(
            [ValueSource("Integers")] int aStart,
            [ValueSource("Booleans")] bool aStartInclusive,
            [ValueSource("Integers")] int aEnd,
            [ValueSource("Booleans")] bool aEndInclusive,
            [ValueSource("Booleans")] bool bStartInclusive,
            [ValueSource("Booleans")] bool bEndInclusive)
        {
            var intervalA = new IntegerInterval(aStart, aStartInclusive, aEnd, aEndInclusive);
            var intervalB = new IntegerInterval(1, bStartInclusive, 0, bEndInclusive);

            var actual = intervalA.IntersectWith(intervalB);

            Assert.That(actual, Is.Null);
        }

        [Test]
        [Pairwise]
        public void IntersectWith_WhenIntervalsAreSameSize_ReturnsIntervalWithMostRestrictedInclusivity(
            [ValueSource("Booleans")] bool aStartInclusive,
            [ValueSource("Booleans")] bool aEndInclusive,
            [ValueSource("Booleans")] bool bStartInclusive,
            [ValueSource("Booleans")] bool bEndInclusive)
        {
            var intervalA = new IntegerInterval(0, aStartInclusive, 3, aEndInclusive);
            var intervalB = new IntegerInterval(0, bStartInclusive, 3, bEndInclusive);

            var actual = intervalA.IntersectWith(intervalB);

            Assert.That(actual.Start, Is.EqualTo(intervalB.Start));
            Assert.That(actual.StartInclusive, Is.EqualTo(intervalA.StartInclusive && intervalB.StartInclusive));
            Assert.That(actual.End, Is.EqualTo(intervalA.End));
            Assert.That(actual.EndInclusive, Is.EqualTo(intervalA.EndInclusive && intervalB.EndInclusive));
        }

        [Test]
        [Pairwise]
        public void IntersectWith_WhenIntervalsAreSameSizeAndOtherIntervalMatchesMostRestrictive_ReturnsOtherInterval(
            [ValueSource("Booleans")] bool aStartInclusive,
            [ValueSource("Booleans")] bool aEndInclusive,
            [ValueSource("Booleans")] bool bStartInclusive,
            [ValueSource("Booleans")] bool bEndInclusive)
        {
            Assume.That(aStartInclusive || aEndInclusive);
            Assume.That(aStartInclusive || (!aStartInclusive && !bStartInclusive));
            Assume.That(aEndInclusive || (!aEndInclusive && !bEndInclusive));
            Assume.That((!bStartInclusive && aStartInclusive) || (!bEndInclusive && aEndInclusive));

            var intervalA = new IntegerInterval(0, aStartInclusive, 3, aEndInclusive);
            var intervalB = new IntegerInterval(0, bStartInclusive, 3, bEndInclusive);

            var actual = intervalA.IntersectWith(intervalB);

            Assert.That(actual, Is.EqualTo(intervalB));
        }

        [Test]
        [Pairwise]
        public void IntersectWith_WhenIntervalsAreSameSizeAndThisIntervalMatchesMostRestrictive_ReturnsThisInterval(
            [ValueSource("Booleans")] bool aStartInclusive,
            [ValueSource("Booleans")] bool aEndInclusive,
            [ValueSource("Booleans")] bool bStartInclusive,
            [ValueSource("Booleans")] bool bEndInclusive)
        {
            Assume.That(bStartInclusive || bEndInclusive);
            Assume.That(bStartInclusive || (!bStartInclusive && !aStartInclusive));
            Assume.That(bEndInclusive || (!bEndInclusive && !aEndInclusive));

            var intervalA = new IntegerInterval(0, aStartInclusive, 3, aEndInclusive);
            var intervalB = new IntegerInterval(0, bStartInclusive, 3, bEndInclusive);

            var actual = intervalA.IntersectWith(intervalB);

            Assert.That(actual, Is.EqualTo(intervalA));
        }

        [Test]
        [Pairwise]
        public void IntersectWith_WhenIntervalsDoNotIntersect_ReturnsNull(
            [ValueSource("Booleans")] bool aStartInclusive,
            [ValueSource("Booleans")] bool aEndInclusive,
            [ValueSource("Booleans")] bool bStartInclusive,
            [ValueSource("Booleans")] bool bEndInclusive)
        {
            var intervalA = new IntegerInterval(0, aStartInclusive, 1, aEndInclusive);
            var intervalB = new IntegerInterval(2, bStartInclusive, 3, bEndInclusive);

            var actual = intervalA.IntersectWith(intervalB);

            Assert.That(actual, Is.Null);
        }

        [Test]
        [Pairwise]
        public void IntersectWith_WhenIntervalsIntersectInAInterval_ReturnsThatIntervalWithMatchingInclusivity(
            [ValueSource("Booleans")] bool aStartInclusive,
            [ValueSource("Booleans")] bool aEndInclusive,
            [ValueSource("Booleans")] bool bStartInclusive,
            [ValueSource("Booleans")] bool bEndInclusive)
        {
            var intervalA = new IntegerInterval(0, aStartInclusive, 2, aEndInclusive);
            var intervalB = new IntegerInterval(1, bStartInclusive, 3, bEndInclusive);

            var actual = intervalA.IntersectWith(intervalB);

            Assert.That(actual.Start, Is.EqualTo(intervalB.Start));
            Assert.That(actual.StartInclusive, Is.EqualTo(intervalB.StartInclusive));
            Assert.That(actual.End, Is.EqualTo(intervalA.End));
            Assert.That(actual.EndInclusive, Is.EqualTo(intervalA.EndInclusive));
        }

        [Test]
        [Pairwise]
        public void IntersectWith_WhenIntervalsIntersectInAIntervalReversed_ReturnsThatIntervalWithMatchingInclusivity(
            [ValueSource("Booleans")] bool aStartInclusive,
            [ValueSource("Booleans")] bool aEndInclusive,
            [ValueSource("Booleans")] bool bStartInclusive,
            [ValueSource("Booleans")] bool bEndInclusive)
        {
            var intervalA = new IntegerInterval(0, aStartInclusive, 2, aEndInclusive);
            var intervalB = new IntegerInterval(1, bStartInclusive, 3, bEndInclusive);

            var actual = intervalB.IntersectWith(intervalA);

            Assert.That(actual.Start, Is.EqualTo(intervalB.Start));
            Assert.That(actual.StartInclusive, Is.EqualTo(intervalB.StartInclusive));
            Assert.That(actual.End, Is.EqualTo(intervalA.End));
            Assert.That(actual.EndInclusive, Is.EqualTo(intervalA.EndInclusive));
        }

        [Test]
        [Pairwise]
        public void IntersectWith_WhenIntervalsIntersectInAnExclusivePoint_ReturnsNull(
            [ValueSource("Booleans")] bool aStartInclusive,
            [ValueSource("Booleans")] bool aEndInclusive,
            [ValueSource("Booleans")] bool bStartInclusive,
            [ValueSource("Booleans")] bool bEndInclusive)
        {
            Assume.That(!aEndInclusive || !bStartInclusive);

            var intervalA = new IntegerInterval(0, aStartInclusive, 2, aEndInclusive);
            var intervalB = new IntegerInterval(2, bStartInclusive, 3, bEndInclusive);

            var actual = intervalA.IntersectWith(intervalB);

            Assert.That(actual, Is.Null);
        }

        [Test]
        public void IntersectWith_WhenIntervalsIntersectInAPoint_ReturnsThatPointWithMatchingInclusivity(
            [ValueSource("Booleans")] bool aStartInclusive,
            [ValueSource("Booleans")] bool bEndInclusive)
        {
            var intervalA = new IntegerInterval(0, aStartInclusive, 2, true);
            var intervalB = new IntegerInterval(2, true, 3, bEndInclusive);

            var actual = intervalA.IntersectWith(intervalB);

            Assert.That(actual.Start, Is.EqualTo(intervalB.Start));
            Assert.That(actual.End, Is.EqualTo(intervalA.End));
            Assert.That(actual.StartInclusive && actual.EndInclusive, Is.EqualTo(true));
        }

        [Test]
        [Pairwise]
        public void IntersectWith_WhenOtherIntervalIsEmpty_ReturnsNull(
            [ValueSource("Integers")] int aStart,
            [ValueSource("Booleans")] bool aStartInclusive,
            [ValueSource("Integers")] int aEnd,
            [ValueSource("Booleans")] bool aEndInclusive,
            [ValueSource("Integers")] int bStartAndEnd,
            [ValueSource("Booleans")] bool bStartInclusive,
            [ValueSource("Booleans")] bool bEndInclusive)
        {
            Assume.That(!bStartInclusive || !bEndInclusive);

            var intervalA = new IntegerInterval(aStart, aStartInclusive, aEnd, aEndInclusive);
            var intervalB = new IntegerInterval(0, false, 0, false);

            var actual = intervalA.IntersectWith(intervalB);

            Assert.That(actual, Is.Null);
        }

        [Test]
        [Pairwise]
        public void IntersectWith_WhenThisIntervalIsEmpty_ReturnsNull(
            [ValueSource("Integers")] int aStartAndEnd,
            [ValueSource("Booleans")] bool aStartInclusive,
            [ValueSource("Booleans")] bool aEndInclusive,
            [ValueSource("Integers")] int bStart,
            [ValueSource("Booleans")] bool bStartInclusive,
            [ValueSource("Integers")] int bEnd,
            [ValueSource("Booleans")] bool bEndInclusive)
        {
            Assume.That(!aStartInclusive || !aEndInclusive);

            var intervalA = new IntegerInterval(aStartAndEnd, aStartInclusive, aStartAndEnd, aEndInclusive);
            var intervalB = new IntegerInterval(bStart, bStartInclusive, bEnd, bEndInclusive);

            var actual = intervalA.IntersectWith(intervalB);

            Assert.That(actual, Is.Null);
        }

        [Test]
        [Pairwise]
        public void IntersectWith_WithEmptyInterval_ReturnsNull(
            [ValueSource("Booleans")] bool aStartInclusive,
            [ValueSource("Booleans")] bool aEndInclusive,
            [ValueSource("Integers")] int bStart,
            [ValueSource("Booleans")] bool bStartInclusive,
            [ValueSource("Integers")] int bEnd,
            [ValueSource("Booleans")] bool bEndInclusive)
        {
            var intervalA = new IntegerInterval(1, aStartInclusive, 0, aEndInclusive);
            var intervalB = new IntegerInterval(bStart, bStartInclusive, bEnd, bEndInclusive);

            var actual = intervalA.IntersectWith(intervalB);

            Assert.That(actual, Is.Null);
        }

        [Test]
        public void IntersectWith_WithSameInterval_ReturnsOriginalReference(
            [ValueSource("Booleans")] bool aStartInclusive,
            [ValueSource("Booleans")] bool aEndInclusive)
        {
            var intervalA = new IntegerInterval(0, aStartInclusive, 3, aEndInclusive);

            var actual = intervalA.IntersectWith(intervalA);

            Assert.That(actual, Is.EqualTo(intervalA));
        }

        [Test]
        [Pairwise]
        public void IntersectWith_WithWhollyContianedInterval_ReturnsContainedInterval(
            [ValueSource("Booleans")] bool aStartInclusive,
            [ValueSource("Booleans")] bool aEndInclusive,
            [ValueSource("Booleans")] bool bStartInclusive,
            [ValueSource("Booleans")] bool bEndInclusive)
        {
            var intervalA = new IntegerInterval(0, aStartInclusive, 3, aEndInclusive);
            var intervalB = new IntegerInterval(1, bStartInclusive, 2, bEndInclusive);

            var actual = intervalA.IntersectWith(intervalB);

            Assert.That(actual, Is.EqualTo(intervalB));
        }

        [Test]
        [Pairwise]
        public void IsEmpty_WithBackwardsSet_ReturnsTrue(
            [ValueSource("Integers")] int start,
            [ValueSource("Integers")] int end,
            [ValueSource("Booleans")] bool startInclusive,
            [ValueSource("Booleans")] bool endInclusive)
        {
            Assume.That(start > end);

            var intervalA = new IntegerInterval(start, startInclusive, end, endInclusive);

            var actual = intervalA.IsEmpty();

            Assert.That(actual, Is.True);
        }

        [Test]
        [Pairwise]
        public void IsEmpty_WithInOrderSetSet_ReturnsFalse(
            [ValueSource("Integers")] int start,
            [ValueSource("Integers")] int end,
            [ValueSource("Booleans")] bool startInclusive,
            [ValueSource("Booleans")] bool endInclusive)
        {
            Assume.That(start < end);

            var intervalA = new IntegerInterval(start, startInclusive, end, endInclusive);

            var actual = intervalA.IsEmpty();

            Assert.That(actual, Is.False);
        }

        [Test]
        public void IsEmpty_WithNonSelfExcludedSet_ReturnsFalse(
            [ValueSource("Integers")] int startAndEnd)
        {
            var intervalA = new IntegerInterval(startAndEnd, true, startAndEnd, true);

            var actual = intervalA.IsEmpty();

            Assert.That(actual, Is.False);
        }

        [Test]
        [Pairwise]
        public void IsEmpty_WithNullSet_ReturnsTrue(
            [ValueSource("Integers")] int start,
            [ValueSource("Integers")] int end,
            [ValueSource("Booleans")] bool startInclusive,
            [ValueSource("Booleans")] bool endInclusive)
        {
            IntegerInterval intervalA = null;

            var actual = intervalA.IsEmpty();

            Assert.That(actual, Is.True);
        }

        [Test]
        [Pairwise]
        public void IsEmpty_WithSelfExcludedSet_ReturnsTrue(
            [ValueSource("Integers")] int startAndEnd,
            [ValueSource("Booleans")] bool startInclusive,
            [ValueSource("Booleans")] bool endInclusive)
        {
            Assume.That(!startInclusive || !endInclusive);

            var intervalA = new IntegerInterval(startAndEnd, startInclusive, startAndEnd, endInclusive);

            var actual = intervalA.IsEmpty();

            Assert.That(actual, Is.True);
        }

        [Test]
        public void UnionWith_SimplifiesSet_ReturnsOtherInterval()
        {
            var set = new[]
            {
                new IntegerInterval(0, true, 1, false),
                new IntegerInterval(2, true, 3, false),
            };

            var interval = new IntegerInterval(1, true, 2, false);

            var actual = set.UnionWith(interval);

            var single = actual.Single(); // Asserts that the array contains a single entry.
            Assert.That(single.Start, Is.EqualTo(0));
            Assert.That(single.End, Is.EqualTo(3));
        }

        [Test]
        public void UnionWith_WhenIntervalsDoNotIntersect_ReturnsBothIntervals()
        {
            var intervalA = new IntegerInterval(0, false, 1, false);
            var intervalB = new IntegerInterval(2, false, 3, false);

            var actual = intervalA.UnionWith(intervalB);

            Assert.That(actual, Is.EquivalentTo(new[] { intervalA, intervalB }));
        }

        [Test]
        public void UnionWith_WhenIntervalsIntersectAtAnExcludedPoint_ReturnsBothIntervals()
        {
            var intervalA = new IntegerInterval(0, false, 2, false);
            var intervalB = new IntegerInterval(2, false, 3, false);

            var actual = intervalA.UnionWith(intervalB);

            Assert.That(actual, Is.EquivalentTo(new[] { intervalA, intervalB }));
        }

        [Test]
        public void UnionWith_WhenIntervalsIntersectAtAnIncludedPoint_ReturnsASingleIntervalThatContainsBothIntervals(
            [ValueSource("Booleans")] bool aEndInclusive,
            [ValueSource("Booleans")] bool bStartInclusive)
        {
            Assume.That(aEndInclusive || bStartInclusive);

            var intervalA = new IntegerInterval(0, false, 2, aEndInclusive);
            var intervalB = new IntegerInterval(2, bStartInclusive, 3, false);

            var actual = intervalA.UnionWith(intervalB);

            var single = actual.Single(); // Asserts that the array contains a single entry.
            Assert.That(single.Contains(intervalA));
            Assert.That(single.Contains(intervalB));
        }

        [Test]
        public void UnionWith_WhenIntervalsOverlap_ReturnsASingleIntervalThatContainsBothIntervals()
        {
            var intervalA = new IntegerInterval(0, false, 2, false);
            var intervalB = new IntegerInterval(1, false, 3, false);

            var actual = intervalA.UnionWith(intervalB);

            var single = actual.Single(); // Asserts that the array contains a single entry.
            Assert.That(single.Contains(intervalA));
            Assert.That(single.Contains(intervalB));
        }

        [Test]
        public void UnionWith_WhenOtherIntervalContainsThisInterval_ReturnsOtherInterval()
        {
            var intervalA = new IntegerInterval(1, false, 2, false);
            var intervalB = new IntegerInterval(0, false, 3, false);

            var actual = intervalA.UnionWith(intervalB);

            Assert.That(actual, Is.EquivalentTo(new[] { intervalB }));
        }

        [Test]
        public void UnionWith_WhenThisIntervalContainsOtherInterval_ReturnsThisInterval()
        {
            var intervalA = new IntegerInterval(0, false, 3, false);
            var intervalB = new IntegerInterval(1, false, 2, false);

            var actual = intervalA.UnionWith(intervalB);

            Assert.That(actual, Is.EquivalentTo(new[] { intervalA }));
        }

        [Test]
        public void UnionWith_WithOtherIntervalEmpty_ReturnsThisInterval()
        {
            var intervalA = new IntegerInterval(1, false, 3, false);
            var intervalB = new IntegerInterval(0, false, 0, false);

            var actual = intervalA.UnionWith(intervalB);

            Assert.That(actual, Is.EquivalentTo(new[] { intervalA }));
        }

        [Test]
        public void UnionWith_WithThisIntervalEmpty_ReturnsOtherInterval()
        {
            var intervalA = new IntegerInterval(0, false, 0, false);
            var intervalB = new IntegerInterval(1, false, 3, false);

            var actual = intervalA.UnionWith(intervalB);

            Assert.That(actual, Is.EquivalentTo(new[] { intervalB }));
        }

        [Test]
        public void UnionWith_WithTwoEmptySets_ReturnsNull()
        {
            IntegerInterval intervalA = null;
            IntegerInterval intervalB = null;

            var actual = intervalA.UnionWith(intervalB);

            Assert.That(actual, Is.Null);
        }
    }
}
