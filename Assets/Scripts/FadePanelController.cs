using System.Collections;
using UnityEngine;

public class FadePanelController : MonoBehaviour
{

    public Animator panelAnimator;
    public Animator gameIngoAnimator;

    public void OK()
    {
        if(panelAnimator != null && gameIngoAnimator != null)
        {
            panelAnimator.SetBool("Out", true);
            gameIngoAnimator.SetBool("Out", true);
            StartCoroutine(GameStartCo());
        } 
    }

    IEnumerator GameStartCo()
    {
        yield return new WaitForSeconds(1f);
        Board board = FindFirstObjectByType<Board>();
        board.currState = GameState.move;
    }
}
