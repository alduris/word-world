using MoreSlugcats;
using UnityEngine;
using WordWorld.Defaults;
using static WordWorld.WordUtil;

namespace WordWorld.Items
{
    public class EnergyCellWords : Wordify<EnergyCell>
    {
        public static FLabel[] Init()
        {
            return [new FLabel(Font, "Cell")];
        }

        public static void Draw(EnergyCell cell, FLabel[] labels, RoomCamera.SpriteLeaser sLeaser, float timeStacker, Vector2 camPos)
        {
            POWords.Draw(cell, labels, timeStacker, camPos);

            var label = labels[0];
            label.scale = cell.scale * 2f / TextWidth("Cell");
            label.color = sLeaser.sprites[2].color;
            if (label.color.b < 0.01f) label.color = new Color(0.01f, 0.01f, 0.01f); // prevents disappearing for some reason
            
            sLeaser.sprites[2].isVisible = true;
            sLeaser.sprites[3].isVisible = true;
            sLeaser.sprites[4].isVisible = true;
        }
    }
}
