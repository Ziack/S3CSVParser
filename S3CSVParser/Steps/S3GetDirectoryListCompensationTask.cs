using System;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace S3CSVParser.Steps
{
    public class S3GetDirectoryContentsCompensationTask : StepBody
    {
        public override ExecutionResult Run(IStepExecutionContext context)
        {
            Console.WriteLine("Files not found.");
            return ExecutionResult.Next();
        }
    }
}
