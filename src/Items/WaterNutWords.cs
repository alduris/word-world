using UnityEngine;
using WordWorld.Defaults;
using static WordWorld.WordUtil;

namespace WordWorld.Items
{
    public class WaterNutWords() : POWordify<WaterNut>("N")
    {
        public override void Draw(RoomCamera.SpriteLeaser sLeaser, float timeStacker, Vector2 camPos)
        {
            base.Draw(sLeaser, timeStacker, camPos);
            if (Drawable.blink <= 1) Label.color = Color.Lerp(sLeaser.sprites[0].color, Drawable.color, 0.4f);
        }
    }
}
