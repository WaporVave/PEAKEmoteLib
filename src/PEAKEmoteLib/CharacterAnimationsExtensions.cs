using System.Runtime.CompilerServices;
using UnityEngine;

namespace PEAKEmoteLib;

internal static class CharacterAnimationsExtensions
{
    private static readonly ConditionalWeakTable<CharacterAnimations, Holder> data = new ConditionalWeakTable<CharacterAnimations, Holder>();

    public static AnimatorOverrideController GetAnimatorOverrideController(this CharacterAnimations characterAnimations)
    {
        return data.GetOrCreateValue(characterAnimations).AnimatorOverrideController;
    }

    public static void SetAnimatorOverrideController(this CharacterAnimations characterAnimations, AnimatorOverrideController overrideController)
    {
        data.GetOrCreateValue(characterAnimations).AnimatorOverrideController = overrideController;
    }

    public static bool GetEmoting(this CharacterAnimations characterAnimations)
    {
        return data.GetOrCreateValue(characterAnimations).Emoting;
    }

    public static void SetEmoting(this CharacterAnimations characterAnimations, bool emoting)
    {
        data.GetOrCreateValue(characterAnimations).Emoting = emoting;
    }

    public static Emote? GetCurrentEmote(this CharacterAnimations characterAnimations)
    {
        return data.GetOrCreateValue(characterAnimations).CurrentEmote;
    }

    public static void SetCurrentEmote(this CharacterAnimations characterAnimations, Emote? emote)
    {
        data.GetOrCreateValue(characterAnimations).CurrentEmote = emote;
    }

    private class Holder
    {
        public AnimatorOverrideController AnimatorOverrideController = new AnimatorOverrideController();
        public bool Emoting = false;
        public Emote? CurrentEmote = null;
    }
}