public class LinearRamp implements AudioComponent{
    private float start_;
    private float stop_;
    private AudioClip linClip = new AudioClip();

    public LinearRamp(){

    }

    public LinearRamp(float start, float stop){
        start_ = start;
        stop_ = stop;
    }
    @Override
    public AudioClip getClip() {


        for (int i = 0; i <(AudioClip.Duration * AudioClip.sampleRate); i++ ){
            linClip.setSample(i, (int) (start_ * (AudioClip.sampleRate - i) + stop_ * i)/ AudioClip.sampleRate);
        }
        return linClip;
    }

    @Override
    public boolean hasInput() {
        return false;
    }

    @Override
    public void connectInput(int index, AudioComponent input) {
        linClip = input.getClip();
    }
}
