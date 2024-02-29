using MoreSlugcats;
using UnityEngine;
using WordWorld.Defaults;
using static WordWorld.WordUtil;

namespace WordWorld.Items
{
    public static class EnergyCellWords
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
            sLeaser.sprites[2].isVisible = true;
            sLeaser.sprites[3].isVisible = true;
            sLeaser.sprites[4].isVisible = true;
        }
    }
}
