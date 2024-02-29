using System.Linq;
using UnityEngine;
using static WordWorld.WordUtil;

namespace WordWorld.Creatures
{
    public static class VultureGrubWords
    {
        public static FLabel[] Init(VultureGrubGraphics grubGraf, CreatureTemplate.Type type, RoomCamera.SpriteLeaser sLeaser)
        {
            var text = Unpascal(type);
            return [new FLabel(Font, text)
            {
                scale = grubGraf.worm.bodyChunks.Sum(x => x.rad) * 3f / TextWidth(text)
            }];
        }

        public static void Draw(VultureGrubGraphics grubGraf, FLabel[] labels, RoomCamera.SpriteLeaser sLeaser, float timeStacker, Vector2 camPos)
        {
            // Body
            labels[0].SetPosition(GetPos(grubGraf.worm.bodyChunks[0], timeStacker) - camPos);
            labels[0].rotation = FixRotation(AngleBtwnChunks(grubGraf.worm.bodyChunks[1], grubGraf.worm.bodyChunks[2], timeStacker)) - 90f;
            labels[0].color = sLeaser.sprites[grubGraf.MeshSprite].color;

            // Show laser sprite
            sLeaser.sprites[grubGraf.LaserSprite].isVisible = Mathf.Lerp(grubGraf.lastLaserActive, grubGraf.laserActive, timeStacker) > 0f;
        }
    }
}
