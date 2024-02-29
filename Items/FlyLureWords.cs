using RWCustom;
using UnityEngine;
using static WordWorld.WordUtil;

namespace WordWorld.Items
{
    public static class FlyLureWords
    {
        private const string WORD = "Batnip";

        public static FLabel[] Init()
        {
            return LabelsFromLetters(WORD);
        }

        public static void Draw(FlyLure plant, FLabel[] labels, RoomCamera.SpriteLeaser sLeaser, float timeStacker, Vector2 camPos)
        {
            var stalk = plant.stalk;
            var plantSize = 0f;
            for (int i = 0; i < stalk.Length - 1; i++)
                plantSize += (Vector2.Lerp(stalk[i].lastPos, stalk[i].pos, timeStacker) - Vector2.Lerp(stalk[i + 1].lastPos, stalk[i + 1].pos, timeStacker)).magnitude;

            var textScale = plantSize / TextWidth(WORD) * 0.875f;
            var plantPos = GetPos(plant.firstChunk, timeStacker);

            for (int i = 0; i < labels.Length; i++)
            {
                var label = labels[i];
                label.scale = textScale;

                // Calculate angle
                var j = Custom.LerpMap(i, 0, labels.Length - 1, 0.25f, stalk.Length - 1.25f);
                var prevPart = stalk[Mathf.FloorToInt(j)];
                var nextPart = stalk[Mathf.CeilToInt(j)];
                var prev = Vector2.Lerp(prevPart.lastPos, prevPart.pos, timeStacker);
                var next = Vector2.Lerp(nextPart.lastPos, nextPart.pos, timeStacker);
                var angle = AngleBtwn(prev, next) - 90f;
                label.rotation = angle;

                // Calculate position
                var xPos = (TextWidth(WORD.Substring(0, i)) - TextWidth(WORD) / 2f + TextWidth(WORD[i].ToString()) / 2f) * textScale;
                var angleOff = xPos < 0 ? Mathf.PI : 0;
                var pos = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad + angleOff), -Mathf.Sin(angle * Mathf.Deg2Rad + angleOff)) * Mathf.Abs(xPos) + plantPos;
                label.SetPosition(pos - camPos);

                // Color
                label.color = plant.StalkColor(Mathf.InverseLerp(0f, labels.Length - 1, i));
            }
        }
    }
}
