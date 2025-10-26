using HarmonyLib;
using UnityEngine;

namespace PEAKEmoteLib;

[HarmonyPatch(typeof(CharacterAnimations), "Update")]
public static class CharacterAnimationsUpdatePatch
{
    public const float MaxEmoteTime = 10.0f;

    [HarmonyPrefix]
    public static void Prefix(CharacterAnimations __instance)
    {
        __instance.SetEmoting(__instance.emoting);
    }

    [HarmonyPostfix]
    public static void Postfix(CharacterAnimations __instance)
    {
        Emote? currentEmote = __instance.GetCurrentEmote();
        if (currentEmote != null && currentEmote.Type == Emote.EmoteType.OneShot)
        {
            if (__instance.GetEmoting() && (__instance.sinceEmoteStart > currentEmote.AnimationClip.length || (__instance.sinceEmoteStart > 0.7f && (__instance.character.input.movementInput.magnitude > 0.1f || __instance.character.input.jumpWasPressed || __instance.character.data.sinceGrounded > 0.2f))))
            {
                // Emote was playing, but is over now
                Plugin.Log.LogDebug("Stopping emote");
                __instance.character.refs.animator.SetBool("Emote", value: false);
                __instance.emoting = false;
                __instance.SetCurrentEmote(null);
                __instance.character.data.overrideIKForSeconds = 0.0f;
            }
            else if (__instance.GetEmoting())
            {
                // Emote should keep playing
                __instance.character.refs.animator.SetBool("Emote", value: true);
                __instance.emoting = true;
            }
            // Otherwise, no emote was playing and it should stay that way
        }
    }
}