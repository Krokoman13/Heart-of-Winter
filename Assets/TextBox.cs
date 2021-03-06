using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

using HeartOfWinter.PlayerInformation;

public class TextBox : MonoBehaviourPun
{
    [SerializeField]
    private Queue<string> textList = new Queue<string>() {};

    [SerializeField]
    private int maxLineCount = 18;

    private Text textComponent;

    // Start is called before the first frame update
    void Start()
    {
        textComponent = GetComponent<Text>();

        setText();
    }

    public void ReadStringInput(string s)
    {
        photonView.RPC(nameof(addLIne), RpcTarget.All, PlayerInfo.character + ": " + s);
    }

    [PunRPC]
    private void addLIne(string line)
    {
        Debug.Log("Added line: " + line);
        textList.Enqueue(line);

        if (textList.Count >= maxLineCount) textList.Dequeue();

        setText();
    }

    private void setText()
    {
        string outp = "";

        foreach (string line in textList)
        {
            outp += line + '\n';
        }

        textComponent.text = outp;
    }
}
