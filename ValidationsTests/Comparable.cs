using System;
using System.Diagnostics.CodeAnalysis;

namespace ValidationsTests
{
    class Comparable : IComparable<Comparable>
    {
        public int I { get; set; }

        public int CompareTo([AllowNull] Comparable other)
            => I.CompareTo(other.I);
    }
}
