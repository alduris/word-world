using UnityEngine;
using WordWorld.Defaults;

namespace WordWorld.Items
{
    public class NeedleEggWords() : POWordify<NeedleEgg>("Egg")
    {
        public override void Draw(RoomCamera.SpriteLeaser sLeaser, float timeStacker, Vector2 camPos)
        {
            base.Draw(sLeaser, timeStacker, camPos);
            if (Drawable.blink <= 1)
            {
                labels[0].color = sLeaser.sprites[0].color;
            }
        }
    }
}
