using Verse;

namespace HalfDragons;

/// <summary>
///     Definition of the settings for the mod
/// </summary>
internal class HalfDragonsSettings : ModSettings
{
    public float DragonBloodGainFromDragonRage = 0.1f;
    public float InjuryHealingCost = 0.02f;
    public int InjuryHealingPoints = 5;
    public int NeedIncreaseInterval = 600;
    public float NeedIncreaseValue = 0.005f;
    public float ThresholdToBeConsideredDamaged = 0.5f;
    public float ThresholdToBeConsideredHealed = 1f;
    public bool UseVanillaEyes;

    /// <summary>
    ///     Saving and loading the values
    /// </summary>
    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Values.Look(ref UseVanillaEyes, "UseVanillaEyes");
        Scribe_Values.Look(ref NeedIncreaseInterval, "NeedIncreaseInterval", 600);
        Scribe_Values.Look(ref NeedIncreaseValue, "NeedIncreaseValue", 0.005f);
        Scribe_Values.Look(ref InjuryHealingCost, "InjuryHealingCost", 0.02f);
        Scribe_Values.Look(ref ThresholdToBeConsideredDamaged, "ThresholdToBeConsideredDamaged", 0.5f);
        Scribe_Values.Look(ref ThresholdToBeConsideredHealed, "ThresholdToBeConsideredHealed", 1f);
        Scribe_Values.Look(ref DragonBloodGainFromDragonRage, "DragonBloodGainFromDragonRage", 0.1f);
        Scribe_Values.Look(ref InjuryHealingPoints, "InjuryHealingPoints", 5);
    }
}