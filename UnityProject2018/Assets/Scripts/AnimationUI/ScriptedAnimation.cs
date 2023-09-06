using System.Collections;
using System.Collections.Generic;
//using UnityEditor.SceneManagement;
using UnityEngine;

public class ScriptedAnimation : MonoBehaviour
{
    public float delay;

    private MeshRenderer _meshRenderer;
    private Material _originalMaterial;
    public Material scriptedAnimationMaterial;

    private Color targetColor = Color.green;

    private Color initialColor;

    private float animationDuration = 2f; // Not including animation mid effect
    private float animationEffectDuration = 0.5f;

    private bool _hasCofactors = false;

    private Coroutine animationRoutine;

    private List<CofactorParent> cofactorParents;
    private List<CofactorLabel> cofactorLabels;
    private List<GameObject> clonedCofactors;
    private EdgeDataDisplay edgeDataDisplay;

    public bool fromReactant = false;
    public bool biDirectional = false;

    // Still gotta check for bidirectional. Right now this will only work one way for cofactors.

    public void InitializeScriptedAnimation()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _originalMaterial = _meshRenderer.material;
        cofactorLabels = new List<CofactorLabel>();

        edgeDataDisplay = GetComponentInChildren<EdgeDataDisplay>();

        if (edgeDataDisplay != null)
        {
            if (edgeDataDisplay.cofactorParents.Count > 0)
            {
                _hasCofactors = true;
                cofactorParents = edgeDataDisplay.cofactorParents;

                for (int i = 0; i < cofactorParents.Count; i++)
                {
                    if (!biDirectional)
                    {
                        cofactorLabels.AddRange(cofactorParents[i].cofactorLabels);
                    }
                    else if (biDirectional && fromReactant)
                    {
                        if (!cofactorParents[i].secondDirection)
                            cofactorLabels.AddRange(cofactorParents[i].cofactorLabels);
                    }
                    else if (biDirectional && !fromReactant)
                    {
                        if (cofactorParents[i].secondDirection)
                            cofactorLabels.AddRange(cofactorParents[i].cofactorLabels);
                    }

                    //cofactorParents[i].ToggleArrow(false);

                }

            }
            else
                _hasCofactors = false;
        }
    }

    public void StartAnimating()
    {
        scriptedAnimationMaterial = new Material(scriptedAnimationMaterial);
        scriptedAnimationMaterial.color = initialColor = _originalMaterial.GetColor("_WiggleColor");
        _meshRenderer.material = scriptedAnimationMaterial;

        animationRoutine = StartCoroutine(AnimationCoroutine());
    }

    public IEnumerator AnimationCoroutine()
    {
        yield return new WaitForSeconds(delay);

        _meshRenderer.material = scriptedAnimationMaterial;

        float elapsedTime = 0.0f;

        float halfDuration = animationDuration / 2f;

        List<Vector3> clonedCofactorLabelStartPositions = new List<Vector3>();

        clonedCofactors = new List<GameObject>();

        // If edge has cofactors, clone them and use the clones for animations
        if (_hasCofactors)
        {
            for (int i = 0; i < cofactorLabels.Count; i++)
            {
                GameObject clonedCofactor = Instantiate(cofactorLabels[i].gameObject);
                clonedCofactor.transform.position = cofactorLabels[i].transform.position;
                clonedCofactors.Add(clonedCofactor);

                clonedCofactorLabelStartPositions.Add(clonedCofactor.transform.position);

                // Enable cofactor if it's a reactant
                clonedCofactor.SetActive(false);



            }

            for (int i = 0; i < cofactorParents.Count; i++)
            {
                cofactorParents[i].ToggleChildren(false);
            }
        }

        List<GameObject> cofactorsToAnimate = new List<GameObject>();
        List<Vector3> cofactorsToAnimateStartPosition = new List<Vector3>();

        // Toggle cofactors on/off depending on reaction/product
        for (int i = 0; i < clonedCofactors.Count; i++)
        {
            if (!biDirectional)
            {
                ToggleCofactorType(clonedCofactors[i], cofactorLabels[i].cofactor.isReactant);
            }
            else if (biDirectional && fromReactant)
            {
                ToggleCofactorType(clonedCofactors[i], cofactorLabels[i].cofactor.isReactant);
            }
            else
            {
                ToggleCofactorType(clonedCofactors[i], !cofactorLabels[i].cofactor.isReactant);
            }

            if (clonedCofactors[i].gameObject.activeInHierarchy)
            {
                cofactorsToAnimate.Add(clonedCofactors[i]);
                cofactorsToAnimateStartPosition.Add(clonedCofactorLabelStartPositions[i]);
            }
        }


        while (elapsedTime < halfDuration)
        {

            float normalizedTime = elapsedTime / halfDuration;
            Color lerpedColor = Color.Lerp(initialColor, targetColor, normalizedTime);
            _meshRenderer.material.color = lerpedColor;

            for (int i = 0; i < cofactorsToAnimate.Count; i++)
            {
                cofactorsToAnimate[i].transform.position = Vector3.Lerp(cofactorsToAnimateStartPosition[i], edgeDataDisplay.transform.position, normalizedTime);
            }


            elapsedTime += Time.deltaTime;
            yield return null;
        }

        elapsedTime = 0.0f;

        // Add effect here to visualize cofactor change

        //while (elapsedTime < animationEffectDuration)
        //{
        //    float normalizedTime = elapsedTime / halfDuration;
        //    //Color lerpedColor = Color.Lerp(targetColor, initialColor, normalizedTime);
        //    //_meshRenderer.material.color = lerpedColor;

        //    for (int i = clonedCofactors.Count; i > 0; i--)
        //    {

        //        clonedCofactors[i - 1].transform.position = Vector3.Lerp(edgeDataDisplay.transform.position, clonedCofactorLabelStartPositions[i - 1], normalizedTime);
        //    }

        //    elapsedTime += Time.deltaTime;
        //    yield return null;
        //}

        //elapsedTime = 0.0f;

        cofactorsToAnimate = new List<GameObject>();
        cofactorsToAnimateStartPosition = new List<Vector3>();

        // Toggle cofactors on/off depending on reaction/product
        for (int i = 0; i < clonedCofactors.Count; i++)
        {
            if (!biDirectional)
            {
                ToggleCofactorType(clonedCofactors[i], !cofactorLabels[i].cofactor.isReactant);
            }
            else if (biDirectional && fromReactant)
            {
                ToggleCofactorType(clonedCofactors[i], !cofactorLabels[i].cofactor.isReactant);
            }
            else
            {
                ToggleCofactorType(clonedCofactors[i], cofactorLabels[i].cofactor.isReactant);
            }

            if (clonedCofactors[i].gameObject.activeInHierarchy)
            {
                cofactorsToAnimate.Add(clonedCofactors[i]);
                cofactorsToAnimateStartPosition.Add(clonedCofactorLabelStartPositions[i]);
            }
        }

        //for (int i = 0; i < clonedCofactors.Count; i++)
        //{
        //    ToggleCofactorType(clonedCofactors[i], !cofactorLabels[i].cofactor.isReactant);
        //    if (clonedCofactors[i].gameObject.activeInHierarchy)
        //    {
        //        cofactorsToAnimate.Add(clonedCofactors[i]);
        //        cofactorsToAnimateStartPosition.Add(clonedCofactorLabelStartPositions[i]);
        //    }
        //}

        while (elapsedTime < halfDuration)
        {
            float normalizedTime = elapsedTime / halfDuration;
            Color lerpedColor = Color.Lerp(targetColor, initialColor, normalizedTime);
            _meshRenderer.material.color = lerpedColor;

            for (int i = 0; i < cofactorsToAnimate.Count; i++)
            {

                cofactorsToAnimate[i].transform.position = Vector3.Lerp(edgeDataDisplay.transform.position, cofactorsToAnimateStartPosition[i], normalizedTime);
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Setting the color back to the initial color
        _meshRenderer.material.color = initialColor;
        _meshRenderer.material = _originalMaterial;

        FinishAnimating();

        yield return null;
    }

    public void FinishAnimating()
    {
        StopCoroutine(animationRoutine);
        DestroyAllCofactors();
        Destroy(this);
    }

    public void StopAnimating()
    {
        if (_meshRenderer.material != _originalMaterial)
            _meshRenderer.material = _originalMaterial;

        DestroyAllCofactors();

        StopAllCoroutines();
        Destroy(this);
    }

    private void DestroyCofactorTypes(bool reactant)
    {
        if (clonedCofactors != null)
        {
            for (int i = 0; i < clonedCofactors.Count; i++)
            {
                if (clonedCofactors[i].GetComponent<CofactorLabel>().cofactorParent.isReactant == reactant)
                    Destroy(clonedCofactors[i].gameObject);
            }
            clonedCofactors.Clear();
        }
    }
    private void DestroyAllCofactors()
    {
        if (clonedCofactors != null)
        {
            for (int i = 0; i < clonedCofactors.Count; i++)
            {
                Destroy(clonedCofactors[i]);
            }
            clonedCofactors.Clear();
        }
    }

    private void ToggleCofactorType(GameObject cofactorObject, bool state) // enable reactants or product
    {
        cofactorObject.SetActive(state);
    }


}
