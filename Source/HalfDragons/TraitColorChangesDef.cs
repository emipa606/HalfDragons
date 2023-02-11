using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;

namespace HalfDragons;

public class TraitColorChangesDef : Def
{
    public readonly List<TraitColorChange> colorChanges = null;

    public void SetHairColor(Pawn pawn)
    {
        if (!TryGetColor(pawn, out var color))
        {
            return;
        }

        if (pawn.story != null)
        {
            pawn.story.HairColor = color;
        }
    }

    public bool TryGetColor(Pawn pawn, out Color color)
    {
        color = default;
        var pawnTraits = pawn.story?.traits?.allTraits?.ConvertAll(trait => trait.def);
        if (pawnTraits.NullOrEmpty())
        {
            return false;
        }

        var colors = new List<Color>();
        if (pawnTraits != null)
        {
            foreach (var trait in pawnTraits)
            {
                if (HasOverrideFor(trait))
                {
                    colors.Add(GetOverridenColor(trait));
                }
            }
        }

        if (colors.NullOrEmpty())
        {
            return false;
        }

        color = colors.RandomElement();
        return true;
    }

    public bool HasOverrideFor(TraitDef trait)
    {
        return !colorChanges.NullOrEmpty() && colorChanges.Any(change => change.trait == trait);
    }

    public bool HasOverrideFor(Trait trait)
    {
        return HasOverrideFor(trait.def);
    }

    public Color GetOverridenColor(TraitDef trait)
    {
        return colorChanges.Find(change => change.trait == trait).color;
    }

    public override IEnumerable<string> ConfigErrors()
    {
        foreach (var error in base.ConfigErrors())
        {
            yield return error;
        }

        if (colorChanges.NullOrEmpty())
        {
            yield return "Required list \"colorChanges\" null or empty";
            yield break;
        }

        foreach (var change in colorChanges)
        {
            foreach (var error in change.ConfigErrors())
            {
                yield return error;
            }
        }
    }
}