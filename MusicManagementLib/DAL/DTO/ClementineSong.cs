namespace MusicManagementLib.DAL.ClementineDTO
{
    public class ClementineSong
    {
        public virtual string Title { get; set; }
        public virtual string Album { get; set; }
        public virtual string Artist { get; set; }
        public virtual string Albumartist { get; set; }
        public virtual string Composer { get; set; }
        public virtual int? Track { get; set; }
        public virtual int? Disc { get; set; }
        public virtual float? Bpm { get; set; }
        public virtual int? Year { get; set; }
        public virtual string Genre { get; set; }
        public virtual string Comment { get; set; }
        public virtual int? Compilation { get; set; }
        public virtual long? Length { get; set; }
        public virtual int? Bitrate { get; set; }
        public virtual int? Samplerate { get; set; }
        public virtual int Directory { get; set; }
        public virtual string Filename { get; set; }
        public virtual long Mtime { get; set; }
        public virtual long Ctime { get; set; }
        public virtual long Filesize { get; set; }
        public virtual int Sampler { get; set; }
        public virtual string ArtAutomatic { get; set; }
        public virtual string ArtManual { get; set; }
        public virtual int Filetype { get; set; }
        public virtual int Playcount { get; set; }
        public virtual int? Lastplayed { get; set; }
        public virtual int? Rating { get; set; }
        public virtual int ForcedCompilationOn { get; set; }
        public virtual int ForcedCompilationOff { get; set; }
        public virtual int EffectiveCompilation { get; set; }
        public virtual int Skipcount { get; set; }
        public virtual int Score { get; set; }
        public virtual int Beginning { get; set; }
        public virtual string CuePath { get; set; }
        public virtual int? Unavailable { get; set; }
        public virtual string EffectiveAlbumartist { get; set; }
        public virtual string Etag { get; set; }
        public virtual string Performer { get; set; }
        public virtual string Grouping { get; set; }
        public virtual string Lyrics { get; set; }
        public virtual int? Originalyear { get; set; }
        public virtual int? EffectiveOriginalyear { get; set; }
    }
}
