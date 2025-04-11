using System.Text;

namespace LabScratch.codeGen
{
    public class MyCodeGenerator
    {
        public bool GenerateCode(Graph[] graphs, Dictionary<string, int> variables)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("class MyProgram");
            sb.AppendLine("{");
            sb.AppendLine(GenerateVariables(variables));
            sb.AppendLine("static object locker = new object();");

            List<Graph> nonEmptyGraphs = new List<Graph>();
            foreach (Graph graph in graphs)
                if(graph.Nodes.Count > 0)
                    nonEmptyGraphs.Add(graph);

            if (nonEmptyGraphs.Count < 1)
                return false;

            sb.AppendLine("}");

            string str = sb.ToString();

            return false;
        }

        private string GenerateVariables(Dictionary<string, int> variables)
        {
            string res = "static int ";
            foreach (KeyValuePair<string, int> v in variables)
                res += v.Key + "=" + v.Value + ",";
            return res.Remove(res.Length - 1) + ";";
        }

        private string NodeTypeAssingmentOne(string v1, string v2) //V1=V2
        {
            return v1 + "=" + v2 + ";";
        }

        private string NodeTypeAssingmentTwo(string v, int c) //V=C
        {
            return v + "=" + c.ToString() + ";";
        }

        private string NodeTypeConsoleOne(string v) //INPUT V
        {
            string exceptionVariable = "e";
            if (v.Equals(exceptionVariable))
                exceptionVariable = "ex";
            string str1 = "try\r\n{\r\n";
            string str2 = $"Console.Write(\"Enter {v}: \");\r\n{v} = Convert.ToInt32(Console.ReadLine());\r\nConsole.WriteLine();\r\n";
            string str3 = "}\r\ncatch(FormatException " + exceptionVariable + 
                ")\r\n{\r\nConsole.WriteLine(\"Wrong format of number: \" + " + exceptionVariable + ".Message);\r\n}";
            return str1 + str2 + str3;
        }

        private string NodeTypeConsoleTwo(string v) //PRINT V
        {
            return $"Console.WriteLine(\"{v} = \" + {v});";
        }
    }
}
