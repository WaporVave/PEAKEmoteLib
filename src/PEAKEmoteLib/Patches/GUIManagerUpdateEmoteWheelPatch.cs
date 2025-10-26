using HarmonyLib;
using UnityEngine;

namespace PEAKEmoteLib;

[HarmonyPatch(typeof(GUIManager), "UpdateEmoteWheel")]
public static class GUIManagerUpdateEmoteWheelPatch
{
    [HarmonyPostfix]
    public static void Postfix(GUIManager __instance)
    {
        if (__instance.emoteWheel.activeSelf)
        {
            if (Input.mouseScrollDelta[1] < 0)
            {
                EmoteWheel emoteWheel = __instance.emoteWheel.GetComponent<EmoteWheel>();
                emoteWheel.NextPage();
                UpdateWheelSlices(emoteWheel);
            }
            else if (Input.mouseScrollDelta[1] > 0)
            {
                EmoteWheel emoteWheel = __instance.emoteWheel.GetComponent<EmoteWheel>();
                emoteWheel.PreviousPage();
                UpdateWheelSlices(emoteWheel);
            }
        }
    }

    private static void UpdateWheelSlices(EmoteWheel emoteWheel)
    {
        EmoteWheelPage? currentPage = emoteWheel.GetCurrentPage();
        if (currentPage == null)
        {
            Plugin.Log.LogWarning("Tried to update wheel slices for new page, but current page is null");
            return;
        }

        for (int i = 0; i < EmoteWheelPage.SlicesPerPage; i++)
        {
            emoteWheel.data[i] = currentPage.data[i];
            emoteWheel.slices[i].Init(emoteWheel.data[i], emoteWheel);
        }
    }
}