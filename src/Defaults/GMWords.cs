using System.Linq;
using UnityEngine;
using static WordWorld.WordUtil;

namespace WordWorld.Defaults
{
    public class GMWords : Wordify<GraphicsModule>
    {
        public override void Init(RoomCamera.SpriteLeaser sLeaser)
        {
            var name = Obj.owner is Creature creature ? creature.abstractCreature.creatureTemplate.type.value : Obj.owner.abstractPhysicalObject.type.value;
            var label = new FLabel(Font, name);

            label.scale = Obj.owner.bodyChunks.Max(chunk => chunk.rad) * 3f / TextWidth(label.text);
            label.color = sLeaser.sprites[0].color;
        }

        public override void Draw(RoomCamera.SpriteLeaser sLeaser, float timeStacker, Vector2 camPos)
        {
            var pos = GetPos(Obj.owner.bodyChunks[0], timeStacker) - camPos;
            var rot = sLeaser.sprites[0].rotation;

            labels[0].SetPosition(pos);
            labels[0].rotation = rot;
        }
    }
}
