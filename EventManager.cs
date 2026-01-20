using System;
using UnityEngine;

public static class EventManager
{
    public static Action OnGameStart;
    public static Action OnPlayerHit;
    public static Action OnPlayerStand;
    public static Action OnDealerTurn;
    public static Action<string> OnGameOver;

    public static Action<GameObject, bool, bool> OnCardDealt; 
    public static Action OnDealerReveal;
    public static Action OnPlayerMoveHandsFast;

}