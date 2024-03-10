using UnityEngine;
using static WordWorld.WordUtil;

namespace WordWorld.Creatures
{
    public static class HazerWords
    {
        public static FLabel[] Init(HazerGraphics hazerGraf, CreatureTemplate.Type type)
        {
            return [new FLabel(Font, Unpascal(type)) {
                scale = hazerGraf.bug.mainBodyChunk.rad * 6f / FontSize
            }];
        }

        public static void Draw(HazerGraphics hazerGraf, FLabel[] labels, RoomCamera.SpriteLeaser sLeaser, float timeStacker, Vector2 camPos)
        {
            FSprite bodySprite = sLeaser.sprites[hazerGraf.BodySprite];
            labels[0].SetPosition(GetPos(hazerGraf.bug.firstChunk, timeStacker) - camPos);
            labels[0].rotation = FixRotation(bodySprite.rotation) - 90f;
            labels[0].color = bodySprite.color;
        }
    }
}
