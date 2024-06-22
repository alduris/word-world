using System;
using UnityEngine;
using WordWorld.Defaults;
using static WordWorld.WordUtil;

namespace WordWorld.Items
{
    public class SporePlantWords : Wordify<SporePlant>
    {
        public static FLabel[] Init(SporePlant hive) => POWords.Init(hive, $"Bee{Environment.NewLine}hive");

        public static void Draw(SporePlant hive, FLabel[] labels, float timeStacker, Vector2 camPos)
        {
            POWords.Draw(hive, labels, timeStacker, camPos);
            labels[0].color = Color.Lerp(hive.colorA, hive.colorB, hive.Pacified ? 0f : Mathf.Lerp(0.4f, 0.8f, hive.angry));
        }

        public static FLabel[] BeeInit()
        {
            return [new(Font, "B") { scale = 0.5f }];
        }
        public static void BeeDraw(SporePlant.Bee bee, FLabel[] labels, RoomCamera.SpriteLeaser sLeaser, float timeStacker, Vector2 camPos)
        {
            labels[0].SetPosition(Vector2.Lerp(bee.lastPos, bee.pos, timeStacker) - camPos);
            labels[0].color = bee.angry && bee.blinkFreq > 0f ? sLeaser.sprites[1].color : sLeaser.sprites[0].color;
        }

        public static FLabel[] AttachedBeeInit(RoomCamera.SpriteLeaser sLeaser)
        {
            return [new(Font, "B") { scale = 0.5f, color = sLeaser.sprites[0].color }];
        }
        public static void AttachedBeeDraw(SporePlant.AttachedBee bee, FLabel[] labels, RoomCamera.SpriteLeaser sLeaser, float timeStacker, Vector2 camPos)
        {
            POWords.Draw(bee, labels, timeStacker, camPos);
            
            if (bee.lastStingerOut || bee.stingerOut)
            {
                for (int i = 0; i < bee.stinger.GetLength(0); i++)
                {
                    sLeaser.sprites[i + 1].isVisible = true;
                }
            }
        }
    }
}
