using HarmonyLib;
using RimWorld;
using Verse;

namespace TinyTweaks
{
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

        [HarmonyPatch(typeof(InteractionWorker_RecruitAttempt), "DoRecruit",
            new[] { typeof(Pawn), typeof(Pawn), typeof(string), typeof(string), typeof(bool), typeof(bool) },
            new[]
            {
                ArgumentType.Normal, ArgumentType.Normal, ArgumentType.Out, ArgumentType.Out, ArgumentType.Normal,
                ArgumentType.Normal
            })]
        public static class InteractionWorker_RecruitAttempt_DoRecruit
        {
            public static void Postfix(Pawn recruitee)
            {
                if (!TinyTweaksSettings.autoOwl)
                {
                    return;
                }

                if (recruitee is not { } pawn)
                {
                    return;
                }

                SetNightOwl(pawn);
            }
        }
    }
}