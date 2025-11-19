using System;
using UnityEngine;
using UnityEngine.UI;
using static PathfindAgent;

public class UserMove : MonoBehaviour
{
    #region VARIABLES
    [SerializeField] PathUser pathUser;
    [SerializeField] Button playButton;
    [SerializeField] Button restartButton;
    [SerializeField] PATHFIND_ALGORITHM algorithm;

    Transform userTransform;
    #endregion

    private void OnEnable()
    {
        playButton.onClick.AddListener(()=>MoveUser(algorithm));
        restartButton.onClick.AddListener(()=>GoBack(algorithm));
    }


    private void OnDisable()
    {
        playButton.onClick.RemoveAllListeners();
        restartButton.onClick.RemoveAllListeners();
    }
    private void Start()
    {
        userTransform = GetComponent<Transform>();
    }

    public void MoveUser(PATHFIND_ALGORITHM algorithm)
    {
        Path userPath = pathUser.GetPathWithAlgorithm(algorithm);
        foreach (PathNode node in userPath.pathNodes) 
        {
            userTransform.position = node.transform.position; // EVITAR EL TELETRANSPORTE, PERO LA LISTA ES ESA
        }
    }
    private void GoBack(PATHFIND_ALGORITHM algorithm)
    {
        
    }
}
