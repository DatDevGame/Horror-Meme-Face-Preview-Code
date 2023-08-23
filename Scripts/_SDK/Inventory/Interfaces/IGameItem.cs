using _SDK.Entities;
using Assets._SDK.Entities;
using System.Collections;
using UnityEngine;

namespace Assets._SDK.Inventory.Interfaces
{
    public interface IGameItem : IEntity
    {
		public GameObject Model { get; set; }
	}
}