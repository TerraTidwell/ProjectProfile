import java.util.ArrayList;
import java.util.Collection;

public class Mixer implements AudioComponent{

    private ArrayList <AudioComponent> inputs = new ArrayList<>();
    private AudioClip mixed = new AudioClip();

    public Mixer(){

    }

    @Override
    public AudioClip getClip() {

        for (int i = 0; i < inputs.size(); i++) {
            AudioComponent input = inputs.get(i);
            AudioClip temp = input.getClip();
            for(int j = 0; j < temp.totalSamples; j++) {
                int sample = mixed.getSample(j);
                int sampleAdd = temp.getSample(j);
                int summedSample = sample + sampleAdd;
                if (summedSample > Short.MAX_VALUE){
                    summedSample = Short.MAX_VALUE;
                }
                else if (summedSample < Short.MIN_VALUE) {
                    summedSample = Short.MIN_VALUE;
                }
                mixed.setSample(j, summedSample/2);
            }
        }
                //add all inputs together


            return mixed;
    }

    @Override
    public boolean hasInput() {
        return true;
    }

    @Override
    public void connectInput(int index, AudioComponent input) {

        inputs.add(index, input);
    }
}
