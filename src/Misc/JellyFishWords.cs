using System;
using UnityEngine;
using static WordWorld.WordUtil;

namespace WordWorld.Misc
{
    public class JellyFishWords : Wordify<JellyFish>
    {
        private FLabel label;

        public override void Init(RoomCamera.SpriteLeaser sLeaser)
        {
            label = new(Font, $"Jelly{Environment.NewLine}fish") { scale = Drawable.bodyChunks[0].rad * 1.5f / FontSize };
            labels.Add(label);
        }

        public override void Draw(RoomCamera.SpriteLeaser sLeaser, float timeStacker, Vector2 camPos)
        {
            label.SetPosition(GetPos(Drawable.bodyChunks[0], timeStacker) - camPos);
            label.rotation = AngleFrom(Drawable.rotation);
            label.color = sLeaser.sprites[Drawable.BodySprite(1)].color;
        }
    }
}
