using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class ExampleCircleTimer : MonoBehaviour
{
    public GameObject imagePrefab; // Assign the Image prefab in the Inspector
    public GameObject battleBoard; // Assign the BattleBoard GameObject in the Inspector
    public float fillDuration; // Time to reduce fill amount from 1 to 0
    public ActualXYCoordinatesController coordinateScript; // The script translating tile XY index into actual unity XY coordinates


    private GameObject imageInstance;
    private Image imageComponent;
    private float elapsedTime = 0f;
    private Coroutine fillCoroutine;

    public void ExampleSpawnCircleTimer(int gridX, int gridY)
    {
        // Reset the coroutine if already running (to avoid timing overlap)
        if (fillCoroutine != null)
        {
            StopCoroutine(fillCoroutine);
        }

        // Instantiate the Image prefab as a child of CanvasFight
        imageInstance = Instantiate(imagePrefab, battleBoard.transform);

        // Set the position of the instantiated object
        RectTransform rectTransform = imageInstance.GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            rectTransform.anchoredPosition = new Vector2(coordinateScript.GetActualXCoordinate(gridX), coordinateScript.GetActualYCoordinate(gridY));
        }

        // Get the Image component and set its initial fill properties
        imageComponent = imageInstance.GetComponent<Image>();
        if (imageComponent != null)
        {
            imageComponent.type = Image.Type.Filled;
            imageComponent.fillMethod = Image.FillMethod.Radial360;
            imageComponent.fillOrigin = (int)Image.Origin360.Top;
            imageComponent.fillAmount = 1f;
            imageComponent.fillClockwise = false;

            // Ensure elapsedTime is reset each time the object is instantiated
            elapsedTime = 0f;

            // Start the coroutine to decrease the fill amount over time
            fillCoroutine = StartCoroutine(DecreaseFillOverTime());
        }
        else
        {
            Debug.LogError("Prefab does not contain an Image component.");
        }
    }

    private System.Collections.IEnumerator DecreaseFillOverTime()
    {
        elapsedTime = 0f;

        Debug.Log(elapsedTime + "," + fillDuration);

        while (elapsedTime < fillDuration)
        {
            if (imageComponent != null) // Check if the imageComponent still exists
            {
                elapsedTime += Time.deltaTime;
                // Gradually decrease the fill amount based on time
                imageComponent.fillAmount = Mathf.Lerp(1f, 0f, elapsedTime / fillDuration);
            }
            yield return null;
        }
        // Once the fill is complete, destroy the image instance
        Destroy(imageInstance);
    }

    private void OnDestroy()
    {
        // Stop the coroutine when the object is destroyed to prevent accessing destroyed components
        if (fillCoroutine != null)
        {
            StopCoroutine(fillCoroutine);
        }
    }
}