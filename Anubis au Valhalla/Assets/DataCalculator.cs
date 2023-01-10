using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;

public class DataCalculator : MonoBehaviour
{
    public bool doCalcul;

    [Header("ENNEMIS")] 
    [Expandable] public EnemyData guerrier;
    [Expandable] public EnemyData guerrierElite;
    [Expandable] public EnemyData loup;
    [Expandable] public EnemyData loupElite;
    [Expandable] public EnemyData corbeau;
    [Expandable] public EnemyData corbeauElite;
    [Expandable] public EnemyData chaman;
    [Expandable] public EnemyData chamanElite;
    [Expandable] public EnemyData valkyrie;
    [Expandable] public EnemyData valkyrieElite;

    [BoxGroup("PRIX ENNEMIS")] public Vector2 priceGuerrier = new(2,6);
    [BoxGroup("PRIX ENNEMIS")] public Vector2 priceLoup = new(3,9);
    [BoxGroup("PRIX ENNEMIS")] public Vector2 priceCorbeau = new(3,6);
    [BoxGroup("PRIX ENNEMIS")] public Vector2 priceChaman = new(6,18);
    [BoxGroup("PRIX ENNEMIS")] public Vector2 priceValkyrie = new(16,32);

    [BoxGroup("LISTS")] public List<int> wallets = new List<int>();
    [BoxGroup("LISTS")] public List<int> addToWallet = new List<int>(16);
    
    [Header("RESULTS")]
    public bool showResults;
    [ShowIf("showResults")] public List<Vector2> potentielNbGuerrier;
    [ShowIf("showResults")] public List<Vector2> potentielNbLoup;
    [ShowIf("showResults")] public List<Vector2> potentielNbCorbeau;
    [ShowIf("showResults")] public List<Vector2> potentielNbChaman;
    [ShowIf("showResults")] public List<Vector2> potentielNbValkyrie;
    [ShowIf("showResults")] public List<Vector2> potSoulForGuerrier;
    [ShowIf("showResults")] public List<Vector2> potSoulForLoup;
    [ShowIf("showResults")] public List<Vector2> potSoulForCorbeau;
    [ShowIf("showResults")] public List<Vector2> potSoulForChaman;
    [ShowIf("showResults")] public List<Vector2> potSoulForValkyrie;

    

    void Awake()
    {
        if (doCalcul)
        {
            Time.timeScale = 0;
        }
        CalcWalletPerRoom(addToWallet);
        
        CalcPotentielNbEnnemi(wallets,priceGuerrier,potentielNbGuerrier);
        CalcPotentielNbEnnemi(wallets,priceLoup,potentielNbLoup);
        CalcPotentielNbEnnemi(wallets,priceCorbeau,potentielNbCorbeau);
        CalcPotentielNbEnnemi(wallets,priceChaman,potentielNbChaman);
        CalcPotentielNbEnnemi(wallets,priceValkyrie,potentielNbValkyrie);
        
        CalcPotentielSoul(guerrier,guerrierElite, potentielNbGuerrier,potSoulForGuerrier);
        CalcPotentielSoul(loup,loupElite, potentielNbLoup,potSoulForLoup);
        CalcPotentielSoul(corbeau,corbeauElite, potentielNbCorbeau,potSoulForCorbeau);
        CalcPotentielSoul(chaman,chamanElite, potentielNbChaman,potSoulForChaman);
        CalcPotentielSoul(valkyrie,valkyrieElite, potentielNbValkyrie,potSoulForValkyrie);
    }

    void CalcWalletPerRoom(List<int> argents)
    {
        List<int> wallet = new List<int>(){0};
        for (int i = 0; i < argents.Count; i++)
        {
            wallet.Add(wallet[i]+argents[i]);
        }
        wallets = wallet;
    }

    void CalcPotentielNbEnnemi(List<int> bank, Vector2 ennemi, List<Vector2> listEnnemis)
    {
        for (int i = 0; i < bank.Count; i++)
        {
            Vector2 vec = new();
            vec.x = Mathf.RoundToInt(bank[i]/ennemi.x);
            vec.y = Mathf.RoundToInt(bank[i]/ennemi.y);
            listEnnemis.Add(vec);
        }
    }

    void CalcPotentielSoul(EnemyData ennemi, EnemyData ennemiElite, List<Vector2> listEnnemis, List<Vector2> listSoul)
    {
        for (int i = 0; i < listEnnemis.Count; i++)
        {
            Vector2 vec = new();
            vec.x = ennemi.soulValue * listEnnemis[i].x;
            vec.y = ennemiElite.soulValue * listEnnemis[i].y;
            listSoul.Add(vec);
        }
    }
    
}
