public class HelperFunctions {

    /**combines 2 bytes in a byte array into an int for storing purposes
     *
     */
    static int combineTwoBytes(byte[] thisArray) {
        return thisArray[1] & 0xFF | (thisArray[0]) << 8;
    }

    /**combines 4 bytes in an array into an int for storing
     *
     */

    static int combineFourBytes(byte[] thisArray) {
        int zeroByte = thisArray[0] << 24;
        zeroByte = zeroByte & 0xFF000000;
        int oneByte = thisArray[1] << 16;
        oneByte = oneByte & 0x00FF0000;
        int twoByte = thisArray[2] << 8;
        twoByte = twoByte & 0x0000FF00;
        int threeByte = thisArray[3];
        threeByte = threeByte & 0x000000FF;
        int result = zeroByte | oneByte | twoByte | threeByte;

        return result;
    }

    /**splits an int into a byte array of two bytes */

    static byte[] splitIntoTwoBytes(int num) {
        byte[] byteArray = new byte[2];

        byteArray[1] = (byte)(0x00ff & num);
        byte zeroByte = (byte) (num | 0xFF00);
        byteArray[0] = (byte) (zeroByte >> 8);

        return byteArray;
    }
}
