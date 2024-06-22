using UnityEngine;
using static WordWorld.WordUtil;

namespace WordWorld.Creatures
{
    public class BigSpiderWords : CreatureWordify<BigSpiderGraphics>
    {
        public override void Init(RoomCamera.SpriteLeaser sLeaser)
        {
            var label = new FLabel(Font, Type == CreatureTemplate.Type.BigSpider ? "Big Spider" : Unpascal(Type.value));
            label.scale = (Drawable.bug.bodyChunks[0].rad + Drawable.bug.bodyChunks[1].rad + Drawable.bug.bodyChunkConnections[0].distance) * 1.5f / TextWidth(label.text);
            if (Drawable.bug.abstractCreature.creatureTemplate.type == CreatureTemplate.Type.BigSpider)
                label.scale *= 2.5f / 1.5f;
            label.color = Drawable.yellowCol;
            labels.Add(label);
        }

        public override void Draw(RoomCamera.SpriteLeaser sLeaser, float timeStacker, Vector2 camPos)
        {
            var label = labels[0];
            var pos = GetPos(Drawable.bug.bodyChunks[1], timeStacker);
            label.SetPosition(pos - camPos);
            label.rotation = FixRotation(sLeaser.sprites[Drawable.HeadSprite].rotation) + 90f;
        }
    }
}
