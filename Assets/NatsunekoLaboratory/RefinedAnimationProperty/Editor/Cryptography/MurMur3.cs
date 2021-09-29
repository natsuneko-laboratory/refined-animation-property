/*-------------------------------------------------------------------------------------------
 * Copyright (c) Natsuneko. All rights reserved.
 * Licensed under the MIT License. See LICENSE in the project root for license information.
 *------------------------------------------------------------------------------------------*/

using System.IO;
using System.Text;

namespace NatsunekoLaboratory.RefinedAnimationProperty.Cryptography
{
    internal static class MurMur3
    {
        private const uint Seed = 0x00000929;

        public static int CalcHash(string str)
        {
            const uint c1 = 0xcc9e2d51;
            const uint c2 = 0x1b873593;

            var h1 = Seed;
            uint k1;
            var len = 0u;

            using (var br = new BinaryReader(new MemoryStream(Encoding.UTF8.GetBytes(str))))
            {
                var chunk = br.ReadBytes(4);
                while (chunk.Length > 0)
                {
                    len += (uint)chunk.Length;
                    k1 = 0u;

                    switch (chunk.Length)
                    {
                        case 4:
                            k1 = (uint)(chunk[0] | (chunk[1] << 8) | (chunk[2] << 16) | (chunk[3] << 24));
                            k1 *= c1;
                            k1 = Rot132(k1, 15);
                            k1 *= c2;

                            h1 ^= k1;
                            h1 = Rot132(h1, 13);
                            h1 = h1 * 5 + 0xe6546b64;
                            break;

                        case 3:
                        case 2:
                        case 1:
                            if (chunk.Length == 3)
                                k1 ^= (uint)chunk[2] << 16;
                            if (chunk.Length >= 2)
                                k1 ^= (uint)chunk[1] << 8;
                            k1 ^= chunk[0];
                            k1 *= c1;
                            k1 = Rot132(k1, 15);
                            k1 *= c2;
                            h1 ^= k1;
                            break;
                    }

                    chunk = br.ReadBytes(4);
                }

                h1 ^= len;
                h1 = FMix(h1);

                unchecked
                {
                    return (int)h1;
                }
            }
        }

        private static uint Rot132(uint x, byte r)
        {
            return (x << r) | (x >> (32 - r));
        }

        private static uint FMix(uint h)
        {
            h ^= h >> 16;
            h *= 0x85ebca6b;
            h ^= h >> 13;
            h *= 0xc2b2ae35;
            h ^= h >> 16;
            return h;
        }
    }
}