using UnityEngine;
using static WordWorld.WordUtil;

namespace WordWorld.Creatures
{
    public class HazerWords : CreatureWordify<HazerGraphics>
    {
        private FLabel label;
        private FSprite bodySprite;

        public override void Init(RoomCamera.SpriteLeaser sLeaser)
        {
            bodySprite = sLeaser.sprites[Drawable.BodySprite];
            label = new FLabel(Font, Unpascal(Type))
            {
                scale = Drawable.bug.mainBodyChunk.rad * 6f / FontSize
            };
            labels.Add(label);
        }

        public override void Draw(RoomCamera.SpriteLeaser sLeaser, float timeStacker, Vector2 camPos)
        {
            label.SetPosition(GetPos(Drawable.bug.firstChunk, timeStacker) - camPos);
            label.rotation = FixRotation(bodySprite.rotation) - 90f;
            label.color = bodySprite.color;
        }
    }
}
