using UnityEngine;
using static WordWorld.WordUtil;

namespace WordWorld.Creatures
{
    public class TempleGuardWords : CreatureWordify<TempleGuardGraphics>
    {
        private FLabel headLabel;

        public override void Init(RoomCamera.SpriteLeaser sLeaser)
        {
            // TODO: robes?
            var text = Unpascal(Type);
            headLabel = new FLabel(Font, text)
            {
                scale = sLeaser.sprites[Drawable.HeadSprite].element.sourcePixelSize.x / TextWidth(text)
            };
        }

        public override void Draw(RoomCamera.SpriteLeaser sLeaser, float timeStacker, Vector2 camPos)
        {
            // Head, color = eye color
            headLabel.SetPosition(sLeaser.sprites[Drawable.HeadSprite].GetPosition());
            headLabel.rotation = sLeaser.sprites[Drawable.HeadSprite].rotation - 180f;
            headLabel.color = sLeaser.sprites[Drawable.EyeSprite(1)].color;

            // Re-enable halo
            for (int i = Drawable.FirstHaloSprite; i < Drawable.FirstHaloSprite + Drawable.halo.totalSprites; i++)
            {
                sLeaser.sprites[i].isVisible = true;
            }
        }
    }
}
