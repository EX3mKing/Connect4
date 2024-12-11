using System;
using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject redToken;
    [SerializeField] private GameObject yellowToken;
    [SerializeField] private GameObject winPlayer1;
    [SerializeField] private GameObject winPlayer2;
    [SerializeField] private GameObject endCanvas;
    
    [Header("Settings")]
    [Tooltip("Speed of the token falling")]
    [SerializeField] private float speed = 1.0f;

    public Transform bottomLeftCorner;
    public Transform bottomLeftCornerOneUp;
    public Transform bottomLeftCornerOneRight;
    public float radius;
    
    [HideInInspector] public bool gameend;
    
    int currentPlayer = 1;
    const int width = 7;
    const int height = 6;
    int[][] space = new int[width][];
    
    public float startingHeight;
    
    private float startingX;
    private float offsetUp;
    private float offsetRight;

    public bool isHostsTurn = true;
    public bool isHost;

    private void Start()
    {
        startingX = bottomLeftCorner.position.x;
        offsetUp = bottomLeftCornerOneUp.position.y - bottomLeftCorner.position.y;
        offsetRight = bottomLeftCornerOneRight.position.x - startingX;
        
        for (int i = 0; i < 7; i++)
        {
            space[i] = new int[6];
        }
        
        MultiplayerManager.Singleton.input.AddListener(mmdroptoken);
        isHost = NetworkManager.Singleton.IsHost;
    }
    
    private void DropToken(int column, int player)
    {
        if (space[column][height - 1] != 0 || gameend)
        {
            return;
        }
        
        GameObject token;
        
        for (int i = 0; i < height; i++)
        {
            if (space[column][i] == 0)
            {
                space[column][i] = player;

                Vector3 spawnPosition = new Vector3();
                spawnPosition.x = startingX + offsetRight * column;
                spawnPosition.y = startingHeight;
                
                if (player == 1)
                {
                    token = Instantiate(redToken, spawnPosition, Quaternion.identity);
                }
                else
                {
                    token = Instantiate(yellowToken, spawnPosition, Quaternion.identity);
                }

                StartCoroutine(TokenFall(token, offsetUp * i));
                break;
            }
        }
        
        CheckWinState();
        currentPlayer = currentPlayer == 1 ? 2 : 1;
    }

    private void CheckWinState()
    {
        bool win = false;
        for (int h = 0; h < height; h++)
        {
            for (int w = 0; w < width - 3; w++)
            {
                if (space[w][h] == currentPlayer && space[w + 1][h] == currentPlayer 
                        && space[w + 2][h] == currentPlayer && space[w + 3][h] == currentPlayer)
                {
                    win = true;
                }
            }
        }
        
        for (int h = 0; h < height - 3; h++)
        {
            for (int w = 0; w < width; w++)
            {
                if (space[w][h] == currentPlayer && space[w][h + 1] == currentPlayer 
                        && space[w][h + 2] == currentPlayer && space[w][h + 3] == currentPlayer)
                {
                    win = true;
                }
            }
        }
        
        for (int h = 0; h < height - 3; h++)
        {
            for (int w = 0; w < width - 3; w++)
            {
                if (space[w][h] == currentPlayer && space[w + 1][h + 1] == currentPlayer 
                        && space[w + 2][h + 2] == currentPlayer && space[w + 3][h + 3] == currentPlayer)
                {
                    win = true;
                }
            }
        }
        
        for (int h = 0; h < height - 3; h++)
        {
            for (int w = 3; w < width; w++)
            {
                if (space[w][h] == currentPlayer && space[w - 1][h + 1] == currentPlayer 
                        && space[w - 2][h + 2] == currentPlayer && space[w - 3][h + 3] == currentPlayer)
                {
                    win = true;
                }
            }
        }
        
        if (win)
        {
            Debug.Log("Player " + currentPlayer + " wins");
            if (currentPlayer == 1) winPlayer1.SetActive(true);
            else winPlayer2.SetActive(true);
            endCanvas.SetActive(true);
            gameend = true;
        }
        
    }

    private IEnumerator TokenFall(GameObject token, float targetHeight)
    {
        float velocity = 0;
        
        while (token.transform.position.y > targetHeight)
        {
            token.transform.position = new Vector3(token.transform.position.x, token.transform.position.y - velocity, token.transform.position.z);
            velocity += Time.deltaTime * speed;
            yield return new WaitForEndOfFrame();
        }
        
        token.transform.position = new Vector3(token.transform.position.x, targetHeight, token.transform.position.z);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(bottomLeftCorner.position, radius);
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(bottomLeftCornerOneUp.position, radius);
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(bottomLeftCornerOneRight.position, radius);
    }
    
    private void mmdroptoken(int column)
    {
        DropToken(column - 1, currentPlayer);
        isHostsTurn = !isHostsTurn;
    }

    public bool CanDrop()
    {
        return isHost == isHostsTurn;
    }
}
