using MoreSlugcats;
using UnityEngine;
using static WordWorld.WordUtil;

namespace WordWorld.Items
{
    public class MoonCloakWords : Wordify<MoonCloak>
    {
        private FLabel label;

        public override void Init(RoomCamera.SpriteLeaser sLeaser)
        {
            label = new FLabel(Font, "Cloak")
            {
                scale = Drawable.firstChunk.rad * 2f / FontSize,
                color = Drawable.Color(0.25f)
            };
            labels.Add(label);
        }

        public override void Draw(RoomCamera.SpriteLeaser sLeaser, float timeStacker, Vector2 camPos)
        {
            label.SetPosition(AvgBodyChunkPos(Drawable.bodyChunks[0], Drawable.bodyChunks[1], timeStacker) - camPos);
            label.rotation = AngleBtwnChunks(Drawable.bodyChunks[0], Drawable.bodyChunks[1], timeStacker);
        }
    }
}
