using System.Linq;
using UnityEngine;
using static WordWorld.WordUtil;

namespace WordWorld.Creatures
{
    public class BigEelWords : CreatureWordify<BigEelGraphics>
    {
        public override void Init(RoomCamera.SpriteLeaser sLeaser)
        {
            labels.AddRange(LabelsFromLetters(Type == CreatureTemplate.Type.BigEel ? "Leviathan" : Type.value));

            var scale = Drawable.eel.bodyChunks.Max(c => c.rad) * 2.5f / FontSize;
            var colorA = Drawable.eel.iVars.patternColorA;
            var colorB = Drawable.eel.iVars.patternColorB;
            for (int i = 0; i < labels.Count; i++)
            {
                var label = labels[i];
                label.scale = scale;
                label.color = HSLColor.Lerp(colorA, colorB, i / (float)(labels.Count - 1)).rgb;
            }
        }

        public override void Draw(RoomCamera.SpriteLeaser sLeaser, float timeStacker, Vector2 camPos)
        {
            var chunks = Drawable.eel.bodyChunks;
            for (int i = 0; i < labels.Count; i++)
            {
                labels[i].SetPosition(PointAlongChunks(i, labels.Count, chunks, timeStacker) - camPos);
                labels[i].rotation = RotationAlongSprites(i, labels.Count, chunks.Length, sLeaser.sprites, Drawable.BodyChunksSprite);
            }
        }
    }
}
