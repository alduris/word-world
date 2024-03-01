using MoreSlugcats;
using UnityEngine;
using static WordWorld.WordUtil;

namespace WordWorld.Items
{
    public static class SpearWords
    {
        private static Color SpearColor(Spear spear, RoomCamera.SpriteLeaser sLeaser)
        {
            Color color = spear.color;
            if (spear is ExplosiveSpear)
            {
                color = (spear as ExplosiveSpear).redColor;
            }
            else if (spear is ElectricSpear)
            {
                color = sLeaser.sprites[1].color;
            }
            else if (spear.bugSpear)
            {
                color = sLeaser.sprites[0].color;
            }
            if (spear.IsNeedle)
            {
                color = sLeaser.sprites[0].color;
            }
            return spear.blink > 1 && Random.value > 0.5f ? spear.blinkColor : color;
        }

        public static FLabel[] Init(Spear spear, RoomCamera.SpriteLeaser sLeaser)
        {
            var label = new FLabel(Font, "Spear");

            bool explosive = spear is ExplosiveSpear;
            label.color = SpearColor(spear, sLeaser);
            label.scale = sLeaser.sprites[spear.bugSpear || explosive ? 1 : 0].element.sourcePixelSize.y * 0.9f / TextWidth(label.text);
            label.scaleY /= 2f;


            return [label];
        }

        public static void Draw(Spear spear, FLabel[] labels, RoomCamera.SpriteLeaser sLeaser, float timeStacker, Vector2 camPos)
        {
            labels[0].SetPosition(GetPos(spear.firstChunk, timeStacker) - camPos);
            labels[0].rotation = FixRotation(sLeaser.sprites[spear.bugSpear || spear is ExplosiveSpear ? 1 : 0].rotation) - 90f;
            labels[0].color = SpearColor(spear, sLeaser);
        }
    }
}
