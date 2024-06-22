using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static WordWorld.WordUtil;

namespace WordWorld.Creatures
{
    public class SnailWords : CreatureWordify<SnailGraphics>
    {
        /*
         * Hi viewers of this class! This one is a bit of a doozie because snails have 2 colors and I wanted to be able to represent that on one word.
         * So my solution was to split the letters of the word and manually move and line up each one. However, I also have to keep compatibility with
         * mods that have custom snails, which you can imagine is a bit... not fun. The multiline support is really the big thing making the code below
         * so complicated.
         */

        private Snail Snail => Critter as Snail;
        private List<List<FLabel>> lines;
        private List<string> words;

        public override void Init(RoomCamera.SpriteLeaser sLeaser)
        {
            // Compute how big the letters have to be
            words = PascalRegex.Split(Type.value).Where(x => x.Length > 0).ToList();
            var snailWidth = Snail.bodyChunks.Sum(x => x.rad) + Snail.bodyChunkConnections.Sum(x => x.distance);
            var maxWidth = words.Max(TextWidth);
            var fontSize = snailWidth * 2f / maxWidth;

            foreach (var word in words)
            {
                // Precompute some math for the whole word
                var thisWidth = TextWidth(word);
                var invlerpMax = thisWidth - TextWidth(word[word.Length - 1].ToString());

                List<FLabel> line = [];
                for (int i = 0; i < word.Length; i++)
                {
                    // Figure out color
                    var blendAmt = Mathf.InverseLerp(0f, invlerpMax, (i == 0 ? 0f : TextWidth(word.Substring(0, i))) + (maxWidth - thisWidth));
                    var colorBlend = Color.Lerp(Snail.shellColor[0], Snail.shellColor[1], blendAmt);

                    // Create label
                    var label = new FLabel(Font, word[i].ToString())
                    {
                        scale = fontSize,
                        color = colorBlend
                    };
                    line.Add(label);
                }
                lines.Add(line);
                labels.AddRange(line);
            }
        }

        public override void Draw(RoomCamera.SpriteLeaser sLeaser, float timeStacker, Vector2 camPos)
        {
            var angle = AngleBtwnChunks(Snail.bodyChunks[0], Snail.bodyChunks[1], timeStacker); // + 90f; // readd +90f for head to end
            var snailPos = GetPos(Snail.bodyChunks[1], timeStacker);
            
            for (int i = 0; i < lines.Count; i++)
            {
                var line = lines[i];
                
                var yPos = -FontSize * (i - (lines.Count - 1f) / 2f); // Negative because positive = up and we want later words to be below if custom creature uses Snail as base
                
                for (int j = 0; j < line.Count; j++)
                {
                    var xPos = (TextWidth(words[i].Substring(0, j)) - TextWidth(words[i]) / 2f + TextWidth(words[i][j].ToString())) * lines[i][j].scale;
                    var angleOff = Mathf.Atan2(yPos, xPos);
                    var dist = Mathf.Sqrt(xPos * xPos + yPos * yPos);

                    // Compute the final position/rotation of the label
                    var pos = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad + angleOff), -Mathf.Sin(angle * Mathf.Deg2Rad + angleOff)) * dist + snailPos - camPos;
                    lines[i][j].SetPosition(pos);
                    lines[i][j].rotation = angle;
                }
            }
        }
    }
}
