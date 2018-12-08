using System;

namespace Swashbuckle.AspNetCore.Filters.Test.TestFixtures.Examples
{
    public class GuidExample : IExamplesProvider<Nullable<Guid>>
    {
        public Guid? GetExamples()
        {
            return new Guid("11111111-2222-3333-4444-555555555555");
        }
    }
}
