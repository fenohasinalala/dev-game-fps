
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private const string playerIdPrefix = "Player";
    private static Dictionary<string, Player> Players = new Dictionary<string, Player>();
    public MatchSettings matchSettings;
    public static GameManager instance;
    private void Awake(){
        if (instance == null)
        {
            instance = this;
            return;
        }
        Debug.LogError("Plus d'instance de gamemanager dans le jeux");
    }
    public static void RegisteurPlayer(string netId, Player player){
        string playerId = playerIdPrefix + netId;
        Players.Add(playerId, player);
        player.transform.name = playerId;
    }
    public static void UnregisterPlayer(string playerId){
        Players.Remove(playerId);
    }
    public static Player GetPlayer(string playerId){
        return Players[playerId];
    }
}
