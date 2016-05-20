using System;
using System.Collections.Generic;

namespace Inga.Tokens
{
    public class Archive
    {
        public string Filename { get; set; }
        public List<DateTime> Stamps { get; } = new List<DateTime>();
    }
}
