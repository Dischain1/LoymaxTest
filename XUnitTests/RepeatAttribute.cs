using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit.Sdk;

namespace XUnitTests
{
    public sealed class RepeatAttribute : DataAttribute
    {
        private readonly int _count;

        public RepeatAttribute(int count)
        {
            if (count < 1)
            {
                throw new System.ArgumentOutOfRangeException(
                    paramName: nameof(count),
                    message: "Repeat count must be greater than 0."
                );
            }
            this._count = count;
        }

        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            return Enumerable.Range(start: 1, count: this._count)
                .Select(iterationNumber => new object[] { iterationNumber });
        }
    }
}
