using System.Linq;
using UnityEngine;
using static WordWorld.WordUtil;

namespace WordWorld.Creatures
{
    public class VultureGrubWords : CreatureWordify<VultureGrubGraphics>
    {
        public override void Init(RoomCamera.SpriteLeaser sLeaser)
        {
            var text = Unpascal(Type);
            var label = new FLabel(Font, text)
            {
                scale = Drawable.worm.bodyChunks.Sum(x => x.rad) * 3f / TextWidth(text)
            };
            labels.Add(label);
            // TODO: should I turn the laser into words as well?
        }

        public override void Draw(RoomCamera.SpriteLeaser sLeaser, float timeStacker, Vector2 camPos)
        {
            // Body
            labels[0].SetPosition(GetPos(Drawable.worm.bodyChunks[0], timeStacker) - camPos);
            labels[0].rotation = FixRotation(AngleBtwnChunks(Drawable.worm.bodyChunks[1], Drawable.worm.bodyChunks[2], timeStacker)) - 90f;
            labels[0].color = sLeaser.sprites[Drawable.MeshSprite].color;

            // Show laser sprite
            sLeaser.sprites[Drawable.LaserSprite].isVisible = Mathf.Lerp(Drawable.lastLaserActive, Drawable.laserActive, timeStacker) > 0f;
        }
    }
}
