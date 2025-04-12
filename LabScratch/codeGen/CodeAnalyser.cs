namespace LabScratch.codeGen
{
    public class CodeAnalyser
    {
        private readonly Graph graph;

        public CodeAnalyser(Graph graph)
        {
            this.graph = graph;
        }

        public List<string> GetExecutionPlan()
        {
            List<string> plan = new List<string>();
            HashSet<int> visited = new HashSet<int>();

            foreach (var node in graph.Nodes.Values)
                if (!visited.Contains(node.Id))
                    BuildExecutionPlan(node.Id, visited, new Stack<int>(), plan);

            return plan;
        }

        private void BuildExecutionPlan(int nodeId, HashSet<int> visited, Stack<int> stack, List<string> plan)
        {
            if (stack.Contains(nodeId))
            {
                Node loopNode = graph.Nodes[nodeId];
                plan.Add($"while({loopNode.Operation}) {{");
                return;
            }

            if (visited.Contains(nodeId)) 
                return;

            visited.Add(nodeId);
            stack.Push(nodeId);

            Node node = graph.Nodes[nodeId];

            if (node.Type == NodeType.Condition)
            {
                if (node.FalseId != -1)
                {
                    plan.Add($"if ({node.Operation}) {{");
                    BuildExecutionPlan(node.NextId, visited, new Stack<int>(stack), plan);
                    plan.Add("} else {");
                    BuildExecutionPlan(node.FalseId, visited, new Stack<int>(stack), plan);
                    plan.Add("}");
                }
                else if (node.NextId != -1)
                {
                    plan.Add($"if ({node.Operation}) {{");
                    BuildExecutionPlan(node.NextId, visited, new Stack<int>(stack), plan);
                    plan.Add("}");
                }
            }
            else
            {
                plan.Add(nodeId.ToString());

                if (node.NextId != -1)
                    BuildExecutionPlan(node.NextId, visited, stack, plan);
            }

            stack.Pop();

            if (stack.Count > 0 && stack.Peek() == nodeId)
                plan.Add("}");
        }
    }
}
