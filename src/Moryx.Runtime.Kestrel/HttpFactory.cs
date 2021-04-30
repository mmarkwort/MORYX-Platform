using Moryx.Container;
using Moryx.Modules;

namespace Moryx.Runtime.Kestrel
{
    [InitializableKernelComponent(typeof(IHttpFactory))]
    public class HttpFactory : IHttpFactory, IInitializable
    {
        public void Initialize()
        {
            
        }

        public int Value => 45;
    }
}
