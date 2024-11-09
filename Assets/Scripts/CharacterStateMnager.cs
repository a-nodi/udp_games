using UnityEngine;

public class CharacterStateMnager : MonoBehaviour
{
    public enum States
    {
        Walking,
        Running,
        Found,
        Jumping
    }

    public Animator animator;

    private States state = States.Walking;

    public States getState(){
        return state;
    }

    public bool setState(States state){
        // set the character's state.
        // return true if the state is changed succesfully, otherwise false.
        switch (state){
            case States.Walking:
                // write code here
                // ex1) check state now before changing it if the state can really be changed
                // ex2) change animation state of the character object.
                break;
            case States.Running:
                // ...
                break;
            // and so on...
        }
        return true;
    }

    public void setAnimatorState(States state){
        // 박예찬이 짤거
    }
}
