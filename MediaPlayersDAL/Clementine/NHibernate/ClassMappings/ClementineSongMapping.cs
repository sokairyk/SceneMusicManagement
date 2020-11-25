using MusicPlayersDAL.DTO.Clementine;
using NHibernate.Mapping.ByCode.Conformist;

namespace MediaPlayersDAL.NHibernate.ClementineClassMappings
{
    public class ClementineSongMapping : ClassMapping<ClementineSong>
    {
        public ClementineSongMapping()
        {
            Table("songs");
            Lazy(true);
            Id(x => x.Filename);
            Property(x => x.Title, map => map.Column("title"));
            Property(x => x.Album, map => map.Column("album"));
            Property(x => x.Artist, map => map.Column("artist"));
            Property(x => x.AlbumArtist, map => map.Column("albumartist"));
            Property(x => x.Composer, map => map.Column("composer"));
            Property(x => x.Track, map => map.Column("track"));
            Property(x => x.Disc, map => map.Column("disc"));
            Property(x => x.Bpm, map => map.Column("bpm"));
            Property(x => x.Year, map => map.Column("year"));
            Property(x => x.Genre, map => map.Column("genre"));
            Property(x => x.Comment, map => map.Column("comment"));
            Property(x => x.Compilation, map => map.Column("compilation"));
            Property(x => x.Length, map => map.Column("length"));
            Property(x => x.Bitrate, map => map.Column("bitrate"));
            Property(x => x.SampleRate, map => map.Column("samplerate"));
            Property(x => x.Directory, map =>
            {
                map.Column("directory");
                map.NotNullable(true);
            });
            Property(x => x.Filename, map =>
            {
                map.Column("filename");
                map.NotNullable(true);
            });
            Property(x => x.ModificationTime, map =>
            {
                map.Column("mtime");
                map.NotNullable(true);
            });
            Property(x => x.CreationTime, map =>
            {
                map.Column("ctime");
                map.NotNullable(true);
            });
            Property(x => x.Filesize, map =>
            {
                map.Column("filesize");
                map.NotNullable(true);
            });
            Property(x => x.Sampler, map =>
            {
                map.Column("sampler");
                map.NotNullable(true);
            });
            Property(x => x.ArtAutomatic, map => map.Column("art_automatic"));
            Property(x => x.ArtManual, map => map.Column("art_manual"));
            Property(x => x.Filetype, map =>
            {
                map.Column("filetype");
                map.NotNullable(true);
            });
            Property(x => x.PlayCount, map =>
            {
                map.Column("playcount");
                map.NotNullable(true);
            });
            Property(x => x.LastPlayed, map => map.Column("lastplayed"));
            Property(x => x.Rating, map => map.Column("rating"));
            Property(x => x.ForcedCompilationOn, map =>
            {
                map.Column("forced_compilation_on");
                map.NotNullable(true);
            });
            Property(x => x.ForcedCompilationOff, map =>
            {
                map.Column("forced_compilation_off");
                map.NotNullable(true);
            });
            Property(x => x.EffectiveCompilation, map =>
            {
                map.Column("effective_compilation");
                map.NotNullable(true);
            });
            Property(x => x.SkipCount, map =>
            {
                map.Column("skipcount");
                map.NotNullable(true);
            });
            Property(x => x.Score, map =>
            {
                map.Column("score");
                map.NotNullable(true);
            });
            Property(x => x.Beginning, map =>
            {
                map.Column("beginning");
                map.NotNullable(true);
            });
            Property(x => x.CuePath, map => map.Column("cue_path"));
            Property(x => x.Unavailable, map => map.Column("unavailable"));
            Property(x => x.EffectiveAlbumArtist, map => map.Column("effective_albumartist"));
            Property(x => x.Etag, map => map.Column("etag"));
            Property(x => x.Performer, map => map.Column("performer"));
            Property(x => x.Grouping, map => map.Column("grouping"));
            Property(x => x.Lyrics, map => map.Column("lyrics"));
            Property(x => x.OriginalYear, map => map.Column("originalyear"));
            Property(x => x.EffectiveOriginalYear, map => map.Column("effective_originalyear"));
        }
    }
}