// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace Intervals.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using NUnit.Framework;

    [TestFixture]
    public class IntervalTests
    {
        public static readonly IntegerInterval[] AllIntervals;
        public static readonly bool[] Booleans;
        public static readonly IntegerInterval[] EmptyIntervals;
        public static readonly int[] Integers;
        public static readonly IntegerInterval[] NonEmptyIntervals;
        public static readonly IntegerInterval[] PointIntervals;
        public static readonly IntegerInterval[] SegmentIntervals;

        static IntervalTests()
        {
            Booleans = new[] { false, true };
            Integers = new[] { -3, -2, -1, 0, 1, 2, 3 };

            EmptyIntervals = new[]
            {
                null,
                new IntegerInterval(3, true, -3, true),
                new IntegerInterval(0, true, 0, false),
                new IntegerInterval(2, false, 2, true),
            };

            PointIntervals = Integers.Select(i => new IntegerInterval(i, true, i, true)).ToArray();

            var segments = new List<IntegerInterval>();
            foreach (var i in Integers)
            {
                foreach (var j in Integers)
                {
                    if (i < j)
                    {
                        segments.Add(new IntegerInterval(i, true, j, false));
                        segments.Add(new IntegerInterval(i, false, j, true));
                        segments.Add(new IntegerInterval(i, true, j, true));
                        segments.Add(new IntegerInterval(i, false, j, false));
                    }
                }
            }

            SegmentIntervals = segments.ToArray();
            NonEmptyIntervals = new[] { PointIntervals, SegmentIntervals }.SelectMany(x => x).ToArray();
            AllIntervals = new[] { EmptyIntervals, NonEmptyIntervals }.SelectMany(x => x).ToArray();
        }

        [Test]
        public void Contains_AnyInterval_EmptyInterval_ReturnsTrue(
            [ValueSource(nameof(AllIntervals))] IntegerInterval intervalA,
            [ValueSource(nameof(EmptyIntervals))] IntegerInterval intervalB)
        {
            var actual = intervalA.Contains(intervalB);

            Assert.That(actual, Is.True);
        }

        [Test]
        public void Contains_EmptyInterval_AnyValue_ReturnsFalse(
            [ValueSource(nameof(EmptyIntervals))] IntegerInterval interval,
            [ValueSource(nameof(Integers))] int value)
        {
            var actual = interval.Contains(value);

            Assert.That(actual, Is.False);
        }

        [Test]
        public void Contains_EmptyInterval_NonEmptyInterval_ReturnsFalse(
            [ValueSource(nameof(EmptyIntervals))] IntegerInterval intervalA,
            [ValueSource(nameof(NonEmptyIntervals))] IntegerInterval intervalB)
        {
            var actual = intervalA.Contains(intervalB);

            Assert.That(actual, Is.False);
        }

        [Test]
        [Pairwise]
        public void Contains_WithEndAdjacentZeroIntervalExcluded_ReturnsFalse(
            [ValueSource(nameof(Booleans))] bool aStartInclusive,
            [ValueSource(nameof(Booleans))] bool bStartInclusive,
            [ValueSource(nameof(Booleans))] bool bEndInclusively)
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
            [ValueSource(nameof(Booleans))] bool aStartInclusive,
            [ValueSource(nameof(Booleans))] bool bStartInclusive,
            [ValueSource(nameof(Booleans))] bool bEndInclusively)
        {
            var intervalA = new IntegerInterval(0, aStartInclusive, 3, true);
            var intervalB = new IntegerInterval(3, bStartInclusive, 3, bEndInclusively);

            var result = intervalA.Contains(intervalB);

            Assert.That(result, Is.True);
        }

        [Test]
        [Pairwise]
        public void Contains_WithExclusiveEnd_ReturnsFalse(
            [ValueSource(nameof(Integers))] int start,
            [ValueSource(nameof(Integers))] int end,
            [ValueSource(nameof(Booleans))] bool startInclusive)
        {
            Assume.That(start <= end);

            var interval = new IntegerInterval(start, startInclusive, end, false);

            var result = interval.Contains(end);

            Assert.That(result, Is.False);
        }

        [Test]
        [Pairwise]
        public void Contains_WithExclusiveStart_ReturnsFalse(
            [ValueSource(nameof(Integers))] int start,
            [ValueSource(nameof(Integers))] int end,
            [ValueSource(nameof(Booleans))] bool endInclusive)
        {
            Assume.That(start <= end);

            var interval = new IntegerInterval(start, false, end, endInclusive);

            var result = interval.Contains(start);

            Assert.That(result, Is.False);
        }

        [Test]
        [Pairwise]
        public void Contains_WithFullyContainedInterval_ReturnsTrue(
            [ValueSource(nameof(Booleans))] bool aStartInclusive,
            [ValueSource(nameof(Booleans))] bool aEndInclusive,
            [ValueSource(nameof(Booleans))] bool bStartInclusive,
            [ValueSource(nameof(Booleans))] bool bEndInclusive)
        {
            var intervalA = new IntegerInterval(0, aStartInclusive, 3, aEndInclusive);
            var intervalB = new IntegerInterval(1, bStartInclusive, 2, bEndInclusive);

            var result = intervalA.Contains(intervalB);

            Assert.That(result, Is.True);
        }

        [Test]
        public void Contains_WithIncludedValue_ReturnsTrue(
            [ValueSource(nameof(SegmentIntervals))] IntegerInterval interval,
            [ValueSource(nameof(Integers))] int value)
        {
            Assume.That(interval != null && interval.Start < value && value < interval.End);

            var result = interval.Contains(value);

            Assert.That(result, Is.True);
        }

        [Test]
        [Pairwise]
        public void Contains_WithInclusiveEnd_ReturnsTrue(
            [ValueSource(nameof(Integers))] int start,
            [ValueSource(nameof(Integers))] int end,
            [ValueSource(nameof(Booleans))] bool startInclusive)
        {
            Assume.That(start < end);

            var interval = new IntegerInterval(start, startInclusive, end, true);

            var result = interval.Contains(end);

            Assert.That(result, Is.True);
        }

        [Test]
        [Pairwise]
        public void Contains_WithInclusiveStart_ReturnsTrue(
            [ValueSource(nameof(Integers))] int start,
            [ValueSource(nameof(Integers))] int end,
            [ValueSource(nameof(Booleans))] bool endInclusive)
        {
            Assume.That(start < end);

            var interval = new IntegerInterval(start, true, end, endInclusive);

            var result = interval.Contains(start);

            Assert.That(result, Is.True);
        }

        [Test]
        public void Contains_WithIntervalAdjacentlyExcludedAtEnd_ReturnsTrue(
            [ValueSource(nameof(Booleans))] bool aStartInclusive,
            [ValueSource(nameof(Booleans))] bool bStartInclusive)
        {
            var intervalA = new IntegerInterval(0, aStartInclusive, 3, false);
            var intervalB = new IntegerInterval(1, bStartInclusive, 3, true);

            var result = intervalA.Contains(intervalB);

            Assert.That(result, Is.False);
        }

        [Test]
        public void Contains_WithIntervalAdjacentlyExcludedAtStart_ReturnsFalse(
            [ValueSource(nameof(Booleans))] bool aEndInclusive,
            [ValueSource(nameof(Booleans))] bool bEndInclusive)
        {
            var intervalA = new IntegerInterval(0, false, 3, aEndInclusive);
            var intervalB = new IntegerInterval(0, true, 2, bEndInclusive);

            var result = intervalA.Contains(intervalB);

            Assert.That(result, Is.False);
        }

        [Test]
        [Pairwise]
        public void Contains_WithIntervalAdjacentlyIncludedAtEnd_ReturnsTrue(
            [ValueSource(nameof(Booleans))] bool aStartInclusive,
            [ValueSource(nameof(Booleans))] bool bStartInclusive,
            [ValueSource(nameof(Booleans))] bool bEndInclusive)
        {
            var intervalA = new IntegerInterval(0, aStartInclusive, 3, true);
            var intervalB = new IntegerInterval(1, bStartInclusive, 3, bEndInclusive);

            var result = intervalA.Contains(intervalB);

            Assert.That(result, Is.True);
        }

        [Test]
        [Pairwise]
        public void Contains_WithIntervalAdjacentlyIncludedAtStart_ReturnsTrue(
            [ValueSource(nameof(Booleans))] bool aEndInclusive,
            [ValueSource(nameof(Booleans))] bool bStartInclusive,
            [ValueSource(nameof(Booleans))] bool bEndInclusive)
        {
            var intervalA = new IntegerInterval(0, true, 3, aEndInclusive);
            var intervalB = new IntegerInterval(0, bStartInclusive, 2, bEndInclusive);

            var result = intervalA.Contains(intervalB);

            Assert.That(result, Is.True);
        }

        [Test]
        [Pairwise]
        public void Contains_WithNonIntersectingInterval_ReturnsFalse(
            [ValueSource(nameof(Booleans))] bool aStartInclusive,
            [ValueSource(nameof(Booleans))] bool aEndInclusive,
            [ValueSource(nameof(Booleans))] bool bStartInclusive,
            [ValueSource(nameof(Booleans))] bool bEndInclusive)
        {
            var intervalA = new IntegerInterval(0, aStartInclusive, 1, aEndInclusive);
            var intervalB = new IntegerInterval(2, bStartInclusive, 3, bEndInclusive);

            var result = intervalA.Contains(intervalB);

            Assert.That(result, Is.False);
        }

        [Test]
        public void Contains_WithNullInvterval_ReturnsFalse(
            [ValueSource(nameof(Integers))] int value)
        {
            IntegerInterval interval = null;

            var result = interval.Contains(value);

            Assert.That(result, Is.False);
        }

        [Test]
        public void Contains_WithNullInvtervalAndNonNullOtherInterval_ReturnsFalse()
        {
            var intervalA = (IntegerInterval)null;
            var intervalB = new IntegerInterval(0, true, 1, false);

            var result = intervalA.Contains(intervalB);

            Assert.That(result, Is.False);
        }

        [Test]
        public void Contains_WithNullInvtervalAndNullOtherInterval_ThrowsArgumentNullException()
        {
            IntegerInterval interval = null;

            var result = interval.Contains(interval);

            Assert.That(result, Is.True);
        }

        [Test]
        [Pairwise]
        public void Contains_WithStartAdjacentZeroIntervalExcluded_ReturnsFalse(
            [ValueSource(nameof(Booleans))] bool aEndInclusive,
            [ValueSource(nameof(Booleans))] bool bStartInclusive,
            [ValueSource(nameof(Booleans))] bool bEndInclusively)
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
            [ValueSource(nameof(Booleans))] bool aEndInclusive,
            [ValueSource(nameof(Booleans))] bool bStartInclusive,
            [ValueSource(nameof(Booleans))] bool bEndInclusively)
        {
            var intervalA = new IntegerInterval(0, true, 3, aEndInclusive);
            var intervalB = new IntegerInterval(0, bStartInclusive, 0, bEndInclusively);

            var result = intervalA.Contains(intervalB);

            Assert.That(result, Is.True);
        }

        [Test]
        public void Contains_WithZeroIntervalExclusive_ReturnsFalse(
            [ValueSource(nameof(Booleans))] bool bStartInclusive,
            [ValueSource(nameof(Booleans))] bool bEndInclusively)
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
            [ValueSource(nameof(Integers))] int startAndEnd,
            [ValueSource(nameof(Booleans))] bool startInclusive,
            [ValueSource(nameof(Booleans))] bool endInclusive,
            [ValueSource(nameof(Integers))] int value)
        {
            Assume.That(!startInclusive || !endInclusive);

            var interval = new IntegerInterval(startAndEnd, startInclusive, startAndEnd, endInclusive);

            var result = interval.Contains(value);

            Assert.That(result, Is.False);
        }

        [Test]
        public void DifferenceWith_EmptyInterval_AnyInterval_ReturnsNull(
            [ValueSource(nameof(EmptyIntervals))] IntegerInterval intervalA,
            [ValueSource(nameof(AllIntervals))] IntegerInterval intervalB)
        {
            var actual = intervalA.DifferenceWith(intervalB);

            Assert.That(actual, Is.Null);
        }

        [Test]
        public void DifferenceWith_WhenOtherIntervalContainedByThis_ReturnsTwoIntervalsThatDoNotIntersectOther()
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
        public void IntersectWith_AnyInterval_EmptyInterval_ReturnsNull(
            [ValueSource(nameof(AllIntervals))] IntegerInterval intervalA,
            [ValueSource(nameof(EmptyIntervals))] IntegerInterval intervalB)
        {
            var actual = intervalA.IntersectWith(intervalB);

            Assert.That(actual, Is.Null);
        }

        [Test]
        public void IntersectWith_EmptyInterval_AnyInterval_ReturnsNull(
            [ValueSource(nameof(EmptyIntervals))] IntegerInterval intervalA,
            [ValueSource(nameof(AllIntervals))] IntegerInterval intervalB)
        {
            var actual = intervalA.IntersectWith(intervalB);

            Assert.That(actual, Is.Null);
        }

        [Test]
        [Pairwise]
        public void IntersectWith_WhenIntervalsAreSameSize_ReturnsIntervalWithMostRestrictedInclusivity(
            [ValueSource(nameof(Booleans))] bool aStartInclusive,
            [ValueSource(nameof(Booleans))] bool aEndInclusive,
            [ValueSource(nameof(Booleans))] bool bStartInclusive,
            [ValueSource(nameof(Booleans))] bool bEndInclusive)
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
            [ValueSource(nameof(Booleans))] bool aStartInclusive,
            [ValueSource(nameof(Booleans))] bool aEndInclusive,
            [ValueSource(nameof(Booleans))] bool bStartInclusive,
            [ValueSource(nameof(Booleans))] bool bEndInclusive)
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
            [ValueSource(nameof(Booleans))] bool aStartInclusive,
            [ValueSource(nameof(Booleans))] bool aEndInclusive,
            [ValueSource(nameof(Booleans))] bool bStartInclusive,
            [ValueSource(nameof(Booleans))] bool bEndInclusive)
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
            [ValueSource(nameof(Booleans))] bool aStartInclusive,
            [ValueSource(nameof(Booleans))] bool aEndInclusive,
            [ValueSource(nameof(Booleans))] bool bStartInclusive,
            [ValueSource(nameof(Booleans))] bool bEndInclusive)
        {
            var intervalA = new IntegerInterval(0, aStartInclusive, 1, aEndInclusive);
            var intervalB = new IntegerInterval(2, bStartInclusive, 3, bEndInclusive);

            var actual = intervalA.IntersectWith(intervalB);

            Assert.That(actual, Is.Null);
        }

        [Test]
        [Pairwise]
        public void IntersectWith_WhenIntervalsIntersectInAInterval_ReturnsThatIntervalWithMatchingInclusivity(
            [ValueSource(nameof(Booleans))] bool aStartInclusive,
            [ValueSource(nameof(Booleans))] bool aEndInclusive,
            [ValueSource(nameof(Booleans))] bool bStartInclusive,
            [ValueSource(nameof(Booleans))] bool bEndInclusive)
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
            [ValueSource(nameof(Booleans))] bool aStartInclusive,
            [ValueSource(nameof(Booleans))] bool aEndInclusive,
            [ValueSource(nameof(Booleans))] bool bStartInclusive,
            [ValueSource(nameof(Booleans))] bool bEndInclusive)
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
            [ValueSource(nameof(Booleans))] bool aStartInclusive,
            [ValueSource(nameof(Booleans))] bool aEndInclusive,
            [ValueSource(nameof(Booleans))] bool bStartInclusive,
            [ValueSource(nameof(Booleans))] bool bEndInclusive)
        {
            Assume.That(!aEndInclusive || !bStartInclusive);

            var intervalA = new IntegerInterval(0, aStartInclusive, 2, aEndInclusive);
            var intervalB = new IntegerInterval(2, bStartInclusive, 3, bEndInclusive);

            var actual = intervalA.IntersectWith(intervalB);

            Assert.That(actual, Is.Null);
        }

        [Test]
        public void IntersectWith_WhenIntervalsIntersectInAPoint_ReturnsThatPointWithMatchingInclusivity(
            [ValueSource(nameof(Booleans))] bool aStartInclusive,
            [ValueSource(nameof(Booleans))] bool bEndInclusive)
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
            [ValueSource(nameof(Integers))] int aStart,
            [ValueSource(nameof(Booleans))] bool aStartInclusive,
            [ValueSource(nameof(Integers))] int aEnd,
            [ValueSource(nameof(Booleans))] bool aEndInclusive,
            [ValueSource(nameof(Integers))] int bStartAndEnd,
            [ValueSource(nameof(Booleans))] bool bStartInclusive,
            [ValueSource(nameof(Booleans))] bool bEndInclusive)
        {
            Assume.That(!bStartInclusive || !bEndInclusive);

            var intervalA = new IntegerInterval(aStart, aStartInclusive, aEnd, aEndInclusive);
            var intervalB = new IntegerInterval(bStartAndEnd, false, bStartAndEnd, false);

            var actual = intervalA.IntersectWith(intervalB);

            Assert.That(actual, Is.Null);
        }

        [Test]
        [Pairwise]
        public void IntersectWith_WhenThisIntervalIsEmpty_ReturnsNull(
            [ValueSource(nameof(Integers))] int aStartAndEnd,
            [ValueSource(nameof(Booleans))] bool aStartInclusive,
            [ValueSource(nameof(Booleans))] bool aEndInclusive,
            [ValueSource(nameof(Integers))] int bStart,
            [ValueSource(nameof(Booleans))] bool bStartInclusive,
            [ValueSource(nameof(Integers))] int bEnd,
            [ValueSource(nameof(Booleans))] bool bEndInclusive)
        {
            Assume.That(!aStartInclusive || !aEndInclusive);

            var intervalA = new IntegerInterval(aStartAndEnd, aStartInclusive, aStartAndEnd, aEndInclusive);
            var intervalB = new IntegerInterval(bStart, bStartInclusive, bEnd, bEndInclusive);

            var actual = intervalA.IntersectWith(intervalB);

            Assert.That(actual, Is.Null);
        }

        [Test]
        public void IntersectWith_WithSameInterval_ReturnsOriginalReference(
            [ValueSource(nameof(Booleans))] bool aStartInclusive,
            [ValueSource(nameof(Booleans))] bool aEndInclusive)
        {
            var intervalA = new IntegerInterval(0, aStartInclusive, 3, aEndInclusive);

            var actual = intervalA.IntersectWith(intervalA);

            Assert.That(actual, Is.SameAs(intervalA));
        }

        [Test]
        [Pairwise]
        public void IntersectWith_WithWhollyContianedInterval_ReturnsContainedInterval(
            [ValueSource(nameof(Booleans))] bool aStartInclusive,
            [ValueSource(nameof(Booleans))] bool aEndInclusive,
            [ValueSource(nameof(Booleans))] bool bStartInclusive,
            [ValueSource(nameof(Booleans))] bool bEndInclusive)
        {
            var intervalA = new IntegerInterval(0, aStartInclusive, 3, aEndInclusive);
            var intervalB = new IntegerInterval(1, bStartInclusive, 2, bEndInclusive);

            var actual = intervalA.IntersectWith(intervalB);

            Assert.That(actual, Is.EqualTo(intervalB));
        }

        [Test]
        public void IsEmpty_EmptyInterval_ReturnsTrue(
            [ValueSource(nameof(EmptyIntervals))] IntegerInterval intervalA)
        {
            var actual = intervalA.IsEmpty();

            Assert.That(actual, Is.True);
        }

        [Test]
        public void IsEmpty_NonEmptyInterval_ReturnsFalse(
            [ValueSource(nameof(NonEmptyIntervals))] IntegerInterval intervalA)
        {
            var actual = intervalA.IsEmpty();

            Assert.That(actual, Is.False);
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
            [ValueSource(nameof(Booleans))] bool aEndInclusive,
            [ValueSource(nameof(Booleans))] bool bStartInclusive)
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
