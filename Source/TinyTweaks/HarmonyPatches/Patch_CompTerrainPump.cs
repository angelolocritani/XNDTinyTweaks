using HarmonyLib;
using RimWorld;
using Verse;

namespace TinyTweaks
{
    public static class Patch_CompTerrainPump
    {
        [HarmonyPatch(typeof(CompTerrainPump), nameof(CompTerrainPump.CompTickRare))]
        public static class CompTickRare
        {
            public static void Prefix(CompTerrainPump __instance, int ___progressTicks, ref bool __state)
            {
                // Pass __state as true if the terrain pump is a moisture pump and isn't yet finished
                if (TinyTweaksSettings.autoRemoveMoisturePumps && __instance is CompTerrainPumpDry)
                {
                    var progressDays = ___progressTicks / (float)GenDate.TicksPerDay;
                    var progressPercentage =
                        progressDays / ((CompProperties_TerrainPump)__instance.props).daysToRadius;
                    __state = progressPercentage < 1;
                }
                else
                {
                    __state = false;
                }
            }

            public static void Postfix(CompTerrainPump __instance, int ___progressTicks, ref bool __state)
            {
                // If __state is true and the moisture pump has since finished, auto-designate an appropriate removal on it and send a message
                if (!__state)
                {
                    return;
                }

                var progressDays = ___progressTicks / (float)GenDate.TicksPerDay;
                var progressPercentage =
                    progressDays / ((CompProperties_TerrainPump)__instance.props).daysToRadius;
                if (!(progressPercentage >= 1))
                {
                    return;
                }

                var parent = __instance.parent;
                var parentMap = __instance.parent.Map;
                string messageText = "TinyTweaks.TerrainPumpDryFinished".Translate(parent.Label);
                if (TinyTweaksSettings.autoRemoveMoisturePumps)
                {
                    if (parent.def.Minifiable)
                    {
                        parentMap.designationManager.AddDesignation(new Designation(parent,
                            DesignationDefOf.Uninstall));
                        messageText += $" {"TinyTweaks.TerrainPumpDryFinished_Uninstall".Translate()}";
                    }
                    else
                    {
                        parentMap.designationManager.AddDesignation(new Designation(parent,
                            DesignationDefOf.Deconstruct));
                        messageText += $" {"TinyTweaks.TerrainPumpDryFinished_Deconstruct".Translate()}";
                    }
                }

                Messages.Message(messageText, parent, MessageTypeDefOf.TaskCompletion, false);
            }
        }
    }
}