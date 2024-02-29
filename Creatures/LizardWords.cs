using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static WordWorld.WordUtil;

namespace WordWorld.Creatures
{
    public static class LizardWords
    {
        public static FLabel[] Init(LizardGraphics lizGraf, CreatureTemplate.Type type)
        {
            var lizard = lizGraf.lizard;
            var name = PascalRegex.Replace(type.value, " ");
            var nameLabel = new FLabel(Font, name)
            {
                scale = (lizard.bodyChunks.Sum(x => x.rad) * 2f + lizard.bodyChunkConnections.Sum(x => x.distance)) / TextWidth(name)
            };

            List<FLabel> labels = [nameLabel];
            if (lizGraf.tongue != null)
            {
                for (int i = 0; i < 6; i++)
                {
                    labels.Add(new(Font, "Tongue"[i].ToString())
                    {
                        scale = 0.875f,
                        color = lizGraf.palette.blackColor
                    });
                }
            }
            return [.. labels];
        }

        public static void Draw(LizardGraphics lizGraf, FLabel[] labels, RoomCamera.SpriteLeaser sLeaser, float timeStacker, Vector2 camPos)
        {
            // Main body
            var chunks = lizGraf.lizard.bodyChunks;
            labels[0].color = lizGraf.HeadColor(timeStacker);
            labels[0].SetPosition(GetPos(chunks[1], timeStacker) - camPos);
            labels[0].rotation = FixRotation(AngleBtwnChunks(chunks[0], chunks[2], timeStacker)) - 90f;

            // Tongue
            if (lizGraf.tongue != null)
            {
                for (int i = 0; i < 6; i++)
                {
                    var label = labels[i + 1];
                    label.SetPosition(PointAlongParts(i, 6, lizGraf.tongue, timeStacker) - camPos);
                    label.isVisible = lizGraf.lizard.tongue.Out;
                    // future thing maybe: cyan lizards have custom tongue color depending on tongue vertex
                }
            }
        }
    }
}
