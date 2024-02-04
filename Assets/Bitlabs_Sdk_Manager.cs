using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Bitlabs_Sdk_Manager : MonoBehaviour
{
    public GameObject Earn_Reward_Panel;
    public string token_id,user_id;
    public Text Coins;
   
    int User_Coin_Balance;
   
    [HideInInspector]
    public bool CanPlayLevel;
    int coins_required_to_Unlock;// coins reuquired to unlock level
    // Start is called before the first frame update
    void Start()
    {
        Invoke(nameof(init), 0.1f);// slight delay to allow smooth initialization
        Coins.text = "COINS : "+User_Coin_Balance.ToString();
        //Level_number.text = "CURRENT LEVEL : " + current_level.ToString();
        //if (current_level == 200)
        //{
        //    Invoke(nameof(ShowOfferWallPanel), 1f);
        //}
        // Create a receiver Method
        
       
    }
    public void ShowOfferWallPanel()
    {
        Earn_Reward_Panel.SetActive(true);
    }
    public void RewardCallback(string payout)
    {
        Debug.Log("BitLabs Unity onReward: " + payout);
      //  _reward.text = payout;
        User_Coin_Balance += int.Parse(payout);
        UpdateCoins();
        CheckIfRequiredCoinsEarned();
        //BitLabs.
    }

    void UpdateCoins()
    {
        Coins.text = "COINS : "+User_Coin_Balance.ToString();
    }

    void CheckIfRequiredCoinsEarned()
    {
        if (User_Coin_Balance >= coins_required_to_Unlock)
        {

            PlayerPrefs.SetInt("level_unlocked", 1);
            CanPlayLevel = true;
            Earn_Reward_Panel.SetActive(false);
        }
    }

    public bool CanPlayThisLevel()
    {
        if (PlayerPrefs.GetInt("level_unlocked") == 0)
            return false;
        else
            return true;
    }
    public void init()
    {
        BitLabs.Init(token_id, user_id);
        Invoke(nameof(reward_callBack), 0.1f);
    }

    void reward_callBack()
    {
        BitLabs.SetRewardCallback(gameObject.name);
    }

    public void Launch_Offer()
    {
        BitLabs.LaunchOfferWall();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
