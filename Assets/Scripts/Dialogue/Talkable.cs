using System.Collections;
using System.Collections.Generic;
using UnityEditor.XR;
using UnityEngine;

public class Talkable : MonoBehaviour
{
    [SerializeField] private bool _canTalk, _isPerson;
    [TextArea][SerializeField] private string[] _lines;
    [SerializeField] private GameObject _dialogueSign;
    [SerializeField] private Questable _questable;
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
        if (_canTalk && Input.GetKeyDown(KeyCode.R) && !UIManager.Instance.GetForm<DialogueForm>().IsTalking)
        {
            UIManager.Instance.Open<DialogueForm>();
            UIManager.Instance.GetForm<DialogueForm>().ShowDialogue(_lines, _isPerson);
            UIManager.Instance.GetForm<DialogueForm>().CurrentQuestable = _questable;
        }
    }
}
