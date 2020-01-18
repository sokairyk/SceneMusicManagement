using System;
using System.Collections.Generic;
using System.Text;

namespace CollectionManagementLib.Helpers
{
    internal static class CRC32
    {
        private const UInt32 DefaultPolynomial = 0xedb88320u;
        private const UInt32 DefaultSeed = 0xffffffffu;
        private static UInt32[] DefaultTable;

        static CRC32()
        {
            DefaultTable = InitializeTable(DefaultPolynomial);
        }

        private static UInt32[] InitializeTable(UInt32 polynomial)
        {
            var createTable = new UInt32[256];
            for (var i = 0; i < 256; i++)
            {
                var entry = (UInt32)i;
                for (var j = 0; j < 8; j++)
                    if ((entry & 1) == 1)
                        entry = (entry >> 1) ^ polynomial;
                    else
                        entry = entry >> 1;
                createTable[i] = entry;
            }

            return createTable;
        }

        private static UInt32 CalculateHash(IList<byte> buffer)
        {
            var start = 0;
            var size = buffer.Count;
            var hash = DefaultSeed;
            for (var i = start; i < start + size; i++)
                hash = (hash >> 8) ^ DefaultTable[buffer[i] ^ hash & 0xff];
            return hash;
        }

        public static UInt32 Compute(byte[] buffer)
        {
            return ~CalculateHash(buffer);
        }
    }
}
