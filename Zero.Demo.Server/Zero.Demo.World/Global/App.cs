using Zero.Demo.Core;
using Zero.Service.Model;

namespace Zero.Demo.World.Global
{
    public static class App
    {
        public static DemoSettings Settings => ZeroConfiguration.GetConfiguration<DemoSettings>();
    }
}
