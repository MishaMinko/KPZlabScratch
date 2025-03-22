using System.Xml.Linq;

namespace LabScratch
{
    public class Graph
    {
        private const int Max = 100;
        public Dictionary<int, Node> Nodes { get; private set; }
        public Dictionary<string, int> Variables { get; private set; }
        public int SelectedNodeId { get; set; }

        public Graph()
        {
            SelectedNodeId = -1;
            Nodes = new Dictionary<int, Node>();
            Variables = new Dictionary<string, int>();
        }

        public bool checkCollision(Point pos)
        {
            Node node = new Node(-1, 0, "", pos);
            foreach (Node n in Nodes.Values)
                if (n.Collides(node))
                    return false;
            return true;
        }

        public bool AddNode(Node node)
        {
            if (Nodes.Count >= Max - 1)
                return false;

            Nodes.Add(node.Id, node);
            return true;
        }

        public void RemoveNode(int id)
        {
            Nodes.Remove(id);
            foreach (Node node in Nodes.Values)
            {
                if (node.NextId == id)
                    node.NextId = -1;
                if (node.FalseId == id)
                    node.FalseId = -1;
            }
        }

        public int GetAvailableId()
        {
            int id = 0;
            while (Nodes.ContainsKey(id))
                id++;
            return id;
        }

        public bool SetVariable(string name, int value)
        {
            if (!Variables.ContainsKey(name))
            {
                if (Variables.Count < Max)
                {
                    Variables[name] = value;
                    return true;
                }
                return false;
            }
            else
            {
                Variables[name] = value;
                return true;
            }
        }

        public int GetVariable(string name)
        {
            return Variables.ContainsKey(name) ? Variables[name] : 0;
        }

        public int? CheckNodeOnPos(Point pos)
        {
            int? nodeId = null;
            int minDist = int.MaxValue;
            foreach (int key in Nodes.Keys)
            {
                Node n = Nodes[key];
                int dist = n.DistanceSquare(pos);
                if (dist <= Math.Pow(n.Rad, 2))
                {
                    if (dist < minDist)
                    {
                        nodeId = key;
                        minDist = dist;
                    }
                }
            }
            return nodeId;
        }
    }
}
