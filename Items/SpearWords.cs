using MoreSlugcats;
using UnityEngine;
using static WordWorld.WordUtil;

namespace WordWorld.Items
{
    public static class SpearWords
    {
        public static FLabel[] Init(Spear spear, RoomCamera.SpriteLeaser sLeaser)
        {
            var label = new FLabel(Font, "Spear");

            bool explosive = spear is ExplosiveSpear;
            label.color = spear.color;
            label.scale = sLeaser.sprites[spear.bugSpear || explosive ? 1 : 0].element.sourcePixelSize.y * 0.9f / TextWidth(label.text);
            label.scaleY /= 2f;

            if (explosive)
            {
                label.color = (spear as ExplosiveSpear).redColor;
            }
            else if (spear.bugSpear)
            {
                label.color = sLeaser.sprites[0].color;
            }

            return [label];
        }

        public static void Draw(Spear spear, FLabel[] labels, RoomCamera.SpriteLeaser sLeaser, float timeStacker, Vector2 camPos)
        {
            labels[0].SetPosition(GetPos(spear.firstChunk, timeStacker) - camPos);
            labels[0].rotation = FixRotation(sLeaser.sprites[spear.bugSpear || spear is ExplosiveSpear ? 1 : 0].rotation) - 90f;

            if (spear.IsNeedle)
            {
                labels[0].color = sLeaser.sprites[0].color;
            }
            else if (spear is ElectricSpear)
            {
                labels[0].color = sLeaser.sprites[1].color;
            }
        }
    }
}
