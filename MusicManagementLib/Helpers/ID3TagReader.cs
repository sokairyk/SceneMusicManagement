using ATL;
using System.IO;

namespace MusicManagementLib.Helpers
{
    public class ID3TagReader
    {
        private readonly Track _track;
        public string Title => _track?.Title;
        public string Artist => _track?.Artist;
        public string Album => _track?.Album;
        public int Track => (int)_track?.TrackNumber;
        public int BitRate => (int)_track?.Bitrate;
        public int Sample => (int)_track?.SampleRate;
        public int DurationInSeconds => (int)_track?.Duration;
        public string Genre => _track?.Genre;
        public string Comment => _track?.Comment;


        public ID3TagReader(string filepath)
        {
            if (File.Exists(filepath))
                _track = new Track(filepath);
        }
    }
}
