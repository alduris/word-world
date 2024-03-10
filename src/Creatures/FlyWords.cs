using UnityEngine;
using static WordWorld.WordUtil;

namespace WordWorld.Creatures
{
    public static class FlyWords
    {
        public static FLabel[] Init(FlyGraphics flyGraf, CreatureTemplate.Type type, RoomCamera.SpriteLeaser spriteLeaser)
        {
            return [new FLabel(Font, Unpascal(type))
            {
                scale = flyGraf.lowerBody.rad * 4f / FontSize,
                color = spriteLeaser.sprites[0].color
            }];
        }

        public static void Draw(FlyGraphics flyGraf, FLabel[] labels, RoomCamera.SpriteLeaser sLeaser, float timeStacker, Vector2 camPos)
        {
            labels[0].SetPosition(GetPos(flyGraf.lowerBody, timeStacker) - camPos);
            labels[0].rotation = sLeaser.sprites[0].rotation;
        }
    }
}
