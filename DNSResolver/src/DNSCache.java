import java.util.HashMap;
import java.io.IOException;


public class DNSCache {

    HashMap<DNSQuestion, DNSRecord> cache = new HashMap<DNSQuestion, DNSRecord>();
    /**stores first answer for any question in the cache
     * too old an entry will return an error and remove it
     */

    void addRecord(DNSQuestion question, DNSRecord response) {
        cache.put(question, response);
        System.out.println("Response added to cache!");

    }

    /**checks to see if the cache exists, verifies TTL
     *
     * @param question
     * @return null if answer not found, returns cache if TTL is valid, removes question if not valid
     * @throws IOException
     */

    DNSRecord queryCatch(DNSQuestion question) throws IOException {

        if (cache.containsKey(question)) {
            System.out.println("Answer found in cache!");
            if(cache.get(question).timestampValid()) {
                System.out.println("TTL valid!");
                return cache.get(question);
            }
            else {
                cache.remove(question);
                System.out.println("TTL not valid!");
            }
        }
        return null;
    }
}
