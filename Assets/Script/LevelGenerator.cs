using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [Header("Pipe speed")]
    [SerializeField] private float pipeMoveSpeed;

    [Header("Pipe Heigt")]
    private float minPipeHeight;
    private float maxPipeHeight;
    private float pipeHeight;

    [Header("Pipe distance")]
    [SerializeField] private float minPipeDistance;
    [SerializeField] private float maxPipeDistance;
    private float pipeDistance;

    [Header("Pipe space")]
    [SerializeField] private float minPipeSpace;
    [SerializeField] private float maxPipeSpace;
    private float pipeSpace;

    [SerializeField] private Transform pipeSpawn;

    [SerializeField] private GameObject pipe;

    private List<GameObject> pipes = new();

    private GameObject lastCreatedPipe;

    private bool canPipeMove;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameManager.ChangeGameState(GameState.StartGame);
    }

    private void FixedUpdate()
    {
        SpawnNewPipe();
        MovePipe();
    }

    private void OnEnable()
    {
        GameManager.GameStateChange += UpdateCanMove;
    }

    private void OnDisable()
    {
        GameManager.GameStateChange -= UpdateCanMove;
    }

    private void CreatePipe()
    {
        SpriteRenderer pipeSprite = pipe.GetComponent<SpriteRenderer>();

        float pipeLenght = pipeSprite.size.y;

        float screenBottom = Camera.main.ScreenToWorldPoint(Vector2.zero).y;

        minPipeHeight = screenBottom - pipeLenght / 2 + 1f ;
        maxPipeHeight = screenBottom + pipeLenght / 2 - 0.5f;

        pipeHeight = Random.Range(minPipeHeight, maxPipeHeight);
        pipeSpace = Random.Range(minPipeSpace, maxPipeSpace);

        Vector3 bottomPipePos = new Vector2(pipeSpawn.position.x, pipeHeight);

        pipes.Add(Instantiate(pipe, bottomPipePos, Quaternion.identity));

        float topPipeheight = pipeHeight + pipeLenght + pipeSpace;

        Vector2 topPipePos = new Vector2(pipeSpawn.position.x, topPipeheight);

        GameObject topPipe = Instantiate(pipe, topPipePos, Quaternion.Euler(0,0,180));

        pipes.Add(topPipe);
        lastCreatedPipe = topPipe;

        pipeDistance = Random.Range(minPipeDistance, maxPipeDistance);
    }

    private void SpawnNewPipe()
    {
        if (lastCreatedPipe == null)
        {
            CreatePipe();
        }
        else
        {
            float distance = Mathf.Abs(pipeSpawn.position.x - lastCreatedPipe.transform.position.x);

            if (distance >= pipeDistance)
            {
                CreatePipe();
            }
        }
    }

    private void MovePipe()
    {
        if (!canPipeMove)
            return;

        foreach(GameObject pipe in pipes)
        {
            if(pipe)
                pipe.transform.position = pipe.transform.position + pipeMoveSpeed * Time.fixedDeltaTime * Vector3.left;
        }
    }

    private void UpdateCanMove(GameState state)
    {
        if (state != GameState.StartGame)
            canPipeMove = false;
        else canPipeMove = true;
    }
}
