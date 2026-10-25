using UnityEngine;

public class SpawnTetris : MonoBehaviour
{
    public GameObject[] tetrisPrefabs;
    public RectTransform spawnParent;   // drag the Board object here

    void Start()
    {
        NewTetris();
    }

    public void NewTetris()
   
{
    int randomIndex = Random.Range(0, tetrisPrefabs.Length);
    GameObject newBlock = Instantiate(tetrisPrefabs[randomIndex], spawnParent);

    RectTransform blockRect = newBlock.GetComponent<RectTransform>();
    blockRect.localRotation = Quaternion.identity;
    blockRect.anchoredPosition = new Vector2(0f, 175f);

    tetrisblock block = newBlock.GetComponent<tetrisblock>();
    block.board = spawnParent;
    block.spawner = this;
    block.AlignToTop();

    // Game over: the new piece overlaps locked cells, so reset and start again
    if (block.IsOverlapping())
    {
        Destroy(newBlock);
        tetrisblock.ResetGame();
        NewTetris();
    }
}
}