using RWCustom;
using UnityEngine;
using static WordWorld.WordUtil;

namespace WordWorld.Creatures
{
    public class DeerWords : CreatureWordify<DeerGraphics>
    {
        private FLabel bodyLabel;
        private FLabel antlerLabel;

        private Deer MyDeer => Critter as Deer;
        private BodyChunk AntlerBodyChunk => MyDeer.bodyChunks[5];

        public override void Init(RoomCamera.SpriteLeaser sLeaser)
        {
            var bodyChunks = MyDeer.bodyChunks;
            bodyLabel = new FLabel(Font, Unpascal(Type))
            {
                scale = (bodyChunks[1].rad + bodyChunks[2].rad + bodyChunks[3].rad + bodyChunks[4].rad) / 2f / FontSize,
                color = Drawable.bodyColor
            };
            antlerLabel = new FLabel(Font, "Antlers")
            {
                scale = AntlerBodyChunk.rad / FontSize,
                color = Drawable.bodyColor
            };

            // TODO: legs

            labels.Add(bodyLabel);
            labels.Add(antlerLabel);
        }

        public override void Draw(RoomCamera.SpriteLeaser sLeaser, float timeStacker, Vector2 camPos)
        {
            var bodyChunks = MyDeer.bodyChunks;
            
            bodyLabel.SetPosition(
                AvgVectors(
                    AvgBodyChunkPos(bodyChunks[1], bodyChunks[2], timeStacker),
                    AvgBodyChunkPos(bodyChunks[3], bodyChunks[4], timeStacker)
                ) - camPos
            );

            antlerLabel.SetPosition(GetPos(AntlerBodyChunk, timeStacker) - camPos);
            antlerLabel.rotation = Custom.VecToDeg(MyDeer.HeadDir);
        }
    }
}
