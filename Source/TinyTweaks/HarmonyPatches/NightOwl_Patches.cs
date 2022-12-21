using HarmonyLib;
using RimWorld;
using Verse;

namespace TinyTweaks;

public static class NightOwl_Patches
{
    private static void SetNightOwl(Pawn pawn)
    {
        if (pawn.Faction?.IsPlayer != true)
        {
            return;
        }

        if (pawn.def?.race?.Humanlike != true)
        {
            return;
        }

        if (!pawn.story.traits.HasTrait(TraitDefOf.NightOwl))
        {
            return;
        }

        if (pawn.timetable == null)
        {
            return;
        }

        for (var i = 0; i < GenDate.HoursPerDay; i++)
        {
            if (i is >= 11 and <= 18)
            {
                pawn.timetable.times[i] = TimeAssignmentDefOf.Sleep;
            }
            else
            {
                if (pawn.timetable.times[i] == TimeAssignmentDefOf.Sleep)
                {
                    pawn.timetable.times[i] = TimeAssignmentDefOf.Anything;
                }
            }
        }
    }

    [HarmonyPatch(typeof(Thing), "SpawnSetup")]
    public static class Thing_SpawnSetup
    {
        public static void Postfix(Thing __instance)
        {
            if (!TinyTweaksSettings.autoOwl)
            {
                return;
            }

            if (!(__instance is Pawn pawn))
            {
                return;
            }

            SetNightOwl(pawn);
        }
    }


    [HarmonyPatch(typeof(Pawn), "SetFaction", typeof(Faction), typeof(Pawn))]
    public static class Pawn_SetFaction

    {
        public static void Postfix(Pawn __instance)
        {
            if (!TinyTweaksSettings.autoOwl)
            {
                return;
            }

            if (__instance is not { })
            {
                return;
            }

            SetNightOwl(__instance);
        }
    }
}