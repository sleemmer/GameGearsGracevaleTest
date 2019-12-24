using System;
using UnityEngine;

public class MoveControlEffect : MonoBehaviour
{
    public event EventHandler OnFinish = null;
    public event EventHandler OnMove = null;
    public event EventHandler OnDelay = null;

    private enum InternalMode
    {
        Idle,
        ConstantTime,
    }

    private float startTime = -1f;
    private float finishTime = -1f;
    private float delayTime = -1f;
    private InternalMode mode = InternalMode.Idle;
    private bool delayStarted = false;

    private float transitionTime = 1.0f;
    private Vector3 startPos = Vector3.zero;
    private Vector3 targetPos = Vector3.zero;

    private Vector3 direction;
    private Vector3 shiftPos;
    private float directionSqrMagnitude = 0;

    private RectTransform rectTransform;
    private bool useAnchoredPosition = false;

    private bool inTransition = false;

    public void MoveConstantTime(Vector3 pos, float time, float delay, bool useAnchoredPosition = false)
    {
        mode = InternalMode.ConstantTime;
        delayStarted = false;

        transitionTime = time;
        startTime = Time.time;
        finishTime = Time.time + transitionTime;
        delayTime = delay;

        this.useAnchoredPosition = useAnchoredPosition;
        if (useAnchoredPosition)
        {
            rectTransform = (RectTransform)transform;
            startPos = rectTransform.anchoredPosition;
        }
        else
        {
            startPos = transform.localPosition;
        }

        targetPos = pos;
        directionSqrMagnitude = (targetPos - startPos).sqrMagnitude;

        inTransition = true;
    }

    public void MoveConstantTime(Vector3 pos, float time, bool useAnchoredPosition = false)
    {
        MoveConstantTime(pos, time, 0, useAnchoredPosition);
    }

    void Update()
    {
        ConstantTimeTransitionUpdate();
    }

    private void ConstantTimeTransitionUpdate()
    {
        if (!inTransition)
            return;

        startTime += Time.deltaTime;

        if (startTime >= finishTime)
        {
            if (startTime >= finishTime + delayTime)
            {
                inTransition = false;

                if (OnFinish != null)
                    OnFinish(this, EventArgs.Empty);
            }
            else
            {
                if (!delayStarted)
                {
                    delayStarted = true;
                    if (OnDelay != null)
                        OnDelay(this, EventArgs.Empty);
                }
            }
        }

        if (delayStarted)
            return;

        if (transitionTime > 0)
        {
            direction = targetPos - startPos;

            if (useAnchoredPosition)
                shiftPos = direction * (Time.deltaTime / transitionTime) + new Vector3(rectTransform.anchoredPosition.x, rectTransform.anchoredPosition.y, 0);
            else
                shiftPos = direction * (Time.deltaTime / transitionTime) + gameObject.transform.localPosition;

            if ((shiftPos - startPos).sqrMagnitude >= directionSqrMagnitude)
            {
                if (useAnchoredPosition)
                    rectTransform.anchoredPosition = targetPos;
                else
                    gameObject.transform.localPosition = targetPos;
            }
            else
            {
                if (useAnchoredPosition)
                    rectTransform.anchoredPosition = shiftPos;
                else
                    gameObject.transform.localPosition = shiftPos;
            }

            if (OnMove != null)
                OnMove(this, EventArgs.Empty);
        }
    }

    public void Reset()
    {
        inTransition = false;
        mode = InternalMode.Idle;
    }

    public void Stop()
    {
        mode = InternalMode.Idle;
    }

    public void Dispose()
    {
        Stop();
    }

    internal void RemoveAllListeners()
    {
        OnFinish = null;
    }

    public void SetPosition(Vector3 position, bool useAnchoredPosition = false)
    {
        this.useAnchoredPosition = useAnchoredPosition;
        if (useAnchoredPosition)
        {
            if (rectTransform == null)
            {
                rectTransform = (RectTransform)transform;
            }
            rectTransform.anchoredPosition = position;
        }
        else
        {
            gameObject.transform.localPosition = position;
        }
    }
}
