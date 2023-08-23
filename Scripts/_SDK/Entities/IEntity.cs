using System.Buffers;

namespace _SDK.Entities
{
    public interface IEntity
    {
        // Must be Unique in the whole system
        public int Id { get; }

        public string Name { get; set; }

        public bool IsOwned { get; }

        public bool IsActivated { get; }
        public void Own();

        public void Activate();

        public void DeActivate();

    }
}