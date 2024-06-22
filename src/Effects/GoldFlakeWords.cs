using UnityEngine;
using static WordWorld.WordUtil;

namespace WordWorld.Effects
{
    internal class GoldFlakeWords : Wordify<GoldFlakes.GoldFlake>
    {
        private const string CharSelection = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        private FLabel label;

        public override void Init(RoomCamera.SpriteLeaser sLeaser)
        {
            label = new FLabel(Font, CharSelection[Random.Range(0, CharSelection.Length - 1)].ToString())
            {
                scale = Mathf.Lerp(0.35f, 0.65f, Drawable.scale),
                color = sLeaser.sprites[0].color
            };
            labels.Add(label);
        }

        public override void Draw(RoomCamera.SpriteLeaser sLeaser, float timeStacker, Vector2 camPos)
        {
            label.SetPosition(Vector2.Lerp(Drawable.lastPos, Drawable.pos, timeStacker) - camPos);
            label.rotation = sLeaser.sprites[0].rotation;
            label.isVisible = Drawable.active && !Drawable.reset;
            label.color = sLeaser.sprites[0].color;
        }
    }
}
