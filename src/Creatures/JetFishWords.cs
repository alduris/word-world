using UnityEngine;
using static WordWorld.WordUtil;

namespace WordWorld.Creatures
{
    public class JetFishWords : CreatureWordify<JetFishGraphics>
    {
        private FLabel label;

        public override void Init(RoomCamera.SpriteLeaser sLeaser)
        {
            label = new FLabel(Font, Type == CreatureTemplate.Type.JetFish ? "Jetfish" : Unpascal(Type))
            {
                scale = Drawable.fish.bodyChunks[0].rad * 2.5f / FontSize,
                color = sLeaser.sprites[Drawable.BodySprite].color
            };
            labels.Add(label);
        }

        public override void Draw(RoomCamera.SpriteLeaser sLeaser, float timeStacker, Vector2 camPos)
        {
            label.SetPosition(AvgBodyChunkPos(Drawable.fish.bodyChunks[0], Drawable.fish.bodyChunks[1], timeStacker) - camPos);
            label.rotation = FixRotation(sLeaser.sprites[Drawable.BodySprite].rotation + 90f);
        }
    }
}
