using System;
using System.Linq;
using HarmonyLib;
using UnityEngine;

namespace PEAKEmoteLib;

/// <summary>
/// A Harmony patch for EmoteWheel.Start that creates new pages as necessary
/// to expose registered custom emotes.
/// </summary>
[HarmonyPatch(typeof(EmoteWheel), "Start")]
public static class EmoteWheelStartPatch
{
    public const int SlicesPerPage = 8;

    /// <summary>
    /// A Harmony postfix that runs after each execution of EmoteWheel.Start, 
    /// discovering emotes from the <see cref="EmoteRegistry"/> and adding 
    /// pages to the vanilla emote wheel as necessary to show each 
    /// <see cref="Emote"/> instance.
    /// </summary>
    /// 
    /// <param name="__instance">
    /// The EmoteWheel instance whose Start method is being invoked.
    /// Provided automatically by Harmony.
    /// </param>
    [HarmonyPostfix]
    public static void Postfix(EmoteWheel __instance)
    {
        int vanillaPages = __instance.pages;

        Emote[] customEmotes = EmoteRegistry.GetEmotes().Values.ToArray<Emote>();
        int customPages = (customEmotes.Length + SlicesPerPage - 1) / SlicesPerPage;
        Plugin.Log.LogDebug($"Discovered {vanillaPages} vanilla emote pages, {customPages} custom emote pages");

        __instance.pages = vanillaPages + customPages;
        Plugin.Log.LogDebug($"Emote wheel page count overridden to {__instance.pages}");

        Array.Resize(ref __instance.data, __instance.pages * SlicesPerPage);
        Plugin.Log.LogDebug($"Resized emote wheel data array to size {__instance.pages * SlicesPerPage}");

        for (int i = vanillaPages * SlicesPerPage; i < __instance.data.Length; i++)
        {
            if (i >= (customEmotes.Length + (vanillaPages * SlicesPerPage)))
            {
                __instance.data[i] = null;
            }
            else
            {
                Emote emote = customEmotes[i - (vanillaPages * SlicesPerPage)];
                EmoteWheelData data = ScriptableObject.CreateInstance<EmoteWheelData>();
                data.emoteName = emote.Name;
                data.anim = emote.Name;
                data.emoteSprite = emote.Icon;

                __instance.data[i] = data;
            }
        }
    }
}