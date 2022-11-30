using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleLerper : MonoBehaviour
{
    public AudioSource audio;
    Vector3 minScale;
    public bool repeateble;
    public Vector3 maxScale;
    public float speed = 2f;
    public float duration = 3f;
    public float time= 4f;

    public void StartFunction()
    {
        repeateble = true;
        StartCoroutine(StartLerp());
    }
    public IEnumerator StartLerp()
    {
        minScale = transform.localScale;
        while (repeateble)
        {
            yield return RepeatLerp(minScale, maxScale, duration);
            yield return new WaitForSeconds(time);
            yield return RepeatLerp(maxScale, minScale, duration);
            repeateble = false;

        } 
    }
    public IEnumerator RepeatLerp(Vector3 a, Vector3 b, float time)
    {
        float i = 0.0f;
        float rate = (1.0f / time) * speed;
        audio.Play();
        while (i < 1.0f)
        {
            i += Time.deltaTime * rate;
            transform.localScale = Vector3.Lerp(a, b, i);
            yield return null;
        }
        audio.Stop();
    }
    public void StopFunction()
    {
        StopAllCoroutines();
    }
}
