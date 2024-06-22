using UnityEngine;
using WordWorld.Defaults;

namespace WordWorld.Items
{
    public class DataPearlWords : Wordify<DataPearl>
    {
        public static FLabel[] Init(DataPearl pearl) => POWords.Init(pearl, "P");

        public static void Draw(DataPearl pearl, FLabel[] labels, float timeStacker, Vector2 camPos)
        {
            POWords.Draw(pearl, labels, timeStacker, camPos);
            labels[0].color = Color.Lerp(pearl.color, pearl.highlightColor ?? pearl.color, Mathf.Lerp(pearl.lastGlimmer, pearl.glimmer, timeStacker));
        }
    }
}
