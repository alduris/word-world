using UnityEngine;
using WordWorld.Defaults;
using static WordWorld.WordUtil;

namespace WordWorld.Items
{
    public class OracleSwarmerWords : Wordify<OracleSwarmer>
    {
        public static FLabel[] Init(OracleSwarmer neuron)
        {
            var labels = POWords.Init(neuron, "N");
            labels[0].scale *= 1.5f;
            return labels;
        }

        public static void Draw(OracleSwarmer neuron, FLabel[] labels, RoomCamera.SpriteLeaser sLeaser, float timeStacker, Vector2 camPos)
        {
            POWords.Draw(neuron, labels, timeStacker, camPos);
            labels[0].color = sLeaser.sprites[0].color;
        }
    }
}
