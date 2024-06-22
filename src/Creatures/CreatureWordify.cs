using System;

namespace WordWorld.Creatures
{
    public abstract class CreatureWordify<T> : Wordify<T> where T : GraphicsModule
    {
        protected CreatureTemplate.Type Type = null;

        public Creature Critter => Drawable.owner as Creature;

        public sealed override void Init(IDrawable drawable, RoomCamera.SpriteLeaser sLeaser)
        {
            if ((drawable as GraphicsModule).owner is Creature c)
            {
                Type = c.abstractCreature.creatureTemplate.type;
            }
            else
            {
                throw new ArgumentException("`CreatureWordify` must be used with a `GraphicsModule` that has a `Creature` owner!");
            }
            base.Init(drawable, sLeaser);
        }
    }
}
