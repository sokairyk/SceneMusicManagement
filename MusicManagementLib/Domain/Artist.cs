using System.Collections.Generic;

namespace MusicManagementLib.Domain
{
    public class Artist
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<Album> Albums { get; set; }
    }
}
