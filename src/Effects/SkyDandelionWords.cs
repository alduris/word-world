using UnityEngine;
using static WordWorld.WordUtil;

namespace WordWorld.Effects
{
    public class SkyDandelionWords : Wordify<SkyDandelion>
    {
        public static FLabel[] Init(RoomCamera.SpriteLeaser sLeaser)
        {
            var label = new FLabel(Font, "D")
            {
                scale = sLeaser.sprites[0].element.sourcePixelSize.y / FontSize * 2f,
                color = sLeaser.sprites[0].color
            };

            if (sLeaser.sprites[0].shader.name == "CustomDepth")
            {
                label.shader = sLeaser.sprites[0].shader;
                label.alpha = sLeaser.sprites[0].alpha;
            }

            if (sLeaser.sprites.Length == 2)
            {
                var shadowLabel = new FLabel(Font, "D")
                {
                    scale = label.scale,
                    color = sLeaser.sprites[1].color
                };
                return [label, shadowLabel];
            }
            return [label];
        }

        public static void Draw(SkyDandelions.SkyDandelion plant, FLabel[] labels, RoomCamera.SpriteLeaser sLeaser, float timeStacker, Vector2 camPos)
        {
            var label = labels[0];
            label.SetPosition(Vector2.Lerp(plant.lastPos, plant.pos, timeStacker) - camPos);
            if (label.scale > 0f)
            {
                var health = Mathf.Lerp(plant.lastHealth, plant.health, timeStacker);
                label.scale = sLeaser.sprites[0].element.sourcePixelSize.y / FontSize * health * 2f;
            }

            if (labels.Length == 2)
            {
                labels[1].SetPosition(sLeaser.sprites[1].GetPosition());
            }
        }
    }
}
