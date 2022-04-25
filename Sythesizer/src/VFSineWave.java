public class VFSineWave implements AudioComponent{

    private AudioComponent input_;
    private AudioClip sample_ = new AudioClip();

    public VFSineWave(){

    }

    @Override
    public AudioClip getClip() {

        sample_ = input_.getClip();
        short maxValue = Short.MAX_VALUE;
        double phase = 0;
        for (int i = 0; i <(AudioClip.Duration * AudioClip.sampleRate); i++ ) {
            phase += 2 * Math.PI * sample_.getSample(i)/AudioClip.sampleRate;
            sample_.setSample(i, (int) (maxValue * Math.sin(phase)));
            System.out.println(i + ":" + sample_.getSample(i));
        }
        return sample_;
    }

    @Override
    public boolean hasInput() {
        return true;
    }

    @Override
    public void connectInput(int index, AudioComponent input) {
        input_ = input;
    }
}
