using UnityEngine;
using static WordWorld.WordUtil;

namespace WordWorld.Items
{
    public class FlareBombWords : Wordify<FlareBomb>
    {
        private FLabel label;

        public override void Init(RoomCamera.SpriteLeaser sLeaser)
        {
            label = new FLabel(Font, "F")
            {
                scale = Drawable.firstChunk.rad * 4f / FontSize
            };
            labels.Add(label);
        }

        public override void Draw(RoomCamera.SpriteLeaser sLeaser, float timeStacker, Vector2 camPos)
        {
            label.SetPosition(GetPos(Drawable.firstChunk, timeStacker) - camPos);
            label.color = Drawable.blink > 1 ? Drawable.blinkColor : Color.Lerp(Drawable.color, new(1f, 1f, 1f), 0.9f);
            sLeaser.sprites[2].isVisible = true;
        }
    }
}
