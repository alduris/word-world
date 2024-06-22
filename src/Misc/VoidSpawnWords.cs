using System.Linq;
using RWCustom;
using UnityEngine;
using static WordWorld.WordUtil;

namespace WordWorld.Misc
{
    public class VoidSpawnWords : Wordify<VoidSpawnGraphics>
    {
        public override void Init(RoomCamera.SpriteLeaser sLeaser)
        {
            labels.AddRange(LabelsFromLetters("VoidSpawn"));
            
            var scale = Drawable.spawn.bodyChunks.Sum(x => x.rad) * 1.75f / TextWidth(string.Join("", labels.Select(x => x.text)));
            for (int i = 0; i < labels.Count; i++)
            {
                labels[i].scale = scale;
                labels[i].color = RainWorld.SaturatedGold;
            }
        }

        public override void Draw(RoomCamera.SpriteLeaser sLeaser, float timeStacker, Vector2 camPos)
        {
            var chunks = Drawable.spawn.bodyChunks;
            for (int i = 0; i < labels.Count; i++)
            {
                labels[i].SetPosition(PointAlongChunks(i, labels.Count, chunks, timeStacker) - camPos);

                labels[i].alpha = 1 - Drawable.AlphaFromGlowDist(labels[i].GetPosition(), Drawable.glowPos);
            }
            sLeaser.sprites[Drawable.GlowSprite].isVisible = true;
            if (Drawable.hasOwnGoldEffect)
                sLeaser.sprites[Drawable.EffectSprite].isVisible = true;
        }
    }
}
