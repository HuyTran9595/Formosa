using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// still need to do: game end not win
public class MGFixWireController : MonoBehaviour
{
    static int NUMBER_OF_BOXES = 8;
    static int[] LeftSideIndexes = {0,2,4,6};
    static int[] RightSideIndexes = {1,3,5,7};
    static float MAX_TIME_IN_SECONDS = 60f;
    static string END_MESSAGE_GOOD = "Congratulation! All animals are matched! The farm is flourishing!";
    static string END_MESSAGE_BAD = "You failed to sort out the animals.";

    static int lineWidth = 2;
    float timeLeft;
    [SerializeField]Text timerText;    

    bool gameInitiated = false;
    [SerializeField]MGWireBox[] boxes = new MGWireBox[NUMBER_OF_BOXES];
    List<GameObject> lines = new List<GameObject>();

    [SerializeField] int matchedBoxes = 0;

    int currentBoxInd = -1;
    int justClickedBoxInd = -1;
    string currentBoxColor;
    string justClickedBoxColor;


    private void Start()
    {
        CheckNulls();
    }

    private void CheckNulls()
    {
        if (timerText == null)
        {
            Debug.LogError("Timer text needed in " + gameObject.name);
        }
        if (boxes == null)
        {
            Debug.LogError("Boxes array needed in " + gameObject.name);
        }
    }

    void Update(){
        UpdateTimer();
    }

    private void UpdateTimer()
    {
        timeLeft -= Time.deltaTime;
        timerText.text = "Time left: " + ((int)timeLeft).ToString();
        if(timeLeft < 0){
            GameEnd(false);
        }   
    }



    //call this when box is clicked, boxIndex is the box's index in boxes array
    public void OnBoxClick(int boxIndex){
        justClickedBoxInd = boxIndex;
        justClickedBoxColor = boxes[justClickedBoxInd].GetColorString();

        //if the just clicked box is already matched, do nothing
        if(boxes[justClickedBoxInd].GetMatchStatus()){
            //Debug.Log("Box "+ justClickedBoxInd + " is already matched." );
            currentBoxInd = justClickedBoxInd;
            currentBoxColor = boxes[currentBoxInd].GetColorString();
            return;
        }

        //Debug.Log("Box "+ justClickedBoxInd + " clicked that has color: " + justClickedBoxColor );

        //first click
        if(currentBoxColor == null){
            currentBoxInd = justClickedBoxInd;
            currentBoxColor = boxes[currentBoxInd].GetColorString();
            return;
        } 
        //not first click


        //if click same box twice, do nothing
        if(justClickedBoxInd == currentBoxInd){
            return;
        }

        //if click different boxes with same color
        if(justClickedBoxColor == currentBoxColor){
           // Debug.Log("Box "+ currentBoxInd + " and " + justClickedBoxInd + " have the same color " + justClickedBoxColor +
            //            ". Therefore they are matched.");
            //mark both boxes as matched
            boxes[currentBoxInd].SetMatchStatus(true);
            boxes[justClickedBoxInd].SetMatchStatus(true);
            //update values
            currentBoxColor = null;
            justClickedBoxColor = null;
            matchedBoxes += 2;
           // Debug.Log(matchedBoxes + " boxes are matched." );


            DrawLine(boxes[currentBoxInd].gameObject.GetComponent<RectTransform>().position,
                     boxes[justClickedBoxInd].gameObject.GetComponent<RectTransform>().position,
                     boxes[justClickedBoxInd].GetColor(), lineWidth);


            if (CheckGameStatus()){
                   StartCoroutine(EndGameAfterTime(true, 1));
            };
            return;

        }
        // if click different boxes with different color
        // update values
        currentBoxInd = justClickedBoxInd;
        currentBoxColor = boxes[justClickedBoxInd].GetColorString();

    }



    /// <summary>
    //  Call the canvas controller parent and notify that the player wins.
    /// </summary>
    private void GameEnd(bool endState)
    {
        string message; //message to be printed on finished notification
        if (endState == true)
        {
            message = END_MESSAGE_GOOD;
        }
        else
        {
            message = END_MESSAGE_BAD;
        }
        transform.parent.gameObject.GetComponent<MiniGameCanvasController>().EventEnd(endState, message);
        ResetGameState();
        this.gameObject.SetActive(false);//deactivate panel
    }

    private bool CheckGameStatus()
    {
        if(matchedBoxes >= 8){
            //Debug.Log("You won. All 8 boxes are matched.");
            return true;
        }
        return false;
    }




    /// <summary>
    //  Get called by MiniGameCanvasController 
    ///</summary>
    public void InitiateGame(){
       // Debug.Log("initiate game called");
        if(!gameInitiated){
            RandomizeBoxPositions(LeftSideIndexes);
            RandomizeBoxPositions(RightSideIndexes);
            timeLeft = MAX_TIME_IN_SECONDS;
            gameInitiated = true;
            
        }
    }

    ///<summary>
    // randomize the box at the start of the game
    ///</summary>
    private void RandomizeBoxPositions(int[] indexArray){
        int size = indexArray.Length;
        for (int randomTimes = 1; randomTimes <= size; randomTimes++){
            int randomIndex1 = UnityEngine.Random.Range(0, size);
            int randomIndex2 = UnityEngine.Random.Range(0, size);
            if(randomIndex1 != randomIndex2){
                SwapBoxPosition(indexArray[randomIndex1], indexArray[randomIndex2]);
            }
        }
    }

    ///<summary>
    // swap position of box index 1 and 2
    ///</summary>
    private void SwapBoxPosition(int ind1, int ind2){
        MGWireBox box1 = boxes[ind1];
        MGWireBox box2 = boxes[ind2];

        Vector3 box1Position = box1.gameObject.GetComponent<RectTransform>().localPosition;
        box1.gameObject.GetComponent<RectTransform>().localPosition = box2.gameObject.GetComponent<RectTransform>().localPosition;
        box2.gameObject.GetComponent<RectTransform>().localPosition = box1Position;
    }

    private bool CheckBoxIndexValid(int ind){
        if(ind < 0 || ind >= NUMBER_OF_BOXES){
            return false;
        }
        return true;
    }

    /// <summary>
    /// reset the whole game state as if the game is completely new
    /// call this function when the game ends, no matter the player wins or loses
    /// </summary>
    private void ResetGameState()
    {
        //reset game variables
        gameInitiated = false;
        timeLeft = MAX_TIME_IN_SECONDS;
        matchedBoxes = 0;
        currentBoxInd = -1;
        justClickedBoxInd = -1;
        currentBoxColor = null;
        justClickedBoxColor = null;

        //reset boxes data
        foreach (MGWireBox box in boxes)
        {
            box.ResetBox();
        }

        //clear all lines
        foreach (GameObject line in lines)
        {
            Destroy(line.gameObject);
        }
        lines.Clear();
    }


    /// <summary>
    /// Draw line (image) between 2 boxes when they are matched.
    /// color is the color of the box
    /// new game objects are created under the game panel
    ///</summary>
    private void DrawLine(Vector3 pointA, Vector3 pointB, Color color, int lineWidth)
    {
        Vector3 differenceVector = pointB - pointA;
        GameObject NewObj = new GameObject("Image Gameobj"); //Create the GameObject
        lines.Add(NewObj);
        NewObj.transform.parent = gameObject.transform;

        Image NewImage = NewObj.AddComponent<Image>();
        NewImage.color = color;
        RectTransform imageRectTransform = NewImage.rectTransform;
        imageRectTransform.sizeDelta = new Vector2(differenceVector.magnitude, lineWidth);
        imageRectTransform.pivot = new Vector2(0, 0.5f);
        imageRectTransform.position = pointA;
        float angle = Mathf.Atan2(differenceVector.y, differenceVector.x) * Mathf.Rad2Deg;
        imageRectTransform.Rotate(new Vector3(0,0,angle));
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
