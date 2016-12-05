// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace Intervals
{
    using System;

    /// <summary>
    /// Represents an interval of characters in a string.
    /// </summary>
    public class StringInterval : IInterval<int>
    {
        private readonly int length;
        private readonly string source;
        private readonly int start;
        private readonly string value;

        /// <summary>
        /// Initializes a new instance of the <see cref="StringInterval"/> class.
        /// </summary>
        /// <param name="source">The string that this interval describes.</param>
        public StringInterval(string source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            this.source = source;
            this.start = 0;
            this.length = source.Length;
            this.value = source;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StringInterval"/> class.
        /// </summary>
        /// <param name="source">The string that this interval describes.</param>
        /// <param name="start">The starting index of this interval.</param>
        /// <param name="length">The length of this interval.</param>
        public StringInterval(string source, int start, int length)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            if (start < 0 || start > source.Length)
            {
                throw new ArgumentOutOfRangeException("start");
            }

            if (length < 0 || start + length > source.Length)
            {
                throw new ArgumentOutOfRangeException("start");
            }

            this.source = source;
            this.start = start;
            this.length = length;
            this.value = source.Substring(start, length);
        }

        /// <summary>
        /// Gets the ending index of this interval.
        /// </summary>
        public int End
        {
            get { return this.start + this.length; }
        }

        bool IInterval<int>.EndInclusive
        {
            get { return false; }
        }

        bool IInterval<int>.StartInclusive
        {
            get { return true; }
        }

        /// <summary>
        /// Gets the length of this interval.
        /// </summary>
        public int Length
        {
            get { return this.length; }
        }

        /// <summary>
        /// Gets the string that this interval describes.
        /// </summary>
        public string Source
        {
            get { return this.source; }
        }

        /// <summary>
        /// Gets the starting index of this interval.
        /// </summary>
        public int Start
        {
            get { return this.start; }
        }

        /// <summary>
        /// Gets the portion of the string represented by this interval.
        /// </summary>
        public string Value
        {
            get { return this.value; }
        }

        IInterval<int> IInterval<int>.Clone(int start, bool startInclusive, int end, bool endInclusive)
        {
            return new StringInterval(this.source, start, end - start);
        }
    }
}
