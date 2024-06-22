using UnityEngine;
using static WordWorld.WordUtil;

namespace WordWorld.Creatures
{
    public class CicadaWords : CreatureWordify<CicadaGraphics>
    {
        public override void Init(RoomCamera.SpriteLeaser sLeaser)
        {
            var str = Type == CreatureTemplate.Type.CicadaA || Type == CreatureTemplate.Type.CicadaB ? "Squidcada" : Unpascal(Type);
            var label = new FLabel(Font, str);

            var bodySprite = sLeaser.sprites[Drawable.BodySprite];
            label.scale = bodySprite.element.sourcePixelSize.y / TextWidth(label.text);
            label.color = bodySprite.color;

            labels.Add(label);
        }

        public override void Draw(RoomCamera.SpriteLeaser sLeaser, float timeStacker, Vector2 camPos)
        {
            var cicada = Critter as Cicada;
            labels[0].SetPosition(AvgBodyChunkPos(cicada.bodyChunks[0], cicada.bodyChunks[1], timeStacker) - camPos);
            labels[0].rotation = AngleBtwnChunks(cicada.bodyChunks[0], cicada.bodyChunks[1], timeStacker) - 90f;
        }
    }
}
