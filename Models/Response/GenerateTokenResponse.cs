using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwsDotnetCsharp.Models.Response
{
    public class GenerateTokenResponse
    {
        public string access_token { get; set; }
        public int expires { get; set; }
    }
}
