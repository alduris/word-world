using UnityEngine;
using WordWorld.Defaults;
using static WordWorld.WordUtil;

namespace WordWorld.Items
{
    public class JokeRifleWords() : POWordify<JokeRifle>("Rifle")
    {
        public override void Init(RoomCamera.SpriteLeaser sLeaser)
        {
            Label = new FLabel(Font, text)
            {
                scale = sLeaser.sprites[0].element.sourcePixelSize.x / TextWidth(text),
                color = sLeaser.sprites[0].color
            };
        }

        public override void Draw(RoomCamera.SpriteLeaser sLeaser, float timeStacker, Vector2 camPos)
        {
            base.Draw(sLeaser, timeStacker, camPos);
            Label.rotation = FixRotation(sLeaser.sprites[0].rotation);
        }
    }
}
