namespace LabScratch.codeGen
{
    public class CodePlanner
    {
        private Tree tree;

        public CodePlanner(Graph graph)
        {
            tree = new Tree(graph.Nodes);
        }

        public List<string> GeneratePlan()
        {
            List<string> plan = new List<string>();
            //(List<int> startCycles, List<int> endCycles) = tree.GetCycles(tree.startNode, new List<int>(), new List<int>());
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
        public NodeTree startNode;

        public Tree(Dictionary<int, Node> nodes)
        {
            this.nodes = nodes;
            GenerateTree();
        }

        private void GenerateTree()
        {
            int startId = -1;

            foreach(Node node in nodes.Values)
                if(node != null)
                {
                    startId = node.Id;
                    break;
                }

            if (startId == -1)
                return;

            startNode = CreateNodeTree(startId, new List<int>());
            SetNodes(startNode, new Dictionary<int, HashSet<int>>());
            SetConnections(startNode, -1, new Dictionary<int, HashSet<int>>());
            Dictionary<int, int> visits = GetPossibleNodeVisitsCount(startNode, new Dictionary<int, int>(), new Dictionary<int, HashSet<int>>());
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

        private void SetNodes(NodeTree node, Dictionary<int, HashSet<int>> visited)
        {
            if (!visited.ContainsKey(node.id))
                visited[node.id] = new HashSet<int>();

            if (node.nextId != -1 && !visited[node.id].Contains(node.nextId))
            {
                if (node.nextNode == null)
                    node.nextNode = FindNodeTreeById(startNode, node.nextId);
                visited[node.id].Add(node.nextId);
                if (node.nextNode != null && node.nextNode != startNode)
                    SetNodes(node.nextNode, visited);
            }

            if (node.falseId != -1 && !visited[node.id].Contains(node.falseId))
            {
                if (node.falseNode == null)
                    node.falseNode = FindNodeTreeById(startNode, node.falseId);
                visited[node.id].Add(node.falseId);
                if (node.falseNode != null && node.falseNode != startNode)
                    SetNodes(node.falseNode, visited);
            }
        }

        private void SetConnections(NodeTree node, int id, Dictionary<int, HashSet<int>> visited)
        {
            if (id > -1)
                if (!node.connectedId.Contains(id))
                    node.connectedId.Add(id);

            if (!visited.ContainsKey(node.id))
                visited[node.id] = new HashSet<int>();

            if (node.nextNode != null)
            {
                if (!visited[node.id].Contains(node.nextId))
                {
                    visited[node.id].Add(node.nextId);

                    if (node.nextNode != startNode)
                        SetConnections(node.nextNode, node.id, visited);
                    else if (!node.nextNode.connectedId.Contains(node.id))
                        node.nextNode.connectedId.Add(node.id);
                }
            }

            if (node.falseNode != null)
            {
                if (!visited[node.id].Contains(node.falseId))
                {
                    visited[node.id].Add(node.falseId);

                    if (node.falseNode != startNode)
                        SetConnections(node.falseNode, node.id, visited);
                    else if (!node.falseNode.connectedId.Contains(node.id))
                        node.falseNode.connectedId.Add(node.id);
                }
            }
        }

        public Dictionary<int, int> GetPossibleNodeVisitsCount(NodeTree node, Dictionary<int, int> visits, Dictionary<int, HashSet<int>> visited)
        {
            if (!visited.ContainsKey(node.id))
                visited[node.id] = new HashSet<int>();

            if (visits.ContainsKey(node.id))
                visits[node.id]++;
            else
                visits[node.id] = 1;

            if (node.nextNode != null && !visited[node.id].Contains(node.nextId))
            {
                visited[node.id].Add(node.nextId);
                visits = GetPossibleNodeVisitsCount(node.nextNode, visits, visited);
            }
            if (node.falseNode != null && !visited[node.id].Contains(node.falseId))
            {
                visited[node.id].Add(node.falseId);
                visits = GetPossibleNodeVisitsCount(node.falseNode, visits, visited);
            }

            return visits;
        }

        //public (List<int>, List<int>) GetCycles(NodeTree node, List<int> startCycleId, List<int> endCycleId)
        //{
        //    if (node.nextId != -1 && node.nextNode == null)
        //    {
        //        startCycleId.Add(node.nextId);
        //        endCycleId.Add(node.id);
        //    }
        //    else if (node.falseId != -1 && node.falseNode == null)
        //    {
        //        startCycleId.Add(node.falseId);
        //        endCycleId.Add(node.id);
        //    }
        //    else
        //    {
        //        if (node.nextNode != null)
        //        {
        //            (startCycleId, endCycleId) = GetCycles(node.nextNode, startCycleId, endCycleId);
        //            if (node.falseNode != null)
        //            {
        //                (List<int> falseStart, List<int> falseEnd) = GetCycles(node.falseNode, startCycleId, endCycleId);
        //                startCycleId = startCycleId.Union(falseStart).ToList();
        //                endCycleId = endCycleId.Union(falseEnd).ToList();
        //            }
        //        }
        //        else if (node.falseNode != null)
        //            (startCycleId, endCycleId) = GetCycles(node.falseNode, startCycleId, endCycleId);
        //    }
        //    return (startCycleId, endCycleId);
        //}

        private NodeTree FindNodeTreeById(NodeTree node, int id)
        {
            if (node.id == id)
                return node;
            else
            {
                if (node.nextNode != null)
                {
                    if (node.falseNode != null)
                    {
                        NodeTree nt = FindNodeTreeById(node.falseNode, id);
                        if (nt != null)
                            return nt;
                    }
                    return FindNodeTreeById(node.nextNode, id);
                }
                else if (node.falseNode != null)
                    return FindNodeTreeById(node.falseNode, id);
                else 
                    return null;
            }
        }
    }

    class NodeTree
    {
        public int id;
        public NodeTree nextNode;
        public NodeTree falseNode;
        public int nextId;
        public int falseId;
        public List<int> connectedId;

        public NodeTree(int id, NodeTree nextNode, NodeTree falseNode, int nextId, int falseId)
        {
            this.id = id;
            this.nextNode = nextNode;
            this.falseNode = falseNode;
            this.nextId = nextId;
            this.falseId = falseId;
            connectedId = new List<int>();
        }
    }
}
