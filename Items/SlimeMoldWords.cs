using MoreSlugcats;
using UnityEngine;
using static WordWorld.WordUtil;

namespace WordWorld.Items
{
    public static class SlimeMoldWords
    {
        public static FLabel[] Init(SlimeMold slime, RoomCamera.SpriteLeaser sLeaser)
        {
            bool isSeed = ModManager.MSC && slime.abstractPhysicalObject.type == MoreSlugcatsEnums.AbstractObjectType.Seed; // why the hell are seeds slime mold

            // Figure out name and scaling
            var str = "Mold";
            var scale = 1f;
            if (slime.JellyfishMode)
            {
                str = "Jelly";
                scale = 2.4f;
            }
            else if (isSeed)
            {
                str = "Seed";
            }

            if (slime.big)
            {
                scale = 1.3f;
            }

            // Create label
            var label = new FLabel(Font, str)
            {
                scale = slime.firstChunk.rad * 3f / FontSize * scale,
                color = isSeed ? sLeaser.sprites[0].color : slime.color
            };

            return [label];
        }

        public static void Draw(SlimeMold slime, FLabel[] labels, RoomCamera.SpriteLeaser sLeaser, float timeStacker, Vector2 camPos)
        {
            bool isSeed = ModManager.MSC && slime.abstractPhysicalObject.type == MoreSlugcatsEnums.AbstractObjectType.Seed;
            labels[0].SetPosition(GetPos(slime.firstChunk, timeStacker) - camPos);
            if (!isSeed)
            {
                sLeaser.sprites[slime.LightSprite].isVisible = slime.darkMode > 0f;
                sLeaser.sprites[slime.BloomSprite].isVisible = slime.darkMode > 0f;
            }
        }
    }
}
