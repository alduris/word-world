using System;
using UnityEngine;
using WordWorld.Defaults;
using static WordWorld.WordUtil;

namespace WordWorld.Items
{
    public class SporePlantWords() : POWordify<SporePlant>($"Bee{Environment.NewLine}hive")
    {
        public override void Draw(RoomCamera.SpriteLeaser sLeaser, float timeStacker, Vector2 camPos)
        {
            base.Draw(sLeaser, timeStacker, camPos);
            if (Drawable.blink <= 1) Label.color = Color.Lerp(Drawable.colorA, Drawable.colorB, Drawable.Pacified ? 0f : Mathf.Lerp(0.4f, 0.8f, Drawable.angry));
        }
    }
}
