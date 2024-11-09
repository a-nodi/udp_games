using UnityEngine;

public class CharacterStateMnager
{
    public enum States
    {
        Walking,
        Running,
        Found,
        Jumping
    }

    private States state = Idle;

    public States getState(){
        return state
    }

    public void setState(States state){
        switch (state){
            case == Walking:
                // write code here
                // ex1) check state now before changing it if the state can really be changed
                // ex2) change animation state of the character object.
                break;
            case == Running:
                // ...
                break;
            // and so on...
        }
    }
}
