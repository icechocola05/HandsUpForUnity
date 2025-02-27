﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BackBtnManager : MonoBehaviour
{

    public void OnClickBackBtn()
    {
        string firstView = "";
        string secondView = "";
        string nextScene = "";

        switch (SceneManager.GetActiveScene().name)
        {
            case "CardViewScene":
                string [] tmp = SetViews(EventSystem.current.currentSelectedGameObject.transform.parent.name);
                firstView = tmp[0];
                secondView = tmp[1];
                nextScene = tmp[2];
                break;
            case "GameSelectScene":
                firstView = "SelectGamePage";
                secondView = "SelectCategoryPage";
                nextScene = "OpeningScene";
                break;
            case "GameScene":
                firstView = "PopUpPages/ResultPopUp";
                secondView = "PopUpPages/CardListPopUp";
                nextScene = "OpeningScene";
                break;

        }

        if (GameObject.Find("Canvas").transform.Find(firstView).gameObject.activeSelf == true)
        {
            if (nextScene.Equals("ItemAddPage"))
            {
                GameObject.Find("CardViewManager").GetComponent<CategoryManager>().InitCategories(false, false);
                GameObject.Find("Canvas").transform.Find("OthersCategoryPage").gameObject.SetActive(false);
                GameObject.Find("Canvas").transform.Find(nextScene).gameObject.SetActive(true);
            }
            else
                SceneManager.LoadScene(nextScene);
        }
        else if (GameObject.Find("Canvas").transform.Find(secondView).gameObject.activeSelf == true)
        {
            GameObject.Find("Canvas").transform.Find(secondView).gameObject.SetActive(false);
            GameObject.Find("Canvas").transform.Find(firstView).gameObject.SetActive(true);
            if(secondView.Equals("OthersCategoryPage/CardsScrollView"))
                GameObject.Find("Canvas").transform.Find("OthersCategoryPage").GetComponentInChildren<Text>().text = "다른 사용자의 카테고리";
            if (secondView.Equals("EditCategoryPage"))
            {
                int category_id = GameObject.Find("CardViewManager").GetComponent<EditManager>().GetEditCardsCategory();
                string category_name = GameObject.Find("CardViewManager").GetComponent<EditManager>().GetEditCardsCategoryName();
                
                if(category_name.Equals("전체"))
                    GameObject.Find("CardViewManager").GetComponent<CardManager>().InitCards(category_id, "CardViewPage", false, "category/card/all");
                else
                    GameObject.Find("CardViewManager").GetComponent<CardManager>().InitCards(category_id, "CardViewPage");
            }
        }

        if (firstView.Equals("CardViewPage/PR_CategoriesScroll"))
        {
            GameObject.Find("CardViewManager").GetComponent<CategoryManager>().InitCategories(true);
        }
    }
    

    private string[] SetViews(string currentObject)
    {
        string[] views = new string[3];
        switch (currentObject)
        {
            case "ItemAddPage":
            case "CustomPage":
                views[0] = "ItemAddPage";
                views[1] = currentObject;
                views[2] = "CardViewScene";
                
                if (currentObject.Equals("CustomPage"))
                    GameObject.Find("Canvas").transform.Find("CustomPage").GetComponent<CustomManager>().InitCustomPages();
                break;
            case "EditCategoryPage":
                views[0] = "CardViewPage";
                views[1] = "EditCategoryPage";
                views[2] = "CardViewScene";
                break;
            case "OthersCategoryPage":
                views[0] = "OthersCategoryPage/PR_CategoriesScroll";
                views[1] = "OthersCategoryPage/CardsScrollView";
                views[2] = "ItemAddPage";
                break;
            default:
                views[0] = "CardViewPage/PR_CategoriesScroll";
                views[1] = "CardViewPage/CardsScrollView";
                views[2] = "OpeningScene";
                GameObject.Find("Canvas").transform.Find("CardViewPage/EditBtn").gameObject.SetActive(false);
                break;

        }
        return views;
    } 
}
