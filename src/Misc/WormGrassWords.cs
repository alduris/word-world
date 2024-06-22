using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static WordWorld.WordUtil;
using Worm = WormGrass.Worm;

namespace WordWorld.Misc
{
    public class WormGrassWords : Wordify<Worm>
    {
        public override void Init(RoomCamera.SpriteLeaser sLeaser)
        {
            labels.AddRange([new(Font, "m"), new(Font, "r")]);

            for (int i = 0; i < Drawable.length / 10f - 4; i++)
            {
                labels.Add(new(Font, "o"));
            }

            labels.Add(new(Font, "o"));
            labels.Add(new(Font, "W"));

            var verticeColors = (sLeaser.sprites[0] as TriangleMesh).verticeColors;
            for (int i = 0; i < labels.Count; i++)
            {
                labels[i].scale = Mathf.Max(0.5f, (Drawable.length / labels.Count) / FontSize * 2f);
                labels[i].color = verticeColors[Mathf.RoundToInt(Mathf.InverseLerp(0, labels.Count - 1, i) * (verticeColors.Length - 1))];
            }
        }

        public override void Draw(RoomCamera.SpriteLeaser sLeaser, float timeStacker, Vector2 camPos)
        {
            var positions = Drawable.segments.Select(x => Vector2.Lerp(x.lastPos, x.pos, timeStacker)).ToList();
            var verticeColors = (sLeaser.sprites[0] as TriangleMesh).verticeColors;
            positions.Insert(0, Vector2.Lerp(Drawable.lastPos, Drawable.pos, timeStacker));
            positions.Add(Drawable.basePos);
            for (int i = 0; i < labels.Count; i++)
            {
                var lerp = Mathf.InverseLerp(0, labels.Count - 1, i);
                var label = labels[i];

                label.SetPosition(PointAlongVectors(1 - lerp, positions) - camPos);
                label.color = verticeColors[Mathf.RoundToInt(lerp * (verticeColors.Length - 1))];
            }
            var last = labels.Count - 1;
            labels[last].color = Color.Lerp(labels[last].color, sLeaser.sprites[1].color, Mathf.Sqrt(Drawable.excitement));
        }
    }
}
