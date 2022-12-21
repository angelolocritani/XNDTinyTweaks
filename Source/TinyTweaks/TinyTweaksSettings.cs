using RimWorld;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace TinyTweaks;

public class TinyTweaksSettings : ModSettings
{
    private const float PageButtonWidth = 150;
    private const float PageButtonHeight = 35;
    private const float PageButtonPosOffsetFromCentre = 60;

    private const int QoLPageIndex = 1;
    private const int BugFixPageIndex = 2;
    private const int BalancePageIndex = 3;
    private const int AdditionsPageIndex = 4;

    private const int MaxPageIndex = AdditionsPageIndex;
    public static bool meleeArmourPenetrationFix = true;

    public static bool randomStartingSeason = true;
    private static int _pageIndex = 1;

    public static bool animalMedicalAlerts = true;
    public static bool caravanFoodRestrictions = true;
    public static bool autoAssignAnimalFollowSettings = true;
    public static bool autoRemoveMoisturePumps = true;
    public static bool autoOwl = true;
    public static bool medBedMedicalAlert = true;
    public static bool alphabeticalBillList = true;

    // Restart
    public static bool changeDefLabels = true;
    public static bool changeBuildableDefDesignationCategories = true;

    public static bool changeQualityDistribution;
    public static bool bloodPumpingAffectsBleeding;
    public static bool delayedSkillDecay;

    private static int PageIndex
    {
        get => _pageIndex;
        set => _pageIndex = Mathf.Clamp(value, 1, MaxPageIndex);
    }

    private void DoHeading(Listing_Standard listing, GameFont font)
    {
        listing.Gap();
        var headingTranslationKey = "TinyTweaks.";
        switch (PageIndex)
        {
            case QoLPageIndex:
                headingTranslationKey += "QualityOfLifeChangesHeading";
                goto WriteHeader;
            case BugFixPageIndex:
                headingTranslationKey += "BugFixesHeading";
                goto WriteHeader;
            case BalancePageIndex:
                headingTranslationKey += "BalanceChangesHeading";
                goto WriteHeader;
            case AdditionsPageIndex:
                headingTranslationKey += "TinyAdditionsHeading";
                goto WriteHeader;
        }

        WriteHeader:
        Text.Font = font + 1;
        listing.Label(headingTranslationKey.Translate());
        Text.Font = font;
        listing.GapLine(24);
    }

    private void GameRestartNotRequired(Listing_Standard listing)
    {
        listing.Gap();
        listing.Label("TinyTweaks.GameRestartNotRequired".Translate());
    }

    private void GameRestartRequired(Listing_Standard listing)
    {
        listing.Gap();
        listing.Label("TinyTweaks.GameRestartRequired".Translate());
    }

    private void DoQualityOfLifeChanges(Listing_Standard options)
    {
        // 'Game restart not required' note
        GameRestartNotRequired(options);

        // Animal medical alerts
        options.Gap();
        options.CheckboxLabeled("TinyTweaks.QoLChanges.AnimalMedicalAlerts".Translate(), ref animalMedicalAlerts,
            "TinyTweaks.QoLChanges.AnimalMedicalAlerts_ToolTip".Translate());

        // Assign food restrictions for caravans
        options.Gap();
        options.CheckboxLabeled("TinyTweaks.QoLChanges.CaravanFoodRestrictions".Translate(),
            ref caravanFoodRestrictions, "TinyTweaks.QoLChanges.CaravanFoodRestrictions_ToolTip".Translate());

        // Automatically assign animals to follow their master
        options.Gap();
        options.CheckboxLabeled("TinyTweaks.QoLChanges.AutoAssignAnimalFollowSettings".Translate(),
            ref autoAssignAnimalFollowSettings,
            "TinyTweaks.QoLChanges.AutoAssignAnimalFollowSettings_ToolTip".Translate());

        // Automatically remove finished moisture pumps
        options.Gap();
        options.CheckboxLabeled("TinyTweaks.QoLChanges.AutoRemoveTerrainPumpDry".Translate(),
            ref autoRemoveMoisturePumps, "TinyTweaks.QoLChanges.AutoRemoveTerrainPumpDry_ToolTip".Translate());

        // Automatically set night owl timetables
        options.Gap();
        options.CheckboxLabeled("TinyTweaks.QoLChanges.AutoOwl".Translate(), ref autoOwl,
            "TinyTweaks.QoLChanges.AutoOwl_ToolTip".Translate());

        // Show 'colonist needs treatment' alerts for pawns in medical beds
        options.Gap();
        options.CheckboxLabeled("TinyTweaks.QoLChanges.MedBedMedicalAlert".Translate(), ref medBedMedicalAlert,
            "TinyTweaks.QoLChanges.MedBedMedicalAlert_ToolTip".Translate());

        // Sort workbench bill list alphabetically
        options.Gap();
        options.CheckboxLabeled("TinyTweaks.QoLChanges.AlphabeticalBillList".Translate(), ref alphabeticalBillList,
            "TinyTweaks.QoLChanges.AlphabeticalBillList_ToolTip".Translate());


        // 'Game restart required' note
        options.GapLine(24);
        GameRestartRequired(options);

        if (ModLister.GetActiveModWithIdentifier("LWM.DeepStorage") == null)
        {
            // Change architect menu tabs
            options.Gap();
            options.CheckboxLabeled("TinyTweaks.QoLChanges.ChangeBuildableDefDesignationCategories".Translate(),
                ref changeBuildableDefDesignationCategories,
                "TinyTweaks.QoLChanges.ChangeBuildableDefDesignationCategories_ToolTip".Translate());
        }

        // Consistent label casing
        options.Gap();
        options.CheckboxLabeled("TinyTweaks.QoLChanges.ChangeDefLabels".Translate(), ref changeDefLabels,
            "TinyTweaks.QoLChanges.ChangeDefLabels_ToolTip".Translate());
    }

    private void DoBugFixes(Listing_Standard options)
    {
        // 'Game restart not required' note
        GameRestartNotRequired(options);

        // Melee weapon AP fix
        options.Gap();
        options.CheckboxLabeled("TinyTweaks.BugFixes.MeleeArmourPenetration".Translate(),
            ref meleeArmourPenetrationFix, "TinyTweaks.BugFixes.MeleeArmourPenetration_ToolTip".Translate());
    }

    private void DoBalanceChanges(Listing_Standard options)
    {
        // 'Game restart not required' note
        GameRestartNotRequired(options);

        // Blood pumping affects bleeding
        options.Gap();
        options.CheckboxLabeled("TinyTweaks.BalanceChanges.BloodPumpingAffectsBleeding".Translate(),
            ref bloodPumpingAffectsBleeding,
            "TinyTweaks.BalanceChanges.BloodPumpingAffectsBleeding_ToolTip".Translate());

        // Change quality distribution
        options.Gap();
        options.CheckboxLabeled("TinyTweaks.BalanceChanges.ChangeQualityDistribution".Translate(),
            ref changeQualityDistribution,
            "TinyTweaks.BalanceChanges.ChangeQualityDistribution_ToolTip".Translate());

        // Delayed skill decay
        options.Gap();
        options.CheckboxLabeled("TinyTweaks.BalanceChanges.DelayedSkillDecay".Translate(), ref delayedSkillDecay,
            "TinyTweaks.BalanceChanges.DelayedSkillDecay_ToolTip".Translate());
    }

    private void DoAdditions(Listing_Standard options)
    {
        // 'Game restart not required' note
        GameRestartNotRequired(options);

        // Random season button
        options.Gap();
        options.CheckboxLabeled("TinyTweaks.TinyAdditions.RandomStartingSeason".Translate(),
            ref randomStartingSeason, "TinyTweaks.TinyAdditions.RandomStartingSeason_ToolTip".Translate());
    }

    private void DoPageButtons(Rect wrect)
    {
        var halfRectWidth = wrect.width / 2;
        var xOffset = (halfRectWidth - PageButtonWidth) / 2;
        var leftButtonRect = new Rect(xOffset + PageButtonPosOffsetFromCentre, wrect.height - PageButtonHeight,
            PageButtonWidth, PageButtonHeight);
        if (Widgets.ButtonText(leftButtonRect, "TinyTweaks.PreviousPage".Translate()))
        {
            SoundDefOf.Click.PlayOneShot(null);
            PageIndex--;
        }

        var rightButtonRect = new Rect(halfRectWidth + xOffset - PageButtonPosOffsetFromCentre,
            wrect.height - PageButtonHeight, PageButtonWidth, PageButtonHeight);

        if (Widgets.ButtonText(rightButtonRect, "TinyTweaks.NextPage".Translate()))
        {
            SoundDefOf.Click.PlayOneShot(null);
            PageIndex++;
        }

        Text.Anchor = TextAnchor.MiddleCenter;
        var pageNumberRect = new Rect(0, wrect.height - PageButtonHeight, wrect.width, PageButtonHeight);
        Widgets.Label(pageNumberRect, $"{PageIndex} / {MaxPageIndex}");
        Text.Anchor = TextAnchor.UpperLeft;
    }

    public void DoWindowContents(Rect wrect)
    {
        var options = new Listing_Standard();
        var defaultColor = GUI.color;
        options.Begin(wrect);
        GUI.color = defaultColor;
        Text.Font = GameFont.Small;
        Text.Anchor = TextAnchor.UpperLeft;

        DoHeading(options, Text.Font);

        switch (PageIndex)
        {
            case QoLPageIndex:
                DoQualityOfLifeChanges(options);
                break;
            case BugFixPageIndex:
                DoBugFixes(options);
                break;
            case BalancePageIndex:
                DoBalanceChanges(options);
                break;
            case AdditionsPageIndex:
                DoAdditions(options);
                break;
        }

        DoPageButtons(wrect);
        if (TinyTweaks.currentVersion != null)
        {
            options.Gap();
            GUI.contentColor = Color.gray;
            options.Label("TinyTweaks.CurrentModVersion".Translate(TinyTweaks.currentVersion));
            GUI.contentColor = Color.white;
        }

        // Finish
        options.End();
        //Mod.GetSettings<TinyTweaksSettings>().Write();
    }

    public override void ExposeData()
    {
        Scribe_Values.Look(ref animalMedicalAlerts, "animalMedicalAlerts", true);
        Scribe_Values.Look(ref caravanFoodRestrictions, "caravanFoodRestrictions", true);
        Scribe_Values.Look(ref autoAssignAnimalFollowSettings, "autoAssignAnimalFollowSettings", true);
        Scribe_Values.Look(ref autoRemoveMoisturePumps, "autoRemoveMoisturePumps", true);
        Scribe_Values.Look(ref autoOwl, "autoOwl", true);
        Scribe_Values.Look(ref medBedMedicalAlert, "medBedMedicalAlert", true);
        Scribe_Values.Look(ref alphabeticalBillList, "alphabeticalBillList", true);

        // Restart
        Scribe_Values.Look(ref changeDefLabels, "changeDefLabels", true);
        Scribe_Values.Look(ref changeBuildableDefDesignationCategories, "changeBuildableDefDesignationCategories",
            true);

        Scribe_Values.Look(ref meleeArmourPenetrationFix, "meleeArmourPenetrationFix", true);

        Scribe_Values.Look(ref changeQualityDistribution, "changeQualityDistribution");
        Scribe_Values.Look(ref bloodPumpingAffectsBleeding, "bloodPumpingAffectsBleeding");
        Scribe_Values.Look(ref delayedSkillDecay, "delayedSkillDecay");

        Scribe_Values.Look(ref randomStartingSeason, "randomStartingSeason", true);
    }
}