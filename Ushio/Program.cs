using System.Threading.Tasks;

namespace Ushio
{
    class Program
    {
        static Task Main(string[] args) => new Startup(args).StartAsync();
    }
}
