using ATL;
using System.IO;

namespace MusicManagementLib.Helpers
{
    public class ID3TagReader
    {
        private readonly string _filepath;
        private Track _track;
        public string Title => _track?.Title;
        public string Artist => _track?.Artist;
        public string Album => _track?.Album;
        public int Track => (int)_track?.TrackNumber;
        public int BitRate => (int)_track?.Bitrate;
        public int SampleRate => (int)_track?.SampleRate;
        public int DurationInSeconds => (int)_track?.Duration;
        public string Genre => _track?.Genre;
        public string Comment => _track?.Comment;


        public ID3TagReader(string filepath)
        {
            _filepath = filepath;
            RefreshTag();
        }

        public void RefreshTag()
        {
            if (File.Exists(_filepath))
                _track = new Track(_filepath);
        }
    }
}
