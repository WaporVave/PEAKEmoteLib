using UnityEngine;
using System.IO;
using System.Reflection;
using UnityEngine.UI;

namespace PEAKEmoteLib;

public class EmoteWheelPage
{
    public const int SlicesPerPage = 8;

    public static Sprite PlaceholderSprite;
    static EmoteWheelPage()
    {
        Texture2D transparentTexture = new Texture2D(1, 1);
        transparentTexture.SetPixel(0, 0, new Color(0, 0, 0, 0));  // Fully transparent
        transparentTexture.Apply();

        // Create the Sprite from the texture
        Sprite invisibleSprite = Sprite.Create(transparentTexture, new Rect(0, 0, 1, 1), new Vector2(0.5f, 0.5f));
        PlaceholderSprite = invisibleSprite;
    }

    public EmoteWheelData?[] data { get; set; }

    public EmoteWheelPage()
    {
        data = new EmoteWheelData[SlicesPerPage];

        for (int i = 0; i < SlicesPerPage; i++)
        {
            data[i] = null;
        }
    }

    public EmoteWheelPage(EmoteWheelData[] data)
    {
        if (data.Length > SlicesPerPage)
            Plugin.Log.LogWarning($"{data.Length} EmoteWheelData items received, which exceeds the maximum of {SlicesPerPage} per page. Discarding extra items.");

        this.data = new EmoteWheelData[SlicesPerPage];
        for (int i = 0; i < data.Length && i < SlicesPerPage; i++)
        {
            this.data[i] = data[i];
        }

        // If the length of data received is less than a full page, explicitly fill the rest with null
        for (int i = data.Length; i < SlicesPerPage; i++)
        {
            this.data[i] = null;
        }
    }
}