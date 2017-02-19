using System;

namespace ConsoleApplication.Helper
{
    public class ByteHelper
    {
        /**
	 * int到byte[]
	 * @param i
	 * @return
	 */
        public static byte[] IntToByteArray(int i) {
            byte[] result = new byte[4];
            //由高位到低位
            result[0] = (byte)((i >> 24) & 0xFF);
            result[1] = (byte)((i >> 16) & 0xFF);
            result[2] = (byte)((i >> 8) & 0xFF);
            result[3] = (byte)(i & 0xFF);
            return result;
        }

        /**
		 * byte[]转int
		 * @param bytes
		 * @return
		 */
        public static int ByteArrayToInt(byte[] bytes) {
            int value= 0;
            //由高位到低位
            for (int i = 0; i < 4; i++) {
                int shift= (4 - 1 - i) * 8;
                value +=(bytes[i] & 0x000000FF) << shift;//往高位游
            }
            return value;
        }

        public static byte[] combineTowBytes(byte[] bytes1, byte[] bytes2) {
            byte[] bytes3 = new byte[bytes1.Length + bytes2.Length];
            Array.Copy(bytes1, 0, bytes3, 0, bytes1.Length);
            Array.Copy(bytes2, 0, bytes3, bytes1.Length, bytes2.Length);
            return bytes3;
        }

        public static byte[] readHeader(byte[] bytes, int readLength) {
            byte[] destBytes = new byte[readLength];
            Array.Copy(bytes, 0, destBytes, 0, readLength);
            return destBytes;
        }

        public static byte[] readTail(byte[] bytes, int readLength) {
            byte[] destBytes = new byte[readLength];
            Array.Copy(bytes, bytes.Length - readLength, destBytes, 0, readLength);
            return destBytes;
        }
    }
}