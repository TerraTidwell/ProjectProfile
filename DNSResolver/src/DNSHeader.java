import javax.imageio.IIOException;
import java.io.*;
import java.util.Arrays;

public class DNSHeader {

/**DNS header fields in byte variables
 * query count, answer count, authority count, additional information count
 * 16-bytes
 * */

private byte[] id, flags, QDcount, ANcount, NScount, ARcount;
private byte qr, opCode, AA, TC, RD, RA, z, AD, CD, RC;


  /**read the header from an input stream
   * returns parsed header*/
  public static DNSHeader decodeHeader(ByteArrayInputStream inputStream) {
        DNSHeader header = new DNSHeader();

        try{
          header.id = inputStream.readNBytes(2);
          header.flags = inputStream.readNBytes(2);
        } catch (IOException e) {
          e.printStackTrace();
    }
        //separating the header by bytes

        header.qr = (byte)(header.flags[0] & 0b10000000);
    header.opCode = (byte)(header.flags[0] & 0b01111000);
    header.AA = (byte)(header.flags[0] & 0b00000100);
    header.TC = (byte)(header.flags[0] & 0b00000010);
    header.RD = (byte)(header.flags[0] & 0b00000001);

    header.RA = (byte)(header.flags[1] & 0b10000000);
    header.z = (byte)(header.flags[1] & 0b01000000);
    header.AD = (byte)(header.flags[1] & 0b00100000);
    header.CD = (byte)(header.flags[1] & 0b00010000);
    header.RC = (byte)(header.flags[1] & 0b00001111);

    //copying into an array

    try{
      header.QDcount = inputStream.readNBytes(2);
      header.ANcount = inputStream.readNBytes(2);
      header.NScount = inputStream.readNBytes(2);
      header.ARcount = inputStream.readNBytes(2);
    } catch (IOException e) {
      e.printStackTrace();
    }

    return header;

    }


    /**This will create the header for the response. It will copy some fields from the request*/

    public static DNSHeader buildResponseHeader(DNSMessage request, DNSMessage response){
      DNSHeader responseHeader = request.getHeader();

      responseHeader.flags = request.getHeader().flags;
      responseHeader.qr = (byte) 0b10000000;
      responseHeader.RA = (byte) 0b10000000;
      responseHeader.flags[0] = (byte) (responseHeader.flags[0] | 0b10000000);
      responseHeader.flags[1] = (byte) (responseHeader.flags[1] | 0b10000000);

      responseHeader.ANcount = HelperFunctions.splitIntoTwoBytes(response.getAnswers().size());
      responseHeader.NScount = HelperFunctions.splitIntoTwoBytes(response.getAuthorityRecords().size());
      responseHeader.ARcount = HelperFunctions.splitIntoTwoBytes(response.getAdditionalRecords().size());

  return responseHeader;

    }
    /** Writes the header bytes to be sent to the client */

    public void writeBytes(ByteArrayOutputStream outputStream) {
      try {
        outputStream.write(id);
        outputStream.write(flags);
        outputStream.write(QDcount);
        outputStream.write(ANcount);
        outputStream.write(NScount);
        outputStream.write(ARcount);

      }
      catch(IOException e) {
        e.printStackTrace();
      }


    }
/**Makes the DNSHeader into a readable string format*/
    @Override
    public String toString(){
      return "DNSHeader{" +
              "id=" + Arrays.toString(id) +
              ", flags=" + Arrays.toString(flags) +
              ", qdCount=" + Arrays.toString(QDcount) +
              ", anCount=" + Arrays.toString(ANcount) +
              ", nsCount=" + Arrays.toString(NScount) +
              ", arCount=" + Arrays.toString(ARcount) +
              ", qr=" + qr +
              ", opCode=" + opCode +
              ", authAnswer=" + AA +
              ", trunCation=" + TC +
              ", recurDesired=" + RD +
              ", recurAvail=" + RA +
              ", z=" + z +
              ", authenticData=" + AD +
              ", checkingDisabled=" + CD +
              ", responseCode=" + RC +
              '}';
    }

//applying the helper functions to get data values in the header

  public int getQuestionCount() {
      return HelperFunctions.combineTwoBytes(QDcount);
  }
  public int getAnswerCount() {
      return HelperFunctions.combineTwoBytes(ANcount);
  }
  public int getAuthorityRecordCount() {
      return HelperFunctions.combineTwoBytes(NScount);
  }
  public int getAdditionalRecordCount() {
      return HelperFunctions.combineTwoBytes(ARcount);
  }
  public byte getResponseCode() {
    return RC;
  }

}
