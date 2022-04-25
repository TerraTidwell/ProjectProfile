
import javax.sound.sampled.AudioFormat;
import javax.sound.sampled.AudioSystem;
import javax.sound.sampled.Clip;
import javax.sound.sampled.LineUnavailableException;
import javafx.application.Application;


public class Main {


    public static void main(String[] args) throws LineUnavailableException {
        Application.launch(SynthApp.class);
   // create sine waves with frequency
//        SineWave sw1 = new SineWave (524);
//        SineWave sw2 = new SineWave (660);
//        SineWave sw3 = new SineWave (784);
//
//        //initialize the Mixer
//
//
//        Mixer mixer = new Mixer();
//
//        //Pass the mixer the sine waves
//        mixer.connectInput(0, sw1);
//        mixer.connectInput(1,sw2);
//        mixer.connectInput(2, sw3);
//
//        //Initialize VolumeAdjuster the mixer
//        VolumeAdjuster volumeAdjuster = new VolumeAdjuster(1);
//
//        //Pass volume filter
//        volumeAdjuster.connectInput(0, mixer);
//
//        AudioComponent LinearRamp = new LinearRamp(50, 2000);
//        VFSineWave vfSineWave = new VFSineWave();
//
//        vfSineWave.connectInput(0, LinearRamp);
//        playSound(vfSineWave);

    }






    public static void playSound (AudioComponent gen) throws LineUnavailableException{

// Get properties from the system about samples rates, etc.
// AudioSystem is a class from the Java standard library.
        Clip c = AudioSystem.getClip(); //terrible name, different from our AudioClip class

// This is the format that we're following, 44.1 KHz mono audio, 16 bits per sample.
        AudioFormat format16 = new AudioFormat( 44100, 16, 1, true, false );


        AudioClip clip = gen.getClip();


        c.open( format16, clip.getData(), 0, clip.getData().length ); // Reads data from our byte array to play it.

        System.out.println( "About to play..." );
        c.start(); // Plays it.
        c.loop( 2 ); // Plays it 2 more times if desired, so 6 seconds total

// Makes sure the program doesn't quit before the sound plays.
        while( c.getFramePosition() < AudioClip.totalSamples || c.isActive() || c.isRunning() ){
            // Do nothing.
        }

        System.out.println( "Done." );
        c.close();

    }
}