using System.Linq;
using MoreSlugcats;
using UnityEngine;
using static WordWorld.WordUtil;

namespace WordWorld.Creatures
{
    public static class PlayerWords
    {
        public static FLabel[] Init(PlayerGraphics playerGraf, CreatureTemplate.Type type)
        {
            var str = Unpascal(type);
            if (ModManager.MSC && type == MoreSlugcatsEnums.CreatureTemplateType.SlugNPC)
                str = "Slugpup";

            var label = new FLabel(Font, str)
            {
                scale = (playerGraf.player.bodyChunks.Sum(x => x.rad) + playerGraf.player.bodyChunkConnections[0].distance) / TextWidth(str),
                color = playerGraf.player.isNPC ? playerGraf.player.ShortCutColor() : PlayerGraphics.SlugcatColor(playerGraf.CharacterForColor)
            };
            return [label];
        }

        public static void Draw(PlayerGraphics playerGraf, FLabel[] labels, float timeStacker, Vector2 camPos)
        {
            labels[0].SetPosition(AvgBodyChunkPos(playerGraf.player.bodyChunks[0], playerGraf.player.bodyChunks[1], timeStacker) - camPos);
            labels[0].rotation = AngleBtwnChunks(playerGraf.player.bodyChunks[0], playerGraf.player.bodyChunks[1], timeStacker) + 90f;
        }
    }
}
