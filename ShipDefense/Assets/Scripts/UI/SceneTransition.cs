using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles the behavior of the UI that provides a smooth transition between acts
/// </summary>
public class SceneTransition : MonoBehaviour
{
    [SerializeField] private Animator transitonAnimator;
    [SerializeField] private TMP_Text actText;

    private void Awake()
    {
        actText.text = SceneManager.GetActiveScene().name;
    }


    public void TriggerSceneExit()
    {
        if(SceneManager.GetActiveScene().name == "Act3")
        {
            FindFirstObjectByType<GameManager>().WinGame();
        } else
        {
            transitonAnimator.SetTrigger("SceneExit");
            StartCoroutine(TransitionSceneCoroutine());
        }
    }

    private IEnumerator TransitionSceneCoroutine()
    {
        yield return new WaitForSeconds(1);
        FindFirstObjectByType<GameManager>().WinGame();
    }
}
