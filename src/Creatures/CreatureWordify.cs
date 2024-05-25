using System;

namespace WordWorld.Creatures
{
    public abstract class CreatureWordify<T> : Wordify<T> where T : GraphicsModule
    {
        protected CreatureTemplate.Type Type = null;
        public Creature Critter => Obj.owner as Creature;

        public override void Init(RoomCamera.SpriteLeaser sLeaser)
        {
            if (Critter == null)
            {
                throw new ArgumentException("`CreatureWordify` must be used with a `GraphicsModule` that has a `Creature` owner!");
            }
            Type = Critter.abstractCreature.creatureTemplate.type;
        }
    }
}
