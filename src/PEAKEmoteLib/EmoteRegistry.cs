using System.Collections.Generic;
using UnityEngine;

namespace PEAKEmoteLib;

internal static class EmoteRegistry
{

    private static Dictionary<string, Emote> _emotes = new Dictionary<string, Emote>();

    public static Emote RegisterEmote(string name, AnimationClip clip, Sprite? icon = null, Emote.EmoteType type = Emote.EmoteType.Vanilla, bool disableIK = false)
    {
        return RegisterEmote(new Emote(name, clip, icon, type, disableIK));
    }

    public static Emote RegisterEmote(string name, AnimationClip clip, Texture2D iconTexture, Emote.EmoteType type = Emote.EmoteType.Vanilla, bool disableIK = false)
    {
        return RegisterEmote(new Emote(name, clip, iconTexture, type, disableIK));
    }

    public static Emote RegisterEmote(Emote emote)
    {
        if (_emotes.ContainsKey(emote.Name))
        {
            Plugin.Log.LogWarning($"Ignoring duplicate emote '{emote.Name}'. Custom emote names must be globally unique.");
            return emote;
        }

        _emotes.Add(emote.Name, emote);

        Plugin.Log.LogInfo($"Registered new emote: '{emote.Name}'");
        return emote;
    }

    public static IEnumerable<Emote> RegisterEmotes(IEnumerable<Emote> emotes)
    {
        foreach (Emote emote in emotes)
        {
            RegisterEmote(emote);
        }
        return emotes;
    }

    public static IReadOnlyDictionary<string, Emote> GetEmotes()
    {
        return _emotes;
    }
}