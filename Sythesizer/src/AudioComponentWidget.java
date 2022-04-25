import java.awt.*;
import java.util.ArrayList;
import java.util.Collections;
import java.util.Random;

import javafx.scene.control.Button;
import javafx.geometry.Bounds;
import javafx.scene.control.Slider;
import javafx.scene.input.MouseEvent;
import javafx.scene.layout.*;
import javafx.scene.paint.Color;
import javafx.geometry.Pos;
import javafx.scene.shape.Circle;
import javafx.scene.shape.Line;
import javafx.scene.text.Text;
import javafx.geometry.Insets;
import javafx.scene.text.TextAlignment;
import javafx.scene.text.Font;


public class AudioComponentWidget extends Pane {

    private AnchorPane parent_;
    private HBox baseLayout_ = new HBox();
    private AudioComponent audioComp_ = null;
    private String name_ = "NAME NOT INITIALIZED";
    private Line line_ = null;


    AudioComponentWidget(AudioComponent ac, AnchorPane parent, String componentName) {
        parent_= parent;
        audioComp_ = ac;
        baseLayout_.setStyle( "-fx-background-color: WHITE");

        baseLayout_.setBorder(new Border(new BorderStroke(Color.BLACK,BorderStrokeStyle.SOLID, CornerRadii.EMPTY, new BorderWidths(3))));
        VBox rightSide = new VBox();


        Button closeBtn = new Button( "x");
        closeBtn.setAlignment(Pos.TOP_LEFT);

        closeBtn.setOnAction(e -> close());

        HBox botSide = new HBox();
        Slider freqSlider = new Slider();
        freqSlider.setMax(1000);
        freqSlider.setMin(0);
        freqSlider.setValue(440);
        freqSlider.setShowTickLabels(true);
        Text frequency = new Text("Frequency");
        freqSlider.setOnDragDetected(event -> handleSlider(freqSlider));

        Circle outputCircle = new Circle (10);
        outputCircle.setFill(Color.BLUE);
        outputCircle.setOnMousePressed( e -> startConnection(e, outputCircle));
        outputCircle.setOnMouseDragged( e-> moveConnection (e, outputCircle));
        outputCircle.setOnMouseReleased(e -> stopConnecting( e, outputCircle));

        rightSide.setAlignment(Pos.CENTER);
        rightSide.setPadding(new Insets(15));
        rightSide.setLayoutX(50);
        rightSide.setSpacing(10);
        rightSide.getChildren().add(closeBtn);
        rightSide.getChildren().add(outputCircle);

        Font  f4  = new Font("Serif Bold Italic",15 );

        VBox leftSide = new VBox();
        Text title = new Text(componentName);
        title.setTextAlignment(TextAlignment.JUSTIFY);
        title.setFont(f4);
        title.setOnMouseDragged(e -> handleMove( e ));


        leftSide.setAlignment(Pos.CENTER);
        leftSide.getChildren().add(title);

        if (componentName == "SineWave") {
            leftSide.getChildren().add(freqSlider);
        }

        //Add in all the pieces
        baseLayout_.getChildren().add(rightSide);
        baseLayout_.getChildren().add(leftSide);
        baseLayout_.getChildren().add(botSide);

        //Get us on the screen
        this.getChildren().add(baseLayout_);
        parent_.getChildren().add(this);

        Random r = new Random();
        double randomTopValue = 300 * r.nextDouble();
        double randomLeftValue = 300 * r.nextDouble();

        AnchorPane.setLeftAnchor(this, randomTopValue);
        AnchorPane.setTopAnchor(this, randomLeftValue);
    }


private void handleSlider (Slider freqSlider) {
double value = freqSlider.getValue();
    System.out.println(value);
    audioComp_ = new SineWave(value);

}




    private void stopConnecting(MouseEvent e, Circle outputCircle) {
        Circle speaker = SynthApp.speaker_;

        Bounds bounds = speaker.localToScene(speaker.getBoundsInLocal());

        double distance = Math.sqrt(Math.pow(bounds.getCenterX() - e.getSceneX(), 2.0) + Math.pow(bounds.getCenterX() - e.getSceneX(), 2.0));
        if (distance < 20) {
            //made a connection
            SynthApp.speakerConnections_.add(audioComp_);
            System.out.println("added ac comp to speakerconnection");
        } else {

            parent_.getChildren().remove(line_);
        }
    }
    private void moveConnection (MouseEvent e, Circle outputCircle) {
        if(line_ != null) {
            Bounds bounds = parent_.getBoundsInParent();
            line_.setEndX(e.getSceneX()- bounds.getMinX());
            line_.setEndY(e.getSceneY()- bounds.getMinY());
        }
    }

    private void startConnection (MouseEvent e, Circle outputCircle) {
            if(line_ != null){
                parent_.getChildren().remove(line_);
                SynthApp.speakerConnections_.remove(audioComp_);
            }

        Bounds bounds = parent_.getBoundsInParent();
        line_= new Line();
        line_.setStrokeWidth(3);
        line_.setStartX(e.getSceneX() - bounds.getMinX());
        line_.setStartY(e.getSceneY() - bounds.getMinY());
        line_.setEndX (e.getSceneX() - bounds.getMinX());
        line_.setEndY (e.getSceneY() - bounds.getMinY());

        parent_.getChildren().add(line_);
    }

    private void handleMove(MouseEvent e) {
        Bounds bounds = parent_.getBoundsInParent();
        AnchorPane.setTopAnchor(this, e.getSceneY() - bounds.getMinY());
        AnchorPane.setLeftAnchor(this, e.getSceneX() - bounds.getMinX());

    }
    private void close() {
        parent_.getChildren().remove(this);

        if(line_ != null) {
            parent_.getChildren().remove(line_);
            SynthApp.speakerConnections_.remove(audioComp_);

        }
    }


}
