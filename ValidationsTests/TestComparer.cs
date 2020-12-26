using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace ValidationsTests
{
    class TestComparer : IEqualityComparer<TestClass>
    {
        public bool Equals([AllowNull] TestClass x, [AllowNull] TestClass y)
            => x.I.Equals(y.I);

        public int GetHashCode([DisallowNull] TestClass obj)
            => obj.I.GetHashCode();
    }
}
