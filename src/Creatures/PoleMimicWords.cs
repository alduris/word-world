using UnityEngine;
using static WordWorld.WordUtil;

namespace WordWorld.Creatures
{
    public class PoleMimicWords : CreatureWordify<PoleMimicGraphics>
    {
        public override void Init(RoomCamera.SpriteLeaser sLeaser)
        {
            labels.AddRange(LabelsFromLetters(Type.value));
            foreach (var label in labels)
            {
                label.scale = 1.5f;
            }
        }

        public override void Draw(RoomCamera.SpriteLeaser sLeaser, float timeStacker, Vector2 camPos)
        {
            float mimicAmt = Mathf.Lerp(Drawable.lastLookLikeAPole, Drawable.lookLikeAPole, timeStacker);

            // Move labels
            for (int i = 0; i < labels.Count; i++)
            {
                var label = labels[i];
                label.SetPosition(PointAlongTentacle(labels.Count - i, labels.Count + 1, Drawable.pole.tentacle, timeStacker) - camPos);

                int leafPair = Mathf.RoundToInt((1f - (float)i / labels.Count) * (Drawable.leafPairs - 1));
                label.color = Color.Lerp(Drawable.blackColor, Drawable.mimicColor, mimicAmt);
                if (leafPair < Drawable.decoratedLeafPairs)
                {
                    FSprite sprite = sLeaser.sprites[Drawable.LeafDecorationSprite(leafPair, 0)];
                    float revealAmt = Mathf.Lerp(Drawable.leavesMimic[leafPair, 0, 4], Drawable.leavesMimic[leafPair, 0, 3], timeStacker);
                    label.color = Color.Lerp(label.color, sprite.color, revealAmt);
                }
            }
        }
    }
}
