using System.Text.Json.Serialization;

namespace LabScratch
{
    public enum NodeType { Assignment, Console, Condition }

    public class Node
    {
        public int Rad = 14;
        public int Id { get; set; }
        public NodeType Type { get; set; }
        public string Operation { get; set; }
        public Point Position { get; set; }
        public int NextId { get; set; } = -1;
        public int FalseId { get; set; } = -1;

        public Node(int Id, NodeType Type, string Operation, Point Position)
        {
            this.Id = Id;
            this.Type = Type;
            this.Operation = Operation;
            this.Position = Position;
        }

        [JsonConstructor]
        public Node(int id, NodeType type, string operation, Point position, int nextId, int falseId)
        {
            Id = id;
            Type = type;
            Operation = operation;
            Position = position;
            NextId = nextId;
            FalseId = falseId;
        }

        public int DistanceSquare(Point p)
        {
            return (int)(Math.Pow((p.X - Position.X), 2) + Math.Pow((p.Y - Position.Y), 2));
        }

        public bool Collides(Node n)
        {
            return DistanceSquare(n.Position) <= Math.Pow(Rad * 3, 2);
        }
    }
}
