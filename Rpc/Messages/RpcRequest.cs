using System.Collections.Generic;

namespace delugesharp.Rpc.Messages
{
    public class RpcRequest
    {
        private static volatile int _nextRequestId;

        public RpcRequest()
        {
            RequestId = RpcRequest.NextRequestId;
            Arguments = new List<object>();
            KeyWordArguments = new Dictionary<object, object>();
        }

        public int RequestId { get; set; }
        public string Method { get; set; }
        public List<object> Arguments { get; private set; }
        public Dictionary<object, object> KeyWordArguments { get; private set; }

        private static int NextRequestId
        {
            get
            {
                int nextRequestid = _nextRequestId++;
                if (_nextRequestId < 0)
                    _nextRequestId = 0;
                return nextRequestid;
            }
        }
    }
}
