//-----------------------------------------------------------------------
// <copyright file="IntegerInterval.cs" company="(none)">
//  Copyright © 2012 John Gietzen. All rights reserved.
// </copyright>
// <author>John Gietzen</author>
//-----------------------------------------------------------------------

namespace Intervals
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class IntegerInterval : IInterval<int>
    {
        private readonly int start;
        private readonly bool startInclusive;
        private readonly int end;
        private readonly bool endInclusive;

        public IntegerInterval(int start, bool startInclusive, int end, bool endInclusive)
        {
            this.start = start;
            this.startInclusive = startInclusive;
            this.end = end;
            this.endInclusive = endInclusive;
        }

        public int Start
        {
            get { return this.start; }
        }

        public bool StartInclusive
        {
            get { return this.startInclusive; }
        }

        public int End
        {
            get { return this.end; }
        }

        public bool EndInclusive
        {
            get { return this.endInclusive; }
        }

        public IntegerInterval Clone(int start, bool startInclusive, int end, bool endInclusive)
        {
            return new IntegerInterval(start, startInclusive, end, endInclusive);
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
