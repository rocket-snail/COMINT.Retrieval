using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Text;
using System.Xml.Linq;
using COMINT.Retrieval.Common.Models;
using Newtonsoft.Json;

namespace COMINT.Retrieval.Common.Helpers
{
    public class TrecHelper
    {
        public static IEnumerable<(string, string)> LoadTrecElements(string trecFile)
        {
            var doc = XDocument.Load(trecFile);
            var jsonText = JsonConvert.SerializeXNode(doc);
            dynamic dyn = JsonConvert.DeserializeObject<ExpandoObject>(jsonText);
            foreach (var item in dyn.TREC)
            {
                foreach (var document in item.Value)
                {
                    yield return (document.recordId, document.text);
                }
            }
        }

        public static FileInfo ExportToFile(IEnumerable<TrecElement> elements, string path)
        {
            var sb = new StringBuilder();
            foreach (var element in elements)
            {
                sb.AppendLine(element.ToString());
            }
            File.WriteAllText(path, sb.ToString());
            return new FileInfo(path);
        }
    }
}
