using UnityEngine;
using static WordWorld.WordUtil;

namespace WordWorld.Items
{
    public static class VultureMaskWords
    {
        public static FLabel[] Init(VultureMask mask)
        {
            return [
                new FLabel(Font, "Mask")
                {
                    scale = 17.5f / FontSize * (mask.King ? 1.15f : 1f),
                    rotation = 90f
                }
            ];
        }

        public static void Draw(VultureMask mask, FLabel[] labels, RoomCamera.SpriteLeaser sLeaser, float timeStacker, Vector2 camPos)
        {
            labels[0].SetPosition(sLeaser.sprites[0].GetPosition());
            labels[0].rotation = sLeaser.sprites[0].rotation + 90f;
            labels[0].color = sLeaser.sprites[0].color;
        }
    }
}
