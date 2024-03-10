using UnityEngine;
using static WordWorld.WordUtil;

namespace WordWorld.Creatures
{
    public static class CicadaWords
    {
        public static FLabel[] Init(CicadaGraphics cicadaGraf, CreatureTemplate.Type type, RoomCamera.SpriteLeaser sLeaser)
        {
            var str = type == CreatureTemplate.Type.CicadaA || type == CreatureTemplate.Type.CicadaB ? "Squidcada" : Unpascal(type);
            var label = new FLabel(Font, str);

            var bodySprite = sLeaser.sprites[cicadaGraf.BodySprite];
            label.scale = bodySprite.element.sourcePixelSize.y / TextWidth(label.text);
            label.color = bodySprite.color;

            return [label];
        }

        public static void Draw(CicadaGraphics cicadaGraf, FLabel[] labels, float timeStacker, Vector2 camPos)
        {
            var cicada = cicadaGraf.cicada;
            labels[0].SetPosition(AvgBodyChunkPos(cicada.bodyChunks[0], cicada.bodyChunks[1], timeStacker) - camPos);
            labels[0].rotation = AngleBtwnChunks(cicada.bodyChunks[0], cicada.bodyChunks[1], timeStacker) - 90f;
        }
    }
}
