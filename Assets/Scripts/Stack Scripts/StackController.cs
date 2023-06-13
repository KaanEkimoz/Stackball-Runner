using System;
using System.Collections;
using UnityEngine;
public class StackController : MonoBehaviour
{
    public AudioClip stackDestroyClip;
    [SerializeField] private StackPartController[] stackPartControls = null;

    private void Start()
    {
        stackPartControls = GetComponentsInChildren<StackPartController>();
    }

    public void ShatterAllParts()
    {
        if(transform.parent != null)
        {
            transform.parent = null;
            StackManager.CurrentBrokenStacks++;
            SoundManager.instance.PlaySoundFX(stackDestroyClip, 0.5f);
            ScoreManager.instance.AddScore(2);
        }

        foreach (StackPartController stackPart in stackPartControls)
        {
            stackPart.Shatter();
        }
        StartCoroutine(RemoveParts());

    }
    private IEnumerator RemoveParts()
    {
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
    
}
