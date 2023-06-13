using UnityEngine;
public class StackManager : MonoBehaviour
{
    public static int TotalStacks;
    public static int CurrentBrokenStacks = 0;
    private void Start()
    {
        TotalStacks = FindObjectsOfType<StackController>().Length;
    }
}
