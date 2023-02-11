using Verse;

namespace HalfDragons;

public static class InjuryUtilities
{
    public static bool IsScar(this Hediff hediff)
    {
        if (hediff is Hediff_Injury injury)
        {
            return injury.IsPermanent() &&
                   injury.TryGetComp<HediffComp_GetsPermanent>() != null;
        }

        return false;
    }

    public static bool IsTendable(this Hediff hediff)
    {
        return hediff.def.tendable;
    }
}