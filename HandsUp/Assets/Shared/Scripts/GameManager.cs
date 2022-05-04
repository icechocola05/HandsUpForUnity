﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int gameVersion = -1;
    public int gameCategory = -1;
    public int timeLimit = 5;
    public int problemNum = 5;

    private int curIndex = 0;
    private List<Card> cards;
    public static bool isCardLoaded = false;
    public static bool isCustomCardLoaded = false;
    public static bool isImgLoaded = false;

    public int curScore = 0;
    private int curProblemNum = 0;
    private float curTime = 0;
    private bool isStart = false;

    private CardManager cardManager;
    private PlayerManager playerManager;

    private void Start()
    {
        cardManager = GameObject.Find("GameManager").GetComponent<CardManager>();
        playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
    }

    private void Update()
    {
        if (isStart)
        {
            TimeSetting();
        }
    }

    // Express the Current Time
    private void TimeSetting()
    {
        curTime += Time.deltaTime;
        float diffTime = timeLimit * 60 - curTime;
        int min = ((int)diffTime / 60 % 60);
        int seconds = ((int)diffTime % 60);
        string minString = min.ToString();
        string secondsString = seconds.ToString();
        if (min / 10 == 0)
            minString = "0" + min.ToString();
        if (seconds / 10 == 0)
            secondsString = "0" + seconds.ToString();
        GameObject.Find("GamePage").transform.Find("LimitedTime").GetComponent<Text>().text = "제한 시간 " + minString + " : " + secondsString;
    }

    public void GetCards()
    {
        // Get cards form cardManager
        cardManager.InitCards(gameCategory, true);
        StartCoroutine((WaitForLoading()));
    }

    IEnumerator WaitForLoading()
    {
        while (true)
        {
            if (isCardLoaded && isCustomCardLoaded)
            {
                InitGame();
                break;
            }else if (isImgLoaded)
            {
                StartGame(cards[curIndex]);
                break;
            }
            else
                yield return new WaitForSeconds(Time.deltaTime);
        }
        yield return null;
    }

    private void InitGame()
    {
        Debug.Log("InitGame");

        isCardLoaded = false;
        isCustomCardLoaded = false;
        isImgLoaded = false;
        cards = cardManager.GetAllCards();

        // Init
        curIndex = 0;
        curScore = 0;
        curProblemNum = problemNum;
        if (cards.Count < problemNum)
            curProblemNum = cards.Count;

        // Get Random Card
        int[] randIndex = GetRandIndex(cards.Count);
        InitCard(cards[curIndex]);
        StartCoroutine(WaitForLoading());
    }

    private int[] GetRandIndex(int length)
    {
        int[] ranArr = Enumerable.Range(0, length).ToArray();
        for (int i = 0; i < length; ++i)
        {
            int ranIdx = Random.Range(i, length);
            int tmp = ranArr[ranIdx];
            ranArr[ranIdx] = ranArr[i];
            ranArr[i] = tmp;
        }
        return ranArr;
    }

    private void InitCard(Card card)
    {
        GameObject.Find("GamePage").transform.Find("Card/CardBGImg").gameObject.SetActive(true);
        StartCoroutine(cardManager.getImagesFromURL(card.GetImagePath(), GameObject.Find("GamePage").transform.Find("Card/CardBGImg").gameObject, true));
        GameObject.Find("GamePage").transform.Find("Card/CardTxt").GetComponent<Text>().text = card.GetName();
    }

    private void StartGame(Card card)
    {
        Debug.Log("Test");
        if (gameVersion == 1)
        {
            GameObject.Find("GamePage").transform.Find("Card/CardBGImg").gameObject.SetActive(true);
            GameObject.Find("GamePage").transform.Find("Card/CardTxt").gameObject.SetActive(false);
            
            // Text Detection Function
        }
        else
        {
            GameObject.Find("GamePage").transform.Find("Card/CardBGImg").gameObject.SetActive(false);
            GameObject.Find("GamePage").transform.Find("Card/CardTxt").gameObject.SetActive(true);
            // Object Detection Function
        }

        // Time Function
        isStart = true;

        // Check Correct/Wrong
        bool isCorrect = true;

        //CheckStatus(isCorrect);
    }

    private void CheckStatus(bool isCorrect)
    {
        // Check Score & Correct/Wrong Cards
        if (isCorrect)
        {
            curScore++;

            // Express the Current Score
            string tmp = curScore.ToString();
            if(curScore / 10 == 0)
                tmp = "0" + curScore.ToString();

            GameObject.Find("GamePage").transform.Find("CurProbNum").GetComponent<Text>().text = "문제 수 : " + tmp + "/" + problemNum;

            // Add Card to Correct Card list

        }
        else
        {
            // Add Card to Wrong Card list

        }

        CheckNextScene();
    }

    public void CheckNextScene()
    {
        if (curIndex < curProblemNum - 1)
        {
            curIndex += 1;
            InitCard(cards[curIndex]);
            StartCoroutine(WaitForLoading());
        }
        else
        {
            // Connect to Result Page
        }
    }

    public int GetGameVersion()
    {
        return gameVersion;
    }

    public void SetGameVersion(int gameVersion)
    {
        this.gameVersion = gameVersion;
    }

    public int GetGameCategory()
    {
        return gameCategory;
    }

    public void SetGameCategory(int gameCategory)
    {
        this.gameCategory = gameCategory;
    }

    public int GetTimeLimit()
    {
        return timeLimit;
    }

    public void SetTimeLimit(int timeLimit)
    {
        this.timeLimit = timeLimit;
    }

    public int GetProblemNum()
    {
        return problemNum;
    }

    public void SetProblemNum(int problemNum)
    {
        this.problemNum = problemNum;
    }
}
