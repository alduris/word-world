using System;
using System.Linq;
using UnityEngine;
using Watcher;
using static WordWorld.WordUtil;

namespace WordWorld.Creatures.Watcher
{
    public class DrillCrabWords : CreatureWordify<DrillCrabGraphics>
    {
        private DrillCrab crab => Critter as DrillCrab;

        public override void Init(RoomCamera.SpriteLeaser sLeaser)
        {
            var text = Unpascal(Type);
            labels.Add(new FLabel(Font, text)
            {
                scale = (Critter.bodyChunks.Sum(x => x.rad) * 2f) / TextWidth(text),
                color = sLeaser.sprites[Drawable.firstDrillSprite].color
            });
        }

        public override void Draw(RoomCamera.SpriteLeaser sLeaser, float timeStacker, Vector2 camPos)
        {
            labels[0].SetPosition(AvgBodyChunkPos(crab.bodyChunks[0], crab.bodyChunks[1], timeStacker) - camPos);
            labels[0].rotation = AngleBtwnChunks(crab.bodyChunks[0], crab.bodyChunks[1], timeStacker) - 90f;
        }
    }
}
