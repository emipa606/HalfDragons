using System;
using System.Collections.Generic;
using HarmonyLib;
using RimWorld;
using Verse;

namespace HalfDragons;

[HarmonyPatch(typeof(Verb_MeleeAttackDamage), "DamageInfosToApply")]
internal class Patch_DamageInfosToApply
{
    [HarmonyPostfix]
    private static IEnumerable<DamageInfo> AddDragonRageDamageBuff(IEnumerable<DamageInfo> __result,
        Verb_MeleeAttackDamage __instance)
    {
        DamageInfo primaryAttack = default;
        var hasPrimaryAttack = false;
        foreach (var damage in __result)
        {
            if (damage.Def == __instance.verbProps.meleeDamageDef)
            {
                primaryAttack = damage;
                hasPrimaryAttack = true;
                continue;
            }

            yield return damage;
        }

        if (!hasPrimaryAttack)
        {
            yield break;
        }

        //Log.Message("previous amount: " + primaryAttack.Amount + " penetration " + primaryAttack.ArmorPenetrationInt);
        try
        {
            primaryAttack = TryMakeNewPrimaryAttack(__instance, primaryAttack);
        }
        catch (Exception e)
        {
            Log.Warning($"Half-Dragons: Something went wrong {e}");
        }

        //Log.Message("after amount " + primaryAttack.Amount + " penetration " + primaryAttack.ArmorPenetrationInt);
        yield return primaryAttack;
    }

    private static DamageInfo TryMakeNewPrimaryAttack(Verb_MeleeAttackDamage __instance, DamageInfo primaryAttack)
    {
        if (__instance.Caster is not Pawn caster)
        {
            return primaryAttack;
        }

        if (!caster.IsHalfDragon())
        {
            //Log.Message("Not a half dragon");
            return primaryAttack;
        }

        if (caster.equipment?.Primary?.def?.IsMeleeWeapon == false)
        {
            //Log.Message("Not a melee weapon");
            return primaryAttack;
        }

        if (!caster.HasDragonRage())
        {
            //Log.Message("Pawn does not have dragon rage");
            return primaryAttack;
        }

        var dragonRage = caster.GetDragonRageHediff();
        var dragonRageMultiplier = 1 + dragonRage.Severity;
        var newAmount = primaryAttack.Amount * dragonRageMultiplier;
        var newPenetration = primaryAttack.ArmorPenetrationInt * dragonRageMultiplier;
        primaryAttack = new DamageInfo(
            primaryAttack.Def,
            newAmount,
            newPenetration,
            primaryAttack.Angle,
            primaryAttack.Instigator,
            primaryAttack.HitPart,
            primaryAttack.Weapon,
            primaryAttack.Category,
            primaryAttack.IntendedTarget,
            primaryAttack.InstigatorGuilty,
            primaryAttack.SpawnFilth);
        return primaryAttack;
    }
}