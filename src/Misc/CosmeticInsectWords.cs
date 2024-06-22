using UnityEngine;
using static WordWorld.WordUtil;
using Type = CosmeticInsect.Type;

namespace WordWorld.Misc
{
    public class CosmeticInsectWords : Wordify<CosmeticInsect>
    {
        public static FLabel[] Init(CosmeticInsect bug, RoomCamera.SpriteLeaser sLeaser)
        {
            var str = bug.type.value[0].ToString();
            if (bug.type == Type.StandardFly) str = "F";
            else if (bug.type == Type.RockFlea) str = "R";
            else if (bug.type == Type.TinyDragonFly) str = "D";
            else if (bug.type == Type.WaterGlowworm) str = "G";

            var label = new FLabel(Font, str) { scale = 0.65f };
            if (sLeaser.sprites[0].shader.name == "SpecificDepth")
            {
                label.shader = sLeaser.sprites[0].shader;
            }
            return [label];
        }

        public static void Draw(CosmeticInsect bug, FLabel[] labels, RoomCamera.SpriteLeaser sLeaser, float timeStacker, Vector2 camPos)
        {
            var label = labels[0];
            label.color = sLeaser.sprites[0].color;
            label.SetPosition(Vector2.Lerp(bug.lastPos, bug.pos, timeStacker) - camPos);

            label.rotation = bug.vel.magnitude > 0 ? AngleFrom(bug.vel) : label.rotation;

            if (sLeaser.sprites[0].shader.name == "SpecificDepth")
            {
                label.alpha = sLeaser.sprites[0].alpha;
            }
        }
    }
}
