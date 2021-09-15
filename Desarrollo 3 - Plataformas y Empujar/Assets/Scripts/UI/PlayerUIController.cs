﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class PlayerUIController : MonoBehaviour
{
    [Header("Lifes")]
    [SerializeField] GameObject emptyHeart;
    [SerializeField] GameObject fullHeart;

    [SerializeField] float distaceBettweenHearts = 10f;
    [SerializeField] PlayerStats playerStats;

    [Header("Coins / Money")]
    [SerializeField] TextMeshProUGUI coinsText;

    //===============================================

    List<GameObject> listOfHearts = new List<GameObject>();
    List<GameObject> listOfEmptyHearts = new List<GameObject>();

    int lastHeartActive = 0;

    float initialPosition = 0f;

    // ==============================================

    private void OnEnable()
    {
        PlayerStats.OnPlayerEarnLive += AddLife;
        PlayerStats.OnPlayerDamaged += RemoveLife;
        PlayerStats.OnPlayerEarnMaxLive += AddMaxLife;
        PlayerStats.OnPlayerLoseMaxLive += RemoveMaxLife;
        PlayerManager.ShowMoney += AddCoins;
    }

    private void OnDisable()
    {
        PlayerStats.OnPlayerDamaged -= RemoveLife;
        PlayerStats.OnPlayerEarnLive -= AddLife;
        PlayerStats.OnPlayerEarnMaxLive -= AddMaxLife;
        PlayerStats.OnPlayerLoseMaxLive -= RemoveMaxLife;
        PlayerManager.ShowMoney -= AddCoins;
    }

    private void Start()
    {
        InitializeLife();
    }

    // ==============================================
    // Lifes:

    void InitializeLife()
    {
        for (int i = 1; i <= playerStats.GetMaxLives(); i++)
        {
            if (i % 2 == 0)
            {
                var emptyH = Instantiate(emptyHeart, new Vector3(initialPosition, 0, 0), Quaternion.identity) as GameObject;
                emptyH.transform.SetParent(this.transform, false);
                emptyH.transform.name = "Empty Heart " + (i / 2);

                listOfEmptyHearts.Add(emptyH);

                var fullH = Instantiate(fullHeart, new Vector3(initialPosition, 0, 0), Quaternion.identity) as GameObject;
                fullH.transform.SetParent(this.transform, false);
                fullH.transform.name = "Full Heart " + (i / 2);

                listOfHearts.Add(fullH);
                lastHeartActive = listOfHearts.Count;

                initialPosition += distaceBettweenHearts;
            }
        }
    }

    void RemoveMaxLife(int max)
    {
        for (int i = 0; i < max; i++)
        {
            Destroy(listOfEmptyHearts[listOfEmptyHearts.Count - 1]);
            Destroy(listOfHearts[listOfHearts.Count - 1]);

            listOfEmptyHearts.RemoveAt(listOfEmptyHearts.Count - 1);
            listOfHearts.RemoveAt(listOfHearts.Count - 1);

            if (listOfHearts.Count < lastHeartActive)
            {
                lastHeartActive = listOfHearts.Count;
            }

            initialPosition -= distaceBettweenHearts;
        }
    }

    void AddMaxLife(int max)
    {
        for (int i = 0; i < max; i++)
        {
            var emptyH = Instantiate(emptyHeart, new Vector3(initialPosition, 0, 0), Quaternion.identity) as GameObject;
            emptyH.transform.SetParent(this.transform, false);

            listOfEmptyHearts.Add(emptyH);

            var fullH = Instantiate(fullHeart, new Vector3(initialPosition, 0, 0), Quaternion.identity) as GameObject;
            fullH.transform.SetParent(this.transform, false);

            listOfHearts.Add(fullH);

            emptyH.transform.name = "Empty Heart " + lastHeartActive;
            fullH.transform.name = "Full Heart " + lastHeartActive;

            listOfHearts[listOfHearts.Count - 1].SetActive(false);

            initialPosition += distaceBettweenHearts;
        }
    }

    void RemoveLife(int max)
    {
        for (int i = 0; i < max; i++)
        {
            if (lastHeartActive > 0)
            {
                if (listOfHearts[lastHeartActive - 1].GetComponent<ControlHeart>().GetState())
                {
                    listOfHearts[lastHeartActive - 1].GetComponent<ControlHeart>().ReduceLife(false);
                }
                else
                {
                    listOfHearts[lastHeartActive - 1].SetActive(false);
                    lastHeartActive--;
                }
            }
            else
            {
                break;
            }
        }
    }

    void AddLife(int max)
    {
        for (int i = 0; i < max; i++)
        {
            if(lastHeartActive <= listOfHearts.Count)
            {
                if (!listOfHearts[lastHeartActive - 1].GetComponent<ControlHeart>().GetState())
                {
                    listOfHearts[lastHeartActive - 1].GetComponent<ControlHeart>().ReduceLife(true);
                }
                else
                {
                    lastHeartActive++;
                    listOfHearts[lastHeartActive - 1].SetActive(true);
                    listOfHearts[lastHeartActive - 1].GetComponent<ControlHeart>().ReduceLife(false);
                }
            }
            else
            {
                break;
            }
        }
    }

    // ==============================================
    // Coins:

    public void AddCoins(int value)
    {
        coinsText.text = value.ToString();
    }
}