using AutoMapper;
using CollectionManagementLib.FileStructure;
using MusicManagementLib.DAL.ClementineDTO;
using MusicManagementLib.Helpers;
using SokairykFramework.AutoMapper;

namespace MusicManagementLib.Domain
{

    public class Song
    {
        public string Title { get; set; }
        public int TrackNumber { get; set; }
        public int DiscNumber { get; set; }
        public string FileName { get; set; }
        public Artist Artist { get; set; }
        public Album Album { get; set; }

        private FileItem FileInfo { get; }
        public ID3TagReader TagReader { get; }

        public Song(FileItem fileItem)
        {
            FileInfo = fileItem;
            TagReader = new ID3TagReader(fileItem?.FullPath);
        }

        class Song2ClementineSongMapperConfig : IAutoMapperConfigurator
        {
            public void ManualConfiguration(IProfileExpression cfg)
            {
                cfg.CreateMappings((Song m) => new ClementineSong
                {
                    Album = m.Album != null ? m.Album.Name : string.Empty,
                    Title = m.Title,
                    Artist = m.Artist != null ? m.Artist.Name : string.Empty,
                    Track = m.TrackNumber,
                    Disc = m.DiscNumber,
                    Bpm = -1,
                    Year = m.Album != null ? m.Album.Year : 0
                });
            }
        }

    }
}
