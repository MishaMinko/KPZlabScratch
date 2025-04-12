namespace LabScratch.codeGen
{
    public class CodePlanner
    {
        private readonly Graph graph;
        private Tree tree;

        public CodePlanner(Graph graph)
        {
            this.graph = graph;
            tree = new Tree(graph.Nodes);
        }

        public List<string> GeneratePlan()
        {
            List<string> plan = new List<string>();
            //HashSet<int> visited = new HashSet<int>();

            //foreach (var node in graph.Nodes.Values)
            //    if (!visited.Contains(node.Id))
            //        BuildExecutionPlan(node.Id, visited, new Stack<int>(), plan);

            return plan;
        }
    }

    class Tree
    {
        Dictionary<int, Node> nodes;
        public int startId;
        public NodeTree startNode;

        public Tree(Dictionary<int, Node> nodes)
        {
            startId = -1;
            this.nodes = nodes;
            GenerateTree();
        }

        private void GenerateTree()
        {
            foreach(Node node in nodes.Values)
                if(node != null)
                {
                    startId = node.Id;
                    break;
                }

            if (startId == -1)
                return;

            startNode = CreateNodeTree(startId, new List<int>());
        }

        private NodeTree CreateNodeTree(int index, List<int> createdIndexes)
        {
            if (createdIndexes.Contains(index))
                return null;
            createdIndexes.Add(index);
            Node node = nodes[index];
            NodeTree nodeTree = new NodeTree(node.Id, 
                node.NextId > -1 ? CreateNodeTree(node.NextId, createdIndexes) : null, 
                node.FalseId > -1 ? CreateNodeTree(node.FalseId, createdIndexes) : null,
                node.NextId, node.FalseId);
            return nodeTree;
        }
    }

    class NodeTree
    {
        int id;
        NodeTree nextNode;
        NodeTree falseNode;
        int nextId;
        int falseId;

        public NodeTree() { }

        public NodeTree(int id, NodeTree nextNode, NodeTree falseNode, int nextId, int falseId)
        {
            this.id = id;
            this.nextNode = nextNode;
            this.falseNode = falseNode;
            this.nextId = nextId;
            this.falseId = falseId;
        }
    }
}
