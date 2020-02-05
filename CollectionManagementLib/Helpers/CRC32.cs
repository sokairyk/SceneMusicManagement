using System;
using System.Collections.Generic;
using System.Text;

namespace CollectionManagementLib.Helpers
{
    internal static class CRC32
    {
        private const uint DefaultPolynomial = 0xedb88320u;
        private const uint DefaultSeed = 0xffffffffu;
        private static uint[] DefaultTable;

        static CRC32()
        {
            DefaultTable = InitializeTable(DefaultPolynomial);
        }

        private static uint[] InitializeTable(uint polynomial)
        {
            var createTable = new uint[256];
            for (var i = 0; i < 256; i++)
            {
                var entry = (uint)i;
                for (var j = 0; j < 8; j++)
                    if ((entry & 1) == 1)
                        entry = (entry >> 1) ^ polynomial;
                    else
                        entry = entry >> 1;
                createTable[i] = entry;
            }

            return createTable;
        }

        public static uint CalculateHash(byte[] buffer, uint? previousHashCalculation)
        {
            var start = 0;
            var size = buffer.Length;
            var hash = previousHashCalculation ?? DefaultSeed;
            for (var i = start; i < start + size; i++)
                hash = (hash >> 8) ^ DefaultTable[buffer[i] ^ hash & 0xff];
            return hash;
        }
    }
}
