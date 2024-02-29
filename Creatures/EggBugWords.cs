using System.Collections.Generic;
using MoreSlugcats;
using UnityEngine;
using static WordWorld.WordUtil;

namespace WordWorld.Creatures
{
    public static class EggBugWords
    {
        public static FLabel[] Init(EggBugGraphics eggBugGraf, CreatureTemplate.Type type)
        {
            string str;
            if (type == CreatureTemplate.Type.EggBug) str = "Eggbug";
            else if (ModManager.MSC && type == MoreSlugcatsEnums.CreatureTemplateType.FireBug) str = "Firebug";
            else str = Unpascal(type);

            List<FLabel> labels = [new(Font, str) {
                scale = (eggBugGraf.bug.bodyChunks[0].rad + eggBugGraf.bug.bodyChunks[1].rad + eggBugGraf.bug.bodyChunkConnections[0].distance) * 1.75f / TextWidth(str),
                color = eggBugGraf.blackColor
            }];

            // Eggs
            for (int i = 0; i < 6; i++)
            {
                labels.Add(new(Font, "Egg")
                {
                    scale = eggBugGraf.eggs[i / 3, i % 2].rad * 3f / TextWidth("Egg"),
                    color = eggBugGraf.eggColors[1]
                });
            }

            return [.. labels];
        }

        public static void Draw(EggBugGraphics eggBugGraf, FLabel[] labels, RoomCamera.SpriteLeaser sLeaser, float timeStacker, Vector2 camPos)
        {
            // Body
            labels[0].SetPosition(GetPos(eggBugGraf.bug.bodyChunks[1], timeStacker) - camPos);
            labels[0].rotation = FixRotation(sLeaser.sprites[eggBugGraf.HeadSprite].rotation) + 90f;

            // Eggs
            for (int i = 0; i < 6; i++)
            {
                var eggSprite = sLeaser.sprites[eggBugGraf.BackEggSprite(i % 2, i / 2, 2)];
                labels[i + 1].x = eggSprite.x;
                labels[i + 1].y = eggSprite.y;
                labels[i + 1].rotation = eggSprite.rotation;
                if (eggBugGraf.bug.FireBug && i >= eggBugGraf.bug.eggsLeft) labels[i + 1].isVisible = false;
            }
        }
    }
}
