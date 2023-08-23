using _SDK.Entities;
using System.Collections;
using UnityEngine;

namespace Assets._SDK.Entities
{
    public abstract class AbstractEntitySettings<T> : ScriptableObject where T : IEntity
	{
        public abstract IEntity Entity { get; }
    }
}