import java.lang.reflect.Array;
import java.util.ArrayList;

public class VolumeAdjuster implements AudioComponent {

    private double scale_;

    public AudioClip inclip;


    public VolumeAdjuster(double scale) {
    scale_ = scale;


    }
    @Override
    public AudioClip getClip(){

        AudioClip volClip = new AudioClip();

        for (int i = 0; i <(AudioClip.Duration * AudioClip.sampleRate); i++ ){
            int summed = (int) (scale_ * (inclip.getSample(i)));
            if (summed > Short.MAX_VALUE){
                summed = Short.MAX_VALUE;
            }
            else if (summed < Short.MIN_VALUE) {
                summed = Short.MIN_VALUE;
            }

            volClip.setSample(i, summed);
        }

        return volClip;
    }


    @Override
    public boolean hasInput() {
        return true;
    }

    @Override
    public void connectInput(int index, AudioComponent input) {
      inclip=input.getClip();

    }


}



