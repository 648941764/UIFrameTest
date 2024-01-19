using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Talkable : MonoBehaviour
{
    [SerializeField]private bool _canTalk, _isPerson;
    [TextArea][SerializeField] private string[] _lines;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _canTalk = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _canTalk = false;
        }
    }

    private void Update()
    {
        if (_canTalk && Input.GetKeyDown(KeyCode.R) && !DialogueManager.Instance.IsTalking)
        {
            DialogueManager.Instance.ShowDialogue(_lines, _isPerson);
        }
    }
}
