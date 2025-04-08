using System.Text;

namespace LabScratch.codeGen
{
    public class MyCodeGenerator
    {
        public bool generateCode(Graph[] graphs, Dictionary<string, int> variables)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("class MyProgram");
            sb.AppendLine("{");
            sb.AppendLine(generateVariables(variables));
            sb.AppendLine("static object locker = new object();");
            string b = sb.ToString();

            List<Graph> nonEmptyGraphs = new List<Graph>();
            foreach (Graph graph in graphs)
                if(graph.Nodes.Count > 1)
                    nonEmptyGraphs.Add(graph);

            if (nonEmptyGraphs.Count < 1)
                return false;

            sb.AppendLine("}");

            return false;
        }

        private string generateVariables(Dictionary<string, int> variables)
        {
            string res = "static int ";
            foreach (KeyValuePair<string, int> v in variables)
                res += v.Key + "=" + v.Value + ",";
            return res.Remove(res.Length - 1) + ";";
        }
    }
}
