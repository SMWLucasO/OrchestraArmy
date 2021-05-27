using System.Collections.Generic;
using OrchestraArmy.Entity.Entities.Enemies;
using OrchestraArmy.Enum;

namespace OrchestraArmy.Utils
{
    /// <summary>
    /// Doubly Linked list that loops from start to end 
    /// </summary>
    public class DoublyLoopedLinkedList<T>
    {
        /// <summary>
        /// Start node
        /// </summary>
        public DoublyLinkedListNode<T> Start;
        
        /// <summary>
        /// Constructor
        /// </summary>
        public DoublyLoopedLinkedList()
        {
        }
        
        /// <summary>
        /// Constructor, pass IEnumerable to initialize
        /// </summary>
        public DoublyLoopedLinkedList(IEnumerable<T> values)
        {
            foreach (var value in values)
            {
                AddToEnd(value);
            }
        }
        
        /// <summary>
        /// Add an item of type T to the linked list
        /// </summary>
        public void AddToEnd(T data)
        {
            if (Start == null)
            {
                Start = new DoublyLinkedListNode<T>()
                {
                    Data = data
                };

                Start.Previous = Start;
                Start.Next = Start;
            }
            else
            {
                var newEl = new DoublyLinkedListNode<T>() {Data = data, Next = Start, Previous = Start.Previous};

                Start.Previous.Next = newEl;
                Start.Previous = newEl;
            }
        }

        /// <summary>
        /// Remove item from the list
        /// </summary>
        public void Remove(T item)
        {
            if (Start.Data.Equals(item))
            {
                if (Start.Next.Equals(Start) || Start.Next == null)
                {
                    Start = null;
                }
                else
                {
                    Start.Next.Previous = Start.Previous;
                    Start.Previous.Next = Start.Next;
                    Start = Start.Next;
                }
            }
            else
            {
                var current = Start.Next;
                
                while (current != Start)
                {
                    if (current.Data.Equals(item))
                    {
                        current.Previous.Next = current.Next;
                        current.Next.Previous = current.Previous;
                        break;
                    }

                    current = current.Next;
                }
            }
        }

        /// <summary>
        /// Get the item
        /// </summary>
        public DoublyLinkedListNode<T> Get(T item)
        {
            if (Start == null)
                return null;
            
            if (Start.Data.Equals(item))
                return Start;
            
            var current = Start.Next;
                
            while (current != Start && current != null)
            {
                if (current.Data.Equals(item))
                {
                    return current;
                }

                current = current.Next;
            }

            return null;
        }

        /// <summary>
        /// Check if the item is in the list
        /// </summary>
        public bool Contains(T item)
        {
            return Get(item) != null;
        }
    }
}