using RWCustom;
using UnityEngine;
using static WordWorld.WordUtil;

namespace WordWorld.Items
{
    public class SeedCobWords : Wordify<SeedCob>
    {
        public static FLabel[] Init()
        {
            return LabelsFromLetters("Popcorn");
        }

        public static void Draw(SeedCob plant, FLabel[] labels, RoomCamera.SpriteLeaser sLeaser, float timeStacker, Vector2 camPos)
        {
            // Precompute stuff
            var pos1 = GetPos(plant.bodyChunks[0], timeStacker);
            var pos2 = GetPos(plant.bodyChunks[1], timeStacker);
            var angle = AngleBtwn(pos1, pos2) - 90f;
            var scale = Vector2.Distance(pos1, pos2) / TextWidth("Popcorn");
            var shellColors = (sLeaser.sprites[plant.ShellSprite(1)] as TriangleMesh).verticeColors;

            // Parts of word
            for (int i = 0; i < labels.Length; i++)
            {
                var label = labels[i];
                var j = (int)Custom.LerpMap(i, 0, labels.Length - 1, plant.seedsPopped.Length - 1, 0);
                var k = (int)Custom.LerpMap(i, 0, labels.Length - 1, 0, shellColors.Length - 1);
                label.SetPosition(Vector2.Lerp(pos1, pos2, Mathf.InverseLerp(0, labels.Length - 1, i)) - camPos);
                label.rotation = angle;
                label.scale = scale;
                label.color = plant.seedsPopped[j] || plant.AbstractCob.dead ? plant.yellowColor : shellColors[k];
            }

            // Show stalk
            sLeaser.sprites[plant.StalkSprite(0)].isVisible = true;
            sLeaser.sprites[plant.StalkSprite(1)].isVisible = true;
        }
    }
}
