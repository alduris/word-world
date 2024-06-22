using UnityEngine;
using static WordWorld.WordUtil;

namespace WordWorld.Creatures
{
    public class SpiderWords : CreatureWordify<SpiderGraphics>
    {
        private FLabel label;

        public override void Init(RoomCamera.SpriteLeaser sLeaser)
        {
            var text = Unpascal(Type);
            label = new FLabel(Font, text)
            {
                scale = Drawable.spider.firstChunk.rad * 4f / TextWidth(text),
                color = Drawable.blackColor
            };
        }

        public override void Draw(RoomCamera.SpriteLeaser sLeaser, float timeStacker, Vector2 camPos)
        {
            label.SetPosition(GetPos(Drawable.spider.mainBodyChunk, timeStacker) - camPos);

            var rot = sLeaser.sprites[Drawable.BodySprite].rotation;
            if (Drawable.spider.dead) rot = FixRotation(rot + 90f);
            label.rotation = rot;
        }
    }
}
