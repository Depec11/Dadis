using UnityEngine;
public sealed class NightPlayerObject : NightMapObject { 
    private void Awake() {
        m_state = NightMapStateEnum.PLAYER;
    }
}