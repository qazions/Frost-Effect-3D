using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrostManager : MonoBehaviour
{
    [Header("Character Materials")]
    // List to hold the character's materials that will have the frost effect
    [SerializeField] private List<Material> characterMaterials;

    [Header("Freeze Settings")]
    [SerializeField, Range(0, 1)] private float freezeSpeed = 0.1f;

    private Coroutine frostCoroutine;

    private void SetFreezeAmount(float targetFreezeAmount)
    {
        // Adjust the freeze amount on each material
        foreach (var material in characterMaterials)
        {
            material.SetFloat("_FreezeAmount", targetFreezeAmount);
        }
    }

    // Call to gradually freeze the character
    public void Freeze()
    {
        // Stop any running frost effect to prevent conflict
        if (frostCoroutine != null)
        {
            StopCoroutine(frostCoroutine);
        }
        frostCoroutine = StartCoroutine(FrostEffectCoroutine(1f)); // Target full freeze
    }

    // Call to gradually unfreeze the character
    public void Unfreeze()
    {
        // Stop any running frost effect to prevent conflict
        if (frostCoroutine != null)
        {
            StopCoroutine(frostCoroutine);
        }
        frostCoroutine = StartCoroutine(FrostEffectCoroutine(0f)); // Target no freeze
    }

    private IEnumerator FrostEffectCoroutine(float targetFreezeAmount)
    {
        // Get the current freeze amount from the first material
        float currentFreezeAmount = characterMaterials[0].GetFloat("_FreezeAmount");

        // Gradually move towards the target freeze amount
        while (!Mathf.Approximately(currentFreezeAmount, targetFreezeAmount))
        {
            currentFreezeAmount = Mathf.MoveTowards(currentFreezeAmount, targetFreezeAmount, freezeSpeed * Time.deltaTime);

            // Update all materials' freeze amounts
            foreach (var material in characterMaterials)
            {
                material.SetFloat("_FreezeAmount", currentFreezeAmount);
            }

            yield return null; // Wait for the next frame
        }
    }
}
