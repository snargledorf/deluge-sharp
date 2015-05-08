using System.Collections.Generic;

namespace delugesharp.Rpc.Messages
{
    public class RpcResponse
    {
        public RpcResponse()
        {
            ReturnValue = new List<string>();
        }

        public int MessageType { get; set; }
        public int RequestId { get; set; }
        public List<string> ReturnValue { get; private set; }
    }
}
