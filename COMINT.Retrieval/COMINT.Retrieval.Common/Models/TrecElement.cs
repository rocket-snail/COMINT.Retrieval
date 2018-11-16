using System;

namespace COMINT.Retrieval.Common.Models
{
    public class TrecElement : IComparable<TrecElement>
    {
        public string Query { get; set; }

        public string Q0 => "Q0";

        public int Rank { get; set; }

        public string Document { get; set; }

        public double RSV { get; set; }

        public string System { get; set; }

        public string File { get; set; }

        public int CompareTo(TrecElement other)
        {
            if (Rank > other.Rank)
            {
                return -1;
            }
            if (Rank < other.Rank)
            {
                return 1;
            }
            return 0;
        }
    }
}
