using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class DialogueManager : SingletonMono<DialogueManager>
{
    [SerializeField] private GameObject _dialogueBox;
    [SerializeField] private Text _dialogueName, _dialogueText;
    [SerializeField] private int  _currentLine;

    [TextArea]
    public string[] DialogueLines;

    private bool _isTalking;//TODO可以增加一个让角色不能移动功能

    public bool IsTalking => _isTalking;

    private void Start()
    {
        _dialogueText.text = DialogueLines[_currentLine];
        //GameManager.Instance.UpdateHandle += DialogueUpdate;
    }

    private void Update()//UPdate测试使用，后面会使用GameManager的Update
    {
        if (Input.GetMouseButtonUp(0) && _isTalking)
        {
            _currentLine++;
            if (_currentLine >= DialogueLines.Length)
            {
                _dialogueBox.SetActive(false);
                _isTalking = false;
            }
            else
            {
                CheckName();
                ShowText(_dialogueText, DialogueLines[_currentLine]);
            }
        }
    }

    public void ShowDialogue(string[] Lines, bool isPerson)
    {
        _dialogueName.SetActivate(false);
        DialogueLines = Lines;
        _currentLine = 0;
        if (isPerson)
        {
            _dialogueName.SetActivate(true);
            CheckName();
        }
        ShowText(_dialogueText, Lines[_currentLine]);
        _dialogueBox.SetActivate(true);
        _isTalking = true;
    }

    public void CheckName()
    {
        if (DialogueLines[_currentLine].StartsWith("n-")) 
        {
            _dialogueName.text = DialogueLines[_currentLine].Replace("n-", "");
            _currentLine++;
        }
    }

    private void ShowText(Text text, string content)
    {
        text.text = " ";
        DOTween.To(
            () => text.text,
            _ => text.text = _,
            content, 0.5f);
    }
}
