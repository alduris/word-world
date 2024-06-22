using MoreSlugcats;
using UnityEngine;
using static WordWorld.WordUtil;

namespace WordWorld.Items
{
    public class SlimeMoldWords : Wordify<SlimeMold> // Slime mold is evil thanks to MSC. It is used for like. 3 different things and they are completely unrelated
    {
        private bool isSeed = false;
        private FLabel label;

        public override void Init(RoomCamera.SpriteLeaser sLeaser)
        {
            isSeed = ModManager.MSC && Drawable.abstractPhysicalObject.type == MoreSlugcatsEnums.AbstractObjectType.Seed; // why the hell are seeds slime mold

            // Figure out name and scaling
            var str = "Mold";
            var scale = 1f;
            if (Drawable.JellyfishMode)
            {
                str = "Jelly";
                scale = 2.4f;
            }
            else if (isSeed)
            {
                str = "Seed";
            }

            if (Drawable.big)
            {
                scale = 1.3f;
            }

            // Create label
            label = new FLabel(Font, str)
            {
                scale = Drawable.firstChunk.rad * 3f / FontSize * scale,
                color = isSeed ? sLeaser.sprites[0].color : Drawable.color
            };
            labels.Add(label);
        }

        public override void Draw(RoomCamera.SpriteLeaser sLeaser, float timeStacker, Vector2 camPos)
        {
            label.SetPosition(GetPos(Drawable.firstChunk, timeStacker) - camPos);
            if (!isSeed)
            {
                sLeaser.sprites[Drawable.LightSprite].isVisible = Drawable.darkMode > 0f;
                sLeaser.sprites[Drawable.BloomSprite].isVisible = Drawable.darkMode > 0f;
            }
        }
    }
}
