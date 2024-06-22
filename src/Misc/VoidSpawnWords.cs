using System.Linq;
using RWCustom;
using UnityEngine;
using static WordWorld.WordUtil;

namespace WordWorld.Misc
{
    public class VoidSpawnWords : Wordify<VoidSpawn>
    {
        public static FLabel[] Init(VoidSpawnGraphics spawnGraf)
        {
            var labels = LabelsFromLetters("VoidSpawn");
            
            var scale = spawnGraf.spawn.bodyChunks.Sum(x => x.rad) * 1.75f / TextWidth(string.Join("", labels.Select(x => x.text)));
            for (int i = 0; i < labels.Length; i++)
            {
                labels[i].scale = scale;
                labels[i].color = RainWorld.SaturatedGold;
            }
            
            return labels;
        }

        public static void Draw(VoidSpawnGraphics spawnGraf, FLabel[] labels, RoomCamera.SpriteLeaser sLeaser, float timeStacker, Vector2 camPos)
        {
            var chunks = spawnGraf.spawn.bodyChunks;
            for (int i = 0; i < labels.Length; i++)
            {
                labels[i].SetPosition(PointAlongChunks(i, labels.Length, chunks, timeStacker) - camPos);
                // var index = Custom.LerpMap(i, 0, labels.Length - 1, 0, chunks.Length - 1);
                // labels[i].rotation = AngleBtwnChunks(chunks[Mathf.FloorToInt(index)], chunks[Mathf.CeilToInt(index)], timeStacker);

                labels[i].alpha = 1 - spawnGraf.AlphaFromGlowDist(labels[i].GetPosition(), spawnGraf.glowPos);
            }
            sLeaser.sprites[spawnGraf.GlowSprite].isVisible = true;
            if (spawnGraf.hasOwnGoldEffect)
                sLeaser.sprites[spawnGraf.EffectSprite].isVisible = true;
        }
    }
}
