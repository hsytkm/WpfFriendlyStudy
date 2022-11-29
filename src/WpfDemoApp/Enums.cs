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

    internal static bool HasMp(this Job job) => job switch
    {
        Job.Wizard => true,
        _ => false
    };
}
