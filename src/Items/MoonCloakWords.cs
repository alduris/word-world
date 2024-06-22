using MoreSlugcats;
using UnityEngine;
using static WordWorld.WordUtil;

namespace WordWorld.Items
{
    public class MoonCloakWords : Wordify<MoonCloak>
    {
        public static FLabel[] Init(MoonCloak cloak)
        {
            var label = new FLabel(Font, "Cloak")
            {
                scale = cloak.firstChunk.rad * 2f / FontSize,
                color = cloak.Color(0.25f)
            };
            return [label];
        }

        public static void Draw(MoonCloak cloak, FLabel[] labels, float timeStacker, Vector2 camPos)
        {
            labels[0].SetPosition(AvgBodyChunkPos(cloak.bodyChunks[0], cloak.bodyChunks[1], timeStacker) - camPos);
            labels[0].rotation = AngleBtwnChunks(cloak.bodyChunks[0], cloak.bodyChunks[1], timeStacker);
        }
    }
}
