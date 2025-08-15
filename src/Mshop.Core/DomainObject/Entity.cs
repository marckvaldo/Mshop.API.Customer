﻿using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Mshop.Core.Message;
using Mshop.Core.Message;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;


namespace Mshop.Core.DomainObject
{
    public abstract class Entity
    {
        public Guid Id { get; set; }
        protected Entity()
        {
            Id = Guid.NewGuid();
        }

        protected void AddId(Guid id)
        {
            Id = id;
        }

        protected readonly List<DomainEvent> _events = new();

        [JsonIgnore]
        public IReadOnlyCollection<DomainEvent> Events
            => new ReadOnlyCollection<DomainEvent>(_events);

        public void RegisterEvent(DomainEvent @event)
            => _events?.Add(@event);

        public void ClearEvents()
            => _events?.Clear();

        public virtual bool IsValid(INotification notification)
        {
            throw new NotImplementedException();
        }

    }
}

