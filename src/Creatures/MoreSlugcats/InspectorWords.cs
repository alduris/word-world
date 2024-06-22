using System.Collections.Generic;
using MoreSlugcats;
using UnityEngine;
using static WordWorld.WordUtil;

namespace WordWorld.Creatures.MoreSlugcats
{
    public class InspectorWords : Wordify<Inspector>
    {
        public static FLabel[] Init(InspectorGraphics inspGraf, CreatureTemplate.Type type)
        {
            List<FLabel> labels = [new(Font, Unpascal(type))];
            for (int i = 0; i < inspGraf.myInspector.heads.Length; i++)
                labels.Add(new(Font, "Head"));

            // Inspectors' main body chunks are teeny tiny little things (1/4 of a tile width in diameter, not radius)
            // Therefore, using it would be a bad idea (can barely see the label) so instead I hardcode it
            labels[0].scale = 1.5f; // inspGraf.myInspector.bodyChunks[0].rad * 2f / FontSize;
            for (int i = 0; i < inspGraf.myInspector.heads.Length; i++)
            {
                labels[i + 1].scale = 1.125f;
            }
            return [.. labels];
        }

        public static void Draw(InspectorGraphics inspGraf, FLabel[] labels, float timeStacker, Vector2 camPos)
        {
            var insp = inspGraf.myInspector;

            // Body
            labels[0].SetPosition(GetPos(insp.bodyChunks[0], timeStacker) - camPos);
            labels[0].color = insp.bodyColor;

            // Heads
            var heads = insp.heads;
            for (int i = 0; i < heads.Length; i++)
            {
                Vector2 pos = GetPos(heads[i].Tip, timeStacker);
                labels[i + 1].SetPosition(pos - camPos);
                labels[i + 1].rotation = AngleBtwn(GetPos(insp.bodyChunks[0], timeStacker), pos);
                labels[i + 1].color = insp.bodyColor;
            }
        }
    }
}
