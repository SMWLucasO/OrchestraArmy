using System.Collections.Generic;

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
    }
}