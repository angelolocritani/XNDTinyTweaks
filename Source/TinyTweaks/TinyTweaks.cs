using HarmonyLib;
using Mlie;
using UnityEngine;
using Verse;

namespace TinyTweaks;

public class TinyTweaks : Mod
{
    public static Harmony harmonyInstance;
    public static string currentVersion;

    public TinyTweaksSettings settings;

    public TinyTweaks(ModContentPack content) : base(content)
    {
#if DEBUG
            Log.Error("XeoNovaDan left debugging enabled in Tiny Tweaks - please let him know!");
#endif

        settings = GetSettings<TinyTweaksSettings>();
        harmonyInstance = new Harmony("XeoNovaDan.TinyTweaks");
        currentVersion =
            VersionFromManifest.GetVersionFromModMetaData(ModLister.GetActiveModWithIdentifier("Mlie.XNDTinyTweaks"));
    }

    public override string SettingsCategory()
    {
        return "TinyTweaks.SettingsCategory".Translate();
    }

    public override void DoSettingsWindowContents(Rect inRect)
    {
        settings.DoWindowContents(inRect);
    }

    public override void WriteSettings()
    {
        base.WriteSettings();

        StartupPatches.SortThingDefRecipes();
    }
}