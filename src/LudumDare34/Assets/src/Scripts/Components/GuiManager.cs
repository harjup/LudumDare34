using UnityEngine;
using System.Collections;
using DG.Tweening;

public class GuiManager : MonoBehaviour
{
    private GameObject _failureGui;
    private GameObject _battlGui;
    private GameObject _dialogGui;

    
    void Start()
    {
        _failureGui = transform.Find("FailureGui").gameObject;
        _battlGui = transform.Find("BattleGui").gameObject;
        _dialogGui = transform.Find("DialogGui").gameObject;
    }

    public void StartFailureSequence()
    {
        if (_failureGui.activeSelf)
        {
            return;
        }

        DOTween.Sequence()
            .AppendInterval(1f)
            .AppendCallback(() => _battlGui.SetActive(false))
            .AppendCallback(() => _failureGui.SetActive(true))
            .AppendCallback(() => _failureGui.transform.localPosition = new Vector3(0, 650))
            .Append(_failureGui.transform.DOLocalMove(new Vector3(0, 0), 1f).SetEase(Ease.OutBounce))
            .Play();
    }

}
