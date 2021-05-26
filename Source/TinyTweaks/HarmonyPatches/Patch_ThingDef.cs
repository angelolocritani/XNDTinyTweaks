﻿using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using Verse;

namespace TinyTweaks
{
    public static class Patch_ThingDef
    {
        [HarmonyPatch(typeof(ThingDef), nameof(ThingDef.AllRecipes), MethodType.Getter)]
        public static class AllRecipes_Getter
        {
            public static void Postfix(ThingDef __instance, ref List<RecipeDef> __result)
            {
                // Sort the resulting list alphabetically
                if (!TinyTweaksSettings.alphabeticalBillList)
                {
                    return;
                }

                if (!TinyTweaksUtility.cachedThingRecipesAlphabetical.ContainsKey(__instance))
                {
                    TinyTweaksUtility.cachedThingRecipesAlphabetical.Add(__instance,
                        __result.OrderBy(r => r.label).ToList());
                }

                __result = TinyTweaksUtility.cachedThingRecipesAlphabetical[__instance];
            }
        }
    }
}