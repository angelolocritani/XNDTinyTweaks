using HarmonyLib;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace TinyTweaks;

public static class Patch_WITab_Caravan_Items
{
    [HarmonyPatch(typeof(WITab_Caravan_Items))]
    [HarmonyPatch("FillTab")]
    public static class FillTab
    {
        public static void Postfix(WITab_Caravan_Items __instance, Vector2 ___size)
        {
            if (!TinyTweaksSettings.caravanFoodRestrictions)
            {
                return;
            }

            var tabRect = new Rect(Vector2.zero, ___size);

            // Add a button to assign food restrictions
            if (Widgets.ButtonText(new Rect(tabRect.x + 220, tabRect.y + 10, 200, 27),
                    "TinyTweaks.AssignFoodRestrictions".Translate()))
            {
                Find.WindowStack.Add(
                    new Dialog_AssignCaravanFoodRestrictions(
                        NonPublicProperties.WITab_get_SelCaravan(__instance)));
            }
        }
    }
}