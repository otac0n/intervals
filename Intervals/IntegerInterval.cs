// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace Intervals
{
    /// <summary>
    /// Represents an interval between integers.
    /// </summary>
    public sealed class IntegerInterval : IInterval<int>
    {
        private readonly int end;
        private readonly bool endInclusive;
        private readonly int start;
        private readonly bool startInclusive;

        /// <summary>
        /// Initializes a new instance of the <see cref="IntegerInterval"/> class.
        /// </summary>
        /// <param name="start">The starting value of the interval.</param>
        /// <param name="startInclusive">A value indicating whether or not the starting value of this interval is included in the interval.</param>
        /// <param name="end">The ending value of the interval.</param>
        /// <param name="endInclusive">A value indicating whether or not the ending value of this interval is included in the interval.</param>
        public IntegerInterval(int start, bool startInclusive, int end, bool endInclusive)
        {
            this.start = start;
            this.startInclusive = startInclusive;
            this.end = end;
            this.endInclusive = endInclusive;
        }

        /// <summary>
        /// Gets the ending value of this interval.
        /// </summary>
        public int End
        {
            get { return this.end; }
        }

        /// <summary>
        /// Gets a value indicating whether or not the ending value of this interval is included in the interval.
        /// </summary>
        public bool EndInclusive
        {
            get { return this.endInclusive; }
        }

        /// <summary>
        /// Gets the starting value of this interval.
        /// </summary>
        public int Start
        {
            get { return this.start; }
        }

        /// <summary>
        /// Gets a value indicating whether or not the starting value of this interval is included in the interval.
        /// </summary>
        public bool StartInclusive
        {
            get { return this.startInclusive; }
        }

        /// <summary>
        /// Clones an interval with the specified values.
        /// </summary>
        /// <param name="start">The new starting value.</param>
        /// <param name="startInclusive">A value indicating whether or not the new starting value is included in the new interval.</param>
        /// <param name="end">The new ending value.</param>
        /// <param name="endInclusive">A value indicating whether or not the new ending value is included in the new interval.</param>
        /// <returns>A copy of this interval with the specified values.</returns>
        IInterval<int> IInterval<int>.Clone(int start, bool startInclusive, int end, bool endInclusive)
        {
            return new IntegerInterval(start, startInclusive, end, endInclusive);
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return
                (this.StartInclusive ? "[" : "(") +
                this.Start +
                "," +
                this.End +
                (this.EndInclusive ? "]" : ")");
        }
    }
}
