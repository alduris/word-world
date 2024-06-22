using UnityEngine;
using WordWorld.Defaults;
using static WordWorld.WordUtil;

namespace WordWorld.Items
{
    public class EggBugEggWords() : POWordify<EggBugEgg>("Egg")
    {
        public override void Init(RoomCamera.SpriteLeaser sLeaser)
        {
            base.Init(sLeaser);
            Label = new FLabel(Font, "Egg") { scale = Drawable.firstChunk.rad * 3f / TextWidth("Egg"), color = Drawable.eggColors[1] };
        }
    }
}
