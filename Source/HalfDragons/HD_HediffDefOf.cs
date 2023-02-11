using RimWorld;
using Verse;

namespace HalfDragons;

[DefOf]
public static class HD_HediffDefOf
{
    public static HediffDef HD_dragonRage;
    public static HediffDef HD_regenerativeExhaustion;

    static HD_HediffDefOf()
    {
        DefOfHelper.EnsureInitializedInCtor(typeof(HD_HediffDefOf));
    }
}