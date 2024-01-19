using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Talkable : MonoBehaviour
{
    [SerializeField]private bool _canTalk, _isPerson;
    [TextArea][SerializeField] private string[] _lines;
    [SerializeField] private GameObject _dialogueSign;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _dialogueSign.SetActivate(true);
            _canTalk = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _dialogueSign.SetActivate(false);
            _canTalk = false;   
        }
    }

    private void Update()//UPdate测试使用，后面会使用GameManager的Update
    {
        if (_canTalk && Input.GetKeyDown(KeyCode.R) && !DialogueManager.Instance.IsTalking)
        {
            DialogueManager.Instance.ShowDialogue(_lines, _isPerson);
        }
    }
}
