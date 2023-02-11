using System;
using HarmonyLib;
using RimWorld;
using Verse;

namespace HalfDragons;

[HarmonyPatch(typeof(Verb_MeleeAttackDamage), "ApplyMeleeDamageToTarget")]
internal class Patch_ApplyMeleeDamageToTarget
{
    [HarmonyPostfix]
    private static void AddDragonRageHediff(Verb_MeleeAttackDamage __instance)
    {
        try
        {
            if (__instance.Caster is not Pawn caster)
            {
                return;
            }

            if (!caster.IsHalfDragon())
            {
                //Log.Message("Not a half dragon");
                return;
            }

            if (caster.equipment?.Primary?.def?.IsMeleeWeapon != true)
            {
                //Log.Message("Not a melee weapon");
                return;
            }

            caster.IncreaseDragonRage();
        }
        catch (Exception e)
        {
            Log.Warning($"Half-Dragons: Something went wrong {e}");
        }
    }
}