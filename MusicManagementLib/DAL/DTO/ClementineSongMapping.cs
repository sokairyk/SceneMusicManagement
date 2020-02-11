using NHibernate.Mapping.ByCode.Conformist;

namespace MusicManagementLib.DAL.ClementineDTO
{
    public class ClementineSongMapping : ClassMapping<ClementineSong>
    {
        public ClementineSongMapping()
        {
            Table("songs");
            Lazy(true);
            Id(x => x.Filename);
            Property(x => x.Title);
            Property(x => x.Album);
            Property(x => x.Artist);
            Property(x => x.Albumartist);
            Property(x => x.Composer);
            Property(x => x.Track);
            Property(x => x.Disc);
            Property(x => x.Bpm);
            Property(x => x.Year);
            Property(x => x.Genre);
            Property(x => x.Comment);
            Property(x => x.Compilation);
            Property(x => x.Length);
            Property(x => x.Bitrate);
            Property(x => x.Samplerate);
            Property(x => x.Directory, map => map.NotNullable(true));
            Property(x => x.Filename, map => map.NotNullable(true));
            Property(x => x.Mtime, map => map.NotNullable(true));
            Property(x => x.Ctime, map => map.NotNullable(true));
            Property(x => x.Filesize, map => map.NotNullable(true));
            Property(x => x.Sampler, map => map.NotNullable(true));
            Property(x => x.ArtAutomatic, map => map.Column("art_automatic"));
            Property(x => x.ArtManual, map => map.Column("art_manual"));
            Property(x => x.Filetype, map => map.NotNullable(true));
            Property(x => x.Playcount, map => map.NotNullable(true));
            Property(x => x.Lastplayed);
            Property(x => x.Rating);
            Property(x => x.ForcedCompilationOn, map => { map.Column("forced_compilation_on"); map.NotNullable(true); });
            Property(x => x.ForcedCompilationOff, map => { map.Column("forced_compilation_off"); map.NotNullable(true); });
            Property(x => x.EffectiveCompilation, map => { map.Column("effective_compilation"); map.NotNullable(true); });
            Property(x => x.Skipcount, map => map.NotNullable(true));
            Property(x => x.Score, map => map.NotNullable(true));
            Property(x => x.Beginning, map => map.NotNullable(true));
            Property(x => x.CuePath, map => map.Column("cue_path"));
            Property(x => x.Unavailable);
            Property(x => x.EffectiveAlbumartist, map => map.Column("effective_albumartist"));
            Property(x => x.Etag);
            Property(x => x.Performer);
            Property(x => x.Grouping);
            Property(x => x.Lyrics);
            Property(x => x.Originalyear);
            Property(x => x.EffectiveOriginalyear, map => map.Column("effective_originalyear"));
        }
    }
}
