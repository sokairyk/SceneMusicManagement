using Sokairyk.Base.CustomAttributes;

namespace Sokairyk.Hashing
{
    public enum HashTypeEnum
    {
        //Use StringValue custom attribute to store default file extensions

        [StringValue("sfv")]
        CRC32 = 0,
        [StringValue("md5")]
        MD5,
        [StringValue("sha1")]
        SHA1,
        [StringValue("sha256")]
        SHA256,
        [StringValue("sha512")]
        SHA512
    }
}
