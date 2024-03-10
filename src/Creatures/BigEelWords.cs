using System.Linq;
using UnityEngine;
using static WordWorld.WordUtil;

namespace WordWorld.Creatures
{
    public static class BigEelWords
    {
        public static FLabel[] Init(BigEelGraphics bigEelGraf, CreatureTemplate.Type type)
        {
            var labels = LabelsFromLetters(type == CreatureTemplate.Type.BigEel ? "Leviathan" : type.value);

            var scale = bigEelGraf.eel.bodyChunks.Max(c => c.rad) * 2.5f / FontSize;
            var colorA = bigEelGraf.eel.iVars.patternColorA;
            var colorB = bigEelGraf.eel.iVars.patternColorB;
            for (int i = 0; i < labels.Length; i++)
            {
                var label = labels[i];
                label.scale = scale;
                label.color = HSLColor.Lerp(colorA, colorB, i / (float)(labels.Length - 1)).rgb;
            }

            return labels;
        }

        public static void Draw(BigEelGraphics bigEelGraf, FLabel[] labels, RoomCamera.SpriteLeaser sLeaser, float timeStacker, Vector2 camPos)
        {

            // Text says "Leviathan" but split into individual chars
            var chunks = bigEelGraf.eel.bodyChunks;
            for (int i = 0; i < labels.Length; i++)
            {
                labels[i].SetPosition(PointAlongChunks(i, labels.Length, chunks, timeStacker) - camPos);
                labels[i].rotation = RotationAlongSprites(i, labels.Length, chunks.Length, sLeaser.sprites, bigEelGraf.BodyChunksSprite);
            }
        }
    }
}
