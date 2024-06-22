using UnityEngine;
using WordWorld.Defaults;
using static WordWorld.WordUtil;

namespace WordWorld.Items
{
    public class SwollenWaterNutWords() : POWordify<SwollenWaterNut>("N")
    {
        public override void Draw(RoomCamera.SpriteLeaser sLeaser, float timeStacker, Vector2 camPos)
        {
            base.Draw(sLeaser, timeStacker, camPos);
            if (Drawable.blink <= 1f) labels[0].color = Color.Lerp(sLeaser.sprites[2].color, new Color(0f, 0.4f, 1f), 0.4f);
        }
    }
}
