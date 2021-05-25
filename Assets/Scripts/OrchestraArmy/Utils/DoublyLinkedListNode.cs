using UnityEngine;

namespace OrchestraArmy.Utils
{
    public class DoublyLinkedListNode<T>
    {
        /// <summary>
        /// Next node
        /// </summary>
        public DoublyLinkedListNode<T> Next { get; set; }
        
        /// <summary>
        /// Previous node
        /// </summary>
        public DoublyLinkedListNode<T> Previous { get; set; }
        
        /// <summary>
        /// Data of the current node
        /// </summary>
        public T Data { get; set; }
    }
}