using UnityEngine;
using static WordWorld.WordUtil;
using SkyDandelion = SkyDandelions.SkyDandelion;

namespace WordWorld.Effects
{
    public class SkyDandelionWords : Wordify<SkyDandelion>
    {
        private bool hasShadow = false;
        private FLabel label;
        private FLabel shadowLabel;

        public override void Init(RoomCamera.SpriteLeaser sLeaser)
        {
            label = new FLabel(Font, "D")
            {
                scale = sLeaser.sprites[0].element.sourcePixelSize.y / FontSize * 2f,
                color = sLeaser.sprites[0].color
            };

            if (sLeaser.sprites[0].shader.name == "CustomDepth")
            {
                label.shader = sLeaser.sprites[0].shader;
                label.alpha = sLeaser.sprites[0].alpha;
            }

            labels.Add(label);

            if (sLeaser.sprites.Length == 2)
            {
                shadowLabel = new FLabel(Font, "D")
                {
                    scale = label.scale,
                    color = sLeaser.sprites[1].color
                };
                hasShadow = true;
                labels.Add(shadowLabel);
            }
        }

        public override void Draw(RoomCamera.SpriteLeaser sLeaser, float timeStacker, Vector2 camPos)
        {
            label.SetPosition(Vector2.Lerp(Drawable.lastPos, Drawable.pos, timeStacker) - camPos);
            if (label.scale > 0f)
            {
                var health = Mathf.Lerp(Drawable.lastHealth, Drawable.health, timeStacker);
                label.scale = sLeaser.sprites[0].element.sourcePixelSize.y / FontSize * health * 2f;
            }

            if (hasShadow)
            {
                shadowLabel.SetPosition(sLeaser.sprites[1].GetPosition());
            }
        }

        protected override void AddToContainer(RoomCamera rCam, RoomCamera.SpriteLeaser sLeaser, FContainer container)
        {
            base.AddToContainer(rCam, sLeaser, container);
            if (hasShadow)
            {
                shadowLabel.RemoveFromContainer();
                rCam.ReturnFContainer("Shadows").AddChild(shadowLabel);
            }
        }
    }
}
