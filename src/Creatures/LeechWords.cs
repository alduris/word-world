using UnityEngine;
using static WordWorld.WordUtil;

namespace WordWorld.Creatures
{
    public class LeechWords : CreatureWordify<LeechGraphics>
    {
        private FLabel label;

        public override void Init(RoomCamera.SpriteLeaser sLeaser)
        {
            label = new FLabel(Font, PascalRegex.Replace(Type.value, " "))
            {
                scale = Drawable.leech.mainBodyChunk.rad * 4f / FontSize
            };
            labels.Add(label);
        }

        public override void Draw(RoomCamera.SpriteLeaser sLeaser, float timeStacker, Vector2 camPos)
        {
            label.color = sLeaser.sprites[0].color;
            label.SetPosition(GetPos(Drawable.leech.mainBodyChunk, timeStacker) - camPos);
            label.rotation = AngleBtwnParts(Drawable.body[0], Drawable.body[Drawable.body.Length - 1], timeStacker) + 90f;
        }
    }
}
