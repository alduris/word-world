using UnityEngine;
using static WordWorld.WordUtil;
using Type = CosmeticInsect.Type;

namespace WordWorld.Misc
{
    public class CosmeticInsectWords : Wordify<CosmeticInsect>
    {
        private FLabel label;
        private Type type;
        public static FLabel[] Init(CosmeticInsect bug, RoomCamera.SpriteLeaser sLeaser)
        {
            return [label];
        }

        public static void Draw(CosmeticInsect bug, FLabel[] labels, RoomCamera.SpriteLeaser sLeaser, float timeStacker, Vector2 camPos)
        {
            var label = labels[0];
        }

        public override void Init(RoomCamera.SpriteLeaser sLeaser)
        {
            type = Drawable.type;
            var str = type.value[0].ToString();

            if (type == Type.StandardFly) str = "F";
            else if (type == Type.RockFlea) str = "R";
            else if (type == Type.TinyDragonFly) str = "D";
            else if (type == Type.WaterGlowworm) str = "G";

            label = new FLabel(Font, str) { scale = 0.65f };
            if (sLeaser.sprites[0].shader.name == "SpecificDepth")
            {
                label.shader = sLeaser.sprites[0].shader;
            }

            labels.Add(label);
        }

        public override void Draw(RoomCamera.SpriteLeaser sLeaser, float timeStacker, Vector2 camPos)
        {
            label.color = sLeaser.sprites[0].color;
            label.SetPosition(Vector2.Lerp(Drawable.lastPos, Drawable.pos, timeStacker) - camPos);

            label.rotation = Drawable.vel.magnitude > 0 ? AngleFrom(Drawable.vel) : label.rotation;

            if (sLeaser.sprites[0].shader.name == "SpecificDepth")
            {
                label.alpha = sLeaser.sprites[0].alpha;
            }
        }
    }
}
