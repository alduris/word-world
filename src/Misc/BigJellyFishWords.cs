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
        private FLabel bodyLabel;
        private FLabel coreLabel;
        private readonly List<List<FLabel>> tentacleLabels = [];

        public override void Init(RoomCamera.SpriteLeaser sLeaser)
        {
            // Create labels
            bodyLabel = new(Font, "Big Jellyfish");
            coreLabel = new(Font, "Core");
            labels.Add(bodyLabel); labels.Add(coreLabel);

            for (int i = 0; i < Drawable.tentacles.Length; i++)
            {
                List<FLabel> list = [];
                foreach (var c in "Tentacle".ToCharArray())
                {
                    list.Add(new(Font, c.ToString()));
                }
                tentacleLabels.Add(list);
                labels.AddRange(list);
            }

            // Color and rescale labels
            foreach (var label in labels)
            {
                label.color = Drawable.color;
            }

            var chunks = Drawable.bodyChunks;
            bodyLabel.scale = (chunks.Sum(x => x.rad) - chunks[Drawable.CoreChunk].rad) * 2f / TextWidth(bodyLabel.text);
            coreLabel.color = Drawable.coreColor;
            coreLabel.scale = chunks[Drawable.CoreChunk].rad * 2f / TextWidth(coreLabel.text);
        }

        public override void Draw(RoomCamera.SpriteLeaser sLeaser, float timeStacker, Vector2 camPos)
        {
            // Main and core
            bodyLabel.SetPosition(GetPos(Drawable.mainBodyChunk, timeStacker) - camPos);
            coreLabel.SetPosition(GetPos(Drawable.bodyChunks[Drawable.CoreChunk], timeStacker) - camPos);

            // Tentacles
            for (int i = 0; i < tentacleLabels.Count; i++)
            {
                var tentacle = Drawable.tentacles[i];
                var labels = tentacleLabels[i];
                for (int j = 0; j < labels.Count; j++)
                {
                    var index = Custom.LerpMap(j, -1, labels.Count, 0, tentacle.GetLength(0) - 1);
                    var prevPos = Vector2.Lerp(tentacle[Mathf.FloorToInt(index), 1], tentacle[Mathf.FloorToInt(index), 0], timeStacker);
                    var nextPos = Vector2.Lerp(tentacle[Mathf.CeilToInt(index), 1], tentacle[Mathf.CeilToInt(index), 0], timeStacker);

                    labels[j].SetPosition(Vector2.Lerp(prevPos, nextPos, index % 1) - camPos);
                    labels[j].rotation = AngleBtwn(nextPos, prevPos);
                }
            }
        }
    }
}
