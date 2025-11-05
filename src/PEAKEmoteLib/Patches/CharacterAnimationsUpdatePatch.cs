using HarmonyLib;
using UnityEngine;

namespace PEAKEmoteLib;

/// <summary>
/// A Harmony patch for CharacterAnimations.Update that handles timing the
/// ending of custom emote plays.
/// </summary>
/// 
/// <remarks>
/// By default, emotes in PEAK will simply play for a set amount of time,
/// looping the AnimationClip if needed. This is especially undesirable for
/// oneshot emotes that are either shorter (the animation will loop) or longer
/// (the animation will be cancelled before finishing) than that set limit.
/// </remarks>
[HarmonyPatch(typeof(CharacterAnimations), "Update")]
public static class CharacterAnimationsUpdatePatch
{
    /// <summary>
    /// An absolute limit, in seconds, after which emote plays will time out
    /// and stop.
    /// </summary>
    /// 
    /// <remarks>
    /// TODO: make this configurable via a setting.
    /// </remarks>
    public const float MaxEmoteTime = 10.0f;

    /// <summary>
    /// A Harmony prefix that runs before each execution of
    /// CharacterAnimations.Update and keeps track of whether the character
    /// was emoting at the start of this logic frame.
    /// </summary>
    /// 
    /// <remarks>
    /// This information needs to be tracked outside of the vanilla 
    /// CharacterAnimations.emoting field, since our goal is to prevent
    /// that value from being changed in a Postfix.
    /// </remarks>
    /// 
    /// <param name="__instance">
    /// The CharacterAnimations instance being updated.
    /// Provided automatically by Harmony.
    /// </param>
    [HarmonyPrefix]
    public static void Prefix(CharacterAnimations __instance)
    {
        __instance.SetEmoting(__instance.emoting);
    }

    /// <summary>
    /// A Harmony postfix that runs after each execution of
    /// CharacterAnimations.Update and overrides the current emote's stop
    /// if necessary.
    /// </summary>
    /// 
    /// <param name="__instance">
    /// The CharacterAnimations instance being updated.
    /// Provided automatically by Harmony.
    /// </param>
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