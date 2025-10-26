using System.Collections.ObjectModel;
using System.Linq;
using HarmonyLib;
using UnityEngine;

namespace PEAKEmoteLib;

[HarmonyPatch(typeof(EmoteWheel), "InitWheel")]
public static class EmoteWheelInitWheelPatch
{
    [HarmonyPrefix]
    public static void Prefix(EmoteWheel __instance)
    {
        // Start with a page for the Vanilla emotes
        EmoteWheelPage vanillaPage = new EmoteWheelPage(__instance.data);
        __instance.AddPage(vanillaPage);

        // Populate new pages with all the emotes from the registry
        var emotes = EmoteRegistry.GetEmotes().Values;

        int i = 0;
        EmoteWheelPage newPage = new EmoteWheelPage();
        foreach (Emote emote in emotes)
        {
            EmoteWheelData data = ScriptableObject.CreateInstance<EmoteWheelData>();
            data.emoteName = emote.Name;
            data.anim = emote.Name;
            data.emoteSprite = emote.Icon;

            newPage.data[i] = data;

            // If the page is full, push it and start a new one
            if (i == (EmoteWheelPage.SlicesPerPage - 1))
            {
                __instance.AddPage(newPage);
                newPage = new EmoteWheelPage();
            }

            i = (i + 1) % EmoteWheelPage.SlicesPerPage;
        }

        // If the last page was incomplete, push it as-is
        if (i > 0)
            __instance.AddPage(newPage);
    }
}