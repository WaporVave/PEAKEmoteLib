using HarmonyLib;
using UnityEngine;

namespace PEAKEmoteLib;

[HarmonyPatch(typeof(CharacterAnimations), "RPCA_PlayRemove")]
public static class CharacterAnimationsRPCA_PlayRemovePatch
{
    [HarmonyPrefix]
    public static void Prefix(CharacterAnimations __instance, ref string emoteName)
    {
        // If the emote is a custom one, write it to the unused "A_Scout_Emote_Dance2" Animator state
        if (emoteName.StartsWith(Emote.CustomEmotePrefix))
        {
            Plugin.Log.LogDebug($"Attempting to play custom emote {emoteName}");

            AnimatorOverrideController overrideController = __instance.GetAnimatorOverrideController();

            Emote emote = EmoteRegistry.GetEmotes()[emoteName];

            __instance.SetCurrentEmote(emote);

            if (emote.DisableIK)
            {
                __instance.character.data.overrideIKForSeconds = emote.AnimationClip.length;
            }

            overrideController["A_Scout_Emote_Dance2"] = emote.AnimationClip;
            emoteName = "A_Scout_Emote_Dance2";
        }

        // Follow vanilla code path for emotes
    }
}