using UnityEngine;

public class NightForestTrap : NightTrap {
    public override void OnPlayerOver() {
        base.OnPlayerOver();
        MainScene.PlayerState.NightHitPoint -= (int)(MainScene.PlayerState.NightHitPoint * 0.1f);
    }
}
