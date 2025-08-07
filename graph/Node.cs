namespace graph
{
    public class Node<T>
    {
        public string Name { get; set; }
        public T Model { get; set; }

        public Node(string name , T model )
        {
            this.Name = name;
            this.Model = model;
        }
    }
}
