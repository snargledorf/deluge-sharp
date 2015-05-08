namespace delugesharp
{
    public interface IDelugeClient
    {
        Torrent[] CurrentTorrents { get; }

        void AddTorrentFile(string torrentFilePath);
        void AddMagnetUrl(string url);
        void RemoveTorrent(Torrent torrent);
    }
}
