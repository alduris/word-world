using UnityEngine;
using static WordWorld.WordUtil;

namespace WordWorld.Effects
{
    internal class GoldFlakeWords
    {
        private const string CharSelection = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        public static FLabel[] Init(GoldFlakes.GoldFlake flake, RoomCamera.SpriteLeaser sLeaser)
        {
            return [
                new FLabel(Font, CharSelection[Random.Range(0, CharSelection.Length - 1)].ToString())
                {
                    scale = Mathf.Lerp(0.35f, 0.65f, flake.scale),
                    color = sLeaser.sprites[0].color
                }
            ];
        }

        public static void Draw(GoldFlakes.GoldFlake flake, FLabel[] labels, RoomCamera.SpriteLeaser sLeaser, float timeStacker, Vector2 camPos)
        {
            labels[0].SetPosition(Vector2.Lerp(flake.lastPos, flake.pos, timeStacker) - camPos);
            labels[0].rotation = sLeaser.sprites[0].rotation;
            labels[0].isVisible = flake.active && !flake.reset;
            labels[0].color = sLeaser.sprites[0].color;
        }
    }
}
