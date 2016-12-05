// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace Intervals.Tests
{
    using System.Linq;
    using NUnit.Framework;

    [TestFixture]
    public class IntervalTests
    {
        [Datapoints]
        private int[] intDatapoints = new[] { -3, -2, -1, 0, 1, 2, 3 };

        [Theory]
        public void IsEmpty_WithNullSet_ReturnsTrue(int start, int end, bool startInclusive, bool endInclusive)
        {
            IntegerInterval intervalA = null;

            var actual = intervalA.IsEmpty();

            Assert.That(actual, Is.True);
        }

        [Theory]
        public void IsEmpty_WithBackwardsSet_ReturnsTrue(int start, int end, bool startInclusive, bool endInclusive)
        {
            Assume.That(start > end);

            var intervalA = new IntegerInterval(start, startInclusive, end, endInclusive);

            var actual = intervalA.IsEmpty();

            Assert.That(actual, Is.True);
        }

        [Theory]
        public void IsEmpty_WithSelfExcludedSet_ReturnsTrue(int startAndEnd, bool startInclusive, bool endInclusive)
        {
            Assume.That(!startInclusive || !endInclusive);

            var intervalA = new IntegerInterval(startAndEnd, startInclusive, startAndEnd, endInclusive);

            var actual = intervalA.IsEmpty();

            Assert.That(actual, Is.True);
        }

        [Theory]
        public void IsEmpty_WithNonSelfExcludedSet_ReturnsFalse(int startAndEnd)
        {
            var intervalA = new IntegerInterval(startAndEnd, true, startAndEnd, true);

            var actual = intervalA.IsEmpty();

            Assert.That(actual, Is.False);
        }

        [Theory]
        public void IsEmpty_WithInOrderSetSet_ReturnsFalse(int start, int end, bool startInclusive, bool endInclusive)
        {
            Assume.That(start < end);

            var intervalA = new IntegerInterval(start, startInclusive, end, endInclusive);

            var actual = intervalA.IsEmpty();

            Assert.That(actual, Is.False);
        }

        [Theory]
        public void Contains_WithIncludedValue_ReturnsTrue(int start, int end, int value, bool startInclusive, bool endInclusive)
        {
            Assume.That(start < value && value < end);

            var interval = new IntegerInterval(start, startInclusive, end, endInclusive);

            var result = interval.Contains(value);

            Assert.That(result, Is.True);
        }

        [Theory]
        public void Contains_WithInclusiveStart_ReturnsTrue(int start, int end, bool endInclusive)
        {
            Assume.That(start < end);

            var interval = new IntegerInterval(start, true, end, endInclusive);

            var result = interval.Contains(start);

            Assert.That(result, Is.True);
        }

        [Theory]
        public void Contains_WithInclusiveEnd_ReturnsTrue(int start, int end, bool startInclusive)
        {
            Assume.That(start < end);

            var interval = new IntegerInterval(start, startInclusive, end, true);

            var result = interval.Contains(end);

            Assert.That(result, Is.True);
        }

        [Theory]
        public void Contains_WithExclusiveStart_ReturnsFalse(int start, int end, bool endInclusive)
        {
            Assume.That(start <= end);

            var interval = new IntegerInterval(start, false, end, endInclusive);

            var result = interval.Contains(start);

            Assert.That(result, Is.False);
        }

        [Theory]
        public void Contains_WithExclusiveEnd_ReturnsFalse(int start, int end, bool startInclusive)
        {
            Assume.That(start <= end);

            var interval = new IntegerInterval(start, startInclusive, end, false);

            var result = interval.Contains(end);

            Assert.That(result, Is.False);
        }

        [Theory]
        public void Contains_WithEmptyInterval_ReturnsFalse(int start, int end, bool startInclusive, bool endInclusive, int value)
        {
            Assume.That(start > end);

            var interval = new IntegerInterval(start, startInclusive, end, endInclusive);

            var result = interval.Contains(value);

            Assert.That(result, Is.False);
        }

        [Theory]
        public void Contains_WithZeroLengthAndExclusiveStartOrEnd_ReturnsFalse(int startAndEnd, bool startInclusive, bool endInclusive, int value)
        {
            Assume.That(!startInclusive || !endInclusive);

            var interval = new IntegerInterval(startAndEnd, startInclusive, startAndEnd, endInclusive);

            var result = interval.Contains(value);

            Assert.That(result, Is.False);
        }

        [Theory]
        public void Contains_WithNonIntersectingInterval_ReturnsFalse(bool aStartInclusive, bool aEndInclusive, bool bStartInclusive, bool bEndInclusive)
        {
            var intervalA = new IntegerInterval(0, aStartInclusive, 1, aEndInclusive);
            var intervalB = new IntegerInterval(2, bStartInclusive, 3, bEndInclusive);

            var result = intervalA.Contains(intervalB);

            Assert.That(result, Is.False);
        }

        [Theory]
        public void Contains_WithEmptyInterval_ReturnsTrue(int aStart, bool aStartInclusive, int aEnd, bool aEndInclusive, bool bStartInclusive, bool bEndInclusive)
        {
            var intervalA = new IntegerInterval(aStart, aStartInclusive, aStart, aEndInclusive);
            var intervalB = new IntegerInterval(1, bStartInclusive, 0, bEndInclusive);

            var result = intervalA.Contains(intervalB);

            Assert.That(result, Is.True);
        }

        [Theory]
        public void Contains_FromEmptyInterval_ReturnsFalse(bool aStartInclusive, bool aEndInclusive, int bStart, bool bStartInclusive, int bEnd, bool bEndInclusive)
        {
            var intervalA = new IntegerInterval(1, aStartInclusive, 0, aEndInclusive);
            var intervalB = new IntegerInterval(bStart, bStartInclusive, bEnd, bEndInclusive);

            Assume.That(!intervalB.IsEmpty());

            var result = intervalA.Contains(intervalB);

            Assert.That(result, Is.False);
        }

        [Theory]
        public void Contains_WithFullyContainedInterval_ReturnsTrue(bool aStartInclusive, bool aEndInclusive, bool bStartInclusive, bool bEndInclusive)
        {
            var intervalA = new IntegerInterval(0, aStartInclusive, 3, aEndInclusive);
            var intervalB = new IntegerInterval(1, bStartInclusive, 2, bEndInclusive);

            var result = intervalA.Contains(intervalB);

            Assert.That(result, Is.True);
        }

        [Theory]
        public void Contains_WithIntervalAdjacentlyIncludedAtStart_ReturnsTrue(bool aEndInclusive, bool bStartInclusive, bool bEndInclusive)
        {
            var intervalA = new IntegerInterval(0, true, 3, aEndInclusive);
            var intervalB = new IntegerInterval(0, bStartInclusive, 2, bEndInclusive);

            var result = intervalA.Contains(intervalB);

            Assert.That(result, Is.True);
        }

        [Theory]
        public void Contains_WithIntervalAdjacentlyIncludedAtEnd_ReturnsTrue(bool aStartInclusive, bool bStartInclusive, bool bEndInclusive)
        {
            var intervalA = new IntegerInterval(0, aStartInclusive, 3, true);
            var intervalB = new IntegerInterval(1, bStartInclusive, 3, bEndInclusive);

            var result = intervalA.Contains(intervalB);

            Assert.That(result, Is.True);
        }

        [Theory]
        public void Contains_WithIntervalAdjacentlyExcludedAtStart_ReturnsFalse(bool aEndInclusive, bool bEndInclusive)
        {
            var intervalA = new IntegerInterval(0, false, 3, aEndInclusive);
            var intervalB = new IntegerInterval(0, true, 2, bEndInclusive);

            var result = intervalA.Contains(intervalB);

            Assert.That(result, Is.False);
        }

        [Theory]
        public void Contains_WithIntervalAdjacentlyExcludedAtEnd_ReturnsTrue(bool aStartInclusive, bool bStartInclusive)
        {
            var intervalA = new IntegerInterval(0, aStartInclusive, 3, false);
            var intervalB = new IntegerInterval(1, bStartInclusive, 3, true);

            var result = intervalA.Contains(intervalB);

            Assert.That(result, Is.False);
        }

        [Theory]
        public void Contains_InDegenerateInterval_ReturnsTrue(bool bStartInclusive, bool bEndInclusively)
        {
            var intervalA = new IntegerInterval(0, true, 0, true);
            var intervalB = new IntegerInterval(0, bStartInclusive, 0, bEndInclusively);

            var result = intervalA.Contains(intervalB);

            Assert.That(result, Is.True);
        }

        [Theory]
        public void Contains_WithZeroIntervalExclusive_ReturnsFalse(bool aStartInclusive, bool aEndInclusive, bool bStartInclusive, bool bEndInclusively)
        {
            var intervalA = new IntegerInterval(0, false, 0, false);
            var intervalB = new IntegerInterval(0, bStartInclusive, 0, bEndInclusively);

            Assume.That(intervalA.IsEmpty());
            Assume.That(!intervalB.IsEmpty());

            var result = intervalA.Contains(intervalB);

            Assert.That(result, Is.False);
        }

        [Theory]
        public void Contains_WithEndAdjacentZeroIntervalExcluded_ReturnsFalse(bool aStartInclusive, bool bStartInclusive, bool bEndInclusively)
        {
            var intervalA = new IntegerInterval(0, aStartInclusive, 3, false);
            var intervalB = new IntegerInterval(3, bStartInclusive, 3, bEndInclusively);

            Assume.That(!intervalB.IsEmpty());

            var result = intervalA.Contains(intervalB);

            Assert.That(result, Is.False);
        }

        [Theory]
        public void Contains_WithEndAdjacentZeroIntervalIncluded_ReturnsTrue(bool aStartInclusive, bool bStartInclusive, bool bEndInclusively)
        {
            var intervalA = new IntegerInterval(0, aStartInclusive, 3, true);
            var intervalB = new IntegerInterval(3, bStartInclusive, 3, bEndInclusively);

            var result = intervalA.Contains(intervalB);

            Assert.That(result, Is.True);
        }

        [Theory]
        public void Contains_WithStartAdjacentZeroIntervalExcluded_ReturnsFalse(bool aEndInclusive, bool bStartInclusive, bool bEndInclusively)
        {
            var intervalA = new IntegerInterval(0, false, 3, aEndInclusive);
            var intervalB = new IntegerInterval(0, bStartInclusive, 0, bEndInclusively);

            Assume.That(!intervalB.IsEmpty());

            var result = intervalA.Contains(intervalB);

            Assert.That(result, Is.False);
        }

        [Theory]
        public void Contains_WithStartAdjacentZeroIntervalIncluded_ReturnsTrue(bool aEndInclusive, bool bStartInclusive, bool bEndInclusively)
        {
            var intervalA = new IntegerInterval(0, true, 3, aEndInclusive);
            var intervalB = new IntegerInterval(0, bStartInclusive, 0, bEndInclusively);

            var result = intervalA.Contains(intervalB);

            Assert.That(result, Is.True);
        }

        [Theory]
        public void IntersectWith_WithEmptyInterval_ReturnsNull(bool aStartInclusive, bool aEndInclusive, int bStart, bool bStartInclusive, int bEnd, bool bEndInclusive)
        {
            var intervalA = new IntegerInterval(1, aStartInclusive, 0, aEndInclusive);
            var intervalB = new IntegerInterval(bStart, bStartInclusive, bEnd, bEndInclusive);

            var actual = intervalA.IntersectWith(intervalB);

            Assert.That(actual, Is.Null);
        }

        [Theory]
        public void IntersectWith_FromEmptyInterval_ReturnsNull(int aStart, bool aStartInclusive, int aEnd, bool aEndInclusive, bool bStartInclusive, bool bEndInclusive)
        {
            var intervalA = new IntegerInterval(aStart, aStartInclusive, aEnd, aEndInclusive);
            var intervalB = new IntegerInterval(1, bStartInclusive, 0, bEndInclusive);

            var actual = intervalA.IntersectWith(intervalB);

            Assert.That(actual, Is.Null);
        }

        [Theory]
        public void IntersectWith_WithSameInterval_ReturnsOriginalReference(bool aStartInclusive, bool aEndInclusive)
        {
            var intervalA = new IntegerInterval(0, aStartInclusive, 3, aEndInclusive);

            var actual = intervalA.IntersectWith(intervalA);

            Assert.That(actual, Is.EqualTo(intervalA));
        }

        [Theory]
        public void IntersectWith_WithWhollyContianedInterval_ReturnsContainedInterval(bool aStartInclusive, bool aEndInclusive, bool bStartInclusive, bool bEndInclusive)
        {
            var intervalA = new IntegerInterval(0, aStartInclusive, 3, aEndInclusive);
            var intervalB = new IntegerInterval(1, bStartInclusive, 2, bEndInclusive);

            var actual = intervalA.IntersectWith(intervalB);

            Assert.That(actual, Is.EqualTo(intervalB));
        }

        [Theory]
        public void IntersectWith_WhenIntervalsDoNotIntersect_ReturnsNull(bool aStartInclusive, bool aEndInclusive, bool bStartInclusive, bool bEndInclusive)
        {
            var intervalA = new IntegerInterval(0, aStartInclusive, 1, aEndInclusive);
            var intervalB = new IntegerInterval(2, bStartInclusive, 3, bEndInclusive);

            var actual = intervalA.IntersectWith(intervalB);

            Assert.That(actual, Is.Null);
        }

        [Theory]
        public void IntersectWith_WhenIntervalsIntersectInAPoint_ReturnsThatPointWithMatchingInclusivity(bool aStartInclusive, bool bEndInclusive)
        {
            var intervalA = new IntegerInterval(0, aStartInclusive, 2, true);
            var intervalB = new IntegerInterval(2, true, 3, bEndInclusive);

            var actual = intervalA.IntersectWith(intervalB);

            Assert.That(actual.Start, Is.EqualTo(intervalB.Start));
            Assert.That(actual.End, Is.EqualTo(intervalA.End));
            Assert.That(actual.StartInclusive && actual.EndInclusive, Is.EqualTo(true));
        }

        [Theory]
        public void IntersectWith_WhenIntervalsIntersectInAnExclusivePoint_ReturnsNull(bool aStartInclusive, bool aEndInclusive, bool bStartInclusive, bool bEndInclusive)
        {
            Assume.That(!aEndInclusive || !bStartInclusive);

            var intervalA = new IntegerInterval(0, aStartInclusive, 2, aEndInclusive);
            var intervalB = new IntegerInterval(2, bStartInclusive, 3, bEndInclusive);

            var actual = intervalA.IntersectWith(intervalB);

            Assert.That(actual, Is.Null);
        }

        [Theory]
        public void IntersectWith_WhenIntervalsIntersectInAInterval_ReturnsThatIntervalWithMatchingInclusivity(bool aStartInclusive, bool aEndInclusive, bool bStartInclusive, bool bEndInclusive)
        {
            var intervalA = new IntegerInterval(0, aStartInclusive, 2, aEndInclusive);
            var intervalB = new IntegerInterval(1, bStartInclusive, 3, bEndInclusive);

            var actual = intervalA.IntersectWith(intervalB);

            Assert.That(actual.Start, Is.EqualTo(intervalB.Start));
            Assert.That(actual.StartInclusive, Is.EqualTo(intervalB.StartInclusive));
            Assert.That(actual.End, Is.EqualTo(intervalA.End));
            Assert.That(actual.EndInclusive, Is.EqualTo(intervalA.EndInclusive));
        }

        [Theory]
        public void IntersectWith_WhenIntervalsIntersectInAIntervalReversed_ReturnsThatIntervalWithMatchingInclusivity(bool aStartInclusive, bool aEndInclusive, bool bStartInclusive, bool bEndInclusive)
        {
            var intervalA = new IntegerInterval(0, aStartInclusive, 2, aEndInclusive);
            var intervalB = new IntegerInterval(1, bStartInclusive, 3, bEndInclusive);

            var actual = intervalB.IntersectWith(intervalA);

            Assert.That(actual.Start, Is.EqualTo(intervalB.Start));
            Assert.That(actual.StartInclusive, Is.EqualTo(intervalB.StartInclusive));
            Assert.That(actual.End, Is.EqualTo(intervalA.End));
            Assert.That(actual.EndInclusive, Is.EqualTo(intervalA.EndInclusive));
        }

        [Theory]
        public void IntersectWith_WhenIntervalsAreSameSize_ReturnsIntervalWithMostRestrictedInclusivity(bool aStartInclusive, bool aEndInclusive, bool bStartInclusive, bool bEndInclusive)
        {
            var intervalA = new IntegerInterval(0, aStartInclusive, 3, aEndInclusive);
            var intervalB = new IntegerInterval(0, bStartInclusive, 3, bEndInclusive);

            var actual = intervalA.IntersectWith(intervalB);

            Assert.That(actual.Start, Is.EqualTo(intervalB.Start));
            Assert.That(actual.StartInclusive, Is.EqualTo(intervalA.StartInclusive && intervalB.StartInclusive));
            Assert.That(actual.End, Is.EqualTo(intervalA.End));
            Assert.That(actual.EndInclusive, Is.EqualTo(intervalA.EndInclusive && intervalB.EndInclusive));
        }

        [Theory]
        public void IntersectWith_WhenIntervalsAreSameSizeAndThisIntervalMatchesMostRestrictive_ReturnsThisInterval(bool aStartInclusive, bool aEndInclusive, bool bStartInclusive, bool bEndInclusive)
        {
            Assume.That(bStartInclusive || bEndInclusive);
            Assume.That(bStartInclusive || (!bStartInclusive && !aStartInclusive));
            Assume.That(bEndInclusive || (!bEndInclusive && !aEndInclusive));

            var intervalA = new IntegerInterval(0, aStartInclusive, 3, aEndInclusive);
            var intervalB = new IntegerInterval(0, bStartInclusive, 3, bEndInclusive);

            var actual = intervalA.IntersectWith(intervalB);

            Assert.That(actual, Is.EqualTo(intervalA));
        }

        [Theory]
        public void IntersectWith_WhenIntervalsAreSameSizeAndOtherIntervalMatchesMostRestrictive_ReturnsOtherInterval(bool aStartInclusive, bool aEndInclusive, bool bStartInclusive, bool bEndInclusive)
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

        [Theory]
        public void IntersectWith_WhenThisIntervalIsEmpty_ReturnsNull(int aStartAndEnd, bool aStartInclusive, bool aEndInclusive, int bStart, bool bStartInclusive, int bEnd, bool bEndInclusive)
        {
            Assume.That(!aStartInclusive || !aEndInclusive);

            var intervalA = new IntegerInterval(aStartAndEnd, aStartInclusive, aStartAndEnd, aEndInclusive);
            var intervalB = new IntegerInterval(bStart, bStartInclusive, bEnd, bEndInclusive);

            var actual = intervalA.IntersectWith(intervalB);

            Assert.That(actual, Is.Null);
        }

        [Theory]
        public void IntersectWith_WhenOtherIntervalIsEmpty_ReturnsNull(int aStart, bool aStartInclusive, int aEnd, bool aEndInclusive, int bStartAndEnd, bool bStartInclusive, bool bEndInclusive)
        {
            Assume.That(!bStartInclusive || !bEndInclusive);

            var intervalA = new IntegerInterval(aStart, aStartInclusive, aEnd, aEndInclusive);
            var intervalB = new IntegerInterval(0, false, 0, false);

            var actual = intervalA.IntersectWith(intervalB);

            Assert.That(actual, Is.Null);
        }

        [Test]
        public void UnionWith_WithTwoEmptySets_ReturnsNull()
        {
            IntegerInterval intervalA = null;
            IntegerInterval intervalB = null;

            var actual = intervalA.UnionWith(intervalB);

            Assert.That(actual, Is.Null);
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

        [Theory]
        public void UnionWith_WhenIntervalsIntersectAtAnIncludedPoint_ReturnsASingleIntervalThatContainsBothIntervals(bool aEndInclusive, bool bStartInclusive)
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
        public void UnionWith_WhenThisIntervalContainsOtherInterval_ReturnsThisInterval()
        {
            var intervalA = new IntegerInterval(0, false, 3, false);
            var intervalB = new IntegerInterval(1, false, 2, false);

            var actual = intervalA.UnionWith(intervalB);

            Assert.That(actual, Is.EquivalentTo(new[] { intervalA }));
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
        public void DifferenceWith_WithEmptyInterval_ReturnsThisInterval()
        {
            var intervalA = new IntegerInterval(0, false, 3, false);
            var intervalB = new IntegerInterval(2, false, 2, false);

            var actual = intervalA.DifferenceWith(intervalB);

            Assert.That(actual, Is.EquivalentTo(new[] { intervalA }));
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
        public void DifferenceWith_WhenThisIntervalIsContainedByOtherInterval_ReturnsNull()
        {
            var intervalA = new IntegerInterval(1, false, 2, false);
            var intervalB = new IntegerInterval(0, false, 3, false);

            var actual = intervalA.DifferenceWith(intervalB);

            Assert.That(actual, Is.Null);
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
    }
}
