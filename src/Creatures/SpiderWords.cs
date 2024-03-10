using UnityEngine;
using static WordWorld.WordUtil;

namespace WordWorld.Creatures
{
    public static class SpiderWords
    {
        public static FLabel[] Init(SpiderGraphics spiderGraf, CreatureTemplate.Type type)
        {
            var text = Unpascal(type);
            return [new FLabel(Font, text)
            {
                scale = spiderGraf.spider.firstChunk.rad * 4f / TextWidth(text),
                color = spiderGraf.blackColor
            }];
        }

        public static void Draw(SpiderGraphics spiderGraf, FLabel[] labels, RoomCamera.SpriteLeaser sLeaser, float timeStacker, Vector2 camPos)
        {
            var label = labels[0];
            label.SetPosition(GetPos(spiderGraf.spider.mainBodyChunk, timeStacker) - camPos);

            var rot = sLeaser.sprites[spiderGraf.BodySprite].rotation;
            if (spiderGraf.spider.dead) rot = FixRotation(rot + 90f);
            label.rotation = rot;
        }
    }
}
