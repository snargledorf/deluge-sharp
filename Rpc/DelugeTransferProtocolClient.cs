using System;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;
using delugesharp.Rpc.Messages;
using Ionic.Zlib;
using rencodesharp;

namespace delugesharp.Rpc
{
    public class DelugeTransferProtocolClient : IDisposable
    {
        private readonly byte[] _buffer = new byte[2048];

        private TcpClient _tcpClient;

        private StreamReader _streamIn;
        private BinaryWriter _streamOut;

        private bool _disposed;

        public DelugeTransferProtocolClient()
        {
            _tcpClient = new TcpClient();
        }

        public void Connect(string hostname = "localhost", int port = 58846)
        {
            CheckDisposed();

            _tcpClient.Connect(hostname, port);

            NetworkStream stream = _tcpClient.GetStream();
            _streamIn = new StreamReader(stream);
            _streamOut = new BinaryWriter(stream);

            BeginListeningForResponsesAndEvents();
        }

        public bool Connected
        {
            get
            {
                CheckDisposed();

                return _tcpClient.Connected;
            }
        }

        public void TransferRequest(RpcRequest rpcRequest)
        {
            CheckDisposed();

            byte[] request = EncodeAndCompressRpcRequest(rpcRequest);
            _streamOut.Write(request, 0, request.Length);
        }

        public void Close()
        {
            CheckDisposed();

            Dispose();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private async void BeginListeningForResponsesAndEvents()
        {
            var buffer = new char[1024];
            while (true)
            {
                int bytesRead = await _streamIn.ReadAsync(buffer, 0, buffer.Length);

                if (bytesRead <= 0)
                    return;
            }
        }

        private static byte[] EncodeAndCompressRpcRequest(RpcRequest request)
        {
            string encodedRequest = Rencode.Encode(new object[] { request.RequestId, request.Method, request.Arguments.ToArray(), request.KeyWordArguments });
            return ZlibStream.CompressString(encodedRequest);
        }

        private void CheckDisposed()
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().Name);
        }

        private void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                TcpClient tcpClient = _tcpClient;
                _tcpClient = null;
                if (tcpClient != null)
                {
                    tcpClient.Close();
                }

                StreamReader streamIn = _streamIn;
                _streamIn = null;
                if (streamIn != null)
                {
                    streamIn.Close();
                }

                StreamWriter streamOut = _streamOut;
                _streamOut = null;
                if (streamOut != null)
                {
                    streamOut.Close();
                }
            }

            _disposed = true;
        }

        ~DelugeTransferProtocolClient()
        {
            Dispose(false);
        }
    }
}
