using System.Collections.Generic;
using OrchestraArmy.Entity.Entities.Enemies;

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
        /// End node
        /// </summary>
        public DoublyLinkedListNode<T> End;
        
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
            else if (End == null)
            {
                End = new DoublyLinkedListNode<T>()
                {
                    Data = data,
                    Previous = Start,
                    Next = Start
                };
                
                Start.Previous = End;
                Start.Next = End;
            }
            else
            {
                var newEl = new DoublyLinkedListNode<T>() {Data = data, Next = Start, Previous = End};

                Start.Previous = newEl;
                End.Next = newEl;

                End = newEl;
            }
        }

        public void Remove(T item)
        {
            if (Start.Data.Equals(item))
            {
                if (Start.Next.Equals(Start))
                {
                    Start = null;
                    End = null;
                }
                else
                {
                    Start = Start.Next;
                }
            } else if (End.Data.Equals(item))
            {
                End.Previous.Next = Start;
                End = End.Previous;
                Start.Previous = End;
            }
            else
            {
                var current = Get(item);

                if (current == null)
                    return;
                
                current.Previous.Next = current.Next;
                current.Next.Previous = current.Previous;
            }
        }

        public DoublyLinkedListNode<T> Get(T item)
        {
            if (Start == null)
                return null;

            if (Start.Next == null)
                return Start.Data.Equals(item) ? Start : null;
            
            var current = Start.Next;
                
            while (current != Start)
            {
                if (current.Data.Equals(item))
                {
                    return current;
                }

                current = current.Next;
            }

            return null;
        }
    }
}