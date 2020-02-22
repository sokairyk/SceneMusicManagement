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
        public string FileName { get; set; }
        public Artist Artist { get; set; }
        public Album Album { get; set; }

        public FileItem FileInfo { get; }
        public ID3TagReader TagReader { get; }

        public Song()
        {

        }

        public Song(FileItem fileItem)
        {
            FileInfo = fileItem;
            TagReader = new ID3TagReader(fileItem?.FullPath);
        }

        class SongMapperConfig : IAutoMapperConfigurator
        {
            public void ManualConfiguration(IProfileExpression cfg)
            {
                cfg.CreateMappings((Song m) => (new ClementineSong { Album = m.Album.Name, Title = m.Title, Artist = m.Artist.Name }));
            }
        }

    }
}
