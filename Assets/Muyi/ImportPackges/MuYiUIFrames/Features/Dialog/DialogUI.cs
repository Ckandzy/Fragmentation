using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogUI : MonoBehaviour {
    public Dialog DialogAsset;
    public GameObject Avatar;
    public Text text;
    public int index = 0;

    public void Start()
    {
        Avatar.GetComponent<Image>().sprite = DialogAsset.TalkerSprite[index];
        text.text = DialogAsset.Words[index];
    }

    public void Next()
    {
        index++;
        if (index >= DialogAsset.TalkerSprite.Length) index = DialogAsset.TalkerSprite.Length - 1;
        Avatar.GetComponent<Image>().sprite = DialogAsset.TalkerSprite[index];
        text.text = DialogAsset.Words[index];
    }

    public void Last()
    {
        index--;
        if (index < 0)  index = 0;
        Avatar.GetComponent<Image>().sprite = DialogAsset.TalkerSprite[index];
        text.text = DialogAsset.Words[index];
    }

    public void Over()
    {

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Next();
            return;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            Last();
            return;
        }
    }
}


