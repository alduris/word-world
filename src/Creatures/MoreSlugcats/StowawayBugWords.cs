using System.Collections.Generic;
using MoreSlugcats;
using UnityEngine;
using static WordWorld.WordUtil;

namespace WordWorld.Creatures.MoreSlugcats
{
    public class StowawayBugWords : CreatureWordify<StowawayBugGraphics>
    {
        public override void Init(RoomCamera.SpriteLeaser sLeaser)
        {
            // Main body
            labels.Add(
                new(Font, Type == MoreSlugcatsEnums.CreatureTemplateType.StowawayBug ? "Stowaway" : Unpascal(Type)) {
                    scale = (Drawable.myBug.bodyChunks[0].rad + Drawable.myBug.bodyChunks[1].rad) * 2f / FontSize,
                    color = Drawable.bodyColor
                }
            );

            // Tentacles
            for (int i = 0; i < Drawable.myBug.heads.Length; i++)
            {
                foreach (var c in "Tentacle")
                {
                    labels.Add(
                        new(Font, c.ToString()) {
                            scale = 1.125f,
                            color = sLeaser.sprites[Drawable.SpritesBegin_Mouth].color
                        }
                    );
                }
            }
        }

        public override void Draw(RoomCamera.SpriteLeaser sLeaser, float timeStacker, Vector2 camPos)
        {
            // Main body
            labels[0].SetPosition(GetPos(Drawable.myBug.bodyChunks[1], timeStacker) - camPos);
            labels[0].rotation = FixRotation(sLeaser.sprites[1].rotation);

            // Tentacles
            for (int i = 0; i < Drawable.myBug.heads.Length; i++)
            {
                var head = Drawable.myBug.heads[i];
                for (int j = 0; j < 8; j++)
                {
                    var label = labels[i * 8 + j + 1];
                    var pos = PointAlongTentacle(j + 1, 9, head, timeStacker);
                    var prevPos = PointAlongTentacle(j, 9, head, timeStacker);
                    label.SetPosition(pos - camPos);
                    label.rotation = AngleBtwn(pos, prevPos);
                    label.isVisible = Drawable.myBug.headFired[i];
                }
            }
        }
    }
}
