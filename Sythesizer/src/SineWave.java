import java.util.Random;

public class SineWave implements AudioComponent{


    private double frequency_;
    private AudioClip sineClip = new AudioClip();

    public SineWave() {

    }

    public SineWave(double frequency) {
        frequency_ = frequency;

    }

    @Override
    public AudioClip getClip() {

        Random rand = new Random();
        short maxValue = Short.MAX_VALUE/2;

        for(int i = 0; i < (AudioClip.Duration * AudioClip.sampleRate); i++) {
            sineClip.setSample(i, (int) (maxValue * Math.sin(2 * Math.PI * frequency_ * i / AudioClip.sampleRate)));
        }
        return sineClip;
    }

    @Override
    public boolean hasInput() {
        return false;
    }

    @Override
    public void connectInput(int index, AudioComponent input) {
      sineClip = input.getClip();
    }


    //This class should take the desired frequency (pitch) as a constructor parameter
    // (you'll eventually want getters and setters for the frequency as well, but we'll save that for later).

//GetClip method. Create and fill in an AudioClip with sample values from a sine wave
    //pseudocode math: sample[i] = maxValue * sine(2*pi*frequency * i / sampleRate);
//440 is an A
    //maxValue controls the loudness of the wave, Short.Max=loud
}
