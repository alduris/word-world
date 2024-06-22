using UnityEngine;
using static WordWorld.WordUtil;

namespace WordWorld.Effects
{
    public class GreenSparkWords : Wordify<GreenSpark>
    {
        public static FLabel[] Init(GreenSparks.GreenSpark spark, RoomCamera.SpriteLeaser sLeaser)
        {
            var label = new FLabel(Font, "S")
            {
                scale = Mathf.Sqrt(0.5f + spark.depth * 0.25f),
                color = sLeaser.sprites[0].color
            };

            if (sLeaser.sprites[0].shader.name == "CustomDepth")
            {
                label.shader = sLeaser.sprites[0].shader;
                label.alpha = sLeaser.sprites[0].alpha;
            }
            return [label];
        }

        public static void Draw(GreenSparks.GreenSpark spark, FLabel[] labels, RoomCamera.SpriteLeaser sLeaser, float timeStacker, Vector2 camPos)
        {
            var label = labels[0];
            label.SetPosition(Vector2.Lerp(spark.lastPos, spark.pos, timeStacker) - camPos);
            label.rotation = FixRotation(sLeaser.sprites[0].rotation);
        }
    }
}
