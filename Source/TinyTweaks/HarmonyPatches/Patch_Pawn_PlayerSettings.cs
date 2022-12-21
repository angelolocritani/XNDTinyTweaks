using HarmonyLib;
using RimWorld;
using Verse;

namespace TinyTweaks;

public static class Patch_Pawn_PlayerSettings
{
    [HarmonyPatch(typeof(Pawn_PlayerSettings), MethodType.Constructor, typeof(Pawn))]
    public static class Ctor
    {
        public static void Postfix(Pawn_PlayerSettings __instance, Pawn pawn)
        {
            // If the pawn is an animal, automatically assign them to follow when drafted/doing fieldwork
            if (!TinyTweaksSettings.autoAssignAnimalFollowSettings || !pawn.RaceProps.Animal)
            {
                return;
            }

            __instance.followDrafted = true;
            __instance.followFieldwork = true;
        }
    }
}