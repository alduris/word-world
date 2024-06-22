using MoreSlugcats;
using UnityEngine;
using static WordWorld.WordUtil;

namespace WordWorld.Items
{
    public class SpearWords : Wordify<Spear>
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

        private FLabel label;

        public override void Init(RoomCamera.SpriteLeaser sLeaser)
        {
            label = new FLabel(Font, "Spear");

            bool explosive = Drawable is ExplosiveSpear;
            label.color = SpearColor(Drawable, sLeaser);
            label.scale = sLeaser.sprites[Drawable.bugSpear || explosive ? 1 : 0].element.sourcePixelSize.y * 0.9f / TextWidth(label.text);
            label.scaleY /= 2f;

            labels.Add(label);
        }

        public override void Draw(RoomCamera.SpriteLeaser sLeaser, float timeStacker, Vector2 camPos)
        {
            label.SetPosition(GetPos(Drawable.firstChunk, timeStacker) - camPos);
            label.rotation = FixRotation(sLeaser.sprites[Drawable.bugSpear || Drawable is ExplosiveSpear ? 1 : 0].rotation) - 90f;
            label.color = SpearColor(Drawable, sLeaser);
        }
    }
}
