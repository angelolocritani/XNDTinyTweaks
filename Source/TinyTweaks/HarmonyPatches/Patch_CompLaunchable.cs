using HarmonyLib;
using RimWorld;

namespace TinyTweaks;

public static class Patch_CompLaunchable
{
    [HarmonyPatch(typeof(CompLaunchable), nameof(CompLaunchable.TryLaunch))]
    public static class TryLaunch
    {
        public static void Postfix(CompLaunchable __instance)
        {
            __instance.parent.BroadcastCompSignal(CompLaunchableAutoRebuild.AutoRebuildSignal);
        }
    }
}