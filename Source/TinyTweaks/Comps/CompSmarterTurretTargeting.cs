using Verse;

namespace TinyTweaks;

public class CompSmarterTurretTargeting : ThingComp
{
    public bool attackingNonDownedPawn;

    public override void PostExposeData()
    {
        Scribe_Values.Look(ref attackingNonDownedPawn, "attackingNonDownedPawn");
    }
}