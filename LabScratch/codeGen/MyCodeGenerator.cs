using System.Text;

namespace LabScratch.codeGen
{
    public class MyCodeGenerator
    {
        public bool generateCode(Graph[] graphs, Dictionary<string, int> variables)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(generateVariables(variables));

            return false;
        }

        private string generateVariables(Dictionary<string, int> variables)
        {
            string res = "int ";
            foreach (KeyValuePair<string, int> v in variables)
                res += v.Key + "=" + v.Value + ",";
            return res.Remove(res.Length - 1) + ";";
        }
    }
}
