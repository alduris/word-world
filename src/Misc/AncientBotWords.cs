using MoreSlugcats;
using UnityEngine;
using static WordWorld.WordUtil;

namespace WordWorld.Misc
{
    public static class AncientBotWords
    {
        public static FLabel[] Init()
        {
            return [new FLabel(Font, "Bot")];
        }

        public static void Draw(AncientBot bot, FLabel[] labels, RoomCamera.SpriteLeaser sLeaser, float timeStacker, Vector2 camPos)
        {
            var label = labels[0];
            label.SetPosition(Vector2.Lerp(bot.lastPos, bot.pos, timeStacker) - camPos);
            label.scale = (bot.Rad / 2.5f) / FontSize;
            label.color = bot.lightOn && bot.flicker <= 0 ? bot.color : sLeaser.sprites[bot.BodyIndex].color;
            if (label.color.b < 0.01f) label.color = new Color(0.01f, 0.01f, 0.01f);
        }
    }
}
