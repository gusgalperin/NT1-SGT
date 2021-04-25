using System;

namespace Domain.Entities
{
    public abstract class Entity<Tid>
    { 
        public Tid Id { get; private set; }

        public Entity(Tid id)
        {
            Id = id;
        }
    }
}