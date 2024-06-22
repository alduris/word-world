using UnityEngine;
using static WordWorld.WordUtil;

namespace WordWorld.Creatures
{
    public class DropBugWords : CreatureWordify<DropBugGraphics>
    {
        private FLabel label;

        public override void Init(RoomCamera.SpriteLeaser sLeaser)
        {
            label = new(Font, Type == CreatureTemplate.Type.DropBug ? "Dropwig" : Unpascal(Type))
            {
                scale = Drawable.bug.mainBodyChunk.rad * 3f / FontSize
            };
            labels.Add(label);
        }

        public override void Draw(RoomCamera.SpriteLeaser sLeaser, float timeStacker, Vector2 camPos)
        {
            label.SetPosition(GetPos(Drawable.bug.bodyChunks[1], timeStacker) - camPos);
            label.rotation = FixRotation(sLeaser.sprites[Drawable.HeadSprite].rotation) + 90f;
            label.color = Drawable.currSkinColor;
        }
    }
}
