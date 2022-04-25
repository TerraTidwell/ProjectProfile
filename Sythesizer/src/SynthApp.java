

import javafx.application.Application;
import javafx.event.ActionEvent;
import javafx.event.EventHandler;
import javafx.geometry.Insets;
import javafx.geometry.Pos;
import javafx.scene.Scene;
import javafx.scene.control.Button;
import javafx.scene.control.Slider;
import javafx.scene.layout.*;
import javafx.scene.paint.Color;
import javafx.scene.shape.Circle;
import javafx.scene.text.Text;
import javafx.stage.Stage;
import javax.sound.sampled.*;
import java.util.ArrayList;

public class SynthApp extends Application {

    class AudioListener implements LineListener {

    public AudioListener (Clip myClip) {clip_ = myClip;}
        @Override
        public void update (LineEvent event) {
            if( event.getType() == LineEvent.Type.STOP) {
            System.out.println("close clip");
            clip_.close();
             }
        }
        private Clip clip_;
    }
    public SynthApp() {
        System.out.println("SynthApp()");
    }

    private void createPiano (Pane root) {
        Button btn = new Button();
        btn.setText("G");
        btn.setOnAction(e -> this.playNote(0));
        root.getChildren().add( btn);
        //new note
        btn = new Button();
        btn.setText("A");
        btn.setOnAction(e -> this.playNote(1));
        root.getChildren().add( btn);
        //new note
        btn = new Button();
        btn.setText("B");
        btn.setOnAction(e -> this.playNote(2));
        root.getChildren().add( btn);
    //new note
        btn = new Button();
        btn.setText("C");
        btn.setOnAction(e -> this.playNote(3));
        root.getChildren().add( btn);
        //new note
        btn = new Button();
        btn.setText("D");
        btn.setOnAction(e -> this.playNote(4));
        root.getChildren().add( btn);
        //new note
        btn = new Button();
        btn.setText("E");
        btn.setOnAction(e -> this.playNote(5));
        root.getChildren().add( btn);
        //new note
        btn = new Button();
        btn.setText("F");
        btn.setOnAction(e -> this.playNote(6));
        root.getChildren().add( btn);
        //new chord
        btn = new Button();
        btn.setText("Chord");
        btn.setOnAction(e -> this.playNote(7));
        root.getChildren().add( btn);
        //new weird noise
        btn = new Button();
        btn.setText("Weird Noise");
        btn.setOnAction( e -> this.playNote(8));
        root.getChildren().add(btn);




    }
    private void lineListener (Clip c) {

    }

    private void playNote(int i) {
        System.out.println("playNote: " + i);
        VolumeAdjuster volume = new VolumeAdjuster(volume_);

        AudioClip clip = null;
        if( i == 0) {
            volume.connectInput(0, G_);
            clip = volume.getClip();
        }
        else if(i == 1){
            volume.connectInput(0, A_);
            clip = volume.getClip();
        }
        else if(i == 2){
            volume.connectInput(0, B_);
            clip = volume.getClip();
        }
        else if(i == 3){
            volume.connectInput(0, C_);
            clip = volume.getClip();
        }
        else if(i == 4){
            volume.connectInput(0, D_);
            clip = volume.getClip();
        }
        else if(i == 5){
            volume.connectInput(0, E_);
            clip = volume.getClip();
        }
        else if(i == 6){
            volume.connectInput(0, FSharp_);
            clip = volume.getClip();
        }
        else if(i==7){
            Mixer mixer = new Mixer();
            mixer.connectInput(0, G_);
            mixer.connectInput(1, C_);
            mixer.connectInput(2, E_);
            volume.connectInput(0, mixer);
            clip = volume.getClip();
        }
        else{
            AudioComponent LinearRamp = new LinearRamp(50, 2000);
            VFSineWave vfSineWave = new VFSineWave();
            vfSineWave.connectInput(0, LinearRamp);
            volume.connectInput(0, vfSineWave);
            clip = volume.getClip();

        }
        try {
            AudioFormat format16 = new AudioFormat( 44100, 16, 1, true, false );
            Clip c = AudioSystem.getClip();

            AudioListener listener = new AudioListener(c);
            c.open( format16, clip.getData(), 0, clip.getData().length );
            c.start();
            c.addLineListener(listener);
        }
        catch (LineUnavailableException e){
            e.printStackTrace();
        }
    }




    private AnchorPane mainScreen_ = new AnchorPane();

    public static Circle speaker_;
    public static ArrayList<AudioComponent> speakerConnections_ = new ArrayList<>();

    @Override
    public void start(Stage primaryStage) throws Exception {
        System.out.println("start");
        primaryStage.setTitle("Terra's Synthesizer");

        BorderPane root = new BorderPane();
        HBox bottom = new HBox();
        createPiano(bottom);
        bottom.setPadding(new Insets(10));
        bottom.setSpacing(50);

        VBox left = new VBox();
        createAudioCompMenu(left);
        left.setPadding(new Insets(10));
        left.setSpacing(50);

        HBox top = new HBox();
        top.setPadding(new Insets(10));
        top.setSpacing(50);


        VBox right = new VBox();
        right.setPadding(new Insets (10));

        root.setTop(top);
       root.setRight(right);
       root.setBottom(bottom);
       root.setLeft(left);
       root.setCenter(mainScreen_);

       mainScreen_.setStyle("-fx-background-color: LIGHTBLUE");

       //SPEAKER CREATOR
        speaker_ = new Circle(25);
        speaker_.setFill(Color.BLACK);
        mainScreen_.getChildren().add(speaker_);
        speaker_.setLayoutX(600);
        speaker_.setLayoutY(200);



       primaryStage.setScene(new Scene (root, 800, 500));
       primaryStage.show();

       Slider volSlider = new Slider();
       volSlider.setMin(0);
       volSlider.setMax(1);
       volSlider.setValue(0.5);
       volSlider.setShowTickLabels(true);
       Text Volume = new Text("Volume");
       volSlider.setOnDragDetected(event -> handleSlider(volSlider));

        Button playBtn = new Button("Play");
        playBtn.setOnAction(e-> playNetwork());
        top.getChildren().add(playBtn);
        top.getChildren().add(volSlider);
        top.getChildren().add(Volume);
        top.setAlignment(Pos.CENTER);

    }

    private void handleSlider(Slider volSlider) {
        double value = volSlider.getValue();
        System.out.println(value);
        volume_= value;
    }


    private void playNetwork() {
        System.out.println("Play Network");
        for (AudioComponent ac : speakerConnections_) {
            Clip c = null;
            try {
                System.out.println("Playing " + ac.toString());
                AudioFormat format16 = new AudioFormat( 44100, 16, 1, true, false );
                c = AudioSystem.getClip();

                AudioListener listener = new AudioListener(c);
                VolumeAdjuster volume = new VolumeAdjuster(volume_);
                volume.connectInput(0, ac);

                byte[] data = volume.getClip().getData();

                c.open( format16, data, 0, data.length );
                c.start();
                c.addLineListener(listener);
            }
            catch (LineUnavailableException e) {
                e.printStackTrace();
                return;

            }
        }
    }
private void createAudioCompMenu (VBox left) {
        left.setPadding(new Insets(10, 10, 10, 10));
        left.setSpacing(50);

        Button swb = new Button("Sine Wave AC");
        left.getChildren().add(swb);

        swb.setOnAction(e-> createAcComponent("SineWave"));

}

   private void createAcComponent (String componentName) {
        switch (componentName) {
            case "SineWave":
                SineWave sw = new SineWave(440);
                AudioComponentWidget acw = new AudioComponentWidget (sw, mainScreen_, "SineWave");
                break;

        }
   }




    private double volume_ = 0.5;
    SineWave G_ = new SineWave (392);
    SineWave A_ = new SineWave (440);
    SineWave B_ = new SineWave (494);
    SineWave C_ = new SineWave (524);
    SineWave D_ = new SineWave (587);
    SineWave E_ = new SineWave (670);
    SineWave FSharp_ = new SineWave (740);


}
