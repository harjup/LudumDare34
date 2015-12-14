using UnityEngine;
using System.Collections;
using DG.Tweening;

public class GuiManager : MonoBehaviour
{
    private GameObject _failureGui;
    private GameObject _battlGui;
    private GameObject _introGui;

    
    void Start()
    {
        _failureGui = transform.Find("FailureGui").gameObject;
        _battlGui = transform.Find("BattleGui").gameObject;
        _introGui = transform.Find("IntroGui").gameObject;

        HideAll();
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

    public void ShowIntroText()
    {
        DOTween.Sequence()
           .AppendCallback(HideAll)
           .AppendCallback(() => _introGui.SetActive(true))
           .AppendCallback(() => _introGui.transform.localPosition = new Vector3(-800, 0))
           .Append(_introGui.transform.DOLocalMove(new Vector3(0, 0), .5f).SetEase(Ease.OutSine))
           .AppendInterval(.5f)
           .Append(_introGui.transform.DOLocalMove(new Vector3(800, 0), .5f).SetEase(Ease.InSine))
           .AppendCallback(() => _introGui.SetActive(false))
           .Play();
    }

    public void ShowBattleGui()
    {
        HideAll();
        _battlGui.SetActive(true);
    }

    public void HideAll()
    {
        _failureGui.SetActive(false);
        _battlGui.SetActive(false);
        _introGui.SetActive(false);

    }

}
