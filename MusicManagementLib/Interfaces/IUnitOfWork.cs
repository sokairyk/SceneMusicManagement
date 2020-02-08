using System;
using System.Collections.Generic;
using System.Text;

namespace MusicManagementLib.Interfaces
{
    public interface IUnitOfWork
    {
        void BeginTransaction();
        void Commit();
        void Rollback();
    }
}
