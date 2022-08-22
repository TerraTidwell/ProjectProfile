
import java.io.IOException;
import java.net.*;
import java.util.ArrayList;

public class DNSServer {

    protected  DatagramSocket datagramSocket;

    void readPacket() throws IOException {
        System.out.println("Server listening...");
        datagramSocket = new DatagramSocket(8053);
        byte[] receivedData = new byte[512];
        DNSCache recordCache = new DNSCache();

        while(true) {
            boolean validDomainName = true;
            DatagramPacket receivedPacket = new DatagramPacket(receivedData, receivedData.length);
            datagramSocket.receive(receivedPacket);
            InetAddress clientIP = receivedPacket.getAddress();
            int clientPort = receivedPacket.getPort();
            System.out.println("Data Received");

            DNSMessage queryMessage = DNSMessage.decodeMessage(receivedData);

            ArrayList<DNSRecord> answerArray = new ArrayList<>();
            for (DNSQuestion question : queryMessage.getQuestions()) {
                DNSRecord answerToQues = recordCache.queryCatch(question);
                if (answerToQues == null) {
                    System.out.println("Didn't find answer, going to google...");
                    DNSMessage googleResponse = sendQueryToGoogle(receivedData);
                    if (googleResponse.getHeader().getResponseCode() == 3) {
                        System.out.println("Domain name doesn't exist---");
                        byte[] responseByteArray = googleResponse.toBytes();
                        DatagramPacket responsePacket = new DatagramPacket(responseByteArray, responseByteArray.length, clientIP, clientPort);
                        datagramSocket.send(responsePacket);
                        validDomainName = false;
                        continue;
                    }
                    answerToQues = googleResponse.getAnswers().get(0);
                    recordCache.addRecord(question, answerToQues);
                }
                answerArray.add(answerToQues);
            }
            if (validDomainName) {
                DNSMessage responseMessage = DNSMessage.buildResponse(queryMessage, answerArray);
                byte[] responseByteArray = responseMessage.toBytes();
                DatagramPacket responsePacket = new DatagramPacket(responseByteArray, responseByteArray.length, clientIP, clientPort);
                datagramSocket.send(responsePacket);
                System.out.println("Packet Sent---\n");
            }
            receivedData = new byte[512];
        }


    }

    DNSMessage sendQueryToGoogle(byte[] message) throws IOException {
        DatagramSocket datagramSocket = new DatagramSocket();
        byte[] googleResponse = new byte[512];
        InetAddress googleIP = InetAddress.getByName("8.8.8.8");
        DatagramPacket queryPacket = new DatagramPacket(message, message.length, googleIP, 53);
        datagramSocket.send(queryPacket);
        DatagramPacket responsePacket = new DatagramPacket(googleResponse, googleResponse.length);
        datagramSocket.receive(responsePacket);
        return DNSMessage.decodeMessage(googleResponse);
    }



}
