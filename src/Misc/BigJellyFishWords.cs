using System.Collections.Generic;
using System.Linq;
using MoreSlugcats;
using RWCustom;
using UnityEngine;
using static WordWorld.WordUtil;

namespace WordWorld.Misc
{
    public class BigJellyFishWords : Wordify<BigJellyFish>
    {
        public static FLabel[] Init(BigJellyFish bigJelly)
        {
            // Create labels
            List<FLabel> labels = [new(Font, "Big Jellyfish"), new(Font, "Core")];
            for (int i = 0; i < bigJelly.tentacles.Length; i++)
            {
                foreach (var c in "Tentacle".ToCharArray())
                {
                    labels.Add(new(Font, c.ToString()));
                }
            }

            // Color and rescale labels
            foreach (var label in labels)
            {
                label.color = bigJelly.color;
            }

            var chunks = bigJelly.bodyChunks;
            labels[0].scale = (chunks.Sum(x => x.rad) - chunks[bigJelly.CoreChunk].rad) * 2f / TextWidth(labels[0].text);
            labels[1].color = bigJelly.coreColor;
            labels[1].scale = chunks[bigJelly.CoreChunk].rad * 2f / TextWidth(labels[1].text);

            return [.. labels];
        }

        public static void Draw(BigJellyFish bigJelly, FLabel[] labels, float timeStacker, Vector2 camPos)
        {
            // Main and core
            labels[0].SetPosition(GetPos(bigJelly.mainBodyChunk, timeStacker) - camPos);
            labels[1].SetPosition(GetPos(bigJelly.bodyChunks[bigJelly.CoreChunk], timeStacker) - camPos);

            // Tentacles
            for (int i = 0; i < bigJelly.tentacles.Length; i++)
            {
                var tentacle = bigJelly.tentacles[i];
                // 8 = "Tentacle".Length
                for (int j = 0; j < 8; j++)
                {
                    int k = 2 + i * 8 + j;
                    var index = Custom.LerpMap(j, -1, 7, 0, tentacle.GetLength(0) - 1);
                    var prevPos = Vector2.Lerp(tentacle[Mathf.FloorToInt(index), 1], tentacle[Mathf.FloorToInt(index), 0], timeStacker);
                    var nextPos = Vector2.Lerp(tentacle[Mathf.CeilToInt(index), 1], tentacle[Mathf.CeilToInt(index), 0], timeStacker);

                    labels[k].SetPosition(Vector2.Lerp(prevPos, nextPos, index % 1) - camPos);
                    labels[k].rotation = AngleBtwn(nextPos, prevPos);
                }
            }
        }
    }
}
