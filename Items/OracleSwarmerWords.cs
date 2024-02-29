using UnityEngine;
using WordWorld.Defaults;
using static WordWorld.WordUtil;

namespace WordWorld.Items
{
    public static class OracleSwarmerWords
    {
        public static FLabel[] Init(OracleSwarmer neuron) => POWords.Init(neuron, "N");

        public static void Draw(OracleSwarmer neuron, FLabel[] labels, RoomCamera.SpriteLeaser sLeaser, float timeStacker, Vector2 camPos)
        {
            POWords.Draw(neuron, labels, timeStacker, camPos);
            labels[0].color = sLeaser.sprites[0].color;
        }
    }
}
