using PEAKLib.UI;
using UnityEngine;

namespace PEAKEmoteLib;

public class Emote
{

    public const string CustomEmotePrefix = "PEAKEmoteLib_";

    public enum EmoteType
    {
        Vanilla,
        OneShot
    }

    public AnimationClip AnimationClip { get; private set; }
    public EmoteType Type { get; private set; }
    public bool DisableIK { get; private set; }
    public string Name { get; private set; }
    public Sprite Icon { get; private set; }

    private TranslationKey translationKey;

    public Emote(string name, AnimationClip animationClip, Sprite? icon = null, EmoteType type = EmoteType.Vanilla, bool disableIK = false)
    {
        this.Name = CustomEmotePrefix + name;
        this.AnimationClip = animationClip;
        this.Type = type;
        this.DisableIK = disableIK;
        this.Icon = icon == null ? EmoteWheelPage.PlaceholderSprite : icon;
        this.translationKey = PEAKLib.UI.MenuAPI.CreateLocalization(this.Name);
    }

    public Emote(string name, AnimationClip animationClip, Texture2D iconTexture, EmoteType type = EmoteType.Vanilla, bool disableIK = false)
        : this(
            name,
            animationClip,
            Sprite.Create(iconTexture, new Rect(0, 0, iconTexture.width, iconTexture.height), new Vector2(0.5f, 0.5f)),
            type,
            disableIK
        )
    { }

    public void AddLocalization(string text, LocalizedText.Language language)
    {
        translationKey.AddLocalization(text, language);
    }
}