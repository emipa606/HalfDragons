using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

namespace HalfDragons;

public class Need_DragonBlood : Need
{
    private readonly List<BodyPartRecord> bodyPartsToHeal = new List<BodyPartRecord>();
    private int age;

    public Need_DragonBlood()
    {
    }

    public Need_DragonBlood(Pawn pawn)
    {
        this.pawn = pawn;
    }

    private int AgeInTicks => age * 150;

    /*protected override bool IsFrozen
    {
        get
        {
            return base.IsFrozen || pawn.health.hediffSet.HasHediff(HD_HediffDefOf.HD_regenerativeExhaustion);
        }
    }*/

    public override void NeedInterval()
    {
        if (IsFrozen)
        {
            //Log.Message("dragonblood need frozen");
            return;
        }

        age++;
        //Log.Message("needincrease interval: " + SettingsAccess.needIncreaseInterval);
        if (AgeInTicks % HalfDragonsMod.instance.Settings.NeedIncreaseInterval == 0)
        {
            CurLevel += HalfDragonsMod.instance.Settings.NeedIncreaseValue;
        }

        DetermineBodyPartsToHeal(); // add new parts below healing threshold
        RemoveHealedParts(); // remove old parts that have been healed above healing threshold
        if (bodyPartsToHeal.Count > 0)
        {
            DoDragonBloodHealing();
        }

        if (CurLevel != 0)
        {
            return;
        }

        var regenExhaustDef = HD_HediffDefOf.HD_regenerativeExhaustion;
        if (pawn.health.hediffSet.HasHediff(regenExhaustDef))
        {
            return;
        }

        var regenExhaustion = HediffMaker.MakeHediff(regenExhaustDef, pawn);
        regenExhaustion.PostMake();
        pawn.health.AddHediff(regenExhaustion);
    }

    private void DoDragonBloodHealing()
    {
        //Log.Message("Body parts to heal: " + string.Join(", ", bodyPartsToHeal.ConvertAll(part => part.def.defName)));
        var partToHeal = bodyPartsToHeal.RandomElement();
        var injuries =
            pawn.health.hediffSet.hediffs.Where(hediff => hediff is Hediff_Injury && hediff.Part == partToHeal);
        var injuryToHeal = injuries.RandomElement();
        injuryToHeal.Heal(HalfDragonsMod.instance.Settings.InjuryHealingPoints);
        CurLevel -= HalfDragonsMod.instance.Settings.InjuryHealingCost;
    }

    private void RemoveHealedParts()
    {
        foreach (var part in bodyPartsToHeal.ToList())
        {
            var thresholdToBeConsideredHealed =
                part.def.GetMaxHealth(pawn) * HalfDragonsMod.instance.Settings.ThresholdToBeConsideredHealed;
            if (pawn.health.hediffSet.GetPartHealth(part) == thresholdToBeConsideredHealed)
            {
                bodyPartsToHeal.Remove(part);
            }
        }
    }

    private void DetermineBodyPartsToHeal()
    {
        var hediffs = pawn.health?.hediffSet;
        IEnumerable<BodyPartRecord> damagedBodyParts = hediffs?.GetInjuredParts();
        if (damagedBodyParts.EnumerableNullOrEmpty())
        {
            return;
        }

        //Log.Message("damaged body parts: " + string.Join(", ", damagedBodyParts.Select(part => part.def.defName)));
        if (damagedBodyParts == null)
        {
            return;
        }

        damagedBodyParts = damagedBodyParts.Where(
            // filter to all parts, that are at 50% or lower
            part => hediffs.GetPartHealth(part) <=
                    part.def.GetMaxHealth(pawn) * HalfDragonsMod.instance.Settings.ThresholdToBeConsideredDamaged
        );

        //Log.Message("damaged body parts below 50%: " + string.Join(", ", damagedBodyParts.Select(part => part.def.defName)));
        foreach (var part in damagedBodyParts)
        {
            if (!bodyPartsToHeal.Contains(part))
            {
                bodyPartsToHeal.Add(part);
            }
        }
    }
}