using HarmonyLib;
using RimWorld;
using Verse;

namespace HalfDragons;

[HarmonyPatch(typeof(Pawn_NeedsTracker), "ShouldHaveNeed")]
internal class Patch_ShouldHaveNeed
{
    [HarmonyPostfix]
    private static void RestrictDragonBloodNeedToHalfDragons(ref bool __result, ref NeedDef nd, Pawn ___pawn)
    {
        if (!__result)
        {
            return;
        }

        if (nd != HD_Common.dragonBlood)
        {
            return;
        }


        __result = ___pawn.IsHalfDragon();
    }
}