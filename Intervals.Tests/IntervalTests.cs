//-----------------------------------------------------------------------
// <copyright file="IntervalTests.cs" company="(none)">
//  Copyright © 2012 John Gietzen. All rights reserved.
// </copyright>
// <author>John Gietzen</author>
//-----------------------------------------------------------------------

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
            NumberInterval intervalA = null;

            var actual = intervalA.IsEmpty();

            Assert.That(actual, Is.True);
        }

        [Theory]
        public void IsEmpty_WithBackwardsSet_ReturnsTrue(int start, int end, bool startInclusive, bool endInclusive)
        {
            Assume.That(start > end);

            var intervalA = new NumberInterval { Start = start, StartInclusive = startInclusive, End = end, EndInclusive = endInclusive };

            var actual = intervalA.IsEmpty();

            Assert.That(actual, Is.True);
        }

        [Theory]
        public void IsEmpty_WithSelfExcludedSet_ReturnsTrue(int startAndEnd, bool startInclusive, bool endInclusive)
        {
            Assume.That(!startInclusive || !endInclusive);

            var intervalA = new NumberInterval { Start = startAndEnd, StartInclusive = startInclusive, End = startAndEnd, EndInclusive = endInclusive };

            var actual = intervalA.IsEmpty();

            Assert.That(actual, Is.True);
        }

        [Theory]
        public void IsEmpty_WithNonSelfExcludedSet_ReturnsFalse(int startAndEnd)
        {
            var intervalA = new NumberInterval { Start = startAndEnd, StartInclusive = true, End = startAndEnd, EndInclusive = true };

            var actual = intervalA.IsEmpty();

            Assert.That(actual, Is.False);
        }

        [Theory]
        public void IsEmpty_WithInOrderSetSet_ReturnsFalse(int start, int end, bool startInclusive, bool endInclusive)
        {
            Assume.That(start < end);

            var intervalA = new NumberInterval { Start = start, StartInclusive = startInclusive, End = end, EndInclusive = endInclusive };

            var actual = intervalA.IsEmpty();

            Assert.That(actual, Is.False);
        }

        [Theory]
        public void Contains_WithIncludedValue_ReturnsTrue(int start, int end, int value, bool startInclusive, bool endInclusive)
        {
            Assume.That(start < value && value < end);

            var interval = new NumberInterval { Start = start, StartInclusive = startInclusive, End = end, EndInclusive = endInclusive };

            var result = interval.Contains(value);

            Assert.That(result, Is.True);
        }

        [Theory]
        public void Contains_WithInclusiveStart_ReturnsTrue(int start, int end, bool endInclusive)
        {
            Assume.That(start < end);

            var interval = new NumberInterval { Start = start, StartInclusive = true, End = end, EndInclusive = endInclusive };

            var result = interval.Contains(start);

            Assert.That(result, Is.True);
        }

        [Theory]
        public void Contains_WithInclusiveEnd_ReturnsTrue(int start, int end, bool startInclusive)
        {
            Assume.That(start < end);

            var interval = new NumberInterval { Start = start, StartInclusive = startInclusive, End = end, EndInclusive = true };

            var result = interval.Contains(end);

            Assert.That(result, Is.True);
        }

        [Theory]
        public void Contains_WithExclusiveStart_ReturnsFalse(int start, int end, bool endInclusive)
        {
            Assume.That(start <= end);

            var interval = new NumberInterval { Start = start, StartInclusive = false, End = end, EndInclusive = endInclusive };

            var result = interval.Contains(start);

            Assert.That(result, Is.False);
        }

        [Theory]
        public void Contains_WithExclusiveEnd_ReturnsFalse(int start, int end, bool startInclusive)
        {
            Assume.That(start <= end);

            var interval = new NumberInterval { Start = start, StartInclusive = startInclusive, End = end, EndInclusive = false };

            var result = interval.Contains(end);

            Assert.That(result, Is.False);
        }

        [Theory]
        public void Contains_WithEmptyInterval_ReturnsFalse(int start, int end, bool startInclusive, bool endInclusive, int value)
        {
            Assume.That(start > end);

            var interval = new NumberInterval { Start = start, StartInclusive = startInclusive, End = end, EndInclusive = endInclusive };

            var result = interval.Contains(value);

            Assert.That(result, Is.False);
        }

        [Theory]
        public void Contains_WithZeroLengthAndExclusiveStartOrEnd_ReturnsFalse(int startAndEnd, bool startInclusive, bool endInclusive, int value)
        {
            Assume.That(!startInclusive || !endInclusive);

            var interval = new NumberInterval { Start = startAndEnd, StartInclusive = startInclusive, End = startAndEnd, EndInclusive = endInclusive };

            var result = interval.Contains(value);

            Assert.That(result, Is.False);
        }

        [Theory]
        public void Contains_WithNonIntersectingInterval_ReturnsFalse(bool aStartInclusive, bool aEndInclusive, bool bStartInclusive, bool bEndInclusive)
        {
            var intervalA = new NumberInterval { Start = 0, StartInclusive = aStartInclusive, End = 1, EndInclusive = aEndInclusive };
            var intervalB = new NumberInterval { Start = 2, StartInclusive = bStartInclusive, End = 3, EndInclusive = bEndInclusive };

            var result = intervalA.Contains(intervalB);

            Assert.That(result, Is.False);
        }

        [Theory]
        public void Contains_WithEmptyInterval_ReturnsTrue(int aStart, bool aStartInclusive, int aEnd, bool aEndInclusive, bool bStartInclusive, bool bEndInclusive)
        {
            var intervalA = new NumberInterval { Start = aStart, StartInclusive = aStartInclusive, End = aStart, EndInclusive = aEndInclusive };
            var intervalB = new NumberInterval { Start = 1, StartInclusive = bStartInclusive, End = 0, EndInclusive = bEndInclusive };

            var result = intervalA.Contains(intervalB);

            Assert.That(result, Is.True);
        }

        [Theory]
        public void Contains_FromEmptyInterval_ReturnsFalse(bool aStartInclusive, bool aEndInclusive, int bStart, bool bStartInclusive, int bEnd, bool bEndInclusive)
        {
            var intervalA = new NumberInterval { Start = 1, StartInclusive = aStartInclusive, End = 0, EndInclusive = aEndInclusive };
            var intervalB = new NumberInterval { Start = bStart, StartInclusive = bStartInclusive, End = bEnd, EndInclusive = bEndInclusive };

            Assume.That(!intervalB.IsEmpty());

            var result = intervalA.Contains(intervalB);

            Assert.That(result, Is.False);
        }

        [Theory]
        public void Contains_WithFullyContainedInterval_ReturnsTrue(bool aStartInclusive, bool aEndInclusive, bool bStartInclusive, bool bEndInclusive)
        {
            var intervalA = new NumberInterval { Start = 0, StartInclusive = aStartInclusive, End = 3, EndInclusive = aEndInclusive };
            var intervalB = new NumberInterval { Start = 1, StartInclusive = bStartInclusive, End = 2, EndInclusive = bEndInclusive };

            var result = intervalA.Contains(intervalB);

            Assert.That(result, Is.True);
        }

        [Theory]
        public void Contains_WithIntervalAdjacentlyIncludedAtStart_ReturnsTrue(bool aEndInclusive, bool bStartInclusive, bool bEndInclusive)
        {
            var intervalA = new NumberInterval { Start = 0, StartInclusive = true, End = 3, EndInclusive = aEndInclusive };
            var intervalB = new NumberInterval { Start = 0, StartInclusive = bStartInclusive, End = 2, EndInclusive = bEndInclusive };

            var result = intervalA.Contains(intervalB);

            Assert.That(result, Is.True);
        }

        [Theory]
        public void Contains_WithIntervalAdjacentlyIncludedAtEnd_ReturnsTrue(bool aStartInclusive, bool bStartInclusive, bool bEndInclusive)
        {
            var intervalA = new NumberInterval { Start = 0, StartInclusive = aStartInclusive, End = 3, EndInclusive = true };
            var intervalB = new NumberInterval { Start = 1, StartInclusive = bStartInclusive, End = 3, EndInclusive = bEndInclusive };

            var result = intervalA.Contains(intervalB);

            Assert.That(result, Is.True);
        }

        [Theory]
        public void Contains_WithIntervalAdjacentlyExcludedAtStart_ReturnsFalse(bool aEndInclusive, bool bEndInclusive)
        {
            var intervalA = new NumberInterval { Start = 0, StartInclusive = false, End = 3, EndInclusive = aEndInclusive };
            var intervalB = new NumberInterval { Start = 0, StartInclusive = true, End = 2, EndInclusive = bEndInclusive };

            var result = intervalA.Contains(intervalB);

            Assert.That(result, Is.False);
        }

        [Theory]
        public void Contains_WithIntervalAdjacentlyExcludedAtEnd_ReturnsTrue(bool aStartInclusive, bool bStartInclusive)
        {
            var intervalA = new NumberInterval { Start = 0, StartInclusive = aStartInclusive, End = 3, EndInclusive = false };
            var intervalB = new NumberInterval { Start = 1, StartInclusive = bStartInclusive, End = 3, EndInclusive = true };

            var result = intervalA.Contains(intervalB);

            Assert.That(result, Is.False);
        }

        [Theory]
        public void Contains_InDegenerateInterval_ReturnsTrue(bool bStartInclusive, bool bEndInclusively)
        {
            var intervalA = new NumberInterval { Start = 0, StartInclusive = true, End = 0, EndInclusive = true };
            var intervalB = new NumberInterval { Start = 0, StartInclusive = bStartInclusive, End = 0, EndInclusive = bEndInclusively };

            var result = intervalA.Contains(intervalB);

            Assert.That(result, Is.True);
        }

        [Theory]
        public void Contains_WithZeroIntervalExclusive_ReturnsFalse(bool aStartInclusive, bool aEndInclusive, bool bStartInclusive, bool bEndInclusively)
        {
            var intervalA = new NumberInterval { Start = 0, StartInclusive = false, End = 0, EndInclusive = false };
            var intervalB = new NumberInterval { Start = 0, StartInclusive = bStartInclusive, End = 0, EndInclusive = bEndInclusively };

            Assume.That(intervalA.IsEmpty());
            Assume.That(!intervalB.IsEmpty());

            var result = intervalA.Contains(intervalB);

            Assert.That(result, Is.False);
        }

        [Theory]
        public void Contains_WithEndAdjacentZeroIntervalExcluded_ReturnsFalse(bool aStartInclusive, bool bStartInclusive, bool bEndInclusively)
        {
            var intervalA = new NumberInterval { Start = 0, StartInclusive = aStartInclusive, End = 3, EndInclusive = false };
            var intervalB = new NumberInterval { Start = 3, StartInclusive = bStartInclusive, End = 3, EndInclusive = bEndInclusively };

            Assume.That(!intervalB.IsEmpty());

            var result = intervalA.Contains(intervalB);

            Assert.That(result, Is.False);
        }

        [Theory]
        public void Contains_WithEndAdjacentZeroIntervalIncluded_ReturnsTrue(bool aStartInclusive, bool bStartInclusive, bool bEndInclusively)
        {
            var intervalA = new NumberInterval { Start = 0, StartInclusive = aStartInclusive, End = 3, EndInclusive = true };
            var intervalB = new NumberInterval { Start = 3, StartInclusive = bStartInclusive, End = 3, EndInclusive = bEndInclusively };

            var result = intervalA.Contains(intervalB);

            Assert.That(result, Is.True);
        }

        [Theory]
        public void Contains_WithStartAdjacentZeroIntervalExcluded_ReturnsFalse(bool aEndInclusive, bool bStartInclusive, bool bEndInclusively)
        {
            var intervalA = new NumberInterval { Start = 0, StartInclusive = false, End = 3, EndInclusive = aEndInclusive };
            var intervalB = new NumberInterval { Start = 0, StartInclusive = bStartInclusive, End = 0, EndInclusive = bEndInclusively };

            Assume.That(!intervalB.IsEmpty());

            var result = intervalA.Contains(intervalB);

            Assert.That(result, Is.False);
        }

        [Theory]
        public void Contains_WithStartAdjacentZeroIntervalIncluded_ReturnsTrue(bool aEndInclusive, bool bStartInclusive, bool bEndInclusively)
        {
            var intervalA = new NumberInterval { Start = 0, StartInclusive = true, End = 3, EndInclusive = aEndInclusive };
            var intervalB = new NumberInterval { Start = 0, StartInclusive = bStartInclusive, End = 0, EndInclusive = bEndInclusively };

            var result = intervalA.Contains(intervalB);

            Assert.That(result, Is.True);
        }

        [Theory]
        public void IntersectWith_WithEmptyInterval_ReturnsNull(bool aStartInclusive, bool aEndInclusive, int bStart, bool bStartInclusive, int bEnd, bool bEndInclusive)
        {
            var intervalA = new NumberInterval { Start = 1, StartInclusive = aStartInclusive, End = 0, EndInclusive = aEndInclusive };
            var intervalB = new NumberInterval { Start = bStart, StartInclusive = bStartInclusive, End = bEnd, EndInclusive = bEndInclusive };

            var actual = intervalA.IntersectWith(intervalB);

            Assert.That(actual, Is.Null);
        }

        [Theory]
        public void IntersectWith_FromEmptyInterval_ReturnsNull(int aStart, bool aStartInclusive, int aEnd, bool aEndInclusive, bool bStartInclusive, bool bEndInclusive)
        {
            var intervalA = new NumberInterval { Start = aStart, StartInclusive = aStartInclusive, End = aEnd, EndInclusive = aEndInclusive };
            var intervalB = new NumberInterval { Start = 1, StartInclusive = bStartInclusive, End = 0, EndInclusive = bEndInclusive };

            var actual = intervalA.IntersectWith(intervalB);

            Assert.That(actual, Is.Null);
        }

        [Theory]
        public void IntersectWith_WithSameInterval_ReturnsOriginalReference(bool aStartInclusive, bool aEndInclusive)
        {
            var intervalA = new NumberInterval { Start = 0, StartInclusive = aStartInclusive, End = 3, EndInclusive = aEndInclusive };

            var actual = intervalA.IntersectWith(intervalA);

            Assert.That(actual, Is.EqualTo(intervalA));
        }

        [Theory]
        public void IntersectWith_WithWhollyContianedInterval_ReturnsContainedInterval(bool aStartInclusive, bool aEndInclusive, bool bStartInclusive, bool bEndInclusive)
        {
            var intervalA = new NumberInterval { Start = 0, StartInclusive = aStartInclusive, End = 3, EndInclusive = aEndInclusive };
            var intervalB = new NumberInterval { Start = 1, StartInclusive = bStartInclusive, End = 2, EndInclusive = bEndInclusive };

            var actual = intervalA.IntersectWith(intervalB);

            Assert.That(actual, Is.EqualTo(intervalB));
        }

        [Theory]
        public void IntersectWith_WhenIntervalsDoNotIntersect_ReturnsNull(bool aStartInclusive, bool aEndInclusive, bool bStartInclusive, bool bEndInclusive)
        {
            var intervalA = new NumberInterval { Start = 0, StartInclusive = aStartInclusive, End = 1, EndInclusive = aEndInclusive };
            var intervalB = new NumberInterval { Start = 2, StartInclusive = bStartInclusive, End = 3, EndInclusive = bEndInclusive };

            var actual = intervalA.IntersectWith(intervalB);

            Assert.That(actual, Is.Null);
        }

        [Theory]
        public void IntersectWith_WhenIntervalsIntersectInAPoint_ReturnsThatPointWithMatchingInclusivity(bool aStartInclusive, bool bEndInclusive)
        {
            var intervalA = new NumberInterval { Start = 0, StartInclusive = aStartInclusive, End = 2, EndInclusive = true };
            var intervalB = new NumberInterval { Start = 2, StartInclusive = true, End = 3, EndInclusive = bEndInclusive };

            var actual = intervalA.IntersectWith(intervalB);

            Assert.That(actual.Start, Is.EqualTo(intervalB.Start));
            Assert.That(actual.End, Is.EqualTo(intervalA.End));
            Assert.That(actual.StartInclusive && actual.EndInclusive, Is.EqualTo(true));
        }

        [Theory]
        public void IntersectWith_WhenIntervalsIntersectInAnExclusivePoint_ReturnsNull(bool aStartInclusive, bool aEndInclusive, bool bStartInclusive, bool bEndInclusive)
        {
            Assume.That(!aEndInclusive || !bStartInclusive);

            var intervalA = new NumberInterval { Start = 0, StartInclusive = aStartInclusive, End = 2, EndInclusive = aEndInclusive };
            var intervalB = new NumberInterval { Start = 2, StartInclusive = bStartInclusive, End = 3, EndInclusive = bEndInclusive };

            var actual = intervalA.IntersectWith(intervalB);

            Assert.That(actual, Is.Null);
        }

        [Theory]
        public void IntersectWith_WhenIntervalsIntersectInAInterval_ReturnsThatIntervalWithMatchingInclusivity(bool aStartInclusive, bool aEndInclusive, bool bStartInclusive, bool bEndInclusive)
        {
            var intervalA = new NumberInterval { Start = 0, StartInclusive = aStartInclusive, End = 2, EndInclusive = aEndInclusive };
            var intervalB = new NumberInterval { Start = 1, StartInclusive = bStartInclusive, End = 3, EndInclusive = bEndInclusive };

            var actual = intervalA.IntersectWith(intervalB);

            Assert.That(actual.Start, Is.EqualTo(intervalB.Start));
            Assert.That(actual.StartInclusive, Is.EqualTo(intervalB.StartInclusive));
            Assert.That(actual.End, Is.EqualTo(intervalA.End));
            Assert.That(actual.EndInclusive, Is.EqualTo(intervalA.EndInclusive));
        }

        [Theory]
        public void IntersectWith_WhenIntervalsIntersectInAIntervalReversed_ReturnsThatIntervalWithMatchingInclusivity(bool aStartInclusive, bool aEndInclusive, bool bStartInclusive, bool bEndInclusive)
        {
            var intervalA = new NumberInterval { Start = 0, StartInclusive = aStartInclusive, End = 2, EndInclusive = aEndInclusive };
            var intervalB = new NumberInterval { Start = 1, StartInclusive = bStartInclusive, End = 3, EndInclusive = bEndInclusive };

            var actual = intervalB.IntersectWith(intervalA);

            Assert.That(actual.Start, Is.EqualTo(intervalB.Start));
            Assert.That(actual.StartInclusive, Is.EqualTo(intervalB.StartInclusive));
            Assert.That(actual.End, Is.EqualTo(intervalA.End));
            Assert.That(actual.EndInclusive, Is.EqualTo(intervalA.EndInclusive));
        }

        [Theory]
        public void IntersectWith_WhenIntervalsAreSameSize_ReturnsIntervalWithMostRestrictedInclusivity(bool aStartInclusive, bool aEndInclusive, bool bStartInclusive, bool bEndInclusive)
        {
            var intervalA = new NumberInterval { Start = 0, StartInclusive = aStartInclusive, End = 3, EndInclusive = aEndInclusive };
            var intervalB = new NumberInterval { Start = 0, StartInclusive = bStartInclusive, End = 3, EndInclusive = bEndInclusive };

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

            var intervalA = new NumberInterval { Start = 0, StartInclusive = aStartInclusive, End = 3, EndInclusive = aEndInclusive };
            var intervalB = new NumberInterval { Start = 0, StartInclusive = bStartInclusive, End = 3, EndInclusive = bEndInclusive };

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

            var intervalA = new NumberInterval { Start = 0, StartInclusive = aStartInclusive, End = 3, EndInclusive = aEndInclusive };
            var intervalB = new NumberInterval { Start = 0, StartInclusive = bStartInclusive, End = 3, EndInclusive = bEndInclusive };

            var actual = intervalA.IntersectWith(intervalB);

            Assert.That(actual, Is.EqualTo(intervalB));
        }

        [Theory]
        public void IntersectWith_WhenThisIntervalIsEmpty_ReturnsNull(int aStartAndEnd, bool aStartInclusive, bool aEndInclusive, int bStart, bool bStartInclusive, int bEnd, bool bEndInclusive)
        {
            Assume.That(!aStartInclusive || !aEndInclusive);

            var intervalA = new NumberInterval { Start = aStartAndEnd, StartInclusive = aStartInclusive, End = aStartAndEnd, EndInclusive = aEndInclusive };
            var intervalB = new NumberInterval { Start = bStart, StartInclusive = bStartInclusive, End = bEnd, EndInclusive = bEndInclusive };

            var actual = intervalA.IntersectWith(intervalB);

            Assert.That(actual, Is.Null);
        }

        [Theory]
        public void IntersectWith_WhenOtherIntervalIsEmpty_ReturnsNull(int aStart, bool aStartInclusive, int aEnd, bool aEndInclusive, int bStartAndEnd, bool bStartInclusive, bool bEndInclusive)
        {
            Assume.That(!bStartInclusive || !bEndInclusive);

            var intervalA = new NumberInterval { Start = aStart, StartInclusive = aStartInclusive, End = aEnd, EndInclusive = aEndInclusive };
            var intervalB = new NumberInterval { Start = 0, StartInclusive = false, End = 0, EndInclusive = false };

            var actual = intervalA.IntersectWith(intervalB);

            Assert.That(actual, Is.Null);
        }

        [Test]
        public void UnionWith_WithTwoEmptySets_ReturnsNull()
        {
            NumberInterval intervalA = null;
            NumberInterval intervalB = null;

            var actual = intervalA.UnionWith(intervalB);

            Assert.That(actual, Is.Null);
        }

        [Test]
        public void UnionWith_WithOtherIntervalEmpty_ReturnsThisInterval()
        {
            var intervalA = new NumberInterval { Start = 1, StartInclusive = false, End = 3, EndInclusive = false };
            var intervalB = new NumberInterval { Start = 0, StartInclusive = false, End = 0, EndInclusive = false };

            var actual = intervalA.UnionWith(intervalB);

            Assert.That(actual, Is.EquivalentTo(new[] { intervalA }));
        }

        [Test]
        public void UnionWith_WithThisIntervalEmpty_ReturnsOtherInterval()
        {
            var intervalA = new NumberInterval { Start = 0, StartInclusive = false, End = 0, EndInclusive = false };
            var intervalB = new NumberInterval { Start = 1, StartInclusive = false, End = 3, EndInclusive = false };

            var actual = intervalA.UnionWith(intervalB);

            Assert.That(actual, Is.EquivalentTo(new[] { intervalB }));
        }

        [Test]
        public void UnionWith_WhenIntervalsDoNotIntersect_ReturnsBothIntervals()
        {
            var intervalA = new NumberInterval { Start = 0, StartInclusive = false, End = 1, EndInclusive = false };
            var intervalB = new NumberInterval { Start = 2, StartInclusive = false, End = 3, EndInclusive = false };

            var actual = intervalA.UnionWith(intervalB);

            Assert.That(actual, Is.EquivalentTo(new[] { intervalA, intervalB }));
        }

        [Test]
        public void UnionWith_WhenIntervalsIntersectAtAnExcludedPoint_ReturnsBothIntervals()
        {
            var intervalA = new NumberInterval { Start = 0, StartInclusive = false, End = 2, EndInclusive = false };
            var intervalB = new NumberInterval { Start = 2, StartInclusive = false, End = 3, EndInclusive = false };

            var actual = intervalA.UnionWith(intervalB);

            Assert.That(actual, Is.EquivalentTo(new[] { intervalA, intervalB }));
        }

        [Theory]
        public void UnionWith_WhenIntervalsIntersectAtAnIncludedPoint_ReturnsASingleIntervalThatContainsBothIntervals(bool aEndInclusive, bool bStartInclusive)
        {
            Assume.That(aEndInclusive || bStartInclusive);

            var intervalA = new NumberInterval { Start = 0, StartInclusive = false, End = 2, EndInclusive = aEndInclusive };
            var intervalB = new NumberInterval { Start = 2, StartInclusive = bStartInclusive, End = 3, EndInclusive = false };

            var actual = intervalA.UnionWith(intervalB);

            var single = actual.Single(); // Asserts that the array contains a single entry.
            Assert.That(single.Contains(intervalA));
            Assert.That(single.Contains(intervalB));
        }

        [Test]
        public void UnionWith_WhenIntervalsOverlap_ReturnsASingleIntervalThatContainsBothIntervals()
        {
            var intervalA = new NumberInterval { Start = 0, StartInclusive = false, End = 2, EndInclusive = false };
            var intervalB = new NumberInterval { Start = 1, StartInclusive = false, End = 3, EndInclusive = false };

            var actual = intervalA.UnionWith(intervalB);

            var single = actual.Single(); // Asserts that the array contains a single entry.
            Assert.That(single.Contains(intervalA));
            Assert.That(single.Contains(intervalB));
        }

        [Test]
        public void UnionWith_WhenThisIntervalContainsOtherInterval_ReturnsThisInterval()
        {
            var intervalA = new NumberInterval { Start = 0, StartInclusive = false, End = 3, EndInclusive = false };
            var intervalB = new NumberInterval { Start = 1, StartInclusive = false, End = 2, EndInclusive = false };

            var actual = intervalA.UnionWith(intervalB);

            Assert.That(actual, Is.EquivalentTo(new[] { intervalA }));
        }

        [Test]
        public void UnionWith_WhenOtherIntervalContainsThisInterval_ReturnsOtherInterval()
        {
            var intervalA = new NumberInterval { Start = 1, StartInclusive = false, End = 2, EndInclusive = false };
            var intervalB = new NumberInterval { Start = 0, StartInclusive = false, End = 3, EndInclusive = false };

            var actual = intervalA.UnionWith(intervalB);

            Assert.That(actual, Is.EquivalentTo(new[] { intervalB }));
        }

        [Test]
        public void UnionWith_SimplifiesSet_ReturnsOtherInterval()
        {
            var set = new[]
            {
                new NumberInterval { Start = 0, StartInclusive = true, End = 1, EndInclusive = false },
                new NumberInterval { Start = 2, StartInclusive = true, End = 3, EndInclusive = false },
            };

            var interval = new NumberInterval { Start = 1, StartInclusive = true, End = 2, EndInclusive = false };

            var actual = set.UnionWith(interval);

            var single = actual.Single(); // Asserts that the array contains a single entry.
            Assert.That(single.Start, Is.EqualTo(0));
            Assert.That(single.End, Is.EqualTo(3));
        }

        [Test]
        public void DifferenceWith_WithEmptyInterval_ReturnsThisInterval()
        {
            var intervalA = new NumberInterval { Start = 0, StartInclusive = false, End = 3, EndInclusive = false };
            var intervalB = new NumberInterval { Start = 2, StartInclusive = false, End = 2, EndInclusive = false };

            var actual = intervalA.DifferenceWith(intervalB);

            Assert.That(actual, Is.EquivalentTo(new[] { intervalA }));
        }

        [Test]
        public void DifferenceWith_FromEmptyInterval_ReturnsNull()
        {
            var intervalA = new NumberInterval { Start = 2, StartInclusive = false, End = 2, EndInclusive = false };
            var intervalB = new NumberInterval { Start = 0, StartInclusive = false, End = 3, EndInclusive = false };

            var actual = intervalA.DifferenceWith(intervalB);

            Assert.That(actual, Is.Null);
        }

        [Test]
        public void DifferenceWith_WhenThisIntervalIsContainedByOtherInterval_ReturnsNull()
        {
            var intervalA = new NumberInterval { Start = 1, StartInclusive = false, End = 2, EndInclusive = false };
            var intervalB = new NumberInterval { Start = 0, StartInclusive = false, End = 3, EndInclusive = false };

            var actual = intervalA.DifferenceWith(intervalB);

            Assert.That(actual, Is.Null);
        }

        [Test]
        public void DifferenceWith_WithIntervalsIntersectingAtStartPoint_ReturnsNewIntervalExcludingStart()
        {
            var intervalA = new NumberInterval { Start = 0, StartInclusive = true, End = 3, EndInclusive = true };
            var intervalB = new NumberInterval { Start = 0, StartInclusive = true, End = 0, EndInclusive = true };

            var actual = intervalA.DifferenceWith(intervalB);

            var single = actual.Single(); // Asserts that the array contains a single entry.
            Assert.That(single.Start, Is.EqualTo(intervalA.Start));
            Assert.That(single.StartInclusive, Is.False);
        }

        [Test]
        public void DifferenceWith_WithIntervalsIntersectingAtEndPoint_ReturnsNewIntervalExcludingEnd()
        {
            var intervalA = new NumberInterval { Start = 0, StartInclusive = true, End = 3, EndInclusive = true };
            var intervalB = new NumberInterval { Start = 3, StartInclusive = true, End = 3, EndInclusive = true };

            var actual = intervalA.DifferenceWith(intervalB);

            var single = actual.Single(); // Asserts that the array contains a single entry.
            Assert.That(single.End, Is.EqualTo(intervalA.End));
            Assert.That(single.EndInclusive, Is.False);
        }

        [Test]
        public void DifferenceWith_WhenOtherOverlapsThisStart_ReturnsNewIntervalExcludingStart()
        {
            var intervalA = new NumberInterval { Start = 1, StartInclusive = true, End = 3, EndInclusive = true };
            var intervalB = new NumberInterval { Start = 0, StartInclusive = true, End = 2, EndInclusive = true };

            var actual = intervalA.DifferenceWith(intervalB);

            var single = actual.Single(); // Asserts that the array contains a single entry.
            Assert.That(single.Start, Is.EqualTo(intervalB.End));
            Assert.That(single.StartInclusive, Is.False);
        }

        [Test]
        public void DifferenceWith_WhenOtherOverlapsThisEnd_ReturnsNewIntervalExcludingStart()
        {
            var intervalA = new NumberInterval { Start = 0, StartInclusive = true, End = 2, EndInclusive = true };
            var intervalB = new NumberInterval { Start = 1, StartInclusive = true, End = 3, EndInclusive = true };

            var actual = intervalA.DifferenceWith(intervalB);

            var single = actual.Single(); // Asserts that the array contains a single entry.
            Assert.That(single.End, Is.EqualTo(intervalB.Start));
            Assert.That(single.EndInclusive, Is.False);
        }

        [Test]
        public void DifferenceWith_WhenOtherIntervalContainedByThis_ReturnsTwoIntervalsThatDontIntersectOther()
        {
            var intervalA = new NumberInterval { Start = 0, StartInclusive = true, End = 3, EndInclusive = true };
            var intervalB = new NumberInterval { Start = 1, StartInclusive = true, End = 2, EndInclusive = true };

            var actual = intervalA.DifferenceWith(intervalB);

            Assert.That(actual.Count, Is.EqualTo(2));
            Assert.That(actual[0].IntersectWith(intervalB).IsEmpty());
            Assert.That(actual[1].IntersectWith(intervalB).IsEmpty());
        }

        [Test]
        public void DifferenceWith_WhenWhollyOverlappedExceptEndPoints_ReturnsTwoDegenerateIntervalsForTheEndPoints()
        {
            var intervalA = new NumberInterval { Start = 0, StartInclusive = true, End = 3, EndInclusive = true };
            var intervalB = new NumberInterval { Start = 0, StartInclusive = false, End = 3, EndInclusive = false };

            var actual = intervalA.DifferenceWith(intervalB);

            Assert.That(actual.Count, Is.EqualTo(2));
            Assert.That(actual[0].Start, Is.EqualTo(actual[0].End));
            Assert.That(actual[1].Start, Is.EqualTo(actual[1].End));
        }

        [Test]
        public void DifferenceWith_WhenSetEsExcluded_ReturnsEmpty()
        {
            var set = new[]
            {
                new NumberInterval { Start = 0, StartInclusive = true, End = 2, EndInclusive = true },
                new NumberInterval { Start = 2, StartInclusive = false, End = 3, EndInclusive = false },
            };

            var interval = new NumberInterval { Start = 0, StartInclusive = true, End = 3, EndInclusive = true };

            var actual = set.DifferenceWith(interval);

            Assert.That(actual.IsEmpty());
        }

        private class NumberInterval : IInterval<int>
        {
            public int Start { get; set; }

            public bool StartInclusive { get; set; }

            public int End { get; set; }

            public bool EndInclusive { get; set; }

            public NumberInterval Clone(int start, bool startInclusive, int end, bool endInclusive)
            {
                return new NumberInterval
                {
                    Start = start,
                    StartInclusive = startInclusive,
                    End = end,
                    EndInclusive = endInclusive,
                };
            }

            IInterval<int> IInterval<int>.Clone(int start, bool startInclusive, int end, bool endInclusive)
            {
                return this.Clone(start, startInclusive, end, endInclusive);
            }

            public override string ToString()
            {
                return
                    (StartInclusive ? "[" : "(") +
                    Start +
                    "," +
                    End +
                    (EndInclusive ? "]" : ")");
            }
        }
    }
}
