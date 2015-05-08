namespace delugesharp.Rpc.Messages
{
    public class RpcError
    {
        public int MessageType { get; set; }
        public int RequestId { get; set; }
        public string ExceptionType { get; set; }
        public string ExceptionMessage { get; set; }
        public string Traceback { get; set; }
    }
}
