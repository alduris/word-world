using System;
using UnityEngine;
using static WordWorld.WordUtil;

namespace WordWorld.Misc
{
    public static class JellyFishWords
    {
        public static FLabel[] Init(JellyFish jelly)
        {
            return [new(Font, $"Jelly{Environment.NewLine}fish") { scale = jelly.bodyChunks[0].rad * 1.5f / FontSize }];
        }

        public static void Draw(JellyFish jelly, FLabel[] labels, RoomCamera.SpriteLeaser sLeaser, float timeStacker, Vector2 camPos)
        {
            labels[0].SetPosition(GetPos(jelly.bodyChunks[0], timeStacker) - camPos);
            labels[0].rotation = AngleFrom(jelly.rotation);
            labels[0].color = sLeaser.sprites[jelly.BodySprite(1)].color;
        }
    }
}
