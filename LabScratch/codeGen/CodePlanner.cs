using System.Text;

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
            List<CycleInfo> cycleInfos = tree.GetCycles();



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
        }

        public List<CycleInfo> GetCycles()
        {
            Dictionary<int, int> visits = GetPossibleNodeVisitsCount(startNode, new Dictionary<int, int>(), new Dictionary<int, HashSet<int>>());
            List<CycleInfo> infos = CheckVisits(visits);
            return infos;
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

        private Dictionary<int, int> GetPossibleNodeVisitsCount(NodeTree node, Dictionary<int, int> visits, Dictionary<int, HashSet<int>> visited)
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

        private List<CycleInfo> CheckVisits(Dictionary<int, int> visits)
        {
            if (visits.Count < 1)
                return null;

            List<CycleInfo> infos = new List<CycleInfo>();
            foreach(KeyValuePair<int, int> visit in visits)
            {
                if(visit.Value.Equals(2))
                {
                    NodeTree node = FindNodeTreeById(startNode, visit.Key);
                    if(node.nextId != -1 || node.falseId != -1)
                    {
                        int startId = FindStartCycleNode(node, node.id, false);
                        if (startId == -1)
                            throw new NotFiniteNumberException();

                        node = FindNodeTreeById(startNode, startId);

                        int endId = -1;
                        foreach(int checkId in node.connectedId)
                        {
                            if(CheckIfReachesNodeByConnections(FindNodeTreeById(startNode, checkId), node.id))
                            {
                                endId = checkId;
                                break;
                            }    
                        }
                        if (endId == -1)
                            throw new NotFiniteNumberException();

                        NodeType nodeType = nodes[startId].Type;
                        InsertTypes cycleType = InsertTypes.Undefined;

                        if (nodeType == NodeType.Condition)
                        {
                            if (node.nextNode != null && node.falseNode != null)
                            {
                                bool checkTrue = CheckIfReachesNode(node.nextNode, endId);
                                bool checkFalse = CheckIfReachesNode(node.falseNode, endId);

                                if (checkTrue && checkFalse)
                                    cycleType = InsertTypes.WhileTrue;
                                else if (checkTrue)
                                    cycleType = InsertTypes.WhileIfTrue;
                                else
                                    cycleType = InsertTypes.WhileIfFalse;
                            }
                            else if (node.nextNode == null)
                                cycleType = InsertTypes.WhileIfFalse;
                            else
                                cycleType = InsertTypes.WhileIfTrue;
                        }
                        else
                            cycleType = InsertTypes.WhileTrue;

                        infos.Add(new CycleInfo { startId = startId, endId = endId, cycleType = cycleType });
                    }
                }
            }
            return infos;
        }

        private bool CheckIfReachesNodeByConnections(NodeTree node, int targetId)
        {
            if (node.id == targetId)
                return true;

            if (node.connectedId.Count > 0)
                foreach (int conId in node.connectedId)
                    if (CheckIfReachesNodeByConnections(FindNodeTreeById(startNode, conId), targetId))
                        return true;

            return false;
        }

        private bool CheckIfReachesNode(NodeTree node, int targetId)
        {
            if (node.id == targetId)
                return true;

            if (node.nextNode != null)
            {
                bool p = CheckIfReachesNode(node.nextNode, targetId);
                if (p)
                    return p;
            }
            if (node.falseNode != null)
            {
                bool p = CheckIfReachesNode(node.falseNode, targetId);
                return p;
            }

            return false;
        }

        private int FindStartCycleNode(NodeTree node, int startId, bool didStart)
        {
            if (nodes[node.id].Type == NodeType.Condition && didStart)
                return node.id;

            if (node.id == startId && didStart)
                return startId;

            List<int> ress = new List<int>();

            if (node.nextNode != null)
                ress.Add(FindStartCycleNode(node.nextNode, startId, true));
            if (node.falseNode != null)
                ress.Add(FindStartCycleNode(node.falseNode, startId, true));

            if (ress.Count < 2)
                return ress[0];
            else if (ress.Count == 2)
            {
                int r1 = ress[0], r2 = ress[1];
                if (r1 == r2)
                    return r1;
                else
                {
                    if (r1 == startId)
                        return r2;
                    else if (r2 == startId)
                        return r1;
                    else
                    {
                        if (nodes[r1].Type == NodeType.Condition && nodes[r2].Type == NodeType.Condition)
                            return -1;
                        else if (nodes[r1].Type == NodeType.Condition)
                            return r1;
                        else
                            return r2;
                    }
                }
            }

            return startId;
        }

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

    struct CycleInfo
    {
        public int startId;
        public int endId;
        public InsertTypes cycleType;
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
