using UnityEngine;

namespace OrchestraArmy.Utils
{
    public class DoublyLinkedListNode<T>
    {
        public DoublyLinkedListNode<T> Next { get; set; }

        public DoublyLinkedListNode<T> Previous { get; set; }
        
        public T Data { get; set; }
    }
}