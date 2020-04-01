using System;
using Amazon;
using Amazon.S3;
using S3CSVParser.Steps;
using WorkflowCore.Interface;

namespace S3CSVParser
{
    public class S3CSVParserWorkflow : IWorkflow<S3CSVParserData>
    {
        public string Id => "compensate-sample";
        public int Version => 1;

        public void Build(IWorkflowBuilder<S3CSVParserData> builder)
        {

            var awsAccessKeyId = "";
            var awsAccessKeySecret = "";
            var bucketName = "";

            var awsClient = new AmazonS3Client(awsAccessKeyId, awsAccessKeySecret, RegionEndpoint.USEast2);

            builder
                .StartWith(context => Console.WriteLine("Begin"))
                .Saga(saga => saga
                    .StartWith<S3GetDirectoryListTask>()
                    .Input( t => t.Client, data => awsClient)
                    .Input(t => t.BucketName, data => bucketName)
                    .Output(data => data.DirectoryList, step => step.Output)
                    .CompensateWith<S3GetDirectoryContentsCompensationTask>()

                    .Then<S3GetDirectoryContentTask>()
                    .Input(t => t.Client, data => awsClient)
                    .Input(t => t.BucketName, data => bucketName)
                    .Input(t => t.DirectoryList, data => data.DirectoryList)
                    .Output(data => data.DirectoryContent, step => step.Output)
                )
                    .OnError(WorkflowCore.Models.WorkflowErrorHandling.Compensate)
                .Then(context => Console.WriteLine("End"));
        }
    }
}
