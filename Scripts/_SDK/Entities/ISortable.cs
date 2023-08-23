using System.Collections;
using UnityEngine;

namespace Assets._SDK.Entities
{
    public interface ISortable 
    {
        public int Order { get; set; }
    }
}