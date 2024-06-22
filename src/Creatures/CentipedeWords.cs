using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static WordWorld.WordUtil;

namespace WordWorld.Creatures
{
    public class CentipedeWords : CreatureWordify<CentipedeGraphics>
    {
        public override void Init(RoomCamera.SpriteLeaser sLeaser)
        {
            // Thanks several users on RW Main discord for this idea
            float scale = Drawable.centipede.bodyChunks.Max(c => c.rad) * 3f / FontSize;
            if (Type == CreatureTemplate.Type.SmallCentipede)
            {
                // Special case for babypede
                labels.AddRange(LabelsFromLetters("Babypede"));
            }
            else
            {
                int numChunks = Drawable.centipede.bodyChunks.Length;

                bool stretch = numChunks >= Type.value.Length;
                if (stretch)
                {
                    // Stretch the name to match segments (calculate how many letters we need to add)
                    int nameE = Type.value.IndexOf("Centi") + 1;
                    if (nameE == 0) nameE = Type.value.IndexOf("pede") + 1;
                    if (nameE == 0) nameE = Type.value.IndexOf("e") + 1;
                    var chars = Type.value.ToCharArray();
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
                    labels.AddRange(LabelsFromLetters(Type.value));
                    scale *= (float)Drawable.centipede.bodyChunks.Length / labels.Count;
                }
            }

            // Scale and color labels
            foreach (var label in labels)
            {
                label.scale = scale * 1.5f;
                label.color = Drawable.ShellColor;
            }
        }

        public override void Draw(RoomCamera.SpriteLeaser sLeaser, float timeStacker, Vector2 camPos)
        {
            var chunks = Drawable.centipede.bodyChunks;
            bool fit = labels.Count > chunks.Length;
            for (int i = 0; i < labels.Count; i++)
            {
                if (fit)
                {
                    labels[i].SetPosition(PointAlongChunks(i, labels.Count, chunks, timeStacker) - camPos);
                    labels[i].rotation = RotationAlongSprites(i, labels.Count, chunks.Length, sLeaser.sprites, x => x);
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
