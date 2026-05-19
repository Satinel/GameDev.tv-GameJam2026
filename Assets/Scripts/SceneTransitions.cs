using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitions : MonoBehaviour
{
    [SerializeField] Animator _animator;

    static readonly int NEXT_HASH = Animator.StringToHash("NextScene");
    static readonly int RETRY_HASH = Animator.StringToHash("Reload");

    bool _isBusy;

    public void RequestNextLevel()
    {
        if(_isBusy) { return; }

        _isBusy = true;

        _animator.SetTrigger(NEXT_HASH);
    }

    public void LoadNextSceneAnimationEvent()
    {
        int buildIndex = SceneManager.GetActiveScene().buildIndex + 1;

        if(SceneManager.sceneCountInBuildSettings > buildIndex)
        {
            SceneManager.LoadScene(buildIndex);
        }
        else
        {
            SceneManager.LoadScene(0);
        }
    }

    public void RequestRestartLevel()
    {
        if(_isBusy) { return; }

        _isBusy = true;

        _animator.SetTrigger(RETRY_HASH);
    }

    public void ReloadCurrentSceneAnimationEvent()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
