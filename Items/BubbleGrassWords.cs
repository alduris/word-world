using System;
using UnityEngine;
using WordWorld.Defaults;
using static WordWorld.WordUtil;

namespace WordWorld.Items
{
    public static class BubbleGrassWords
    {
        public static FLabel[] Init(BubbleGrass bubbleWeed) => POWords.Init(bubbleWeed, $"Bubble{Environment.NewLine}Weed");

        public static void Draw(BubbleGrass bubbleWeed, FLabel[] labels, float timeStacker, Vector2 camPos)
        {
            var oxygen = Mathf.Lerp(bubbleWeed.lastOxygen, bubbleWeed.oxygen, timeStacker);
            var end = bubbleWeed.stalk.Length - 1;
            labels[0].SetPosition(GetPos(bubbleWeed.firstChunk, timeStacker) - camPos);
            labels[0].color = Color.Lerp(bubbleWeed.blackColor, bubbleWeed.color, oxygen);
            labels[0].scale = Vector2.Lerp(bubbleWeed.stalk[end].lastPos - bubbleWeed.stalk[0].lastPos, bubbleWeed.stalk[end].pos - bubbleWeed.stalk[0].pos, timeStacker).magnitude
                / FontSize * Mathf.Lerp(0.5f, 0.75f, oxygen);
        }
    }
}
