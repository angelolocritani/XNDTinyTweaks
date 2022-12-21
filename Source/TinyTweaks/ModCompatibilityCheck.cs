using System;
using System.Linq;
using Verse;

namespace TinyTweaks;

[StaticConstructorOnStartup]
public static class ModCompatibilityCheck
{
    public static bool DubsBadHygiene;

    static ModCompatibilityCheck()
    {
        var loadedMods = ModsConfig.ActiveModsInLoadOrder.ToList();

        foreach (var curMod in loadedMods)
        {
            if (curMod.PackageId.Equals("Dubwise.DubsBadHygiene", StringComparison.CurrentCultureIgnoreCase))
            {
                DubsBadHygiene = true;
            }
        }
    }
}