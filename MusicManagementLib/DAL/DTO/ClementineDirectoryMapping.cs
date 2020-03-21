using NHibernate.Mapping.ByCode.Conformist;

namespace MusicManagementLib.DAL.DTO
{
    public class ClementineDirectoryMapping : ClassMapping<ClementineDirectory>
    {
        public ClementineDirectoryMapping()
        {
            Table("directories");
            Lazy(true);
            Id(x => x.Id);
            Property(x => x.Id, map => { map.NotNullable(true); map.Column("subdirs"); });
            Property(x => x.Path, map => { map.NotNullable(true); map.Column("path"); });
        }

    }
}
