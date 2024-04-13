
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class DarkMode : UdonSharpBehaviour
{
    public GUIManager guiManager;
    public SkinnedMeshRenderer meshRenderer;
    public Invisible invisible;

    private float _fade;
    private float _alpha;
    private float _delta;
    private bool _timer_fade;
    private bool _fadeInOrOut;

    private const float TIME_FADE = 1f;

    public void Update() {
        if (_timer_fade) {
            _delta += Time.deltaTime;
            _fade += Time.deltaTime;
            if (_delta >= 0.02f) {
                _delta -= 0.02f;
                if (_fadeInOrOut)
                    FadeIn();
                else
                    FadeOut();

                if (_fade > TIME_FADE) {
                    _delta = 0f;
                    _fade = 0f;
                    _timer_fade = false;
                }
            }
        }
    }

    public void BlindStart() {
        _timer_fade = true;
        _fadeInOrOut = true;
        invisible.gameObject.SetActive(true);
    }
    public void BlindEnd() {
        _timer_fade = true;
        _fadeInOrOut = false;
        invisible.gameObject.SetActive(false);
    }


    private void FadeIn() {
        float time = _fade / TIME_FADE;
        _alpha = Mathf.Lerp(0, 1, time);
        meshRenderer.material.SetFloat("_Alpha", _alpha);
    }
    private void FadeOut() {
        float time = _fade / TIME_FADE;
        _alpha = Mathf.Lerp(1, 0, time);
        meshRenderer.material.SetFloat("_Alpha", _alpha);
    }
}
