using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DelusionUIController : MonoBehaviour
{

    public float maxDelusion;
    public float currentDelusion;

    public int delusionGain;
	
    public Image bar;

	private Animator anim;

	private int _closeToInsanityHashParam;

    // Use this for initialization
    void Start()
    {
		anim = GetComponent<Animator>();
        currentDelusion = 0;
        maxDelusion = 100;
        delusion();
		_closeToInsanityHashParam = Animator.StringToHash("CloseToInsanity");
    }

    // Update is called once per frame
    void Update()
    {
        IncreaseDelusion(delusionGain * Time.deltaTime);
    }

    public void IncreaseDelusion(float del)
    {

        if (currentDelusion + del < maxDelusion) currentDelusion += del;
        else
        {
            currentDelusion = maxDelusion;
            Debug.Log("Insanity!!");
        }
        delusion();
    }

    private void delusion()
    {

        bar.fillAmount = currentDelusion / maxDelusion;
        if (bar.fillAmount >= 0.8) { bar.color = Color.red; 
		//anim.SetBool("CloseToInsanity",true);
		anim.SetBool(_closeToInsanityHashParam, true);

		}
        else if (bar.fillAmount >= 0.5f) { bar.color = Color.yellow; }

    }
}
