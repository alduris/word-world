using UnityEngine;
using WordWorld.Defaults;

namespace WordWorld.Items
{
    public class ScavengerBombWords() : POWordify<ScavengerBomb>("B")
    {
        public override void Draw(RoomCamera.SpriteLeaser sLeaser, float timeStacker, Vector2 camPos)
        {
            base.Draw(sLeaser, timeStacker, camPos);
            if (Drawable.blink <= 1) Label.color = sLeaser.sprites[0].color;
        }
    }
}
