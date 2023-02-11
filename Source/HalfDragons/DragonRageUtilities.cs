using Verse;

namespace HalfDragons;

public static class DragonRageUtilities
{
    public static Hediff GetDragonRageHediff(this Pawn pawn)
    {
        var hediffs = pawn?.health?.hediffSet?.hediffs;
        return hediffs.NullOrEmpty() ? null : hediffs?.Find(hediff => hediff.def == HD_HediffDefOf.HD_dragonRage);
    }

    public static bool IsHalfDragon(this Pawn pawn)
    {
        return pawn.def == HD_Common.halfDragonRace;
    }

    public static bool HasDragonRage(this Pawn pawn)
    {
        return pawn.GetDragonRageHediff() != null;
    }

    public static void AddDragonRage(this Pawn pawn)
    {
        var dragonRage = HediffMaker.MakeHediff(HD_HediffDefOf.HD_dragonRage, pawn);
        dragonRage.PostMake();
        pawn.health.AddHediff(dragonRage);
    }

    public static void IncreaseDragonRage(this Pawn pawn)
    {
        //Log.Message("Increasing dragon rage");
        var dragonRage = pawn.GetDragonRageHediff();
        if (dragonRage?.Severity >= HD_Common.dragonRageMaxSeverity)
        {
            //Log.Message("Resetting dragon rage");
            pawn.health.RemoveHediff(dragonRage);
            var dragonBloodGainFromDragonRage = HalfDragonsMod.instance.Settings.DragonBloodGainFromDragonRage;
            pawn.needs.TryGetNeed<Need_DragonBlood>().CurLevel += dragonBloodGainFromDragonRage;
        }

        if (dragonRage == null)
        {
            //Log.Message("Adding dragon rage");
            pawn.AddDragonRage();
            dragonRage = pawn.GetDragonRageHediff();
        }

        dragonRage.Severity += 0.1f;
        //Log.Message("Increased dragon rage for " + pawn.LabelShort + ", new severity: " + dragonRage.Severity);
    }
}