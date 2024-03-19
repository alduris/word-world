using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static WordWorld.WordUtil;

namespace WordWorld.Creatures
{
    public static class CentipedeWords
    {
        public static FLabel[] Init(CentipedeGraphics centiGraf, CreatureTemplate.Type type)
        {
            // Thanks several users on RW Main discord for this idea
            List<FLabel> labels = [];
            float scale = centiGraf.centipede.bodyChunks.Max(c => c.rad) * 3f / FontSize;
            if (type == CreatureTemplate.Type.SmallCentipede)
            {
                // Special case for babypede
                labels.AddRange(LabelsFromLetters("Babypede"));
            }
            else
            {
                int numChunks = centiGraf.centipede.bodyChunks.Length;

                bool stretch = numChunks >= type.value.Length;
                if (stretch)
                {
                    // Stretch the name to match segments (calculate how many letters we need to add)
                    int nameE = type.value.IndexOf("Centi") + 1;
                    if (nameE == 0) nameE = type.value.IndexOf("pede") + 1;
                    if (nameE == 0) nameE = type.value.IndexOf("e") + 1;
                    var chars = type.value.ToCharArray();
                    int numOfEs = numChunks - chars.Length;

                    for (int i = 0; i < numChunks; i++)
                    {
                        int j = (i >= nameE && i < nameE + numOfEs) ? nameE : (i < nameE ? i : i - numOfEs);
                        labels.Add(new(Font, chars[j].ToString()));
                    }
                }
                else
                {
                    // Fit name in segments
                    labels.AddRange(LabelsFromLetters(type.value));
                    scale *= (float)centiGraf.centipede.bodyChunks.Length / labels.Count;
                }
            }

            // Scale and color labels
            foreach (var label in labels)
            {
                label.scale = scale * 1.5f;
                label.color = centiGraf.ShellColor;
            }

            return [.. labels];
        }

        public static void Draw(CentipedeGraphics centiGraf, FLabel[] labels, RoomCamera.SpriteLeaser sLeaser, float timeStacker, Vector2 camPos)
        {
            var chunks = centiGraf.centipede.bodyChunks;
            bool fit = labels.Length > chunks.Length;
            for (int i = 0; i < labels.Length; i++)
            {
                if (fit)
                {
                    labels[i].SetPosition(PointAlongChunks(i, labels.Length, chunks, timeStacker) - camPos);
                    labels[i].rotation = RotationAlongSprites(i, labels.Length, chunks.Length, sLeaser.sprites, x => x);
                }
                else
                {
                    labels[i].SetPosition(chunks[i].pos - camPos);
                    labels[i].rotation = sLeaser.sprites[i].rotation;
                }
            }
        }
    }
}
