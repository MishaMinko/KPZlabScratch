using System.Text.Json;

namespace LabScratch
{
    public static class GraphIO
    {
        private class GraphDataBundle
        {
            public Graph[] graphs { get; set; }
            public Dictionary<string, int> variables { get; set; }
        }

        private static readonly JsonSerializerOptions options = new JsonSerializerOptions
        {
            WriteIndented = true,
            IncludeFields = true
        };

        public static void ExportGraphs(Graph[] graphs, Dictionary<string, int> variables, string filePath)
        {
            var bundle = new GraphDataBundle
            {
                graphs = graphs,
                variables = variables
            };

            string json = JsonSerializer.Serialize(bundle, options);
            File.WriteAllText(filePath, json);
        }

        public static (Graph[] Graphs, Dictionary<string, int> Variables) ImportGraphs(string filePath)
        {
            if (!File.Exists(filePath))
                return (Array.Empty<Graph>(), new Dictionary<string, int>());

            string json = File.ReadAllText(filePath);
            var bundle = JsonSerializer.Deserialize<GraphDataBundle>(json, options);
            return (bundle?.graphs ?? Array.Empty<Graph>(), bundle?.variables ?? new Dictionary<string, int>());
        }
    }
}
