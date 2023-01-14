using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SpriteGlow;

public class block : MonoBehaviour
{
    public ParticleSystem explosion;
    public int health = 1;
    public int scoreValue;
    private Color32 orginalColor;
    public static bool loadblocks=false;
    [SerializeField]
    SpriteGlowEffect spriteGlowEffect;

    void Awake()
    {
        if(this != null)
        {
            GameSession.blocks.Add(this);
        }
        
    }
    private void Start()
    {
        spriteGlowEffect = GetComponent<SpriteGlowEffect>();
        orginalColor = spriteGlowEffect.GlowColor;
     


    }
    public void Update()
    {
        if (loadblocks == true)
        {
            blockCounter();
            loadblocks = false;
        }
    }
    public void blockCounter()
    {
       
       
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ball")
        {
            StartCoroutine(ColorChanger());
            health--;
            if(health <= 0)
            {
                ParticleSystem newExplosion = Instantiate(explosion);
                newExplosion.transform.position = gameObject.transform.position;
                newExplosion.Play();
                FindObjectOfType<GameSession>().increaseScore(scoreValue);
                FindObjectOfType<GameSession>().blockPrefabs.Remove(this);
                GameSession.blocks.Remove(this);
                Destroy(gameObject);
            }
        }
    }
    IEnumerator ColorChanger()
    {
        spriteGlowEffect.GlowColor = new Color(0.5f,0.5f, 0.5f);
        yield return new WaitForSeconds(0.2f);
        spriteGlowEffect.GlowColor = orginalColor;
    }
    public void SetActiveFalse()
    {
        gameObject.SetActive(false);
    }
    
}

  
