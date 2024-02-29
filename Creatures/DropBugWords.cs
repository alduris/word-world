using UnityEngine;
using static WordWorld.WordUtil;

namespace WordWorld.Creatures
{
    public static class DropBugWords
    {
        public static FLabel[] Init(DropBugGraphics dropBugGraf, CreatureTemplate.Type type)
        {
            return [
                new FLabel(Font, type == CreatureTemplate.Type.DropBug ? "Dropwig" : Unpascal(type))
                {
                    scale = dropBugGraf.bug.mainBodyChunk.rad * 3f / FontSize
                }
            ];
        }

        public static void Draw(DropBugGraphics dropBugGraf, FLabel[] labels, RoomCamera.SpriteLeaser sLeaser, float timeStacker, Vector2 camPos)
        {
            labels[0].SetPosition(GetPos(dropBugGraf.bug.bodyChunks[1], timeStacker) - camPos);
            labels[0].rotation = FixRotation(sLeaser.sprites[dropBugGraf.HeadSprite].rotation) + 90f;
            labels[0].color = dropBugGraf.currSkinColor;
        }
    }
}
