using System.Collections.Generic;
using BepInEx;
using BepInEx.Logging;
using UnityEngine;
using HarmonyLib;
using PEAKLib.UI;

namespace PEAKEmoteLib;

[BepInDependency(PEAKLib.UI.UIPlugin.Id)]
[BepInAutoPlugin]
public partial class Plugin : BaseUnityPlugin
{
    internal static ManualLogSource Log { get; private set; } = null!;

    private Harmony _harmony = new Harmony("PEAKEmoteLib");

    private void Awake()
    {
        Log = Logger;

        _harmony.PatchAll();

        if (LocalizedText.mainTable != null)
        {
            Log.LogInfo("Touching LocalizedText.mainTable to ensure it's initialized");
        }

        Log.LogInfo($"Plugin {Name} is loaded!");
    }

    private void OnDestroy()
    {
        _harmony.UnpatchSelf();
        Log.LogInfo($"Plugin {Name} unloaded.");
    }
}

public static class BaseUnityPluginExtensions
{
    public static Emote RegisterEmote(this BaseUnityPlugin plugin, Emote emote)
    {
        return EmoteRegistry.RegisterEmote(emote);
    }

    public static Emote RegisterEmote(this BaseUnityPlugin plugin, string name, AnimationClip clip)
    {
        return EmoteRegistry.RegisterEmote(name, clip);
    }

    public static Emote RegisterEmote(this BaseUnityPlugin plugin, string name, AnimationClip clip, Sprite? icon = null, Emote.EmoteType type = Emote.EmoteType.Vanilla, bool disableIK = false)
    {
        return EmoteRegistry.RegisterEmote(name, clip, icon, type, disableIK);
    }

    public static Emote RegisterEmote(this BaseUnityPlugin plugin, string name, AnimationClip clip, Texture2D iconTexture, Emote.EmoteType type = Emote.EmoteType.Vanilla, bool disableIK = false)
    {
        return EmoteRegistry.RegisterEmote(name, clip, iconTexture, type, disableIK);
    }

    public static IEnumerable<Emote> RegisterEmotes(this BaseUnityPlugin plugin, IEnumerable<Emote> emotes)
    {
        return EmoteRegistry.RegisterEmotes(emotes);
    }
}