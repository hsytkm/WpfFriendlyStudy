using System.ComponentModel;

namespace WpfDemoApp;

internal static class Enums
{
    internal enum Job
    {
        NotImplemented,  // for Test (not really necessary)
        [Description("🗡")]
        Warrior,
        [Description("🧙")]
        Wizard,
        //[Description("💰")]
        //Merchant,
    };
}
