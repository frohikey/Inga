using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Inga.Tokens;
using static System.Int32;

namespace Inga.Tools
{
    public static class FileHelper
    {
        /// <summary>
        /// Convert list of filenames to archives.
        /// </summary>
        public static List<Archive> ToArchives(this List<string> files)
        {
            if (files == null)
                throw new ArgumentNullException(nameof(files));

            var result = new List<Archive>();
            
            var regex = new Regex(@"^(?<filename>.+)_(?<year>[\d]{4})(?<month>[\d]{2})(?<day>[\d]{2})(?<extension>\..+)$");            

            foreach (var file in files)
            {
                var match = regex.Match(file);
                var fn = match.Groups["filename"].Value + match.Groups["extension"].Value;

                if (match.Success)
                {
                    var r = result.FirstOrDefault(x => x.Filename.Equals(fn, StringComparison.OrdinalIgnoreCase));

                    if (r == null)
                    {
                        r = new Archive { Filename = fn };
                        result.Add(r);
                    }

                    r.Stamps.Add(new DateTime(Parse(match.Groups["year"].Value), Parse(match.Groups["month"].Value), Parse(match.Groups["day"].Value)));                    
                }
            }

            return result;
        }        
    }
}
