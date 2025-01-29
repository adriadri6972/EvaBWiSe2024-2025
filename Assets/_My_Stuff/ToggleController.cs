using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ToggleController:MonoBehaviour {
    public Toggle toggle;

    void Start() {
        if (toggle != null) {
            toggle.isOn = SlowRotate.startAtDay;
            toggle.onValueChanged.AddListener(SetStartAtDay);
        }
    }

    public void SetStartAtDay(bool value) {
        SlowRotate.startAtDay = value;
    }

    public void LoadSceneOne() {
        SceneManager.LoadScene(1);
    }
}