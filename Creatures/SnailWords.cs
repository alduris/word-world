using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static WordWorld.WordUtil;

namespace WordWorld.Creatures
{
    public static class SnailWords
    {
        /*
         * Hi viewers of this class! This one is a bit of a doozie because snails have 2 colors and I wanted to be able to represent that on one word.
         * So my solution was to split the letters of the word and manually move and line up each one. However, I also have to keep compatibility with
         * mods that have custom snails, which you can imagine is a bit... not fun. The multiline support is really the big thing making the code below
         * so complicated.
         */

        public static FLabel[] Init(SnailGraphics snailGraf, CreatureTemplate.Type type)
        {
            var labels = new List<FLabel>();
            
            // Compute how big the letters have to be
            var snailWidth = snailGraf.snail.bodyChunks.Sum(x => x.rad) + snailGraf.snail.bodyChunkConnections.Sum(x => x.distance);
            var words = PascalRegex.Split(type.value).Where(x => x.Length > 0);
            var maxWidth = words.Max(TextWidth);
            var fontSize = snailWidth * 2f / maxWidth;

            foreach (var word in words)
            {
                // Precompute some math for the whole word
                var thisWidth = TextWidth(word);
                var invlerpMax = thisWidth - TextWidth(word[word.Length - 1].ToString());

                for (int i = 0; i < word.Length; i++)
                {
                    // Figure out color
                    var blendAmt = Mathf.InverseLerp(0f, invlerpMax, (i == 0 ? 0f : TextWidth(word.Substring(0, i))) + (maxWidth - thisWidth));
                    var colorBlend = Color.Lerp(snailGraf.snail.shellColor[0], snailGraf.snail.shellColor[1], blendAmt);

                    // Create label
                    var label = new FLabel(Font, word[i].ToString())
                    {
                        scale = fontSize,
                        color = colorBlend
                    };
                    labels.Add(label);
                }
            }
            return [.. labels];
        }

        public static void Draw(SnailGraphics snailGraf, FLabel[] labels, RoomCamera.SpriteLeaser sLeaser, float timeStacker, Vector2 camPos)
        {
            // Get the words and calculate the angle/pos of the snail
            var words = PascalRegex.Split(snailGraf.snail.abstractCreature.creatureTemplate.type.value).Where(x => x.Length > 0).ToArray();

            var angle = AngleBtwnChunks(snailGraf.snail.bodyChunks[0], snailGraf.snail.bodyChunks[1], timeStacker); // + 90f; // readd +90f for head to end
            var snailPos = GetPos(snailGraf.snail.bodyChunks[1], timeStacker);
            
            int k = 0;
            for (int i = 0; i < words.Length; i++)
            {
                // Negative because positive = up and we want later words to be below if custom creature uses Snail as base
                var yPos = -FontSize * (i - (words.Length - 1f) / 2f);
                
                for (int j = 0; j < words[i].Length; j++, k++)
                {
                    var xPos = (TextWidth(words[i].Substring(0, j)) - TextWidth(words[i]) / 2f + TextWidth(words[i][j].ToString())) * labels[k].scale;
                    var angleOff = Mathf.Atan2(yPos, xPos);
                    var dist = Mathf.Sqrt(xPos * xPos + yPos * yPos);

                    // Compute the final position/rotation of the label
                    var pos = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad + angleOff), -Mathf.Sin(angle * Mathf.Deg2Rad + angleOff)) * dist + snailPos - camPos;
                    labels[k].SetPosition(pos);
                    labels[k].rotation = angle;
                }
            }
        }
    }
}
