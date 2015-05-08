using System;
using delugesharp.Rpc.Messages;
using Ionic.Zlib;
using rencodesharp;

namespace delugesharp.Rpc
{
    public class DelugeRpcClient : DelugeTransferProtocolClient, IDelugeClient
    {
        public Torrent[] CurrentTorrents
        {
            get { return GetCurrentTorrents(); }
        }

        public void AddTorrentFile(string torrentFilePath)
        {
            RpcRequest addTorrentFileRequest = RpcRequestFactory.AddTorrentFile(torrentFilePath);
            TransferRequest(addTorrentFileRequest);
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
}
