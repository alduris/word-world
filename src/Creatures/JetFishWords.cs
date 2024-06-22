using UnityEngine;
using static WordWorld.WordUtil;

namespace WordWorld.Creatures
{
    public class JetFishWords : Wordify<JetFish>
    {
        public static FLabel[] Init(JetFishGraphics jetfishGraf, CreatureTemplate.Type type, RoomCamera.SpriteLeaser sLeaser)
        {
            var label = new FLabel(Font, type == CreatureTemplate.Type.JetFish ? "Jetfish" : Unpascal(type))
            {
                scale = jetfishGraf.fish.bodyChunks[0].rad * 2.5f / FontSize,
                color = sLeaser.sprites[jetfishGraf.BodySprite].color
            };
            return [label];
        }

        public static void Draw(JetFishGraphics jetfishGraf, FLabel[] labels, RoomCamera.SpriteLeaser sLeaser, float timeStacker, Vector2 camPos)
        {
            labels[0].SetPosition(AvgBodyChunkPos(jetfishGraf.fish.bodyChunks[0], jetfishGraf.fish.bodyChunks[1], timeStacker) - camPos);
            labels[0].rotation = FixRotation(sLeaser.sprites[jetfishGraf.BodySprite].rotation + 90f);
        }
    }
}
