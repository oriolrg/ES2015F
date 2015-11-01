using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Countdown : MonoBehaviour
{
    [SerializeField] private int remainingTime;

    private UnityAction callback;

    private Text text;

    private bool counting;

    void Awake()
    {
        text = gameObject.GetComponentOrEnd<Text>();
        counting = false;
    }
	public void stop()
    {
        CancelInvoke("decrement");
        counting = false;
    }

    public void setTimer( int time, UnityAction callback )
    {
        print("called");
        if (!counting)
        {
            counting = true;
            remainingTime = time;
            text.text = remainingTime.ToString();
            this.callback = callback;

            InvokeRepeating("decrement", 1, 1);
        }
    }

    private void decrement()
    {
        remainingTime--;
        text.text = remainingTime.ToString();

        if ( remainingTime <= 0)
        {
            callback.Invoke();
            CancelInvoke("decrement");
        }
         
    }

    
}
