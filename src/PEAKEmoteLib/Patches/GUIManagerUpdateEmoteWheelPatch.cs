using HarmonyLib;
using UnityEngine;

namespace PEAKEmoteLib;

/// <summary>
/// A Harmony patch for GUIManager.UpdateEmoteWheel that enables scrolling
/// through emote wheel tabs using the mouse scroll wheel
/// </summary>
[HarmonyPatch(typeof(GUIManager), "UpdateEmoteWheel")]
public static class GUIManagerUpdateEmoteWheelPatch
{
    /// <summary>
    /// A postfix executed after GUIManager.UpdateEmoteWheel that
    /// checks for mouse scroll wheel input and tabs the emote wheel
    /// accordingly; mouse wheel up scrolls back, and mouse wheel down scrolls
    /// forward.
    /// </summary>
    /// 
    /// <remarks>
    /// This logic runs once every GUIManager logic update frame while the
    /// emote wheel is open.
    /// </remarks>
    /// 
    /// <param name="__instance">
    /// The instance of GUIManager whose UpdateEmoteWheel method was called.
    /// Provided automatically by Harmony.
    /// </param>
    [HarmonyPostfix]
    public static void Postfix(GUIManager __instance)
    {
        if (__instance.emoteWheel.activeSelf)
        {
            if (Input.mouseScrollDelta[1] < 0)
            {
                EmoteWheel emoteWheel = __instance.emoteWheel.GetComponent<EmoteWheel>();
                emoteWheel.TabNext();
            }
            else if (Input.mouseScrollDelta[1] > 0)
            {
                EmoteWheel emoteWheel = __instance.emoteWheel.GetComponent<EmoteWheel>();
                emoteWheel.TabPrev();
            }
        }
    }
}