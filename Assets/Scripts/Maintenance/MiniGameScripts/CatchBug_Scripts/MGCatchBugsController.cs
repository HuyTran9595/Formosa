using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MGCatchBugsController : MonoBehaviour
{
    static int TOTAL_NUMBER_OF_BUGS = 15; //total bug positions on canvas
    static int MIN_BUG_NUMBER = 8; //minimum number of bugs per game
    static int MAX_BUG_NUMBER = 10;//maximum number of bugs per game
    static string END_MESSAGE_GOOD = "The bugs are removed. The plants are flourishing!";
    static string END_MESSAGE_BAD = "You lost the plants.";
    //[SerializeField] Text progress;
    int effective_number_of_bugs;
    bool initiated = false;

    [SerializeField] MGBugs[] bugs = new MGBugs[TOTAL_NUMBER_OF_BUGS];
    [SerializeField] ProgressBar progressBar;
    int numBugsCatched = 0;
    private List<int> bugIndexes = new List<int>();

    private void Start()
    {
        CheckNulls();
    }

    private void CheckNulls()
    {
        if(progressBar == null)
        {
            Debug.LogError("progress bar needed in " + gameObject.name);
        }
        if(bugs == null)
        {
            Debug.LogError(" Bugs array needed in " + gameObject.name);
        }
        if (bugIndexes == null)
        {
            Debug.LogError(" Bugs indexes array needed in " + gameObject.name);
        }
    }

    private void GameEnd(bool endState)
    {
        string message; //message to be printed on finished notification
        if(endState == true)
        {
            message = END_MESSAGE_GOOD;
        }
        else
        {
            message = END_MESSAGE_BAD;
        }
        transform.parent.gameObject.GetComponent<MiniGameCanvasControllerCatchBugs>().EventEnd(endState,message);
        ResetGameState();
        this.gameObject.SetActive(false);//deactivate panel
    }


    public void InitiateGame()
    {
        if (initiated)
        {
            return;
        }

        RandomizeBugIndexes();
        DeactivateNotIncludedIndexes();
        UpdateProgress();
        initiated = true;
        progressBar.SetSize(0f);

        //print out results
       // Debug.Log("effective number of bugs: " + effective_number_of_bugs);
        string toPrint = "list of indexes: ";
        foreach (var x in bugIndexes)
        {
            toPrint += x.ToString() + " ";
        }
        //Debug.Log(toPrint);
       // Debug.Log("number of catched bugs: " + numBugsCatched);


    }

    private void UpdateProgress()
    {
        int currentProgress = effective_number_of_bugs + numBugsCatched - TOTAL_NUMBER_OF_BUGS;
        progressBar.SetSize(1f * currentProgress/effective_number_of_bugs);
    }

    private void DeactivateNotIncludedIndexes()
    {
        //deactivate the bugs that aren't in the bugIndexes
        for (int j = 0; j < TOTAL_NUMBER_OF_BUGS; j++)
        {

            if (!bugIndexes.Contains(j))
            {//if the bug index is not in the bugIndexes list
                bugs[j].gameObject.SetActive(false);
                numBugsCatched++;
            }
        }
    }

    private void RandomizeBugIndexes()
    {
        effective_number_of_bugs = UnityEngine.Random.Range(MIN_BUG_NUMBER, MAX_BUG_NUMBER + 1);
        int i = 0;
        bugIndexes.Clear();
        while (i < effective_number_of_bugs)
        {
            int currentIndex = UnityEngine.Random.Range(0, TOTAL_NUMBER_OF_BUGS);
            if (!bugIndexes.Contains(currentIndex))
            {
                bugIndexes.Add(currentIndex);
                i++;
            }
        }
        //make sure each tree has at least 1 bug
        for (int j = 0; j < 3; j++){
            if(!CheckTreeValid(j)){//if the tree is not valid, i.e. have no bug
                j = 3; //break out of the loop later
                Debug.Log("Tree is not valid, randomize again.");
                RandomizeBugIndexes(); //randomize again
                
            };
        }
    }

    private bool CheckTreeValid(int treeIndex)
    {
        if(treeIndex < 0 || treeIndex > 2){
            return true;
        }
        for (int i = 0; i < 5; i++){
            int currentTreeInd = 5 * treeIndex + i;
            if(bugIndexes.Contains(currentTreeInd)){
                return true;
            }
        }
        //if reached here: there is a tree without a bug
        return false;

    }

    //return true if player catched all the bugs -> win
    //else false
    private bool CheckGameStatus(){
        if(numBugsCatched >= TOTAL_NUMBER_OF_BUGS){
            return true;
        }
        return false;
    }

    public void OnBugCatched(int bugInd){
       // Debug.Log("Bug " + bugInd + " catched.");
        numBugsCatched++;
        UpdateProgress();
        if(CheckGameStatus()){
            StartCoroutine(EndGameAfterTime(true, 1));
        }
    }

    private void ResetGameState()
    {
        //reset variables
        effective_number_of_bugs = 0;
        initiated = false;
        numBugsCatched = 0;

        //reset lists
        foreach(MGBugs bug in bugs)
        {
            bug.gameObject.SetActive(true);
            bug.Reset();
        }
        bugIndexes.Clear();
        progressBar.Reset();
    }

    /// <summary>
    /// End the game after "time"
    ///</summary>
    IEnumerator EndGameAfterTime(bool b, float time)
    {
        yield return new WaitForSeconds(time);
        GameEnd(b); ;
    }
}
