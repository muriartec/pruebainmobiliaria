using Amazon.Lambda.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwsDotnetCsharp.Infrastructure.Utils
{
    public class Helpers
    {
        public static void PrintLogMessage(ILambdaContext lct, string msg)
        {
            lct.Logger.LogLine(
                    string.Format("{0}:{1} - {2}",
                        lct.AwsRequestId,
                        lct.FunctionName,
                        msg));
        }
    }
}
