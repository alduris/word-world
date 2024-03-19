using UnityEngine;
using static WordWorld.WordUtil;

namespace WordWorld.Creatures
{
    public static class PoleMimicWords
    {
        public static FLabel[] Init(PoleMimicGraphics poleMimicGraf, CreatureTemplate.Type type)
        {
            // TODO: add extension trick like in centis and dlls with pole mimics, so that when hiding they are all l's or something to make a line and reveal letters sometimes
            var labels = LabelsFromLetters(type.value);
            foreach (var label in labels)
            {
                label.scale = 1.5f;
            }
            return labels;
        }

        public static void Draw(PoleMimicGraphics poleMimicGraf, FLabel[] labels, RoomCamera.SpriteLeaser sLeaser, float timeStacker, Vector2 camPos)
        {
            float mimicAmt = Mathf.Lerp(poleMimicGraf.lastLookLikeAPole, poleMimicGraf.lookLikeAPole, timeStacker);
            // Move labels
            for (int i = 0; i < labels.Length; i++)
            {
                var label = labels[i];
                label.SetPosition(PointAlongTentacle(labels.Length - i, labels.Length + 1, poleMimicGraf.pole.tentacle, timeStacker) - camPos);

                int leafPair = Mathf.RoundToInt((1f - (float)i / labels.Length) * (poleMimicGraf.leafPairs - 1));
                label.color = Color.Lerp(poleMimicGraf.blackColor, poleMimicGraf.mimicColor, mimicAmt);
                if (leafPair < poleMimicGraf.decoratedLeafPairs)
                {
                    FSprite sprite = sLeaser.sprites[poleMimicGraf.LeafDecorationSprite(leafPair, 0)];
                    float revealAmt = Mathf.Lerp(poleMimicGraf.leavesMimic[leafPair, 0, 4], poleMimicGraf.leavesMimic[leafPair, 0, 3], timeStacker);
                    label.color = Color.Lerp(label.color, sprite.color, revealAmt);
                }
            }
        }
    }
}
