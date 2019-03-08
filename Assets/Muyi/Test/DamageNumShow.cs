using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DamageNumShow : MonoBehaviour
{
    public Text[] texts;

    private void Awake()
    {
        texts = GetComponentsInChildren<Text>();
        for(int i = 0; i < texts.Length; i++)
        {
            texts[i].gameObject.SetActive(false);
        }
    }

    private Text FindFreeText()
    {
        for(int i = 0;  i < texts.Length; i++)
        {
            if (!texts[i].gameObject.activeSelf)
                return texts[i];
        }
        return null;
    }

    private IEnumerator ShowAndFindPath(Text _text)
    {
        Debug.Log("show");
        float timer = 0;
        float flyTime = 1;
        float angle = UnityEngine.Random.Range(70, 110);
        float speed = UnityEngine.Random.Range(4, 7);
        
        Vector3 vtest = Quaternion.Euler(new Vector3(0, 0, angle)) * new Vector2(1, 0);
        while (timer < flyTime)
        {
            yield return new WaitForSeconds(Time.deltaTime);
            timer += Time.deltaTime;
            _text.rectTransform.position += vtest * speed;
        }
        _text.gameObject.SetActive(false);
        timer = 0;
    }

    public void ShowNum(TakeDamager damager, TakeDamageable able)
    {
        Text text = FindFreeText();
        text.text = damager.CurrentDamagNum.ToString();
        text.transform.localPosition = Vector3.zero;
        text.gameObject.SetActive(true);
        StartCoroutine(ShowAndFindPath(text));
    }

    public void Miss(TakeDamager damager, TakeDamageable able)
    {

    }
}
