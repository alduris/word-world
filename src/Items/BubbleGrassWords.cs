using System;
using UnityEngine;
using WordWorld.Defaults;
using static WordWorld.WordUtil;

namespace WordWorld.Items
{
    public class BubbleGrassWords() : POWordify<BubbleGrass>($"Bubble{Environment.NewLine}Weed")
    {
        public override void Draw(RoomCamera.SpriteLeaser sLeaser, float timeStacker, Vector2 camPos)
        {
            base.Draw(sLeaser, timeStacker, camPos);
            var oxygen = Mathf.Lerp(Drawable.lastOxygen, Drawable.oxygen, timeStacker);
            var end = Drawable.stalk.Length - 1;
            var length = Vector2.Lerp(Drawable.stalk[end].lastPos - Drawable.stalk[0].lastPos, Drawable.stalk[end].pos - Drawable.stalk[0].pos, timeStacker).magnitude;
            labels[0].SetPosition(GetPos(Drawable.firstChunk, timeStacker) - camPos);
            labels[0].scale = length / FontSize * Mathf.Lerp(0.5f, 0.75f, oxygen);
            if (Drawable.blink == 0) labels[0].color = Color.Lerp(Drawable.blackColor, Drawable.color, oxygen);
        }
    }
}
