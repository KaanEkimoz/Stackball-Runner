using UnityEngine;
public class LevelSlider : MonoBehaviour
{ 
    [SerializeField] private GameUI gameUI;
    private void OnEnable()
    {
        if(Player.OnPlayerSmash != null)
            Player.OnPlayerSmash.AddListener(FillSlider);
       
    }
    private void OnDisable()
    {
        if(Player.OnPlayerSmash != null)
            Player.OnPlayerSmash.RemoveListener(FillSlider);
    }
    private void FillSlider()
    {
        gameUI.LevelSliderFill(StackManager.CurrentBrokenStacks / (float) StackManager.TotalStacks);
    }
}
