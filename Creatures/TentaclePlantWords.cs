using RWCustom;
using UnityEngine;
using static WordWorld.WordUtil;

namespace WordWorld.Creatures
{
    public static class TentaclePlantWords
    {
        public static FLabel[] Init(TentaclePlantGraphics kelpGraf, CreatureTemplate.Type type)
        {
            var labels = LabelsFromLetters(Unpascal(type));
            var size = kelpGraf.plant.tentacle.idealLength / labels.Length / FontSize * 0.875f;
            foreach (var label in labels)
            {
                label.scale = size;
            }
            return labels;
        }

        public static void Draw(TentaclePlantGraphics kelpGraf, FLabel[] labels, RoomCamera.SpriteLeaser sLeaser, float timeStacker, Vector2 camPos)
        {
            for (int i = 0; i < labels.Length; i++)
            {
                // Calculate position (we don't care about rotation lol)
                labels[i].SetPosition(PointAlongRope(i, labels.Length, kelpGraf.ropeGraphic, timeStacker) - camPos);

                // Calculate color since it can vary
                float colorIndex = Custom.LerpMap(i, 0, labels.Length - 1, 1, kelpGraf.danglers.Length - 1);
                Color color = Color.Lerp(sLeaser.sprites[Mathf.FloorToInt(colorIndex)].color, sLeaser.sprites[Mathf.CeilToInt(colorIndex)].color, colorIndex % 1f);
                labels[i].color = color; // UndoColorLerp(color, rCam.currentPalette.blackColor, rCam.room.Darkness(kelpGraf.plant.rootPos));
            }
        }
    }
}
