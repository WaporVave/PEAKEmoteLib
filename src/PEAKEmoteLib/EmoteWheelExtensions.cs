using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace PEAKEmoteLib;

internal static class EmoteWheelExtensions
{
    private static readonly ConditionalWeakTable<EmoteWheel, Holder> data = new ConditionalWeakTable<EmoteWheel, Holder>();

    public static EmoteWheelPage? GetCurrentPage(this EmoteWheel emoteWheel)
    {
        LinkedListNode<EmoteWheelPage>? currentPageNode = data.GetOrCreateValue(emoteWheel).CurrentPage;
        if (currentPageNode == null)
        {
            return null;
        }
        else
        {
            return currentPageNode.Value;
        }
    }

    public static void AddPage(this EmoteWheel emoteWheel, EmoteWheelPage newPage)
    {
        LinkedListNode<EmoteWheelPage> newNode = data.GetOrCreateValue(emoteWheel).Pages.AddLast(newPage);

        // If this is the first page added, select it as the current page
        if (data.GetOrCreateValue(emoteWheel).CurrentPage == null)
        {
            data.GetOrCreateValue(emoteWheel).CurrentPage = newNode;
        }
    }

    public static void NextPage(this EmoteWheel emoteWheel)
    {
        LinkedListNode<EmoteWheelPage>? currentPageNode = data.GetOrCreateValue(emoteWheel).CurrentPage;
        if (currentPageNode == null)
        {
            Plugin.Log.LogWarning("Next page called, but no page is selected yet.");
            return;
        }

        if (currentPageNode.Next != null)
        {
            data.GetOrCreateValue(emoteWheel).CurrentPage = currentPageNode.Next;
        }
        else
        {
            Plugin.Log.LogDebug("Next page requested, but no more pages are available. Wrapping to top.");
            data.GetOrCreateValue(emoteWheel).CurrentPage = data.GetOrCreateValue(emoteWheel).Pages.First;
        }
    }

    public static void PreviousPage(this EmoteWheel emoteWheel)
    {
        LinkedListNode<EmoteWheelPage>? currentPageNode = data.GetOrCreateValue(emoteWheel).CurrentPage;
        if (currentPageNode == null)
        {
            Plugin.Log.LogWarning("Previous page called, but no page is selected yet.");
            return;
        }

        if (currentPageNode.Previous != null)
        {
            data.GetOrCreateValue(emoteWheel).CurrentPage = currentPageNode.Previous;
        }
        else
        {
            Plugin.Log.LogDebug("Previous page requested, but no more pages are available. Wrapping to bottom.");
            data.GetOrCreateValue(emoteWheel).CurrentPage = data.GetOrCreateValue(emoteWheel).Pages.Last;
        }
    }

    private class Holder
    {
        public LinkedList<EmoteWheelPage> Pages = new LinkedList<EmoteWheelPage>();
        public LinkedListNode<EmoteWheelPage>? CurrentPage;
    }
}