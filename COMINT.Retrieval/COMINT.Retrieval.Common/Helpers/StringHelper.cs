using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace COMINT.Retrieval.Common.Helpers
{
    public static class StringHelper
    {
        private static readonly Regex _tokenRegex = new Regex("\\w*");

        public static IEnumerable<string> Tokenize(string content)
        {
            var matches = _tokenRegex.Matches(content);
            for (var i = 0; i < matches.Count; i++)
            {
                string value = matches[i].Value;
                if (!string.IsNullOrWhiteSpace(value))
                {
                    yield return value;
                }
            }
        }
    }
}
