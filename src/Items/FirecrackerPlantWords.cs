using RWCustom;
using UnityEngine;
using static WordWorld.WordUtil;

namespace WordWorld.Items
{
    public class FirecrackerPlantWords : Wordify<FirecrackerPlant>
    {
        private readonly string word = "Cherrybomb";

        public override void Init(RoomCamera.SpriteLeaser sLeaser)
        {
            labels.AddRange(LabelsFromLetters(word));
        }

        public override void Draw(RoomCamera.SpriteLeaser sLeaser, float timeStacker, Vector2 camPos)
        {
            var stalk = Drawable.stalk;
            var plantSize = 0f;
            for (int i = 0; i < stalk.Length - 1; i++)
                plantSize += (Vector2.Lerp(stalk[i].lastPos, stalk[i].pos, timeStacker) - Vector2.Lerp(stalk[i + 1].lastPos, stalk[i + 1].pos, timeStacker)).magnitude;

            var textScale = plantSize / TextWidth(word);
            var plantPos = GetPos(Drawable.firstChunk, timeStacker);

            for (int i = 0; i < labels.Count; i++)
            {
                var label = labels[i];
                label.scale = textScale;

                // Calculate angle
                var j = Custom.LerpMap(i, 0, labels.Count - 1, 0.25f, stalk.Length - 1.25f);
                var prevPart = stalk[Mathf.FloorToInt(j)];
                var nextPart = stalk[Mathf.CeilToInt(j)];
                var prev = Vector2.Lerp(prevPart.lastPos, prevPart.pos, timeStacker);
                var next = Vector2.Lerp(nextPart.lastPos, nextPart.pos, timeStacker);
                var angle = AngleBtwn(prev, next) - 90f;
                label.rotation = angle;

                // Calculate position
                var xPos = (TextWidth(word.Substring(0, i)) - TextWidth(word) / 2f + TextWidth(word[i].ToString()) / 2f) * textScale;
                var angleOff = xPos < 0 ? Mathf.PI : 0;
                var pos = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad + angleOff), -Mathf.Sin(angle * Mathf.Deg2Rad + angleOff)) * Mathf.Abs(xPos) + plantPos;
                label.SetPosition(pos - camPos);

                // Color
                var k = Mathf.RoundToInt(Custom.LerpMap(i, 0, labels.Count - 1, 0, Drawable.lumps.Length - 1));
                label.color = Drawable.lumpsPopped[k] ? Drawable.color : sLeaser.sprites[Drawable.LumpSprite(k, 1)].color;
            }
        }
    }
}
