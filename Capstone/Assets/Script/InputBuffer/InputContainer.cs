using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;

public class InputContainer : MonoBehaviour
{

    //List For all Input
    List<BaseInput> inputBuffer = new List<BaseInput>();
    Dictionary<TimerRelated, InputTimer> bufferTimer = new Dictionary<TimerRelated, InputTimer>();

    //Dictionary For all Input Tag
    Dictionary<InputType, InputTag> inputTags = new Dictionary<InputType, InputTag>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public void AddAnInput(InputType type, bool ifDebug = false)
    {
        //if it outputs an tag, add to input tags

        //add to buffer and start a timer
        BaseInput newInput = new BaseInput(type, ifDebug);
        inputBuffer.Add(newInput);
        InputTimer timer = new InputTimer();
        timer.Init(0.5f);
        bufferTimer.Add(newInput, timer);

        //reorder the buffer
        inputBuffer = inputBuffer.OrderByDescending(x => GetPriotiryOfInputType(x.GetInput())).ToList();

        DebugPrintInputBuffer();
    }

    public static int GetPriotiryOfInputType(InputType type)
    {
        switch (type)
        {
            case InputType.E:
                return 1;

            case InputType.W:
                return 2;

        }

        return -999;

    }

    public BaseInput PopInput()
    {
        if (inputBuffer.Count == 0)
            return null;

        BaseInput toReturn = inputBuffer[0];
        inputBuffer.RemoveAt(0);
        bufferTimer.Remove(toReturn);

        return toReturn;

    }

    public void DebugPrintInputBuffer()
    {
        if (inputBuffer == null || inputBuffer.Count == 0)
        {
            Debug.Log("Input buffer is empty.");
            return;
        }

        Debug.Log("==== Input Buffer Debug Start ====");
        for (int i = 0; i < inputBuffer.Count; i++)
        {
            var input = inputBuffer[i];
            string inputName = input.GetInput().ToString();
            Debug.Log($"Index {i}: {inputName}");
        }
        Debug.Log("==== Input Buffer Debug End ====");
    }

    private void Update()
    {
        List<BaseInput> toRemove = new List<BaseInput>();
        foreach (BaseInput input in bufferTimer.Keys)
        {
            InputTimer timer = bufferTimer[input];

            if (timer.Count(Time.deltaTime))
            {
                toRemove.Add(input);
            }
        }

        foreach (BaseInput input in toRemove)
        {
            inputBuffer.Remove(input);
            bufferTimer.Remove(input);
        }

    }
}

class InputTimer
{
    private float recordTime = 999;
    private bool start = false;
    public void Init(float time)
    {
        recordTime = time;
        start = true;
    }

    public bool Count(float t)
    {
        if (start)
        {
            recordTime -= t;
            if (recordTime <= 0)
            {
                //start = false;
                return true;

            }

        }
        return false;

    }

}
