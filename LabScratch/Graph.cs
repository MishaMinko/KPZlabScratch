using System.Text.Json.Serialization;
using System.Xml.Linq;

namespace LabScratch
{
    public class Graph
    {
        private const int Max = 100;
        public Dictionary<int, Node> Nodes { get; private set; }
        public List<string> Variables { get; private set; }
        public int SelectedNodeId { get; set; }

        public Graph()
        {
            SelectedNodeId = -1;
            Nodes = new Dictionary<int, Node>();
            Variables = new List<string>();
        }

        [JsonConstructor]
        public Graph(Dictionary<int, Node> nodes, List<string> variables, int selectedNodeId)
        {
            Nodes = nodes;
            Variables = variables;
            SelectedNodeId = selectedNodeId;
        }

        public bool checkCollision(Point pos)
        {
            Node node = new Node(-1, 0, "", pos);
            foreach (Node n in Nodes.Values)
                if (n.Collides(node))
                    return false;
            return true;
        }

        public int AddNode(Node node)
        {
            if (Nodes.Count >= Max)
                return -1;

            if (node.Type == NodeType.Assignment)
            {
                string[] str = node.Operation.Split('=');
                string v1 = str[0];
                string v2 = str[1];
                if (!Char.IsDigit(v2[0]))
                {
                    if (!Variables.Contains(v1) && !Variables.Contains(v2) && !v1.Equals(v2))
                    {
                        if (Variables.Count + 1 >= Max)
                            return -2;
                        else
                        {
                            Variables.Add(v1);
                            Variables.Add(v2);
                        }
                    }
                    else
                    {
                        if (Variables.Count >= Max)
                            return -2;
                        else if (!Variables.Contains(v1))
                            Variables.Add(v1);
                        else
                            Variables.Add(v2);
                    }
                }
                else
                {
                    if (!Variables.Contains(v1))
                    {
                        if (Variables.Count >= Max)
                            return -2;
                        else
                            Variables.Add(v1);
                    }
                }
            }
            else if (node.Type == NodeType.Console)
            {
                string[] str = node.Operation.Split(' ');
                string v = str[1];
                if (!Variables.Contains(v))
                {
                    if (Variables.Count >= Max)
                        return -2;
                    else
                        Variables.Add(v);
                }
            }
            else
            {
                string[] str = null;
                if (node.Operation.Contains('<'))
                    str = node.Operation.Split('<');
                else
                    str = node.Operation.Split("==");
                if (str == null)
                    return 0;
                string v = str[0];
                if (!Variables.Contains(v))
                {
                    if (Variables.Count >= Max)
                        return -2;
                    else
                        Variables.Add(v);
                }
            }

            Nodes.Add(node.Id, node);
            return 1;
        }

        public void RemoveNode(int id)
        {
            Nodes.Remove(id);
            foreach (Node node in Nodes.Values)
            {
                if (node.NextId == id)
                    node.NextId = -1;
                else if (node.Type == NodeType.Condition && node.FalseId == id)
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
