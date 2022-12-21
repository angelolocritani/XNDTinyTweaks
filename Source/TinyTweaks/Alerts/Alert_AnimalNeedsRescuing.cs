using System.Collections.Generic;
using System.Text;
using RimWorld;
using Verse;

namespace TinyTweaks;

public class Alert_AnimalNeedsRescuing : Alert_Critical
{
    private List<Pawn> AnimalsNeedingRescue
    {
        get
        {
            var pawns = PawnsFinder.AllMaps_Spawned;
            var result = new List<Pawn>();
            foreach (var p in pawns)
            {
                if (p.PlayerColonyAnimal() && Alert_ColonistNeedsRescuing.NeedsRescue(p))
                {
                    result.Add(p);
                }
            }

            return result;
        }
    }

    public override string GetLabel()
    {
        return AnimalsNeedingRescue.Count < 1
            ? "TinyTweaks.AnimalNeedsRescue".Translate()
            : "TinyTweaks.AnimalsNeedRescue".Translate();
    }

    public override TaggedString GetExplanation()
    {
        var stringBuilder = new StringBuilder();
        var sortedAnimals = TinyTweaksUtility.SortedAnimalList(AnimalsNeedingRescue);
        foreach (var pawn in sortedAnimals)
        {
            var listEntry = pawn.NameShortColored.CapitalizeFirst();
            if (pawn.HasBondRelation())
            {
                listEntry += $" {"BondBrackets".Translate()}".Colorize(ColoredText.NameColor);
            }

            stringBuilder.AppendLine("  - " + listEntry.Resolve());
        }

        return string.Format("TinyTweaks.AnimalsNeedRescue_Desc".Translate(), stringBuilder);
    }

    public override AlertReport GetReport()
    {
        return !TinyTweaksSettings.animalMedicalAlerts ? false : AlertReport.CulpritsAre(AnimalsNeedingRescue);
    }
}