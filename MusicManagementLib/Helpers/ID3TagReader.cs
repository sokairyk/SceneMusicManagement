using ATL;
using CollectionManagementLib.FileStructure;
using System.IO;

namespace MusicManagementLib.Helpers
{
    public class ID3TagReader
    {
        private readonly FileItem fileItem;
        private readonly Track _track;
        public string Title { get { return _track.Title; } }
        public string Artist { get { return _track.Artist; } }
        public string Album { get { return _track.Album; } }
        public int BitRate { get { return _track.Bitrate; } }
        public int Sample { get { return (int)_track.SampleRate; } }
        public int DurationInSeconds { get { return _track.Duration; } }

        public ID3TagReader(string filepath)
        {
            if (!File.Exists(filepath))
                throw new FileNotFoundException($"Audio file not found in path {filepath}");


            _track = new Track(filepath);
        }
    }
}
