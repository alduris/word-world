using UnityEngine;
using static WordWorld.WordUtil;

namespace WordWorld.Misc
{
    public class LizardSpitWords : Wordify<LizardSpit>
    {
        private FLabel label;

        public override void Init(RoomCamera.SpriteLeaser sLeaser)
        {
            label = new(Font, "Spit") { color = Color.Lerp(sLeaser.sprites[Drawable.DotSprite].color, sLeaser.sprites[Drawable.JaggedSprite].color, 0.4f) };
        }

        public override void Draw(RoomCamera.SpriteLeaser sLeaser, float timeStacker, Vector2 camPos)
        {
            label.SetPosition(Vector2.Lerp(Drawable.lastPos, Drawable.pos, timeStacker) - camPos);
            label.scale = Drawable.Rad * 4f / FontSize;
        }
    }
}
