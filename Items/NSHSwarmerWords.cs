using RWCustom;
using UnityEngine;
using WordWorld.Defaults;
using static WordWorld.WordUtil;

namespace WordWorld.Items
{
    public static class NSHSwarmerWords // why doesn't NSHSwarmer extend OracleSwarmer :(
    {
        public static FLabel[] Init(NSHSwarmer neuron)
        {
            var labels = POWords.Init(neuron, "N");
            labels[0].color = neuron.myColor;
            return labels;
        }

        public static void Draw(NSHSwarmer neuron, FLabel[] labels, RoomCamera.SpriteLeaser sLeaser, float timeStacker, Vector2 camPos)
        {
            labels[0].SetPosition(GetPos(neuron.firstChunk, timeStacker) - camPos);

            // Show the hologram thingy
            var active = Custom.SCurve(Mathf.Lerp(neuron.lastHoloFade, neuron.holoFade, timeStacker), 0.65f) * neuron.holoShape.Fade.SmoothValue(timeStacker);
            if (active > 0)
            {
                for (int i = 5; i < sLeaser.sprites.Length; i++)
                {
                    sLeaser.sprites[i].isVisible = true;
                }
            }
        }
    }
}
