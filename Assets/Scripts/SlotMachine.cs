using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using wheelIcons = Spinning.wheelIcons;


[Serializable]
public class SymbolScoreData
{
    public Spinning.wheelIcons icon;
    public int score;
   
}

public class SlotMachine : MonoBehaviour
{
    [SerializeField] private List<Spinning> slotWheels;
    [SerializeField] private float runningTime;
    [SerializeField] private List<SymbolScoreData> scoreDatas;

    public GameObject lever;
    private float leverZangle;
    public PointSystem pointSystem;
    public int requiredPoints = 50;
    public AudioSource SpinSound;
    public AudioSource StopSound;
    public AudioSource WinSound;
    public AudioSource LoseSound;

    [Range(0, 100)] public float chanceOfSameSymbol;

    private enum SlotMachineState
    {
        Idle,
        Spinning,
        Result,
    }

    private SlotMachineState state;

    private void Start()
    {
        state = SlotMachineState.Idle;
    }

    // Update is called once per frame
    void Update()
    {
        
        leverZangle =  lever.transform.localEulerAngles.z;

        if (leverZangle >= 70 && state == SlotMachineState.Idle &&  pointSystem.HasEnoughtPoints(requiredPoints))
        {
            
            state = SlotMachineState.Spinning;
            StartCoroutine(StartSpinningCoroutine());
            pointSystem.SubtractPoints(requiredPoints);
        }
       
        if (Input.GetKeyDown(KeyCode.Space) && state == SlotMachineState.Idle)
        {
            state = SlotMachineState.Spinning;
            StartCoroutine(StartSpinningCoroutine());
        }
    }

    private IEnumerator StartSpinningCoroutine()
    {
        for (int i = 0; i < slotWheels.Count; i++)
        {
            SpinSound.Play();
            var slotWheel = slotWheels[i];
            slotWheel.StartSpinning();
            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(runningTime);
        yield return StartCoroutine(StopSpinningCoroutine());
    }

    private int sameSymbol;

    private IEnumerator StopSpinningCoroutine()
    {
        sameSymbol = -1;
        var random = Random.Range(0, 100);
        if (random <= chanceOfSameSymbol)
        {
            sameSymbol = Random.Range(0, 8);
        }

        state = SlotMachineState.Result;
        for (int i = 0; i < slotWheels.Count; i++)
        {
            var slotWheel = slotWheels[i];
            slotWheel.StopSpinning(sameSymbol > -1 ? sameSymbol : Random.Range(0, 8));
            StopSound.Play();
            yield return new WaitForSeconds(0.5f);
        }
        SpinSound.Stop();
        if (sameSymbol > -1)
        {
            var symbol = (wheelIcons)sameSymbol;
            
            pointSystem.AddPoints(GetScoreForSymbol(symbol)); //added 
           
            WinSound.Play();
            Debug.Log($"WOOOOOOOOOON WITH SYMBOL {symbol} REWARDING {GetScoreForSymbol(symbol)}");
        }
        else
        {
            LoseSound.Play();
        }

        yield return new WaitForSeconds(2.0f);
        state = SlotMachineState.Idle;
        
       
    }

    private int GetScoreForSymbol(wheelIcons icon)
    {
        for (int i = 0; i < scoreDatas.Count; i++)
        {
            var scoreData = scoreDatas[i];
            if (scoreData.icon == icon)
                return scoreData.score;
        }

        return 0;
    }
}
