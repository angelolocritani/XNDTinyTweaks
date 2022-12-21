using System.Collections.Generic;
using System.Text;
using RimWorld;
using Verse;

namespace TinyTweaks;

public class Alert_LifeThreateningHediffAnimal : Alert_Critical
{
    private List<Pawn> SickAnimals
    {
        get
        {
            var pawns = PawnsFinder.AllMaps_Spawned;
            var result = new List<Pawn>();
            foreach (var p in pawns)
            {
                if (!p.PlayerColonyAnimal_Alive_NoCryptosleep())
                {
                    continue;
                }

                foreach (var diff in p.health.hediffSet.hediffs)
                {
                    if (diff.CurStage == null || !diff.CurStage.lifeThreatening || diff.FullyImmune())
                    {
                        continue;
                    }

                    result.Add(p);
                    break;
                }
            }

            return result;
        }
    }

    public override string GetLabel()
    {
        return "TinyTweaks.AnimalsWithLifeThreateningDisease".Translate();
    }

    public override TaggedString GetExplanation()
    {
        var stringBuilder = new StringBuilder();
        var amputatable = false;
        var sortedAnimals = TinyTweaksUtility.SortedAnimalList(SickAnimals);
        foreach (var pawn in sortedAnimals)
        {
            var listEntry = pawn.NameShortColored.CapitalizeFirst();
            if (pawn.HasBondRelation())
            {
                listEntry += $" {"BondBrackets".Translate()}".Colorize(ColoredText.NameColor);
            }

            stringBuilder.AppendLine("  - " + listEntry.Resolve());
            var hediffs = pawn.health.hediffSet.hediffs;
            foreach (var hediff in hediffs)
            {
                if (hediff.CurStage == null || !hediff.CurStage.lifeThreatening || hediff.Part == null ||
                    hediff.Part == pawn.RaceProps.body.corePart)
                {
                    continue;
                }

                amputatable = true;
                break;
            }
        }

        return string.Format(
            amputatable
                ? "TinyTweaks.AnimalsWithLifeThreateningDiseaseAmputation_Desc".Translate()
                : "TinyTweaks.AnimalsWithLifeThreateningDisease_Desc".Translate(), stringBuilder);
    }

    public override AlertReport GetReport()
    {
        return !TinyTweaksSettings.animalMedicalAlerts ? false : AlertReport.CulpritsAre(SickAnimals);
    }
}