using System.Linq;
using UnityEngine;
using static WordWorld.WordUtil;

namespace WordWorld.Defaults
{
    internal class GMWords
    {
        public static FLabel[] Init(GraphicsModule module, RoomCamera.SpriteLeaser sLeaser, string name)
        {
            var label = new FLabel(Font, name);

            label.scale = module.owner.bodyChunks.Max(chunk => chunk.rad) * 3f / TextWidth(label.text);
            label.color = sLeaser.sprites[0].color;

            return [label];
        }

        public static void Draw(GraphicsModule module, FLabel[] labels, RoomCamera.SpriteLeaser sLeaser, float timeStacker, Vector2 camPos)
        {
            var pos = GetPos(module.owner.bodyChunks[0], timeStacker) - camPos;
            var rot = sLeaser.sprites[0].rotation;

            labels[0].SetPosition(pos);
            labels[0].rotation = rot;
        }
    }
}
