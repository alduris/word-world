using System.Linq;
using UnityEngine;
using static WordWorld.WordUtil;

namespace WordWorld.Creatures
{
    public class MouseWords : Wordify<Mouse>
    {
        public static FLabel[] Init(MouseGraphics mouseGraf, CreatureTemplate.Type type)
        {
            var text = type == CreatureTemplate.Type.LanternMouse ? "Mouse" : Unpascal(type);
            return [new FLabel(Font, text) {
                scale = (mouseGraf.mouse.bodyChunks.Sum(x => x.rad) + mouseGraf.mouse.bodyChunkConnections[0].distance) / TextWidth(text)
            }];
        }

        public static void Draw(MouseGraphics mouseGraf, FLabel[] labels, float timeStacker, Vector2 camPos)
        {
            labels[0].SetPosition(AvgBodyChunkPos(mouseGraf.mouse.bodyChunks[0], mouseGraf.mouse.bodyChunks[1], timeStacker) - camPos);
            labels[0].rotation = AngleBtwnParts(mouseGraf.head, mouseGraf.tail, timeStacker) + 90f;
            labels[0].color = mouseGraf.BodyColor;
        }
    }
}
