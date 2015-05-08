using System;
using System.IO;

namespace delugesharp.Rpc.Messages
{
    internal static class RpcRequestFactory
    {
        public static RpcRequest AddTorrentFile(string torrentFilePath)
        {
            if (torrentFilePath == null) throw new ArgumentNullException("torrentFilePath");
            if (!File.Exists(torrentFilePath)) throw new FileNotFoundException(torrentFilePath);

            string fileName = Path.GetFileName(torrentFilePath);

            byte[] fileBytes = File.ReadAllBytes(torrentFilePath);
            string fileData = Convert.ToBase64String(fileBytes);


            var request = new RpcRequest
            {
                Method = "add_torrent_file"
            };

            request.Arguments.Add(fileName);
            request.Arguments.Add(fileData);

            return request;
        }
    }
}
