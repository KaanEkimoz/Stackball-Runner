using UnityEngine;
public class LevelSlider : MonoBehaviour
{
    private void OnEnable()
    {
       Player.OnPlayerSmash.AddListener(FillSlider);
    }
    private void OnDisable()
    {
        Player.OnPlayerSmash.RemoveListener(FillSlider);
    }
    private void FillSlider()
    {
        FindObjectOfType<GameUI>().LevelSliderFill(StackManager.CurrentBrokenStacks / (float) StackManager.TotalStacks);
    }
}
