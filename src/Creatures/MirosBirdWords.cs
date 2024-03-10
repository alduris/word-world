using static WordWorld.WordUtil;

namespace WordWorld.Creatures
{
    public static class MirosBirdWords
    {
        public static FLabel[] Init(MirosBirdGraphics mirosGraf, CreatureTemplate.Type type, RoomCamera.SpriteLeaser sLeaser)
        {
            return [
                new(Font, Unpascal(type))
                {
                    scale = mirosGraf.bird.mainBodyChunk.rad * 2f / FontSize,
                    color = sLeaser.sprites[0].color
                },
                new(Font, "Eye")
                {
                    scale = mirosGraf.bird.Head.rad * 2f / FontSize,
                    color = mirosGraf.EyeColor
                }
            ];
        }

        public static void Draw(MirosBirdGraphics mirosGraf, FLabel[] labels, RoomCamera.SpriteLeaser sLeaser)
        {
            // Main body label
            labels[0].SetPosition(sLeaser.sprites[mirosGraf.BodySprite].GetPosition());
            labels[0].rotation = sLeaser.sprites[mirosGraf.BodySprite].rotation;

            // Eye label
            labels[1].SetPosition(sLeaser.sprites[mirosGraf.HeadSprite].GetPosition());
            labels[1].rotation = sLeaser.sprites[mirosGraf.HeadSprite].rotation;
            labels[1].color = mirosGraf.EyeColor;

            // Re-enable eye trail sprite
            sLeaser.sprites[mirosGraf.EyeTrailSprite].isVisible = true;
        }
    }
}
