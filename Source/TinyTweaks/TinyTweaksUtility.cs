using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace TinyTweaks;

[StaticConstructorOnStartup]
public static class TinyTweaksUtility
{
    public static Dictionary<ThingDef, List<RecipeDef>> cachedThingRecipesAlphabetical =
        new Dictionary<ThingDef, List<RecipeDef>>();

    public static void AddComp(this ThingDef def, Type compType)
    {
        if (def.comps.NullOrEmpty())
        {
            def.comps = new List<CompProperties>();
        }

        def.comps.Add(new CompProperties(compType));
    }

    public static void CollapsibleSubheading(this Listing_Standard listing, string label, ref bool checkOn)
    {
        var lineHeight = Text.LineHeight;
        var rowRect = listing.GetRect(lineHeight);
        var anchor = Text.Anchor;

        Text.Anchor = TextAnchor.MiddleLeft;
        Widgets.Label(rowRect, label);

        var buttonRect = new Rect(rowRect.x + rowRect.width - 150, rowRect.y, 150, rowRect.height);
        Text.Anchor = TextAnchor.MiddleCenter;
        if (Widgets.ButtonText(buttonRect, (checkOn ? "TinyTweaks.Collapse" : "TinyTweaks.Expand").Translate()))
        {
            SoundDefOf.Click.PlayOneShot(null);
            checkOn = !checkOn;
        }

        Text.Anchor = anchor;
        listing.Gap(listing.verticalSpacing);
    }

    public static IEnumerable<T> Split<T>(this IEnumerable<T> sequence, Predicate<T> validator)
    {
        foreach (var obj in sequence)
        {
            if (validator(obj))
            {
                yield return obj;
            }
        }
    }

    public static List<Pawn> SortedAnimalList(List<Pawn> pawnList)
    {
        pawnList.SortBy(p => !p.HasBondRelation(), p => p.LabelShort);
        return pawnList;
    }

    public static bool PlayerColonyAnimal(this Pawn p)
    {
        return p.Faction == Faction.OfPlayer && p.RaceProps.Animal;
    }

    public static bool PlayerColonyAnimal_Alive_NoCryptosleep(this Pawn p)
    {
        return p.PlayerColonyAnimal() && !p.Dead && !p.Suspended;
    }

    public static bool HasBondRelation(this Pawn p)
    {
        return TrainableUtility.GetAllColonistBondsFor(p).Any();
    }
}