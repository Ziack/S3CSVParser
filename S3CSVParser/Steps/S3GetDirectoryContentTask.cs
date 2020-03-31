using System;
using System.Collections.Generic;
using System.IO;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Nito.AsyncEx.Synchronous;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace S3CSVParser.Steps
{
    public class S3GetDirectoryContentTask : StepBody
    {
        public AmazonS3Client Client { get; set; }
        public String BucketName { get; set; }
        public IList<S3Object> DirectoryList { get; set; }

        public IDictionary<String, String> Output { get; set; }

        public override ExecutionResult Run(IStepExecutionContext context)
        {
            var results = new Dictionary<String, String>();

            foreach (var item in DirectoryList)
            {
                GetObjectRequest request = new GetObjectRequest
                {
                    BucketName = BucketName,
                    Key = item.Key
                };

                using (GetObjectResponse response = Client.GetObjectAsync(request).WaitAndUnwrapException())
                using (Stream responseStream = response.ResponseStream)
                using (StreamReader reader = new StreamReader(responseStream))
                {
                    string title = response.Metadata["x-amz-meta-title"]; // Assume you have "title" as medata added to the object.
                    string contentType = response.Headers["Content-Type"];
                    Console.WriteLine("Object metadata, Title: {0}", title);
                    Console.WriteLine("Content type: {0}", contentType);

                    results.Add(item.Key, reader.ReadToEnd());
                }
            }


            if (results.Count > 0)
            {
                Output = results;
                return ExecutionResult.Next();
            }
            else
                throw new Exception("Sample");

        }
    }
}
