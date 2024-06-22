using MoreSlugcats;
using UnityEngine;
using static WordWorld.WordUtil;

namespace WordWorld.Misc
{
    public class AncientBotWords : Wordify<AncientBot>
    {
        private FLabel label;

        public override void Init(RoomCamera.SpriteLeaser sLeaser)
        {
            label = new FLabel(Font, "Bot");
            labels.Add(label);
        }

        public override void Draw(RoomCamera.SpriteLeaser sLeaser, float timeStacker, Vector2 camPos)
        {
            label.SetPosition(Vector2.Lerp(Drawable.lastPos, Drawable.pos, timeStacker) - camPos);
            label.scale = (Drawable.Rad / 2.5f) / FontSize;
            label.color = Drawable.lightOn && Drawable.flicker <= 0 ? Drawable.color : sLeaser.sprites[Drawable.BodyIndex].color;
            if (label.color.b < 0.01f) label.color = new Color(0.01f, 0.01f, 0.01f);
        }
    }
}
