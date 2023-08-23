using _SDK.Entities;
using _SDK.Shop;
using _SDK.UI;
using System.Collections;
using UnityEngine;

namespace Assets._SDK.UI
{
    public abstract class AbstractEntityPanel<T> : AbstractPanel where T: IEntity
    {
        protected T _item;

        public int? ItemId => _item?.Id;
        public virtual void SetData(T item)
        {
            _item = item;
        }

        public abstract void DataChanged();
    }
}