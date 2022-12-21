using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using HarmonyLib;
using RimWorld;
using Verse;

namespace TinyTweaks;

public static class Patch_VerbProperties
{
    [HarmonyPatch(typeof(VerbProperties), nameof(VerbProperties.AdjustedArmorPenetration), typeof(Tool),
        typeof(Pawn), typeof(Thing), typeof(HediffComp_VerbGiver))]
    public static class AdjustedArmorPenetration
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
#if DEBUG
                Log.Message("Transpiler start: VerbProperties.AdjustedArmorPenetration (1 match)");
#endif

            var instructionList = instructions.ToList();

            var armourPenetrationInfo = AccessTools.Field(typeof(Tool), nameof(Tool.armorPenetration));
            var adjustedToolArmourPenetrationInfo = AccessTools.Method(typeof(AdjustedArmorPenetration),
                nameof(AdjustedToolArmourPenetration));

            foreach (var codeInstruction in instructionList)
            {
                var instruction = codeInstruction;

                // If a tool's AP is set, adjust it based on other factors (i.e. stuff and pawn age)
                if (instruction.opcode == OpCodes.Ldfld && instruction.OperandIs(armourPenetrationInfo))
                {
#if DEBUG
                        Log.Message("VerbProperties.AdjustedArmorPenetration match 1 of 1");
#endif

                    yield return instruction; // tool.armorPenetration
                    yield return new CodeInstruction(OpCodes.Ldarg_0); // this
                    yield return new CodeInstruction(OpCodes.Ldarg_1); // tool 
                    yield return new CodeInstruction(OpCodes.Ldarg_2); // attacker
                    yield return new CodeInstruction(OpCodes.Ldarg_3); // equipment
                    yield return new CodeInstruction(OpCodes.Ldarg_S, 4); // hediffCompSource
                    instruction =
                        new CodeInstruction(OpCodes.Call,
                            adjustedToolArmourPenetrationInfo); // AdjustedToolArmourPenetration(tool.armorPenetration, this, tool, attacker, equipment, hediffCompSource)
                }

                yield return instruction;
            }
        }

        private static float AdjustedToolArmourPenetration(float armourPenetration, VerbProperties instance,
            Tool tool, Pawn attacker, Thing equipment, HediffComp_VerbGiver hediffCompSource)
        {
            // Scale AP with stuff and pawn age
            if (!TinyTweaksSettings.meleeArmourPenetrationFix || !(armourPenetration > -1))
            {
                return armourPenetration;
            }

            // Factor in equipment stuff
            if (equipment is { Stuff: { } } && instance.meleeDamageDef != null)
            {
                armourPenetration *=
                    equipment.Stuff.GetStatValueAbstract(instance.meleeDamageDef.armorCategory.multStat);
            }

            // Factor in attacker
            if (attacker != null)
            {
                armourPenetration *= instance.GetDamageFactorFor(tool, attacker, hediffCompSource);
            }

            return armourPenetration;
        }
    }
}