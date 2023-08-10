using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class ScriptedAnimation : MonoBehaviour
{
    public float delay;

    private MeshRenderer _meshRenderer;
    private Material _originalMaterial;
    public Material scriptedAnimationMaterial;

    private Color targetColor = Color.red;

    private float animationDuration = 1f;

    public void StartAnimating()
    {
        //StopAnimating();
        _meshRenderer = GetComponent<MeshRenderer>();
        scriptedAnimationMaterial = new Material(scriptedAnimationMaterial);
        StartCoroutine(AnimationCoroutine());
    }

    public IEnumerator AnimationCoroutine()
    {
        yield return new WaitForSeconds(delay);

        _originalMaterial = _meshRenderer.material;
        _meshRenderer.material = scriptedAnimationMaterial;

        Color initialColor = _meshRenderer.material.color;
        float elapsedTime = 0.0f;

        float halfDuration = animationDuration / 2f;

        while (elapsedTime < halfDuration)
        {
            float normalizedTime = elapsedTime / halfDuration;
            Color lerpedColor = Color.Lerp(initialColor, targetColor, normalizedTime);
            _meshRenderer.material.color = lerpedColor;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        elapsedTime = 0.0f;

        while (elapsedTime < halfDuration)
        {
            float normalizedTime = elapsedTime / halfDuration;
            Color lerpedColor = Color.Lerp(targetColor, initialColor, normalizedTime);
            _meshRenderer.material.color = lerpedColor;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Setting the color back to the initial color
        _meshRenderer.material.color = initialColor;


        StopAnimating();

        yield return null;
    }

    public void StopAnimating()
    {
        StopAllCoroutines();
        GetComponent<MeshRenderer>().material = _originalMaterial;
        Destroy(this);
    }


}
