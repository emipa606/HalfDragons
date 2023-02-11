using AlienRace;
using RimWorld;
using Verse;

namespace HalfDragons;

public static class HD_Common
{
    public static ThingDef_AlienRace halfDragonRace = DefDatabase<ThingDef_AlienRace>.GetNamed("HalfDragon");
    public static float dragonRageMaxSeverity = 0.3f;
    public static NeedDef dragonBlood = DefDatabase<NeedDef>.GetNamed("HD_DragonBlood");
}