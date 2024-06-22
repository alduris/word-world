using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static WordWorld.WordUtil;

namespace WordWorld.Creatures
{
    public class LizardWords : Wordify<Lizard>
    {
        public static FLabel[] Init(LizardGraphics lizGraf, CreatureTemplate.Type type)
        {
            List<FLabel> labels = [];
            var lizard = lizGraf.lizard;
            var name = PascalRegex.Replace(type.value, " ");
            var nameScale = (lizard.bodyChunks.Sum(x => x.rad) * 2f + lizard.bodyChunkConnections.Sum(x => x.distance)) / TextWidth(name);
            foreach (var c in name)
            {
                labels.Add(new FLabel(Font, c.ToString()) { scale = nameScale });
            }

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

        public static void Draw(LizardGraphics lizGraf, FLabel[] labels, float timeStacker, Vector2 camPos)
        {
            // Main body
            var name = PascalRegex.Replace(lizGraf.lizard.abstractCreature.creatureTemplate.type.value, " ");
            var nameLen = TextWidth(name);
            var chunks = lizGraf.lizard.bodyChunks;
            var headSize = Mathf.Max(0.3f, (lizGraf.head.rad + lizGraf.headConnectionRad) / (lizGraf.head.rad + lizGraf.headConnectionRad + lizGraf.BodyAndTailLength));

            bool side = GetPos(chunks[0], timeStacker).x < GetPos(chunks[2], timeStacker).x;
            var angle = FixRotation(AngleBtwnChunks(chunks[0], chunks[2], timeStacker)) - 90f;
            List<Vector2> points = chunks.Select(x => GetPos(x, timeStacker)).ToList();
            foreach (var part in lizGraf.tail)
            {
                points.Add(GetPos(part, timeStacker));
            }

            for (int i = 0; i < name.Length; i++)
            {
                var lerp = (TextWidth(name.Substring(0, i)) + TextWidth(name[i].ToString()) / 2f) / nameLen; //(float)i / (nameLen - 1);
                lerp = side ? lerp : 1 - lerp;
                labels[i].color = (lerp <= headSize || i == 0) ? lizGraf.HeadColor(timeStacker) : lizGraf.BodyColor(lerp);
                labels[i].SetPosition(PointAlongVectors(lerp, points) - camPos);
                labels[i].rotation = RotationAlongVectors(lerp, points) + (side ? -90f : 90f);
            }
            
            // Tongue
            if (lizGraf.tongue != null)
            {
                for (int i = 0; i < 6; i++)
                {
                    var label = labels[i + name.Length];
                    label.SetPosition(PointAlongParts(i, 6, lizGraf.tongue, timeStacker) - camPos);
                    label.isVisible = lizGraf.lizard.tongue.Out;
                    
                    // Cyans get special tongue color
                    if (lizGraf.lizard.abstractCreature.creatureTemplate.type == CreatureTemplate.Type.CyanLizard)
                    {
                        label.color = Color.Lerp(lizGraf.HeadColor(timeStacker), lizGraf.palette.blackColor, i / 5f);
                    }
                }
            }
        }
    }
}
