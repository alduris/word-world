using RWCustom;
using UnityEngine;
using static WordWorld.WordUtil;

namespace WordWorld.Creatures
{
    public class TentaclePlantWords : CreatureWordify<TentaclePlantGraphics>
    {
        public override void Init(RoomCamera.SpriteLeaser sLeaser)
        {
            labels.AddRange(LabelsFromLetters(Unpascal(Type)));
            var size = Drawable.plant.tentacle.idealLength / labels.Count / FontSize * 2f;
            foreach (var label in labels)
            {
                label.scale = size;
            }
        }

        public override void Draw(RoomCamera.SpriteLeaser sLeaser, float timeStacker, Vector2 camPos)
        {
            for (int i = 0; i < labels.Count; i++)
            {
                // Calculate position (we don't care about rotation lol)
                labels[i].SetPosition(PointAlongRope(i, labels.Count, Drawable.ropeGraphic, timeStacker) - camPos);

                // Calculate color since it can vary
                float colorIndex = Custom.LerpMap(i, 0, labels.Count - 1, 1, Drawable.danglers.Length);
                Color color = Color.Lerp(sLeaser.sprites[Mathf.FloorToInt(colorIndex)].color, sLeaser.sprites[Mathf.CeilToInt(colorIndex)].color, colorIndex % 1f);
                labels[i].color = color;
            }
        }
    }
}
