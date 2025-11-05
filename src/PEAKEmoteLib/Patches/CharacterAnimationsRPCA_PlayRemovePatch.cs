using HarmonyLib;
using UnityEngine;

namespace PEAKEmoteLib;

/// <summary>
/// A Harmony patch for CharacterAnimations.RPCA_PlayRemove that handles
/// branching logic for playing custom emote animation data.
/// </summary>
/// 
/// <remarks>
/// The CharacterAnimations.RPCA_PlayRemove function is an RPC call, meaning
/// this behavior is synced between multiplayer clients.
/// </remarks>
[HarmonyPatch(typeof(CharacterAnimations), "RPCA_PlayRemove")]
public static class CharacterAnimationsRPCA_PlayRemovePatch
{
    /// <summary>
    /// The vanilla emote name whose data is overriden by the Unity
    /// AnimatorOverrideController.
    /// </summary>
    /// 
    /// <remarks>
    /// Because Unity's animation system bakes AnimatorControllers into
    /// compiled, pre-optimized RuntimeAnimatorController state machines,
    /// it isn't possible to add brand-new animation states at runtime. What
    /// is possible, though, is overriding an existing state's animation data
    /// with a new AnimationClip then calling it.
    /// </remarks>
    public const string OverrideState = "A_Scout_Emote_Dance2";

    /// <summary>
    /// A reference to the original AnimationClip data contained in the vanilla
    /// controller, in the state named by <see cref="OverrideState"/> .
    /// </summary>
    private static AnimationClip? OriginalOverridenClip;

    /// <summary>
    /// A prefix executed before CharacterAnimations.RPCA_PlayRemove that
    /// intercepts the incoming emote play call and overrides its behavior
    /// if the emote name starts with <see cref="Emote.CustomEmotePrefix"/>
    /// </summary>
    /// 
    /// <param name="__instance">
    /// The CharacterAnimations instance attempting to play an emote.
    /// Provided automatically by Harmony.
    /// </param>
    /// 
    /// <param name="emoteName">
    /// The system-facing name of the desired emote.
    /// Corresponds to a named state in the character's AnimatorController.
    /// </param>
    [HarmonyPrefix]
    public static void Prefix(CharacterAnimations __instance, ref string emoteName)
    {
        AnimatorOverrideController overrideController = __instance.GetAnimatorOverrideController();

        // Make sure to save a reference to the original clip we're overriding so it can be used later
        if (OriginalOverridenClip == null)
        {
            OriginalOverridenClip = overrideController[OverrideState];
        }

        if (emoteName.StartsWith(Emote.CustomEmotePrefix))
        {
            Plugin.Log.LogDebug($"Attempting to play custom emote {emoteName}");

            Emote emote = EmoteRegistry.GetEmotes()[emoteName];

            __instance.SetCurrentEmote(emote);

            if (emote.DisableIK)
            {
                __instance.character.data.overrideIKForSeconds = emote.AnimationClip.length;
            }

            overrideController[OverrideState] = emote.AnimationClip;
            emoteName = OverrideState;
        }
        else if ((emoteName == OverrideState) && (OriginalOverridenClip != null))
        {
            overrideController[OverrideState] = OriginalOverridenClip;
        }

        // Follow the remaining vanilla code path for emotes
    }
}