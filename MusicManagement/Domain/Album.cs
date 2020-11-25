using System.Collections.Generic;

namespace MusicManagementLib.Domain
{
    public class Album
    {
        public string Name { get; set; }
        public int Year { get; set; }
        public Artist Artist { get; set; }
        public IEnumerable<Song> Songs { get; set; }
    }
}
