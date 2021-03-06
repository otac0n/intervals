﻿// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace Intervals
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Represents an interval of comparable values.
    /// </summary>
    /// <typeparam name="T">The type of values included in the interval.</typeparam>
    public interface IInterval<T>
        where T : IComparable<T>
    {
        /// <summary>
        /// Gets the ending value of this interval.
        /// </summary>
        [SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "End", Justification = "This cannot be changed.")]
        T End { get; }

        /// <summary>
        /// Gets a value indicating whether or not the ending value of this interval is included in the interval.
        /// </summary>
        bool EndInclusive { get; }

        /// <summary>
        /// Gets the starting value of this interval.
        /// </summary>
        T Start { get; }

        /// <summary>
        /// Gets a value indicating whether or not the starting value of this interval is included in the interval.
        /// </summary>
        bool StartInclusive { get; }

        /// <summary>
        /// Clones an interval with the specified values.
        /// </summary>
        /// <param name="start">The new starting value.</param>
        /// <param name="startInclusive">A value indicating whether or not the new starting value is included in the new interval.</param>
        /// <param name="end">The new ending value.</param>
        /// <param name="endInclusive">A value indicating whether or not the new ending value is included in the new interval.</param>
        /// <returns>A copy of this interval with the specified values.</returns>
        [SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "End", Justification = "This cannot be changed.")]
        IInterval<T> Clone(T start, bool startInclusive, T end, bool endInclusive);
    }
}
