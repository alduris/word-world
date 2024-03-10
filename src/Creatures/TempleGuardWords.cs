using UnityEngine;
using static WordWorld.WordUtil;

namespace WordWorld.Creatures
{
    public static class TempleGuardWords
    {
        public static FLabel[] Init(TempleGuardGraphics guardGraf, CreatureTemplate.Type type, RoomCamera.SpriteLeaser sLeaser)
        {
            // TODO: robes?
            var text = Unpascal(type);
            return [new FLabel(Font, text)
            {
                scale = sLeaser.sprites[guardGraf.HeadSprite].element.sourcePixelSize.x / TextWidth(text)
            }];
        }

        public static void Draw(TempleGuardGraphics guardGraf, FLabel[] labels, RoomCamera.SpriteLeaser sLeaser, float timeStacker, Vector2 camPos)
        {
            // Head, color = eye color
            labels[0].SetPosition(sLeaser.sprites[guardGraf.HeadSprite].GetPosition());
            labels[0].rotation = sLeaser.sprites[guardGraf.HeadSprite].rotation - 180f;
            labels[0].color = sLeaser.sprites[guardGraf.EyeSprite(1)].color;

            // Re-enable halo
            for (int i = guardGraf.FirstHaloSprite; i < guardGraf.FirstHaloSprite + guardGraf.halo.totalSprites; i++)
            {
                sLeaser.sprites[i].isVisible = true;
            }
        }
    }
}
