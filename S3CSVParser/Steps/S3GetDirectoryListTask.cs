using System;
using System.Collections.Generic;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Nito.AsyncEx.Synchronous;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace S3CSVParser.Steps
{
    public class S3GetDirectoryListTask : StepBody
    {
        public AmazonS3Client Client { get; set; }
        public String BucketName { get; set; }
        public IList<S3Object> Output { get; set; }

        public override ExecutionResult Run(IStepExecutionContext context)
        {
            ListObjectsV2Request request = new ListObjectsV2Request
            {
                BucketName = BucketName,
                MaxKeys = 10
            };

            ListObjectsV2Response response;
            do
            {
                response = Client.ListObjectsV2Async(request).WaitAndUnwrapException();

                // Process the response.
                foreach (S3Object entry in response.S3Objects)
                {
                    Console.WriteLine("key = {0} size = {1}",
                        entry.Key, entry.Size);
                }
                Console.WriteLine("Next Continuation Token: {0}", response.NextContinuationToken);
                request.ContinuationToken = response.NextContinuationToken;
            } while (response.IsTruncated);

            if (response.KeyCount > 0)
            {
                Output = response.S3Objects;
                return ExecutionResult.Next();
            }
            else
                throw new Exception("Sample");
        }
    }
}
