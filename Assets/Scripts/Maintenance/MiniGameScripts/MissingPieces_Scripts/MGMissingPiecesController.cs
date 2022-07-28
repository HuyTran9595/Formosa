using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MGMissingPiecesController : MonoBehaviour
{
    static int NUMBER_OF_MATCH_BOXES = 6;
    static int NUMBER_OF_IMAGES = 9;
    static int NUMBER_OF_MISSING_BOXES = 3;
    static int NUMBER_OF_POSSIBLE_GAMES = 5; //currently there is only 1 picture 
    static string END_MESSAGE_GOOD = "Congratulation! The picture is fixed";
    static string END_MESSAGE_BAD = "You failed to fix the picture.";

    [SerializeField] MatchBox[] matchBoxes = new MatchBox[NUMBER_OF_MATCH_BOXES];
    [SerializeField] ImageTransfer[] movingImages = new ImageTransfer[NUMBER_OF_MATCH_BOXES];
    [SerializeField] MatchBox[] allMissingBoxes = new MatchBox[NUMBER_OF_IMAGES];
    [SerializeField] GameObject[] allMissingBoxesBackground = new GameObject[NUMBER_OF_IMAGES];
    [SerializeField] ImageSet[] imageSets = new ImageSet[NUMBER_OF_POSSIBLE_GAMES];
    [SerializeField] Image correctImage;

    int imageSetIndex = -1;
    ImageSet currentImageSet;
    private List<int> missingImagesIndices = new List<int>(); //this list contains the true answers
    private List<int> possibleImages = new List<int>(); //this list contains the true answers and some random images
    bool isGameWon = false;

    [SerializeField]Text CheckResultText;

    public void InitiateGame()
    {
        //Debug.Log("Setting up new game");
        RandomizeAnImageSet();
        RandomizeMissingImgesIndexes();
        SetupCorrectImage();
        SetupMissingBoxes();
        SetupMatchBoxesAndMovingImages();
        CheckResultText.gameObject.SetActive(false);
        return;
    }

    private void SetupCorrectImage()
    {
        correctImage.sprite = currentImageSet.GetCorrectImage();
    }

    /// <summary>
    /// if all images are matched, player wins
    /// </summary>
    public void CheckGameStatus()
    {
        //missing box at i (of allMissingBoxes array) has to have imageId = i in order to match
        for (int i = 0; i < missingImagesIndices.Count; i++)
        {
            int currentImageIndex = missingImagesIndices[i];
            MatchBox currentMissingBox = allMissingBoxes[currentImageIndex];
            //Debug.Log("Missing box " + currentImageIndex + " has image id = " + currentMissingBox.GetImageId() +
            //                ", the correct image id is " + currentImageIndex);
            if (currentMissingBox.GetImageId() != currentImageIndex)
            {
                //game is not won yet
                //Debug.Log("Game is not won yet.");
                CheckResultText.gameObject.SetActive(true);
                CheckResultText.text = "Pieces are not in the right order!";
                CheckResultText.color = Color.red;
                return;
                
            }
            
        }
        //Debug.Log("Everything matched. Player Wins!");
        CheckResultText.gameObject.SetActive(true);
        CheckResultText.text = "You fixed the picture!";
        CheckResultText.color = Color.green;

        StartCoroutine(WaitThenEndGame(2));
        return ;
    }

    //Setup the match boxes with possible images
    //Update their MatchBox and Image Transfer component of Moving Images properly
    private void SetupMatchBoxesAndMovingImages()
    {
        for(int i = 0; i < NUMBER_OF_MATCH_BOXES; i++)
        {
            if( i >= possibleImages.Count)
            {
                Debug.LogError("Something is wrong when initiate lists");
                return;
            }
            int currentIndex = possibleImages[i];//this list is already shuffled
            MatchBox currentMatchBox = matchBoxes[i];
            ImageTransfer currentImageTransfer = movingImages[i];
            
            //this setup is done in the inspector: MovingImage1 parent box id = MatchBox 1 and so on
            if(currentImageTransfer.parentMatchBoxid != currentMatchBox.GetBoxId())
            {
                Debug.LogError("parent box id does not match at i = " + i);
            }

            //update Sprite
            currentImageTransfer.gameObject.GetComponent<Image>().sprite = currentImageSet.GetImage(currentIndex);
            //update image ID
            currentMatchBox.SetImageId(currentIndex);
            currentImageTransfer.imageId = currentIndex;
        }
    }

    //Set active false to other missing boxes and their background
    //Set the rest to active
    private void SetupMissingBoxes()
    {
        for (int i = 0; i < NUMBER_OF_IMAGES; i++)
        {
            if (!missingImagesIndices.Contains(i)) //these boxes are not in the game
            {
                allMissingBoxes[i].gameObject.SetActive(false);
                allMissingBoxesBackground[i].gameObject.SetActive(false);
            }
            else //all missing boxes do not have child image yet
            {
                allMissingBoxes[i].gameObject.SetActive(true);
                allMissingBoxes[i].OnImageOut();
            }
        }
    }

 

    //randomize 2 int lists
    private void RandomizeMissingImgesIndexes()
    {
        missingImagesIndices.Clear();
        possibleImages.Clear();

        //randomize the answer list
        int i = 0;
        while (i < NUMBER_OF_MISSING_BOXES)
        {
            int currentIndex = UnityEngine.Random.Range(0, NUMBER_OF_IMAGES);
            if (!missingImagesIndices.Contains(currentIndex))
            {
                missingImagesIndices.Add(currentIndex);
                possibleImages.Add(currentIndex);
                i++;
            }
        }
        //random the rest of the possible answers list
        while(i < NUMBER_OF_MATCH_BOXES)
        {
            int currentIndex = UnityEngine.Random.Range(0, NUMBER_OF_IMAGES);
            if (!possibleImages.Contains(currentIndex))
            {
                possibleImages.Add(currentIndex);
                i++;
            }
        }

        //shuffle the possible images list
        for (int j = 0; j < possibleImages.Count; j++)
        {
            int temp = possibleImages[j];
            int randomIndex = UnityEngine.Random.Range(j, possibleImages.Count);
            possibleImages[j] = possibleImages[randomIndex];
            possibleImages[randomIndex] = temp;
        }

        ////print out both lists:
        //foreach (var x in missingImagesIndices)
        //{
        //    Debug.Log(x.ToString());
        //}
        //Debug.Log("End answer list, next is possible answers list");
        //foreach (var x in possibleImages)
        //{
        //    Debug.Log(x.ToString());
        //}
    }


    //ramdomize the image we will use in this game from the image list
    private void RandomizeAnImageSet()
    {
        imageSetIndex = UnityEngine.Random.Range(0, NUMBER_OF_POSSIBLE_GAMES);
        currentImageSet = imageSets[imageSetIndex];
    }


    
    private void ResetGameState()
    {
        imageSetIndex = -1;
        missingImagesIndices.Clear();
        possibleImages.Clear();
        CheckResultText.gameObject.SetActive(false);
        //reset missing boxes
        for(int i = 0; i < NUMBER_OF_IMAGES; i++)
        {
            allMissingBoxes[i].gameObject.SetActive(true);
            allMissingBoxesBackground[i].gameObject.SetActive(true);
            allMissingBoxes[i].OnImageOut();
        }
        isGameWon = false;

        //reset moving images and match boxes
        for(int i = 0; i < NUMBER_OF_MATCH_BOXES; i++)
        {
            movingImages[i].ResetState(i, matchBoxes[i]);
            //matchBoxes[i].ResetState(i);
            //move all movingImages back to their box
            

        }

        //InitiateGame(); this will be called my Maintenance manager


    }

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
        transform.parent.gameObject.GetComponent<MiniGameCanvasControllerMissingPieces>().EventEnd(endState, message);
        ResetGameState();
        this.gameObject.SetActive(false);//deactivate panel
    }


    IEnumerator WaitThenEndGame(float seconds)
    {


        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(seconds);
        isGameWon = true;
        GameEnd(true);
    }
}
