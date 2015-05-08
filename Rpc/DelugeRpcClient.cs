using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using delugesharp.Rpc.Messages;
using Ionic.Zlib;
using rencodesharp;

namespace delugesharp.Rpc
{
    public partial class DelugeRpcClient : IDelugeClient
    {
        private Socket _socket;

        private bool _disposed;

        private readonly byte[] _buffer = new byte[2048];

        public DelugeRpcClient()
        {
            _socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
        }

        public bool Connected
        {
            get
            {
                CheckDisposed();

                return _socket.Connected;
            }
        }

        public void Connect(string host = "localhost", int port = 58846)
        {
            CheckDisposed();

            _socket.Connect(host, port);

            _socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, BeginReceiveCallback, null);
        }

        private void BeginReceiveCallback(IAsyncResult asyncResult)
        {
             int bytesReceived = _socket.EndReceive(asyncResult);

            _socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, BeginReceiveCallback, null);
        }

        public void Disconnect()
        {
            CheckDisposed();

            if (!_socket.Connected)
                return;

            _socket.Shutdown(SocketShutdown.Both);

            _socket.Disconnect(true);
        }

        public Torrent[] CurrentTorrents
        {
            get { return GetCurrentTorrents(); }
        }

        public void AddTorrentFile(string torrentFilePath)
        {
            RpcRequest addTorrentFileRequest = RpcRequestFactory.AddTorrentFile(torrentFilePath);
            SendRequest(addTorrentFileRequest);
        }

        private void SendRequest(RpcRequest request)
        {
            byte[] requestBytes = EncodeAndCompressRpcRequest(request);
            int sentBytes = _socket.Send(requestBytes);
        }

        public void AddMagnetUrl(string url)
        {
            throw new NotImplementedException();
        }

        public void RemoveTorrent(Torrent torrent)
        {
            throw new NotImplementedException();
        }

        private Torrent[] GetCurrentTorrents()
        {
            throw new NotImplementedException();
        }

        private static byte[] EncodeAndCompressRpcRequest(RpcRequest request)
        {
            string encode = Rencode.Encode(new object[] { request.RequestId, request.Method, request.Arguments.ToArray(), request.KeyWordArguments });
            return ZlibStream.CompressString(encode);
        }
    }

    public partial class DelugeRpcClient : IDisposable
    {
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
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
                Socket socket = _socket;
                _socket = null;
                if (socket != null)
                {
                    socket.Close();
                }
            }

            _disposed = true;
        }

        ~DelugeRpcClient()
        {
            Dispose(false);
        }
    }
}
