using UnityEngine;

namespace OrchestraArmy.Utils
{
    public class DoublyLinkedListNode<T>
    {
        [field: SerializeField]
        public DoublyLinkedListNode<T> Next { get; set; }

        [field: SerializeField]
        public DoublyLinkedListNode<T> Previous { get; set; }
        
        public T Data { get; set; }
    }
}