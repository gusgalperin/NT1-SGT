using System;

namespace Domain.Entities
{
    public abstract class Entity<TId>
        where TId : IEquatable<TId>
    { 
        public TId Id { get; private set; }

        protected Entity() { }

        public Entity(TId id)
        {
            Id = id;
        }
    }
}