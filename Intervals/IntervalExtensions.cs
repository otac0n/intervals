//-----------------------------------------------------------------------
// <copyright file="IntervalExtensions.cs" company="(none)">
//  Copyright © 2012 John Gietzen. All rights reserved.
// </copyright>
// <author>John Gietzen</author>
//-----------------------------------------------------------------------

namespace Intervals
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Provides methods for working with object implementing <see cref="IInterval&lt;T&gt;"/>.
    /// </summary>
    public static class IntervalExtensions
    {
        /// <summary>
        /// Tests an interval to determine if it is empty.
        /// </summary>
        /// <typeparam name="T">The type of values included in the interval.</typeparam>
        /// <param name="interval">The interval to test.</param>
        /// <returns>true, if the interval is null or considered empty; false, otherwise.</returns>
        /// <remarks>
        /// If an interval has the same start and end values, but either the start or end value is not inclusive, the interval is empty.
        /// An interval is also considered empty if its start value is greater than its end value.
        /// </remarks>
        public static bool IsEmpty<T>(this IInterval<T> interval) where T : IComparable<T>
        {
            if (interval == null)
            {
                return true;
            }

            var startToEnd = interval.Start.CompareTo(interval.End);
            if (startToEnd > 0)
            {
                return true;
            }
            else if (startToEnd == 0 && (!interval.StartInclusive || !interval.EndInclusive))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Tests a set of intervals to see if it is empty.
        /// </summary>
        /// <typeparam name="T">The type of values included in the intervals.</typeparam>
        /// <param name="set">The set of intervals to test.</param>
        /// <returns>true, if the set is null or considered empty; false, otherwise.</returns>
        /// <remarks>
        /// See <see cref="IsEmpty&lt;T&gt;(IInterval&lt;T&gt;)"/> for remarks.
        /// </remarks>
        public static bool IsEmpty<T>(this IEnumerable<IInterval<T>> set) where T : IComparable<T>
        {
            if (set == null)
            {
                return true;
            }

            foreach (var interval in set)
            {
                if (!interval.IsEmpty())
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Tests an interval to see if it contains a given value.
        /// </summary>
        /// <typeparam name="T">The type of values included in the interval.</typeparam>
        /// <param name="interval">The interval to test.</param>
        /// <param name="value">The value to test.</param>
        /// <returns>true, if the interval is non-empty and contains the value; false, otherwise.</returns>
        public static bool Contains<T>(this IInterval<T> interval, T value) where T : IComparable<T>
        {
            if (interval.IsEmpty())
            {
                return false;
            }

            var start = interval.Start.CompareTo(value);
            var end = interval.End.CompareTo(value);

            if ((interval.StartInclusive && start == 0) ||
                (interval.EndInclusive && end == 0))
            {
                return true;
            }

            if (start >= 0 ||
                end <= 0)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Tests an interval to see if it wholly contains another interval.
        /// </summary>
        /// <typeparam name="T">The type of values included in the intervals.</typeparam>
        /// <param name="interval">The interval to test.</param>
        /// <param name="other">The other interval.</param>
        /// <returns>true, if <paramref name="interval"/> contains <paramref name="other"/>; false, otherwise.</returns>
        /// <remarks>
        /// All intervals (including empty ones) contain every other empty interval.  So, if <paramref name="other"/> is empty, this method always returns true.
        /// Otherwise, this method only returns true if the start and end values of <paramref name="interval"/> surround the values of <paramref name="other"/>.
        /// </remarks>
        public static bool Contains<T>(this IInterval<T> interval, IInterval<T> other) where T : IComparable<T>
        {
            if (other.IsEmpty())
            {
                return true;
            }

            var intersection = other.IntersectWith(interval);

            return !intersection.IsEmpty() &&
                    intersection == other;
        }

        /// <summary>
        /// Tests a set of intervals to see if they contain a value.
        /// </summary>
        /// <typeparam name="T">The type of values included in the intervals.</typeparam>
        /// <param name="set">The set to test.</param>
        /// <param name="value">The value to test.</param>
        /// <returns>true, if the set is non-empty and at least on of its intervals contains the value; false, otherwise.</returns>
        public static bool Contains<T>(this IEnumerable<IInterval<T>> set, T value) where T : IComparable<T>
        {
            foreach (var interval in set)
            {
                if (interval.Contains(value))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Returns the intersection of two intervals.
        /// </summary>
        /// <typeparam name="T">The type of values included in the intervals.</typeparam>
        /// <param name="interval">The first interval.</param>
        /// <param name="other">The second interval.</param>
        /// <returns>An interval that represents the intersection of the intervals.</returns>
        public static IInterval<T> IntersectWith<T>(this IInterval<T> interval, IInterval<T> other) where T : IComparable<T>
        {
            if (interval.IsEmpty() ||
                other.IsEmpty())
            {
                return null;
            }

            T start, end;
            bool startInclusive, endInclusive;

            bool startMatchesInterval = false,
                 startMatchesOther = false,
                 endMatchesInterval = false,
                 endMatchesOther = false;

            var startToStart = interval.Start.CompareTo(other.Start);
            if (startToStart > 0)
            {
                start = interval.Start;
                startInclusive = interval.StartInclusive;
                startMatchesInterval = true;
            }
            else if (startToStart < 0)
            {
                start = other.Start;
                startInclusive = other.StartInclusive;
                startMatchesOther = true;
            }
            else
            {
                start = interval.Start;
                startInclusive = interval.StartInclusive && other.StartInclusive;
                startMatchesInterval = startInclusive == interval.StartInclusive;
                startMatchesOther = startInclusive == other.StartInclusive;
            }

            var endToEnd = interval.End.CompareTo(other.End);
            if (endToEnd < 0)
            {
                end = interval.End;
                endInclusive = interval.EndInclusive;
                endMatchesInterval = true;
            }
            else if (endToEnd > 0)
            {
                end = other.End;
                endInclusive = other.EndInclusive;
                endMatchesOther = true;
            }
            else
            {
                end = interval.End;
                endInclusive = interval.EndInclusive && other.EndInclusive;
                endMatchesInterval = endInclusive == interval.EndInclusive;
                endMatchesOther = endInclusive == other.EndInclusive;
            }

            var startToEnd = start.CompareTo(end);
            if (startToEnd > 0)
            {
                return null;
            }
            else if (startToEnd == 0 && (!startInclusive || !endInclusive))
            {
                return null;
            }

            if (startMatchesInterval && endMatchesInterval)
            {
                return interval;
            }
            else if (startMatchesOther && endMatchesOther)
            {
                return other;
            }

            return interval.Clone(
                start,
                startInclusive,
                end,
                endInclusive);
        }

        /// <summary>
        /// Returns the union of two intervals.
        /// </summary>
        /// <typeparam name="T">The type of values included in the intervals.</typeparam>
        /// <param name="interval">The first interval.</param>
        /// <param name="other">The second interval.</param>
        /// <returns>A set that contains both intervals.</returns>
        /// <remarks>
        /// If the intervals intersect, the result will be a set with a single interval.
        /// </remarks>
        public static IList<IInterval<T>> UnionWith<T>(this IInterval<T> interval, IInterval<T> other) where T : IComparable<T>
        {
            var intervalEmpty = interval.IsEmpty();
            var otherEmpty = other.IsEmpty();

            if (intervalEmpty && otherEmpty)
            {
                return null;
            }
            else if (intervalEmpty)
            {
                return new[] { other };
            }
            else if (otherEmpty)
            {
                return new[] { interval };
            }

            int startToStart, endToEnd, startToEnd;
            T start, end;
            bool startInclusive, endInclusive;

            startToStart = interval.Start.CompareTo(other.Start);
            if (startToStart > 0)
            {
                start = interval.Start;
                startInclusive = interval.StartInclusive;
            }
            else if (startToStart < 0)
            {
                start = other.Start;
                startInclusive = other.StartInclusive;
            }
            else
            {
                start = interval.Start;
                startInclusive = interval.StartInclusive && other.StartInclusive;
            }

            endToEnd = interval.End.CompareTo(other.End);
            if (endToEnd < 0)
            {
                end = interval.End;
                endInclusive = interval.EndInclusive;
            }
            else if (endToEnd > 0)
            {
                end = other.End;
                endInclusive = other.EndInclusive;
            }
            else
            {
                end = interval.End;
                endInclusive = interval.EndInclusive && other.EndInclusive;
            }

            startToEnd = start.CompareTo(end);
            if (startToEnd > 0)
            {
                return new[] { interval, other };
            }
            else if (startToEnd == 0 && !(startInclusive || endInclusive))
            {
                return new[] { interval, other };
            }

            bool startMatchesInterval = false,
                 startMatchesOther = false,
                 endMatchesInterval = false,
                 endMatchesOther = false;

            if (startToStart < 0)
            {
                start = interval.Start;
                startInclusive = interval.StartInclusive;
                startMatchesInterval = true;
            }
            else if (startToStart > 0)
            {
                start = other.Start;
                startInclusive = other.StartInclusive;
                startMatchesOther = true;
            }
            else
            {
                start = interval.Start;
                startInclusive = interval.StartInclusive || other.StartInclusive;
                startMatchesInterval = startInclusive == interval.StartInclusive;
                startMatchesOther = startInclusive == other.StartInclusive;
            }

            if (endToEnd > 0)
            {
                end = interval.End;
                endInclusive = interval.EndInclusive;
                endMatchesInterval = true;
            }
            else if (endToEnd < 0)
            {
                end = other.End;
                endInclusive = other.EndInclusive;
                endMatchesOther = true;
            }
            else
            {
                end = interval.End;
                endInclusive = interval.EndInclusive || other.EndInclusive;
                endMatchesInterval = endInclusive == interval.EndInclusive;
                endMatchesOther = endInclusive == other.EndInclusive;
            }

            if (startMatchesInterval && endMatchesInterval)
            {
                return new[] { interval };
            }
            else if (startMatchesOther && endMatchesOther)
            {
                return new[] { other };
            }

            return new[]
            {
                interval.Clone(
                    start,
                    startInclusive,
                    end,
                    endInclusive)
            };
        }

        /// <summary>
        /// Returns the union of a set and an interval.
        /// </summary>
        /// <typeparam name="T">The type of values included in the intervals.</typeparam>
        /// <param name="set">The set.</param>
        /// <param name="interval">The interval.</param>
        /// <returns>A set containing the union of the set and interval.</returns>
        public static IList<IInterval<T>> UnionWith<T>(this IEnumerable<IInterval<T>> set, IInterval<T> interval) where T : IComparable<T>
        {
            return set.UnionWith(new[] { interval });
        }

        /// <summary>
        /// Returns the union of an interval and a set.
        /// </summary>
        /// <typeparam name="T">The type of values included in the intervals.</typeparam>
        /// <param name="interval">The interval.</param>
        /// <param name="set">The set.</param>
        /// <returns>A set containing the union of the interval and the set.</returns>
        public static IList<IInterval<T>> UnionWith<T>(this IInterval<T> interval, IEnumerable<IInterval<T>> set) where T : IComparable<T>
        {
            return set.UnionWith(new[] { interval });
        }

        /// <summary>
        /// Returns the union of two sets.
        /// </summary>
        /// <typeparam name="T">The type of values included in the intervals.</typeparam>
        /// <param name="setA">The first set.</param>
        /// <param name="setB">The second set.</param>
        /// <returns>A set that contains both sets.</returns>
        public static IList<IInterval<T>> UnionWith<T>(this IEnumerable<IInterval<T>> setA, IEnumerable<IInterval<T>> setB) where T : IComparable<T>
        {
            setA = setA ?? new IInterval<T>[0];
            setB = setB ?? new IInterval<T>[0];

            return setA.Concat(setB).Simplify();
        }

        /// <summary>
        /// Returns a simplified version of a given set.
        /// </summary>
        /// <typeparam name="T">The type of values included in the intervals.</typeparam>
        /// <param name="set">The set to simplify.</param>
        /// <returns>The simplified set.</returns>
        /// <remarks>
        /// Sets can be simplified if any of their components intersect.
        /// </remarks>
        public static IList<IInterval<T>> Simplify<T>(this IEnumerable<IInterval<T>> set) where T : IComparable<T>
        {
            var list = (from r in set
                        where !r.IsEmpty()
                        orderby r.Start descending
                        select r).ToList();

            if (list.Count == 0)
            {
                return null;
            }

            Func<IInterval<T>> dequeue = () =>
            {
                var a = list[list.Count - 1];
                list.RemoveAt(list.Count - 1);
                return a;
            };

            var results = new Stack<IInterval<T>>();
            results.Push(dequeue());

            while (list.Count > 0)
            {
                var intervalA = results.Pop();
                var intervalB = dequeue();

                foreach (var result in intervalA.UnionWith(intervalB))
                {
                    results.Push(result);
                }
            }

            return results.ToArray();
        }

        /// <summary>
        /// Returns the difference of one interval to another.
        /// </summary>
        /// <typeparam name="T">The type of values included in the intervals.</typeparam>
        /// <param name="interval">The source interval.</param>
        /// <param name="other">The interval to exclude.</param>
        /// <returns>A set that contains every part of <paramref name="interval"/> that is not also contained by <paramref name="other"/>.</returns>
        public static IList<IInterval<T>> DifferenceWith<T>(this IInterval<T> interval, IInterval<T> other) where T : IComparable<T>
        {
            if (interval.IsEmpty())
            {
                return null;
            }

            var intersection = interval.IntersectWith(other);

            if (intersection.IsEmpty())
            {
                return new[] { interval };
            }
            else if (intersection == interval)
            {
                return null;
            }

            var intervals = new List<IInterval<T>>();

            var startToStart = interval.Start.CompareTo(intersection.Start);

            if (startToStart != 0 ||
                (interval.StartInclusive && !intersection.StartInclusive))
            {
                intervals.Add(interval.Clone(
                        interval.Start,
                        interval.StartInclusive,
                        intersection.Start,
                        !intersection.StartInclusive));
            }

            var endToEnd = interval.End.CompareTo(intersection.End);

            if (endToEnd != 0 ||
                (interval.EndInclusive && !intersection.EndInclusive))
            {
                intervals.Add(interval.Clone(
                        intersection.End,
                        !intersection.EndInclusive,
                        interval.End,
                        interval.EndInclusive));
            }

            return intervals;
        }

        /// <summary>
        /// Returns the difference between a set and an interval.
        /// </summary>
        /// <typeparam name="T">The type of values included in the intervals.</typeparam>
        /// <param name="set">The source set.</param>
        /// <param name="interval">The interval to exclude.</param>
        /// <returns>A set that contains every part of <paramref name="set"/> that is not also contained by <paramref name="interval"/>.</returns>
        public static IList<IInterval<T>> DifferenceWith<T>(this IEnumerable<IInterval<T>> set, IInterval<T> interval) where T : IComparable<T>
        {
            return set.DifferenceWith(new[] { interval });
        }

        /// <summary>
        /// Returns the difference between an interval and a set.
        /// </summary>
        /// <typeparam name="T">The type of values included in the intervals.</typeparam>
        /// <param name="interval">The source interval.</param>
        /// <param name="set">The set to exclude.</param>
        /// <returns>A set that contains every part of <paramref name="interval"/> that is not also contained by <paramref name="set"/>.</returns>
        public static IList<IInterval<T>> DifferenceWith<T>(this IInterval<T> interval, IEnumerable<IInterval<T>> set) where T : IComparable<T>
        {
            return set.DifferenceWith(new[] { interval });
        }

        /// <summary>
        /// Returns the difference between two sets.
        /// </summary>
        /// <typeparam name="T">The type of values included in the intervals.</typeparam>
        /// <param name="setA">The source set.</param>
        /// <param name="setB">The set to exclude.</param>
        /// <returns>A set that contains every part of <paramref name="setA"/> that is not also contained by <paramref name="setB"/>.</returns>
        public static IList<IInterval<T>> DifferenceWith<T>(this IEnumerable<IInterval<T>> setA, IEnumerable<IInterval<T>> setB) where T : IComparable<T>
        {
            if (setA.IsEmpty())
            {
                return null;
            }
            else if (setB.IsEmpty())
            {
                return setA.ToList();
            }

            List<IInterval<T>> results = null;

            foreach (var intervalB in setB)
            {
                results = new List<IInterval<T>>();

                foreach (var intervalA in setA)
                {
                    var diff = intervalA.DifferenceWith(intervalB);
                    if (diff != null)
                    {
                        results.AddRange(diff);
                    }
                }

                if (results.Count == 0)
                {
                    return null;
                }

                setA = results;
            }

            return results;
        }
    }
}
