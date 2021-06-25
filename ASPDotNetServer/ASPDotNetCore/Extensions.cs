using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace ASPDotNetCore
{
    static public class Extensions
    {
        static public void Add(this IServiceCollection serviceCollection, List<ServiceDescriptor> collection)
        {
            int cnt = collection.Count;
            for(int i = 0; i < cnt; ++i)
            {
                serviceCollection.Add(collection[i]);
            }
        }
    }
}
