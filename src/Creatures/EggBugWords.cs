using System.Collections.Generic;
using MoreSlugcats;
using UnityEngine;
using static WordWorld.WordUtil;

namespace WordWorld.Creatures
{
    public class EggBugWords : CreatureWordify<EggBugGraphics>
    {
        private FLabel bodyLabel;
        private FLabel[,] eggLabels;
        private bool Firebug => (Critter as EggBug).FireBug;

        public override void Init(RoomCamera.SpriteLeaser sLeaser)
        {
            string str;
            if (Type == CreatureTemplate.Type.EggBug) str = "Eggbug";
            else if (ModManager.MSC && Type == MoreSlugcatsEnums.CreatureTemplateType.FireBug) str = "Firebug";
            else str = Unpascal(Type);

            bodyLabel = new(Font, str)
            {
                scale = (Drawable.bug.bodyChunks[0].rad + Drawable.bug.bodyChunks[1].rad + Drawable.bug.bodyChunkConnections[0].distance) * 1.75f / TextWidth(str),
                color = Drawable.blackColor
            };
            labels.Add(bodyLabel);

            // Eggs
            var eggs = Drawable.eggs;
            int a = eggs.GetLength(0), b = eggs.GetLength(1);
            eggLabels = new FLabel[a, b];
            for (int i = 0; i < eggs.Length; i++)
            {
                // Ignore how cursed this is, you can't enumerate over a multidimensional array
                labels.Add(
                    eggLabels[i / a, i % b] = new(Font, "Egg")
                    {
                        scale = eggs[i / a, i % b].rad * 3f / TextWidth("Egg"),
                        color = Drawable.eggColors[1]
                    }
                );
            }
        }

        public override void Draw(RoomCamera.SpriteLeaser sLeaser, float timeStacker, Vector2 camPos)
        {
            // Body
            bodyLabel.SetPosition(GetPos(Drawable.bug.bodyChunks[1], timeStacker) - camPos);
            bodyLabel.rotation = FixRotation(sLeaser.sprites[Drawable.HeadSprite].rotation) + 90f;

            // Eggs
            for (int i = 0; i < eggLabels.GetLength(0); i++)
            {
                for (int j = 0; j < eggLabels.GetLength(1); j++)
                {
                    var eggSprite = sLeaser.sprites[Drawable.BackEggSprite(j, i, 2)];
                    var label = eggLabels[i, j];
                    label.x = eggSprite.x;
                    label.y = eggSprite.y;
                    label.rotation = eggSprite.rotation;
                    label.isVisible = eggSprite.isVisible && !(Firebug && i >= Drawable.bug.eggsLeft);
                }
            }
        }
    }
}
