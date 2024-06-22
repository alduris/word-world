using UnityEngine;
using WordWorld.Defaults;

namespace WordWorld.Items
{
    public class LanternWords() : POWordify<Lantern>("L")
    {
        public override void Init(RoomCamera.SpriteLeaser sLeaser)
        {
            base.Init(sLeaser);
            Label.color = sLeaser.sprites[0].color;
        }

        public override void Draw(RoomCamera.SpriteLeaser sLeaser, float timeStacker, Vector2 camPos)
        {
            base.Draw(sLeaser, timeStacker, camPos);
            Label.color = Drawable.blink > 1 ? Drawable.blinkColor : Color.Lerp(sLeaser.sprites[0].color, new Color(1f, 1f, 1f), 0.4f);
            sLeaser.sprites[3].isVisible = true;
        }
    }
}
