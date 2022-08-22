import java.io.ByteArrayInputStream;
import java.io.ByteArrayOutputStream;
import java.io.IOException;
import java.io.InputStream;
import java.sql.Array;
import java.util.ArrayList;
import java.util.HashMap;

public class DNSMessage {

    private DNSHeader header;
    private DNSQuestion[]questions;
    private ArrayList<DNSRecord>answers = new ArrayList<>();
    private ArrayList<DNSRecord>authorityRecords = new ArrayList<>();
    private ArrayList<DNSRecord>additionalRecords = new ArrayList<>();
    private byte[] entireMessage;


    static DNSMessage decodeMessage(byte[] messageBytes) throws IOException {
        DNSMessage message = new DNSMessage();
        message.entireMessage = messageBytes;
        ByteArrayInputStream inputStream = new ByteArrayInputStream(messageBytes);

        int questionCount = message.header.getQuestionCount();
        DNSQuestion[] questionArray = new DNSQuestion[questionCount];
        for(int i = 0; i< questionCount; i++) {
            questionArray[i] = DNSQuestion.decodeQuestion(inputStream, message);
        }
        message.questions = questionArray;

        int answerCount = message.header.getAnswerCount();
        for(int i = 0; i < answerCount; i++) {
            message.getAnswers().add(DNSRecord.decodeRecord(inputStream, message));
        }

        int authorityRecordCount = message.header.getAuthorityRecordCount();
        for(int i = 0; i< authorityRecordCount; i++) {
            message.getAuthorityRecords().add(DNSRecord.decodeRecord(inputStream, message));
        }

        int additionalRecordCount = message.header.getAdditionalRecordCount();
        for(int i= 0; i < additionalRecordCount; i ++) {
            message.getAdditionalRecords().add(DNSRecord.decodeRecord(inputStream, message));
        }


        return message;
    }

    /** reads in from the input stream and parses the parts of the domain name and stores them in to string array
     *
     * @param inputStream
     * @return
     */
    String[] readDomainName(InputStream inputStream) throws IOException{

        ArrayList<String> qNameList = new ArrayList<>();
        byte octet = (byte) inputStream.read();
        if(octet == (byte) 0xc0) {
            return readDomainName(inputStream.read());
        }
        else {
            while (octet != 0) {
                StringBuilder currentString = new StringBuilder();
                for (int i = 0; i < octet; i++) {
                    currentString.append((char) inputStream.read());
                }
                qNameList.add(currentString.toString());
                try {
                    octet = (byte) inputStream.read();
                } catch (IOException e) {
                    e.printStackTrace();
                }
            }
        }
        String[] qNameStringList = new String[qNameList.size()];
        for( int j= 0; j < qNameStringList.length; j++) {
            qNameStringList[j] = qNameList.get(j);
        }
        return qNameStringList;
    }

    String[] readDomainName(int firstByte) throws IOException{
        ByteArrayInputStream inputStream = new ByteArrayInputStream(entireMessage);
        inputStream.readNBytes(firstByte);
        return readDomainName(inputStream);
    }

    static DNSMessage buildResponse(DNSMessage request, ArrayList<DNSRecord> answers) {
        DNSMessage response = new DNSMessage();
        response.questions = request.getQuestions();
        response.answers = answers;
        response.additionalRecords = request.getAdditionalRecords();
        response.authorityRecords = request.getAuthorityRecords();
        response.header = DNSHeader.buildResponseHeader(request, response);

        return response;
    }

    /** converts message into bytes to be sent over to the socket
     *
     * @return
     */

    byte[] toBytes() throws IOException{
        ByteArrayOutputStream outputStream = new ByteArrayOutputStream();
        HashMap<String, Integer> domainNameLocations = new HashMap<>();
        header.writeBytes(outputStream);

        for(DNSQuestion question: questions) {
            question.writeBytes(outputStream, domainNameLocations);
        }

        for(DNSRecord answer : answers) {
            answer.writeBytes(outputStream, domainNameLocations);
        }
        for(DNSRecord authorityRecord : authorityRecords) {
            authorityRecord.writeBytes(outputStream, domainNameLocations);
        }
        for(DNSRecord additionalRecord : additionalRecords) {
            additionalRecord.writeBytes(outputStream, domainNameLocations);
        }

        return outputStream.toByteArray();

    }

    /** If this is the first time we've seen this domain name in the packet, write it using DNS encoding, add to hashmap
     * else, pointer is created to where the domain was seen previously.
     */

    static void writeDomainName(ByteArrayOutputStream outputStream, HashMap<String,Integer> domainLocations, String[] domainPieces){
    String url = octetsToString(domainPieces);
    if(domainLocations.containsKey(url)) {
        byte[]offset = HelperFunctions.splitIntoTwoBytes(domainLocations.get(url));
        offset[0] = (byte) (offset[0] | 0xc0);
        try {
            outputStream.write(offset);
        } catch (IOException e) {
            e.printStackTrace();
        }

    }
    //split parts off each domain piece??
    else {
        domainLocations.put(url, outputStream.size());
        for (String s : domainPieces){
            outputStream.write((byte) s.length());
            for(char c : s.toCharArray()){
                outputStream.write((byte) c);
            }
        }
        outputStream.write(0);
    }


    }



    static String octetsToString(String[] octets) {
        StringBuilder url = new StringBuilder();
        for(int i = 0; i < octets.length; i++) {
            url.append(octets[i]);
                if (i < octets.length -1) {
                    url.append('.');
                }
            }
            return url.toString();
        }


    @Override
    public String toString() {
        return "DNSMessage {" +
                "header:" + getHeader().toString() +
                ", questions: " + getQuestions().toString() +
                ", answers: " + getAnswers().toString() +
                ", additional records: " + getAdditionalRecords().toString() +
                ", authority records: " + getAuthorityRecords().toString();

    }


    public DNSHeader getHeader() {
        return header;
    }
    DNSQuestion[] getQuestions() {
        return questions;
    }

    ArrayList<DNSRecord> getAnswers() {
        return answers;
    }

    ArrayList<DNSRecord> getAdditionalRecords() {
        return additionalRecords;
    }
    ArrayList<DNSRecord> getAuthorityRecords() {
        return authorityRecords;
    }
}
