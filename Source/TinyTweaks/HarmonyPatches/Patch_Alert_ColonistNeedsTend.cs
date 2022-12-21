using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using HarmonyLib;
using RimWorld;

namespace TinyTweaks;

public static class Patch_Alert_ColonistNeedsTend
{
    [HarmonyPatch(typeof(Alert_ColonistNeedsTend), "NeedingColonists", MethodType.Getter)]
    public static class Get_NeedingColonists
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
#if DEBUG
                Log.Message("Transpiler start: Alert_ColonistNeedsTend.NeedingColonists (1 match)");
#endif

            var instructionList = instructions.ToList();

            var getMedicalInfo = AccessTools.Property(typeof(Building_Bed), nameof(Building_Bed.Medical))
                .GetGetMethod();
            var allowIfMedicalInfo = AccessTools.Method(typeof(Get_NeedingColonists), nameof(AllowIfMedical));

            foreach (var codeInstruction in instructionList)
            {
                var instruction = codeInstruction;

                // Look for the part of the code that returns false if the pawn's bed is medical; have it return true instead if our settings allow
                if (instruction.opcode == OpCodes.Callvirt && instruction.OperandIs(getMedicalInfo))
                {
#if DEBUG
                        Log.Message("Alert_ColonistNeedsTend.NeedingColonists match 1 of 1");
#endif

                    yield return instruction; // !curBed.Medical
                    instruction = new CodeInstruction(OpCodes.Call, allowIfMedicalInfo);
                }

                yield return instruction;
            }
        }

        private static bool AllowIfMedical(bool original)
        {
            // Return false here because this'll satisfy the condition to show the alert
            return !TinyTweaksSettings.medBedMedicalAlert && original;
        }
    }
}