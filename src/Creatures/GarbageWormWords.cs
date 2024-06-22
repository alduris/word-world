using UnityEngine;
using static WordWorld.WordUtil;

namespace WordWorld.Creatures
{
    public class GarbageWormWords : CreatureWordify<GarbageWormGraphics>
    {
        public override void Init(RoomCamera.SpriteLeaser sLeaser)
        {
            labels.AddRange(LabelsFromLetters(Type.value));

            foreach (var label in labels)
            {
                label.scale = 1.25f;
                label.color = sLeaser.sprites[1].color;
                label.isVisible = Drawable.worm.extended > 0f;
            }
        }

        public override void Draw(RoomCamera.SpriteLeaser sLeaser, float timeStacker, Vector2 camPos)
        {
            // Place letters along body
            for (int i = 0; i < labels.Count; i++)
            {
                labels[i].SetPosition(PointAlongTentacle(labels.Count - i - 1, labels.Count, Drawable.worm.tentacle, timeStacker) - camPos);
                labels[i].isVisible = Drawable.worm.extended > 0f;
            }

            // Set first letter to eye color
            labels[0].color = sLeaser.sprites[0].color;
        }
    }
}
