using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using COMINT.Retrieval.Common.Extensions;
using COMINT.Retrieval.Common.Helpers;
using COMINT.Retrieval.Common.Models;
using COMINT.Retrieval.Engine;
using Newtonsoft.Json;

namespace COMINT.Retrieval.Tool
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("COMINT.Retrieval.Tool");
            Console.WriteLine("Argument 1: Documents-Path");
            Console.WriteLine("Argument 2: Queries-File (TREC)");
            Console.WriteLine("Argument 3: Output-Path");
            Console.WriteLine("****************************************");
            var documentsPath = @"C:\Data\COMIT.Retrieval\Google_1";
            var queriesFile = @"C:\Data\COMIT.Retrieval\irg_queries.trec";
            var outputPath = @"C:\Data\COMIT.Retrieval";
            var outputPath = @"C:\Data\COMIT.Retrieval";
            if (args.Any())
            {
                documentsPath = args[0];
                queriesFile = args[1];
                outputPath = args[2];
            }

            var goldenResults = new Dictionary<string, List<TrecElement>>();

            var results = new Dictionary<string, List<TrecElement>>();


            var systemName = Path.GetFileName(documentsPath);
            var documents = Directory.GetFiles(documentsPath);
            var queries = TrecHelper.LoadTrecElements(queriesFile).ToList();

            Console.WriteLine("Initializing...");
            var engine = new RetrievalEngine(documents, systemName);
            Console.WriteLine("Finished Initializing");
            
            var jaccardIndices = new Dictionary<string, double>();
            foreach (var (name, query) in queries)
            {
                Console.WriteLine($"Process query: {name}");
                var result = engine.ProcessQuery(name, query).ToList();
                results.Add(name, result);
                var goldenResult = goldenResults.ContainsKey(name) ? goldenResults[name] : new List<TrecElement>();
                var common = goldenResult.Join(result, a => a, b => b, (a, b) => a);
                var jaccardIndex = common.Count() / (double)(result.Count + goldenResult.Count);
                jaccardIndices.Add(name, jaccardIndex);
            }

            var avarage = jaccardIndices.Average(x => x.Value);
            jaccardIndices.Add("*AVERAGE*", avarage);
            TrecHelper.ExportToFile(results.SelectMany(x => x.Value), Path.Combine(outputPath, $"Results_{systemName}.trec"));

            var sb = new StringBuilder();
            sb.AppendLine($"Name;Index");
            foreach (var (name, index) in jaccardIndices)
            {
                sb.AppendLine($"{name};{index}");
            }
            File.WriteAllText(Path.Combine(outputPath, $"JaccardIndex_{systemName}.csv"), sb.ToString());

            File.WriteAllText(Path.Combine(outputPath, $"Collection_{systemName}"), JsonConvert.SerializeObject(results));

            Console.WriteLine("**********************");
            Console.WriteLine("****** FINISHED ******");
            Console.WriteLine("**********************");
            Console.ReadLine();
        }
    }
}
