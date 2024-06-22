using UnityEngine;
using static WordWorld.WordUtil;
using GreenSpark = GreenSparks.GreenSpark;

namespace WordWorld.Effects
{
    public class GreenSparkWords : Wordify<GreenSpark>
    {
        private FLabel label;

        public override void Init(RoomCamera.SpriteLeaser sLeaser)
        {
            label = new FLabel(Font, "S")
            {
                scale = Mathf.Sqrt(0.5f + Drawable.depth * 0.25f),
                color = sLeaser.sprites[0].color
            };

            if (sLeaser.sprites[0].shader.name == "CustomDepth")
            {
                label.shader = sLeaser.sprites[0].shader;
                label.alpha = sLeaser.sprites[0].alpha;
            }

            labels.Add(label);
        }

        public override void Draw(RoomCamera.SpriteLeaser sLeaser, float timeStacker, Vector2 camPos)
        {
            label.SetPosition(Vector2.Lerp(Drawable.lastPos, Drawable.pos, timeStacker) - camPos);
            label.rotation = FixRotation(sLeaser.sprites[0].rotation);
        }
    }
}
