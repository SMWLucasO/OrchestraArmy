namespace OrchestraArmy.Utils
{
    public class DoublyLinkedList<T>
    {
        public DoublyLinkedListNode<T> Start;
        public DoublyLinkedListNode<T> End;

        public DoublyLinkedList(T[] values)
        {
            foreach (var value in values)
            {
                AddToEnd(value);
            }
        }
        
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
            } else if (End == null)
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