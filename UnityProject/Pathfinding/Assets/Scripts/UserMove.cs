using UnityEngine;
using UnityEngine.UI;
using static PathfindAgent;

public class UserMove : MonoBehaviour
{
    #region VARIABLES
    [SerializeField] PathUser pathUser;
    [SerializeField] Button playButton;
    [SerializeField] Button restartButton;
    #endregion

    private void OnEnable()
    {
        //playButton.onClick.AddListener();
        //restartButton.onClick.AddListener();
    }

    private void OnDisable()
    {
        playButton.onClick.RemoveAllListeners();
        restartButton.onClick.RemoveAllListeners();
    }

    public void MoveUser(PATHFIND_ALGORITHM algorithm)
    {
        pathUser.GetPathWithAlgorithm(algorithm);
    }
}
