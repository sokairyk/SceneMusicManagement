using CollectionManagementLib.FileStructure;
using MusicManagementLib.Helpers;

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

        public Song(FileItem fileItem)
        {
            FileInfo = fileItem;
            TagReader = new ID3TagReader(fileItem?.FullPath);
        }

    }
}
