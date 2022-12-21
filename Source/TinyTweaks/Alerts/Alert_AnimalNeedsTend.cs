using System.Collections.Generic;
using System.Text;
using RimWorld;
using Verse;

namespace TinyTweaks;

public class Alert_AnimalNeedsTend : Alert
{
    public Alert_AnimalNeedsTend()
    {
        defaultLabel = "TinyTweaks.AnimalNeedsTreatment".Translate();
        defaultPriority = AlertPriority.High;
    }

    private List<Pawn> NeedingAnimals
    {
        get
        {
            var pawns = PawnsFinder.AllMaps_Spawned;
            var result = new List<Pawn>();
            foreach (var p in pawns)
            {
                if (!p.PlayerColonyAnimal() || !p.health.HasHediffsNeedingTendByPlayer(true))
                {
                    continue;
                }

                var curBed = p.CurrentBed();
                if (curBed != null && !TinyTweaksSettings.medBedMedicalAlert && curBed.Medical)
                {
                    continue;
                }

                if (!Alert_ColonistNeedsRescuing.NeedsRescue(p))
                {
                    result.Add(p);
                }
            }

            return result;
        }
    }

    public override string GetLabel()
    {
        return NeedingAnimals.Count <= 1
            ? "TinyTweaks.AnimalNeedsTreatment".Translate()
            : "TinyTweaks.AnimalsNeedTreatment".Translate();
    }

    public override TaggedString GetExplanation()
    {
        var stringBuilder = new StringBuilder();
        var sortedAnimals = TinyTweaksUtility.SortedAnimalList(NeedingAnimals);
        foreach (var pawn in sortedAnimals)
        {
            var listEntry = pawn.NameShortColored.CapitalizeFirst();
            if (pawn.HasBondRelation())
            {
                listEntry += $" {"BondBrackets".Translate()}".Colorize(ColoredText.NameColor);
            }

            stringBuilder.AppendLine("  - " + listEntry.Resolve());
        }

        return string.Format("TinyTweaks.AnimalNeedsTreatment_Desc".Translate(), stringBuilder);
    }

    public override AlertReport GetReport()
    {
        return !TinyTweaksSettings.animalMedicalAlerts ? false : AlertReport.CulpritsAre(NeedingAnimals);
    }
}