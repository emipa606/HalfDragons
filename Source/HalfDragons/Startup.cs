using System.Linq;
using System.Reflection;
using AlienRace;
using HarmonyLib;
using Verse;

namespace HalfDragons;

[StaticConstructorOnStartup]
public static class Startup
{
    static Startup()
    {
        new Harmony("Mlie.halfdragons").PatchAll(Assembly.GetExecutingAssembly());
        UpdateEyes();
    }

    public static void UpdateEyes()
    {
        var race = DefDatabase<ThingDef_AlienRace>.AllDefs.First(race => race.defName == "HalfDragon");
        foreach (var bodyAddon in race.alienRace.generalSettings.alienPartGenerator.bodyAddons)
        {
            if (!bodyAddon.path.Contains("eyes"))
            {
                continue;
            }

            if (HalfDragonsMod.instance.Settings.UseVanillaEyes)
            {
                bodyAddon.path = bodyAddon.path.Replace("pawn/eyes/REye", "pawn/eyes/vanilla/REye")
                    .Replace("pawn/eyes/LEye", "pawn/eyes/vanilla/LEye");
            }
            else
            {
                bodyAddon.path = bodyAddon.path.Replace("pawn/eyes/vanilla/REye", "pawn/eyes/REye")
                    .Replace("pawn/eyes/vanilla/LEye", "pawn/eyes/LEye");
            }
        }
    }
}