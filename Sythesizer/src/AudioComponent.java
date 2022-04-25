public interface AudioComponent {


    AudioClip getClip(); //return the current sound produced by this component
    boolean hasInput(); //can you connect something to this as an input?
    void connectInput(int index, AudioComponent input);
     //connect another device to this input. For most classes implementing
    //this interface, this method will just store a reference to the AudioComponent

}
