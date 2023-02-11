using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace HalfDragons;

public class TraitColorChange
{
    public Color color;
    public TraitDef trait;

    public IEnumerable<string> ConfigErrors()
    {
        if (trait == default(TraitDef))
        {
            yield return "Required field \"trait\" not set";
        }
    }
}