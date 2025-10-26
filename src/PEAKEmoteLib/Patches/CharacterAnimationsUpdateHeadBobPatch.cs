using HarmonyLib;
using UnityEngine;

namespace PEAKEmoteLib;

[HarmonyPatch(typeof(CharacterAnimations), "UpdateHeadBob")]
public static class CharacterAnimationsUpdateHeadBobPatch
{
    [HarmonyPostfix]
    public static void Postfix(CharacterAnimations __instance)
    {
        Plugin.Log.LogDebug("Applying AnimatorOverrideController to character Animator");
        AnimatorOverrideController overrideController = new AnimatorOverrideController(__instance.character.refs.animator.runtimeAnimatorController);
        __instance.SetAnimatorOverrideController(overrideController);
        __instance.character.refs.animator.runtimeAnimatorController = overrideController;
    }
}