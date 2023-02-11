using Mlie;
using UnityEngine;
using Verse;

namespace HalfDragons;

[StaticConstructorOnStartup]
internal class HalfDragonsMod : Mod
{
    /// <summary>
    ///     The instance of the settings to be read by the mod
    /// </summary>
    public static HalfDragonsMod instance;

    private static string currentVersion;

    /// <summary>
    ///     The private settings
    /// </summary>
    private HalfDragonsSettings settings;

    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="content"></param>
    public HalfDragonsMod(ModContentPack content) : base(content)
    {
        instance = this;
        currentVersion = VersionFromManifest.GetVersionFromModMetaData(content.ModMetaData);
    }

    /// <summary>
    ///     The instance-settings for the mod
    /// </summary>
    internal HalfDragonsSettings Settings
    {
        get
        {
            if (settings == null)
            {
                settings = GetSettings<HalfDragonsSettings>();
            }

            return settings;
        }
        set => settings = value;
    }

    /// <summary>
    ///     The title for the mod-settings
    /// </summary>
    /// <returns></returns>
    public override string SettingsCategory()
    {
        return "Half Dragons";
    }

    /// <summary>
    ///     The settings-window
    ///     For more info: https://rimworldwiki.com/wiki/Modding_Tutorials/ModSettings
    /// </summary>
    /// <param name="rect"></param>
    public override void DoSettingsWindowContents(Rect rect)
    {
        var listing_Standard = new Listing_Standard();
        listing_Standard.Begin(rect);
        listing_Standard.Gap();
        listing_Standard.Label("HaDr.NeedIncreaseInterval".Translate(Settings.NeedIncreaseInterval), -1f,
            "HaDr.NeedIncreaseIntervalTT".Translate());
        listing_Standard.IntAdjuster(ref Settings.NeedIncreaseInterval, 100, 100);
        listing_Standard.Gap();

        listing_Standard.Label("HaDr.NeedIncreaseValue".Translate(), -1f, "HaDr.NeedIncreaseValueTT".Translate());
        Settings.NeedIncreaseValue = Widgets.HorizontalSlider_NewTemp(listing_Standard.GetRect(20f),
            Settings.NeedIncreaseValue, 0, 0.2f, true,
            Settings.NeedIncreaseValue.ToStringPercent(), null, null, 0.001f);

        listing_Standard.Gap();
        listing_Standard.Label("HaDr.InjuryHealingPoints".Translate(Settings.InjuryHealingPoints), -1f,
            "HaDr.InjuryHealingPointsTT".Translate());
        listing_Standard.IntAdjuster(ref Settings.InjuryHealingPoints, 1, 1);

        listing_Standard.Gap();
        listing_Standard.Label("HaDr.InjuryHealingCost".Translate(), -1f, "HaDr.InjuryHealingCostTT".Translate());
        Settings.InjuryHealingCost = Widgets.HorizontalSlider_NewTemp(listing_Standard.GetRect(20f),
            Settings.InjuryHealingCost, 0, 1f, true,
            Settings.InjuryHealingCost.ToString("N"), null, null, 0.001f);

        listing_Standard.Gap();
        listing_Standard.Label("HaDr.ThresholdToBeConsideredDamaged".Translate(), -1f,
            "HaDr.ThresholdToBeConsideredDamagedTT".Translate());
        Settings.ThresholdToBeConsideredDamaged = Widgets.HorizontalSlider_NewTemp(listing_Standard.GetRect(20f),
            Settings.ThresholdToBeConsideredDamaged, 0, 1f, true,
            Settings.ThresholdToBeConsideredDamaged.ToStringPercent(), null, null, 0.01f);

        listing_Standard.Gap();
        listing_Standard.Label("HaDr.ThresholdToBeConsideredHealed".Translate(), -1f,
            "HaDr.ThresholdToBeConsideredHealedTT".Translate());
        Settings.ThresholdToBeConsideredHealed = Widgets.HorizontalSlider_NewTemp(listing_Standard.GetRect(20f),
            Settings.ThresholdToBeConsideredHealed, 0, 1f, true,
            Settings.ThresholdToBeConsideredHealed.ToStringPercent(), null, null, 0.01f);

        listing_Standard.Gap();
        listing_Standard.Label("HaDr.DragonBloodGainFromDragonRage".Translate(), -1f,
            "HaDr.DragonBloodGainFromDragonRageTT".Translate());
        Settings.DragonBloodGainFromDragonRage = Widgets.HorizontalSlider_NewTemp(listing_Standard.GetRect(20f),
            Settings.DragonBloodGainFromDragonRage, 0, 1f, true,
            Settings.DragonBloodGainFromDragonRage.ToString("N"), null, null, 0.01f);


        listing_Standard.Gap();
        var vanillaEyesText = "HaDr.UseVanillaEyes";
        if (Current.Game != null)
        {
            vanillaEyesText = "HaDr.UseVanillaEyesGameActive";
        }

        listing_Standard.CheckboxLabeled(vanillaEyesText.Translate(), ref Settings.UseVanillaEyes);
        if (currentVersion != null)
        {
            listing_Standard.Gap();
            GUI.contentColor = Color.gray;
            listing_Standard.Label("HaDr.CurrentModVersion".Translate(currentVersion));
            GUI.contentColor = Color.white;
        }

        listing_Standard.End();
    }

    public override void WriteSettings()
    {
        base.WriteSettings();
        Startup.UpdateEyes();
    }
}