using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LetterContainer : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] private SpriteRenderer letterContainer; 
    [SerializeField] private TMP_Text letter;





    public void Initialize()
    {
        letter.text = "";
        letterContainer.color = Color.white; // base color
    }

    public char GetLetter()
    {
        return letter.text[0];
    }

    public void SetValid()
    {
        letterContainer.gameObject.LeanColor(Color.green,.7f);
    }

    public void SetPotential()
    {
        letterContainer.gameObject.LeanColor(Color.yellow, .7f);
    }

    public void SetInvalid()
    {
        letterContainer.gameObject.LeanColor(Color.gray, .7f);
    }
    
    public void SetVoid()
    {
        Shake(.1f,1f);       
    }
    private void Shake(float duration, float magnitude)
    {        
        Vector3 originalPosition = transform.position;

        // Shake effect
        LeanTween.move(gameObject, originalPosition + new Vector3(Random.Range(-magnitude, magnitude), 0f, 0f), duration)
            .setEase(LeanTweenType.easeShake)
            .setOnComplete(() =>
            {
                // Titreme tamamlandığında, objeyi orijinal pozisyonuna geri getiriyoruz
                transform.position = originalPosition;
            });
    }

    public void SetLatter(char letter, bool isHint = false)
    {
        if(isHint)
            this.letter.color = Color.gray;
        else   
            this.letter.color = Color.black;
            
        this.letter.text = letter.ToString();
    }
}
