import java.util.Arrays;

public class AudioClip {

    public static double Duration = 2.0;
    public static int sampleRate = 44100;
    public final static int totalSamples = (int)(Duration * sampleRate);

    private byte[] actualByte = new byte[(int)(sampleRate * Duration) * 2];

    public AudioClip() {
        actualByte= new byte[(int)(sampleRate * Duration) * 2];

    }

    public int getSample(int i) {

        int b1 = Byte.toUnsignedInt(actualByte[2 * i]);
        int b2 = actualByte[2 * i + 1];
//        int mask = b1 & 0x0000FF;
        b1 = b1 | (b2 << 8);
        return b1;

//return the sample passed as an int. Using bitwise operators to return these functions. Be in the range of shorts
    }
    public void setSample(int index, int value) {

        //byte b2 = (byte) (value & 0x00FF);
        byte b2 = (byte) value;
        byte b1 = (byte) (value >>> 8);
        actualByte[2* index] = b2;
        actualByte[2 *(index) + 1] = b1;

    }
    //sets the sample passed as an int. Using bitwise operators to return these functions. Be in the range of shorts.
//}
    public byte [] getData() {
        return Arrays.copyOf(actualByte, actualByte.length);
    }

    //Arrays.copyOf. Returns our array
    //The values should be stored in Little Endian order.
    // In other words, for the value of sample i, the lower 8 bits should be stored at array[2*i]
    // and the upper 8 bits should be stored at array[(2*i)+1].

}