using AutoMapper;
using MusicManagementLib.Helpers;
using SokairykFramework.AutoMapper;
using SokairykFramework.Extensions;
using System.IO;
using MusicPlayersDAL.DTO.Clementine;

namespace MusicManagementLib.Domain
{

    public class Song
    {
        private readonly string _filepath;
        public long Id { get; set; }
        public string Title { get; set; }
        public int TrackNumber { get; set; }
        public int DiscNumber { get; set; }
        public string FileName { get; set; }
        public Artist Artist { get; set; }
        public Album Album { get; set; }
        protected ID3TagReader ID3Tag { get; }

        private FileInfo _fileInformation;
        protected FileInfo FileInformation
        {
            get
            {
                if (_fileInformation == null && File.Exists(_filepath))
                    _fileInformation = new FileInfo(_filepath);

                return _fileInformation;
            }
        }

        public Song(string filepath)
        {
            _filepath = filepath;
            ID3Tag = new ID3TagReader(_filepath);
        }

        class Song2ClementineSongMapperConfig : IAutoMapperConfigurator
        {
            public void ManualConfiguration(IProfileExpression cfg)
            {
                cfg.CreateMappings((Song m) => new ClementineSong
                {
                    Title = m.Title,
                    Album = m.Album != null ? m.Album.Name : "",
                    Artist = m.Artist != null ? m.Artist.Name : "",
                    Track = m.TrackNumber,
                    Disc = m.DiscNumber,
                    Bpm = -1,
                    Year = m.Album != null ? m.Album.Year : 0,
                    Compilation = 0,
                    Length = m.ID3Tag.DurationInSeconds * (uint)1000000000,
                    Bitrate = m.ID3Tag.BitRate,
                    SampleRate = m.ID3Tag.SampleRate,
                    Directory = 999,
                    Filename = m._filepath.ToHexString(),
                    ModificationTime = m.FileInformation != null ? (long)m.FileInformation.LastWriteTimeUtc.ToUnixEpoch().TotalSeconds : 0,
                    CreationTime = m.FileInformation != null ? (long)m.FileInformation.CreationTimeUtc.ToUnixEpoch().TotalSeconds : 0,
                    Filesize = m.FileInformation != null ? m.FileInformation.Length : 0,
                    Filetype = m.FileInformation != null 
                               ? EnumHelper.GetAudioTypeFromExtension(m.FileInformation.Extension).HasValue  
                                    ? (int)EnumHelper.GetAudioTypeFromExtension(m.FileInformation.Extension).Value.GetMusicLibraryFileTypeValue(MusicLibrary.Clementine)
                                    : 0
                               : 0,
                    LastPlayed = -1,
                    Rating = -1,
                    ForcedCompilationOn = 1,
                    EffectiveCompilation = 1,
                    EffectiveAlbumArtist = m.Artist != null ? m.Artist.Name : "",
                    Grouping = "",
                    OriginalYear = -1,
                    EffectiveOriginalYear = -1
                });
            }
        }

    }
}
