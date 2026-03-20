namespace Test
{
    public enum Endian
    {
        Little,
        Big,
    }

    public class BitConverter
    {

        public static byte[] FromUInt32(uint value, Endian endian)
        {
            byte[] buffer = new byte[4];
            if (endian == Endian.Little)
            {
                buffer[0] = (byte)value;
                buffer[1] = (byte)((value >> 8) & 0xFF);
                buffer[2] = (byte)((value >> 16) & 0xFF);
                buffer[3] = (byte)((value >> 24) & 0xFF);
            }
            else
            {
                buffer[0] = (byte)((value >> 24) & 0xFF);
                buffer[1] = (byte)((value >> 16) & 0xFF);
                buffer[2] = (byte)((value >> 8) & 0xFF);
                buffer[3] = (byte)value;
            }

            return buffer;
        }
    }
    
}