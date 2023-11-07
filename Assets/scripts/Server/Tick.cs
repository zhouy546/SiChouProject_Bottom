﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VideoServer;

public class Tick : MonoBehaviour
{
    public delegate void FinishCountDown();
    public event FinishCountDown finishCountDownEvent;

    public delegate IEnumerator StartCountDown();
    public event StartCountDown startCountDownEvent;


    public delegate void StopCountDonw();
    public event StopCountDonw stopCountDonwEvent;

    public delegate void ResetTime();
    public event ResetTime resetTimeEvent;

    public float DefaultCountDonwTime = 5;
    public float CurrentCountDonwTime = 5;

    public bool enableKeyBoardDebug;

    public bool IsCountDonw = false;

    void Awake() {


        EventCenter.AddListener(EventDefine.ShowInteraction, OnInteractionShow);

        EventCenter.AddListener(EventDefine.ShowVideo, OnVideoShow);

        EventCenter.AddListener(EventDefine.ShowPb, OnPBShow);

        EventCenter.AddListener(EventDefine.ini, Ini);

    }

    // Start is called before the first frame update
    void Ini()
    {
        startCountDownEvent += CountDown;
        finishCountDownEvent += EndCountDown;
        stopCountDonwEvent += stopCountdonw;
        resetTimeEvent += resetCurrentCountDonwTime;


        DefaultCountDonwTime =CurrentCountDonwTime = ValueSheet.serverRoot.VideoDuration;


        if (ValueSheet.serverRoot.isAutoLoop)
        {
            Func_StartCountDonw();
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (enableKeyBoardDebug) {
            //if (Input.GetKeyDown(KeyCode.G))
            //{
            //    Func_StartCountDonw();
            //}
            //if (Input.GetKeyDown(KeyCode.S))
            //{
            //    Func_StopCountDonw();
            //}
        }
    }

    public void Func_ResetTime() {
        resetTimeEvent.Invoke();
    }

    public void Func_StopCountDonw() {
        IsCountDonw = false;
        stopCountDonwEvent.Invoke();
    }

    public void Func_StartCountDonw() {
        if (!IsCountDonw) {
            IsCountDonw = true;
            StartCoroutine(startCountDownEvent.Invoke());
            //    Debug.Log("Invoke CountDonwEvent");
        }
    }

    public void Func_FinishCountDonw() {
        IsCountDonw = false;
        finishCountDownEvent.Invoke();
    }

    private IEnumerator CountDown() {
        CurrentCountDonwTime--;
        yield return new WaitForSeconds(1f);
        //Debug.Log(CurrentCountDonwTime);
        if (CurrentCountDonwTime <= 0)
        {
            Func_FinishCountDonw();
        }
        else {
            StartCoroutine(CountDown());
        }
    }

    private void EndCountDown() {
        Func_ResetTime();
        Debug.Log("Count Donw End");


        Debug.Log(ValueSheet.state);

        if (ValueSheet.state == State.video)
        {
            ValueSheet.state = State.interaction;
            EventCenter.Broadcast(EventDefine.ShowInteraction);
        }
        else if (ValueSheet.state == State.interaction)
        {
            ValueSheet.state = State.pb;

            EventCenter.Broadcast(EventDefine.ShowPb);
        }else if(ValueSheet.state == State.pb)
        {
            ValueSheet.state = State.video;
            EventCenter.Broadcast(EventDefine.ShowVideo);

        }

    }

    private void resetCurrentCountDonwTime() {
        //Debug.Log("reset time");
        CurrentCountDonwTime = DefaultCountDonwTime;
    }

    private void stopCountdonw() {
        //     Debug.Log("Stop CountDonw");
        StopAllCoroutines();
        Func_ResetTime();
    }

    public void OnPBShow()
    {
        Debug.Log("ShowPb");
        DefaultCountDonwTime = CurrentCountDonwTime = ValueSheet.serverRoot.PbDuration;

        Func_ResetTime();

        if (ValueSheet.serverRoot.isAutoLoop)
        {
            Func_StartCountDonw();
        }
    }

    private void OnVideoShow()
    {
        Debug.Log("ShowVideo");
        DefaultCountDonwTime = CurrentCountDonwTime = ValueSheet.serverRoot.VideoDuration;

        Func_ResetTime();

        if (ValueSheet.serverRoot.isAutoLoop)
        {
            Func_StartCountDonw();
        }
    }

    private void OnInteractionShow()
    {
        Debug.Log("ShowInteraction");

        DefaultCountDonwTime = CurrentCountDonwTime = ValueSheet.serverRoot.InteractionDuration;

        Func_ResetTime();

        if (ValueSheet.serverRoot.isAutoLoop)
        {
            Func_StartCountDonw();
        }
    }

}
