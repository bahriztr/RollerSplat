using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private int paintedGroundCount = 0;
    private int level = 0;

    [SerializeField] private List<int> groundList = new List<int>();
    [SerializeField] private List<GameObject> levelList = new List<GameObject>();
    [SerializeField] private List<Renderer> paintedGroundList = new List<Renderer>();

    public static GameManager Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }
    public void PaintedGround(Renderer rend)
    {
        paintedGroundList.Add(rend);
        paintedGroundCount++;
        if(paintedGroundCount >= groundList[level])
            LevelCompleted();
    }

    public void LevelCompleted()
    {
        StartCoroutine(nameof(LevelEndCoroutine));
    }
    IEnumerator LevelEndCoroutine()
    {
        yield return new WaitForSeconds(1);
        levelList[level].SetActive(false);

        foreach(Renderer rend in paintedGroundList)
            rend.material.color = Color.white;

        paintedGroundList.Clear();
        paintedGroundCount = 0;
        Player.Instance.PlayerReset();
        level++;

        if (level >= levelList.Count)
            level = 0;

        levelList[level].SetActive(true);
    }


}
