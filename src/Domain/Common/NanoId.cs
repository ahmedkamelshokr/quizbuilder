 

namespace Domain.Common
{ 
    public static class NanoRandomNumberGenerator
    {
        private static readonly string ALPHANUMERIC =
           "0123456789" +
           "ABCDEFGHIJKLMNOPQRSTUVWXYZ" +
           "abcdefghijklmnopqrstuvwxyz";

        public static string GenerateNew(int nanoIdLimit)
        {
            return Nanoid.Nanoid.Generate(ALPHANUMERIC, nanoIdLimit);
        }
    }
}
