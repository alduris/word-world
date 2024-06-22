
using UnityEngine;
using WordWorld.Defaults;
using static WordWorld.WordUtil;

namespace WordWorld.Misc
{
    public class SporePlantAttachedBeeWords() : POWordify<SporePlant.AttachedBee>("B")
    {
        public override void Init(RoomCamera.SpriteLeaser sLeaser)
        {
            Label = new(Font, "B") { scale = 0.5f, color = sLeaser.sprites[0].color };
        }

        public override void Draw(RoomCamera.SpriteLeaser sLeaser, float timeStacker, Vector2 camPos)
        {
            base.Draw(sLeaser, timeStacker, camPos);

            if (Drawable.lastStingerOut || Drawable.stingerOut)
            {
                for (int i = 0; i < Drawable.stinger.GetLength(0); i++)
                {
                    sLeaser.sprites[i + 1].isVisible = true;
                }
            }
        }
    }
}
