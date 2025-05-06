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
            sb.AppendLine("static object locker = new object();\r\n");

            List<Graph> nonEmptyGraphs = new List<Graph>();
            foreach (Graph graph in graphs)
                if(graph.Nodes.Count > 0)
                    nonEmptyGraphs.Add(graph);

            if (nonEmptyGraphs.Count < 1)
                return false;

            for (int i = 0; i < nonEmptyGraphs.Count; i++)
                sb.AppendLine(GenerateThreadFunc(nonEmptyGraphs[i], i));

            sb.AppendLine("}\r\n");

            ExportCode(sb.ToString());

            return true;
        }

        private string GenerateThreadFunc(Graph graph, int index)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("static void ThreadFunc" + index + "()");
            sb.AppendLine("{");

            CodePlanner codePlanner = new CodePlanner(graph);
            string plannedCode = codePlanner.GeneratePlan();
            string[] strings = plannedCode.Split("\r\n");

            for (int i = 0; i < strings.Length; i++)
            {
                string str = strings[i];
                if (!Char.IsDigit(str[0]))
                {
                    Enum.TryParse(str, out InsertTypes ins);
                    switch (ins)
                    {
                        case InsertTypes.Undefined: sb.AppendLine("//UNDEFINED ERROR"); break;
                        case InsertTypes.OpenBorder: sb.AppendLine("{"); break;
                        case InsertTypes.CloseBorder: sb.AppendLine("}"); break;
                        case InsertTypes.WhileTrue: sb.AppendLine("while (true)"); break;
                        case InsertTypes.Else: sb.AppendLine("else"); break;
                        case InsertTypes.WhileIfTrue:
                        case InsertTypes.WhileIfFalse:
                        case InsertTypes.If:
                        case InsertTypes.IfNot:
                            {
                                string prefix = ins == InsertTypes.If || ins == InsertTypes.WhileIfTrue ? "" : "!";
                                string keyword = ins == InsertTypes.If || ins == InsertTypes.IfNot ? "if" : "while";
                                str = strings[++i];
                                string condition = graph.Nodes[Convert.ToInt32(str)].Operation;
                                sb.AppendLine($"{keyword} ({prefix}{condition})");
                            }
                            break;
                    }
                }
                else
                {
                    Node node = graph.Nodes[Convert.ToInt32(str)];
                    if (node.Type == NodeType.Assignment)
                        sb.AppendLine(node.Operation + ";");
                    else if (node.Type == NodeType.Console)
                    {
                        List<string> variables = graph.GetVariablesNames(node);
                        if (node.Operation.StartsWith("INPUT"))
                            sb.AppendLine(NodeTypeConsoleOne(variables[0]));
                        else
                            sb.AppendLine(NodeTypeConsoleTwo(variables[0]));
                    }
                }
            }

            sb.AppendLine("}");

            return sb.ToString();
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

        private string GenerateVariables(Dictionary<string, int> variables)
        {
            string res = "static int ";
            foreach (KeyValuePair<string, int> v in variables)
                res += v.Key + "=" + v.Value + ",";
            return res.Remove(res.Length - 1) + ";";
        }

        private void ExportCode(string str)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "C# Source Files (*.cs)|*.cs";
            saveFileDialog.Title = "Export program code";
            saveFileDialog.FileName = "MyProgram.cs";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    File.WriteAllText(saveFileDialog.FileName, str);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error exporting program code:\n" + ex.Message);
                }
            }
        }
    }

    public enum InsertTypes
    {
        WhileTrue = 100,
        Else,
        OpenBorder,         //{
        CloseBorder,        //}
        WhileIfTrue,
        WhileIfFalse,
        If,
        IfNot,
        Undefined = 1000
    }
}
