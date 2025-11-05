using PEAKLib.UI;
using UnityEngine;

namespace PEAKEmoteLib;

/// <summary>
/// A class representing a custom emote and the data necessary to list it on
/// the game's emote wheel and play its animation.
/// </summary>
public class Emote
{

    /// <summary>
    /// A unique prefix for an emote's name, identifying that it's custom and 
    /// needs special handling, for example in <see cref="CharacterAnimationsRPCA_PlayRemovePatch.Prefix"/> .
    /// </summary>
    public const string CustomEmotePrefix = "PEAKEmoteLib_";

    /// <summary>
    /// Represents whether this emote should be a Vanilla style emote (loops 
    /// for a set amount of time) or OneShot style emote (plays its animation 
    /// once completely then finishes).
    /// </summary>
    public enum EmoteType
    {
        Vanilla,
        OneShot
    }

    public AnimationClip AnimationClip { get; private set; }
    public EmoteType Type { get; private set; }
    public bool DisableIK { get; private set; }

    /// <summary>
    /// The system-facing name used to reference this emote; should always start 
    /// with <see cref="CustomEmotePrefix"/>. 
    /// </summary>
    /// 
    /// <remarks>
    /// This is not the human-readable name displayed on the in-game emote 
    /// wheel. That name should be set via <see cref="Emote.AddLocalization"/> .
    /// </remarks>
    public string Name { get; private set; }

    public Sprite Icon { get; private set; }

    /// <summary>
    /// An empty (transparent) sprite used as a placeholder 
    /// <see cref="Emote.Icon"/> for Emote objects created without a Sprite 
    /// or Texture2D specified. 
    /// </summary>
    public static Sprite PlaceholderSprite;
    static Emote()
    {
        Texture2D transparentTexture = new Texture2D(1, 1);
        transparentTexture.SetPixel(0, 0, new Color(0, 0, 0, 0));  // Fully transparent
        transparentTexture.Apply();

        // Create the Sprite from the texture
        Sprite invisibleSprite = Sprite.Create(transparentTexture, new Rect(0, 0, 1, 1), new Vector2(0.5f, 0.5f));
        PlaceholderSprite = invisibleSprite;
    }

    private TranslationKey translationKey;

    /// <summary>
    /// Constructs a new Emote object.
    /// </summary>
    /// 
    /// <param name="name">
    /// A globally unique, system-facing name. See <see cref="Emote.Name"/> for more details. 
    /// </param>
    /// <param name="animationClip">
    /// A Unity AnimationClip object containing the animation data for this emote.
    /// </param>
    /// <param name="icon">
    /// An optional Unity Sprite object used to represent this emote on the in-game emote wheel. 
    /// If <see cref="null"/> is passed, the Emote's <see cref="Emote.Icon"/> is set to 
    /// <see cref="Emote.PlaceholderSprite"/>.   
    /// </param>
    /// <param name="type">
    /// Either <see cref="Emote.Type.Vanilla"/> or <see cref="Emote.Type.OneShot"/>. 
    /// See <see cref="Emote.Type"/> for more details.  
    /// </param>
    /// <param name="disableIK">
    /// Whether or not PEAKEmoteLib should attempt to override the character's IK 
    /// (Inverse Kinematics) rig while this emote is playing.
    /// </param>
    public Emote(string name, AnimationClip animationClip, Sprite? icon = null, EmoteType type = EmoteType.Vanilla, bool disableIK = false)
    {
        this.Name = CustomEmotePrefix + name;
        this.AnimationClip = animationClip;
        this.Type = type;
        this.DisableIK = disableIK;
        this.Icon = icon == null ? PlaceholderSprite : icon;
        this.translationKey = PEAKLib.UI.MenuAPI.CreateLocalization(this.Name);
    }

    /// <summary>
    /// Constructs a new Emote object.
    /// </summary>
    /// 
    /// <param name="name">
    /// A globally unique, system-facing name. See <see cref="Emote.Name"/> for more details. 
    /// </param>
    /// <param name="animationClip">
    /// A Unity AnimationClip object containing the animation data for this emote.
    /// </param>
    /// <param name="iconTexture">
    /// A Unity Texture2D object used to represent this emote on the in-game emote wheel.
    /// </param>
    /// <param name="type">
    /// Either <see cref="Emote.Type.Vanilla"/> or <see cref="Emote.Type.OneShot"/>. 
    /// See <see cref="Emote.Type"/> for more details.  
    /// </param>
    /// <param name="disableIK">
    /// Whether or not PEAKEmoteLib should attempt to override the character's IK 
    /// (Inverse Kinematics) rig while this emote is playing.
    /// </param>
    public Emote(string name, AnimationClip animationClip, Texture2D iconTexture, EmoteType type = EmoteType.Vanilla, bool disableIK = false)
        : this(
            name,
            animationClip,
            Sprite.Create(iconTexture, new Rect(0, 0, iconTexture.width, iconTexture.height), new Vector2(0.5f, 0.5f)),
            type,
            disableIK
        )
    { }

    /// <summary>
    /// Adds a localization for this Emote's display name on the in-game emote 
    /// wheel.
    /// </summary>
    /// <param name="text">The localized text to display in-game.</param>
    /// <param name="language">The language of the given text.</param>
    public void AddLocalization(string text, LocalizedText.Language language)
    {
        translationKey.AddLocalization(text, language);
    }
}