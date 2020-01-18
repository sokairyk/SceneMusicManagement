using CollectionManagementLib.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace CollectionManagementLib.Interfaces
{
    public interface IHashCheck
    {
        HashType HashAlgorithm { get; }
        string GetHash(string filepath);        
    }
}
