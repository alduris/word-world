using System.Linq;
using UnityEngine;
using static WordWorld.WordUtil;

namespace WordWorld.Creatures
{
    public class MouseWords : CreatureWordify<MouseGraphics>
    {
        private FLabel label;

        public override void Init(RoomCamera.SpriteLeaser sLeaser)
        {
            string text = Type == CreatureTemplate.Type.LanternMouse ? "Mouse" : Unpascal(Type);
            label = new FLabel(Font, text)
            {
                scale = (Drawable.mouse.bodyChunks.Sum(x => x.rad) + Drawable.mouse.bodyChunkConnections[0].distance) / TextWidth(text)
            };
        }

        public override void Draw(RoomCamera.SpriteLeaser sLeaser, float timeStacker, Vector2 camPos)
        {
            label.SetPosition(AvgBodyChunkPos(Drawable.mouse.bodyChunks[0], Drawable.mouse.bodyChunks[1], timeStacker) - camPos);
            label.rotation = AngleBtwnParts(Drawable.head, Drawable.tail, timeStacker) + 90f;
            label.color = Drawable.BodyColor;
        }
    }
}
