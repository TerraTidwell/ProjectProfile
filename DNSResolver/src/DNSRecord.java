import java.io.ByteArrayOutputStream;
import java.io.IOException;
import java.io.InputStream;
import java.io.InvalidObjectException;
import java.util.Arrays;
import java.util.HashMap;

public class DNSRecord {
    private long date = System.currentTimeMillis();
    private String[] name;
    private byte[] type, _class, ttl, rdLength, rData;

    /** parses DNSRecord
     *
     * @return
     */


    static DNSRecord decodeRecord(InputStream inputStream, DNSMessage message) throws IOException{
        DNSRecord record = new DNSRecord();
        record.name = message.readDomainName(inputStream);
        try{
            record.type = inputStream.readNBytes(2);
            record._class = inputStream.readNBytes(2);
            record.ttl = inputStream.readNBytes(4);
            record.rdLength = inputStream.readNBytes(2);
            int rDataLength = HelperFunctions.combineTwoBytes(record.rdLength);
            record.rData = inputStream.readNBytes(rDataLength);
        } catch (IOException e) {
            e.printStackTrace();
        }
        return record;
    }

    /** writes the bytes to be sent to the client
     *
     * @param outputStream
     * @param domainLocations
     */
   void writeBytes(ByteArrayOutputStream outputStream, HashMap<String, Integer> domainLocations) {
    DNSMessage.writeDomainName(outputStream, domainLocations, name);
    try {
        outputStream.write(type);
        outputStream.write(_class);
        outputStream.write(ttl);
        outputStream.write(rdLength);
        outputStream.write(rData);

    } catch(IOException e) {
        e.printStackTrace();
    }

    }

    @Override
   public String toString() {
        return "DNSRecord{" +
                "date=" + date +
                ", name=" + Arrays.toString(name) +
                ", type=" + Arrays.toString(type) +
                ", class_=" + Arrays.toString(_class) +
                ", ttl=" + Arrays.toString(ttl) +
                ", rdLength=" + Arrays.toString(rdLength) +
                ", rData=" + Arrays.toString(rData) +
                '}';
    }

    boolean timestampValid() {
       int TTLparsed = HelperFunctions.combineFourBytes(ttl);
       TTLparsed *=1000;
       return (date + TTLparsed) > System.currentTimeMillis();
    }

}
