using MoreSlugcats;
using UnityEngine;
using static WordWorld.WordUtil;

namespace WordWorld.Items
{
    public class FireEggWords : Wordify<FireEgg>
    {
        private FLabel label;

        public override void Init(RoomCamera.SpriteLeaser sLeaser)
        {
            label = new FLabel(Font, "Egg") { scale = Drawable.firstChunk.rad * 3f / TextWidth("Egg"), color = Drawable.eggColors[1] };
            labels.Add(label);
        }

        public override void Draw(RoomCamera.SpriteLeaser sLeaser, float timeStacker, Vector2 camPos)
        {
            label.SetPosition(GetPos(Drawable.firstChunk, timeStacker) - camPos);
            label.scale = Drawable.firstChunk.rad * 3f / TextWidth(labels[0].text);
            label.color = Drawable.blink > 1 ? Drawable.blinkColor : sLeaser.sprites[1].color;
        }
    }
}
