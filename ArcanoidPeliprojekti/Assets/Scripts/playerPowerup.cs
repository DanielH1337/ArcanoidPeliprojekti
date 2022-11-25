using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerPowerup : MonoBehaviour
{
    
    [SerializeField] AudioSource powerupSound;
    [SerializeField] int powerUpTime;

    private Vector3 scaleChange;
    public int growSize;
    public float shrinkSize;
    int time = 0;

  
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(Time.timeScale == 0f)
        {
            powerupSound.Stop();
        }
    }
    public void increasePaddleLenght()
    {
        if (time == powerUpTime)
        {
            powerupSound.Play();
            time = 0;
            StartCoroutine(slowDecrease());

        }
        else
        {
            powerupSound.Play();
            StartCoroutine(slowIncrease());
            StartCoroutine(PowerUpTimer());
        }

    }
    private IEnumerator PowerUpTimer()
    {
        while (time < powerUpTime)
        {
            time += 1;
            Debug.Log(time);
            yield return new WaitForSeconds(1);
        }
        increasePaddleLenght();

    }
    private IEnumerator slowIncrease()
    {
        scaleChange = new Vector3(1f, 0.0f, 0.0f);

        while (true)
        {
            if (growSize > gameObject.transform.localScale.x)
            {
                gameObject.transform.localScale += scaleChange;
            }
            if (growSize == gameObject.transform.localScale.x)
            {
                powerupSound.Stop();
            }
            yield return new WaitForSeconds(0.001f);
        }

    }
    private IEnumerator slowDecrease()
    {
        scaleChange = new Vector3(-1f, 0.0f, 0.0f);
        while (shrinkSize +1 < gameObject.transform.localScale.x)
        {
            gameObject.transform.localScale += scaleChange;
            if (gameObject.transform.localScale.x == shrinkSize+1)
            {
                Debug.Log("moI");
                scaleChange = new Vector3(0.0f, 0.0f, 0.0f);
                powerupSound.Stop();
            }
            yield return new WaitForSeconds(0.001f);

        }


    }
}
