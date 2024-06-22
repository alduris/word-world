using UnityEngine;
using static WordWorld.WordUtil;

namespace WordWorld.Creatures
{
    public class GarbageWormWords : Wordify<GarbageWorm>
    {
        public static FLabel[] Init(GarbageWormGraphics wormGraf, CreatureTemplate.Type type, RoomCamera.SpriteLeaser spriteLeaser)
        {
            var labels = LabelsFromLetters(type.value);

            foreach (var label in labels)
            {
                label.scale = 1.25f;
                label.color = spriteLeaser.sprites[1].color;
                label.isVisible = wormGraf.worm.extended > 0f;
            }
            return labels;
        }

        public static void Draw(GarbageWormGraphics wormGraf, FLabel[] labels, RoomCamera.SpriteLeaser sLeaser, float timeStacker, Vector2 camPos)
        {
            // Place letters along body
            for (int i = 0; i < labels.Length; i++)
            {
                labels[i].SetPosition(PointAlongTentacle(labels.Length - i - 1, labels.Length, wormGraf.worm.tentacle, timeStacker) - camPos);
                labels[i].isVisible = wormGraf.worm.extended > 0f;
            }

            // Set first letter to eye color
            labels[0].color = sLeaser.sprites[0].color;
        }
    }
}
