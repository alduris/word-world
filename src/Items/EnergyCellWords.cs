using MoreSlugcats;
using UnityEngine;
using WordWorld.Defaults;
using static WordWorld.WordUtil;

namespace WordWorld.Items
{
    public class EnergyCellWords() : POWordify<EnergyCell>("Cell")
    {
        private float textWidth = 1f;

        public override void Init(RoomCamera.SpriteLeaser sLeaser)
        {
            base.Init(sLeaser);
            textWidth = TextWidth(text);
        }

        public override void Draw(RoomCamera.SpriteLeaser sLeaser, float timeStacker, Vector2 camPos)
        {
            base.Draw(sLeaser, timeStacker, camPos);

            var label = labels[0];
            label.scale = Drawable.scale * 2f / textWidth;
            label.color = sLeaser.sprites[2].color;
            if (label.color.b < 0.01f) label.color = new Color(0.01f, 0.01f, 0.01f); // prevents disappearing for some reason
            
            sLeaser.sprites[2].isVisible = true;
            sLeaser.sprites[3].isVisible = true;
            sLeaser.sprites[4].isVisible = true;
        }

        protected override void AddToContainer(RoomCamera rCam, RoomCamera.SpriteLeaser sLeaser, FContainer container)
        {
            base.AddToContainer(rCam, sLeaser, rCam.ReturnFContainer("Items"));
        }
    }
}
