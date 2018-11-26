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


#pragma warning disable 659
        public override bool Equals(object obj)
#pragma warning restore 659
        {
            if (!(obj is TrecElement other))
            {
                return false;
            }

            return Equals(Query, other.Query) &&
                   Equals(Rank, other.Rank) &&
                   Equals(Document, other.Document);
        }

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

        public override string ToString()
        {
            return $"{Query} {Q0} {Document} {Rank} {RSV} {System}";
        }
    }
}
