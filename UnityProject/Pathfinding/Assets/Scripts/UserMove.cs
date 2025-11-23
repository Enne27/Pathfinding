using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using static PathfindAgent;

public class UserMove : MonoBehaviour
{
    #region VARIABLES
    [SerializeField] PathUser pathUser;
    [SerializeField] Button playButton;
    [SerializeField, FormerlySerializedAs("restartButton")] Button goBackButton;
    [SerializeField] Button restartPositionButton;
    [SerializeField] PATHFIND_ALGORITHM algorithm;

    [SerializeField] float speed = 2f;

    Transform userTransform;
    Coroutine moveCoroutine;

    List<PathNode> visitedNodes = new List<PathNode>();
    Vector3 originalPos;
    bool goingBack = false;
    #endregion

    private void OnEnable()
    {
        playButton.onClick.AddListener(()=>StartUserMovement(algorithm));
        goBackButton.onClick.AddListener(()=>GoBack(algorithm));
        restartPositionButton.onClick.AddListener(ReturnToOriginalPos);
    }

    private void OnDisable()
    {
        playButton.onClick.RemoveAllListeners();
        goBackButton.onClick.RemoveAllListeners();
        restartPositionButton.onClick.RemoveAllListeners();
    }
    private void Start()
    {
        userTransform = GetComponent<Transform>();
        originalPos = userTransform.position;
    }

    void ReturnToOriginalPos() 
    { 
        StopAllCoroutines();
        transform.position = originalPos; 
        visitedNodes.Clear();
        goingBack = false;
    }

    public void StartUserMovement(PATHFIND_ALGORITHM algorithm)
    {
        Path userPath = pathUser.GetPathWithAlgorithm(algorithm);
        
        if (userPath != null) 
        {
            goingBack = false;
            visitedNodes.Clear();
            if (moveCoroutine != null) StopCoroutine(moveCoroutine);
            moveCoroutine = StartCoroutine(FollowPath(userPath));
        }
    }

    private IEnumerator FollowPath(Path userPath)
    {
        foreach (PathNode node in userPath.pathNodes) 
        {
            Vector3 target = node.transform.position;
            while (Vector3.Distance(userTransform.position, target) > 0.05f)
            {
                userTransform.position = Vector3.MoveTowards(
                    userTransform.position,
                    target,
                    speed * Time.deltaTime
                );
                yield return null; // Espera 1 frame antes de moverse.
            }
            if(!goingBack) 
                visitedNodes.Add(node);
        }
    }

    private void GoBack(PATHFIND_ALGORITHM algorithm)
    {
        if (visitedNodes.Count == 0) return;

        if (moveCoroutine != null) StopCoroutine(moveCoroutine);

        goingBack = true;
        visitedNodes.RemoveAt(visitedNodes.Count - 1);

        // Path invertido
        PathNode[] reversedNodes = visitedNodes.ToArray();
        Array.Reverse(reversedNodes);

        Path reversedPath = new Path(reversedNodes);

        moveCoroutine = StartCoroutine(FollowPath(reversedPath));
    }
}
