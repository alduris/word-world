using RWCustom;
using UnityEngine;
using static WordWorld.WordUtil;

namespace WordWorld.Items
{
    public class SeedCobWords : Wordify<SeedCob>
    {
        public override void Init(RoomCamera.SpriteLeaser sLeaser)
        {
            labels.AddRange(LabelsFromLetters("Popcorn"));
        }

        public override void Draw(RoomCamera.SpriteLeaser sLeaser, float timeStacker, Vector2 camPos)
        {
            // Precompute stuff
            var pos1 = GetPos(Drawable.bodyChunks[0], timeStacker);
            var pos2 = GetPos(Drawable.bodyChunks[1], timeStacker);
            var angle = AngleBtwn(pos1, pos2) - 90f;
            var scale = Vector2.Distance(pos1, pos2) / TextWidth("Popcorn");
            var shellColors = (sLeaser.sprites[Drawable.ShellSprite(1)] as TriangleMesh).verticeColors;

            // Parts of word
            for (int i = 0; i < labels.Count; i++)
            {
                var label = labels[i];
                var j = (int)Custom.LerpMap(i, 0, labels.Count - 1, Drawable.seedsPopped.Length - 1, 0);
                var k = (int)Custom.LerpMap(i, 0, labels.Count - 1, 0, shellColors.Length - 1);
                label.SetPosition(Vector2.Lerp(pos1, pos2, Mathf.InverseLerp(0, labels.Count - 1, i)) - camPos);
                label.rotation = angle;
                label.scale = scale;
                label.color = Drawable.seedsPopped[j] || Drawable.AbstractCob.dead ? Drawable.yellowColor : shellColors[k];
            }

            // Show stalk
            sLeaser.sprites[Drawable.StalkSprite(0)].isVisible = true;
            sLeaser.sprites[Drawable.StalkSprite(1)].isVisible = true;
        }
    }
}
