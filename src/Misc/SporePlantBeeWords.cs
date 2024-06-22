using System;
using UnityEngine;
using static WordWorld.WordUtil;

namespace WordWorld.Misc
{
    public class SporePlantBeeWords : Wordify<SporePlant.Bee>
    {
        private FLabel label;

        public override void Init(RoomCamera.SpriteLeaser sLeaser)
        {
            label = new(Font, "B") { scale = 0.5f };
            labels.Add(label);
        }

        public override void Draw(RoomCamera.SpriteLeaser sLeaser, float timeStacker, Vector2 camPos)
        {
            labels[0].SetPosition(Vector2.Lerp(Drawable.lastPos, Drawable.pos, timeStacker) - camPos);
            labels[0].color = Drawable.angry && Drawable.blinkFreq > 0f ? sLeaser.sprites[1].color : sLeaser.sprites[0].color;
        }
    }
}
