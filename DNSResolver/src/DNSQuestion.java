import javax.imageio.IIOException;
import java.io.ByteArrayOutputStream;
import java.io.IOException;
import java.io.InputStream;
import java.util.Arrays;
import java.util.HashMap;

public class DNSQuestion {

    private String[] questionName;
    private byte[] questionType, questionClass;


    /**parses out the DNSQuestion
     *
     * @param inputStream
     * @param message
     * @return
     * @throws IOException
     */
    static DNSQuestion decodeQuestion(InputStream inputStream, DNSMessage message) throws IOException {
        DNSQuestion question = new DNSQuestion();

        question.questionName = message.readDomainName(inputStream);
        try{
            question.questionType = inputStream.readNBytes(2);
            question.questionClass = inputStream.readNBytes(2);
        } catch (IOException e) {
            e.printStackTrace();
        }
        return question;
    }

    /**Writes the question bytes to be sent to the client
     *
     * @param domainNameLocations
     */

    void writeBytes(ByteArrayOutputStream outputStream, HashMap<String,Integer> domainNameLocations) {
        DNSMessage.writeDomainName(outputStream, domainNameLocations, questionName);
        outputStream.writeBytes(questionType);
        outputStream.writeBytes(questionClass);
    }

    /** creates a string out of the DNSQuestion for an easy read
     *
     * @return
     */
    @Override 

    public String toString() {
        return "DNSQuestion{" +
                "questionName=" + Arrays.toString(questionName) +
                ", questionType=" + Arrays.toString(questionType) +
                ", questionClass=" + Arrays.toString(questionClass) +
                "}";
    }

    /**equality comparison of objects
     *
     * @return
     */
    @Override

    public boolean equals(Object o) {
        if (this == o) {
            return true;
        }
        if(o == null || getClass() != o.getClass()){
            return false;
        }
        DNSQuestion question = (DNSQuestion) o;
        return Arrays.equals(questionName, question.questionName) && Arrays.equals(questionType, question.questionType) && Arrays.equals(questionClass, question.questionClass);
    }

    /** for use for DNSCache
     *
     * @return
     */
    @Override
    public int hashCode() {
        int result = Arrays.hashCode(questionName);
        result = 31 * result + Arrays.hashCode(questionType);
        result = 31 * result + Arrays.hashCode(questionClass);
        return result;

    }


}
