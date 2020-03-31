using System;
using System.Collections.Generic;
using Amazon.S3.Model;

namespace S3CSVParser
{
    public class S3CSVParserData
    {
        public IList<S3Object> DirectoryList { get; set; }

        public IDictionary<String, String> DirectoryContent { get; set; }
    }
}
