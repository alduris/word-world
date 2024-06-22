using UnityEngine;
using WordWorld.Defaults;

namespace WordWorld.Items
{
    public class DataPearlWords() : POWordify<DataPearl>("P")
    {
        public override void Draw(RoomCamera.SpriteLeaser sLeaser, float timeStacker, Vector2 camPos)
        {
            base.Draw(sLeaser, timeStacker, camPos);
            labels[0].color = Color.Lerp(Drawable.color, Drawable.highlightColor ?? Drawable.color, Mathf.Lerp(Drawable.lastGlimmer, Drawable.glimmer, timeStacker));
        }
    }
}
