using HarmonyLib;
using RimWorld;
using Verse;

namespace TinyTweaks;

public static class Patch_SkillRecord
{
    [HarmonyPatch(typeof(SkillRecord), nameof(SkillRecord.Interval))]
    public static class Interval
    {
        public static bool Prefix(SkillRecord __instance, Pawn ___pawn)
        {
            // Delay skill decay
            return !TinyTweaksSettings.delayedSkillDecay ||
                   ___pawn.GetComp<CompSkillRecordCache>() is not { } cache ||
                   cache.CanDecaySkill(__instance.def);
        }
    }

    [HarmonyPatch(typeof(SkillRecord), nameof(SkillRecord.Learn))]
    public static class Learn
    {
        public static void Postfix(SkillRecord __instance, Pawn ___pawn, float xp)
        {
            // Update the pawn's CompSkillTrackerCache
            if (!(xp >= CompSkillRecordCache.MinExpToDelaySkillDecay) && (!(xp > 0) ||
                                                                          !(__instance.xpSinceMidnight >=
                                                                            CompSkillRecordCache
                                                                                .MinExpToDelaySkillDecay)))
            {
                return;
            }

            var skillRecordCache = ___pawn.GetComp<CompSkillRecordCache>();
            if (skillRecordCache != null)
            {
                skillRecordCache.NotifySubstantialExperienceGainedFor(__instance.def);
            }
            else
            {
                Log.Warning($"{___pawn} has null CompSkillRecordCache (def={___pawn.def.defName})");
            }
        }
    }
}