namespace delugesharp
{
    public interface IDelugeClient
    {
        void Connect(string host = "localhost", int port = 58846);
        void Disconnect();

        Torrent[] CurrentTorrents { get; }

        void AddTorrentFile(string torrentFilePath);
        void AddMagnetUrl(string url);
        void RemoveTorrent(Torrent torrent);
    }
}
