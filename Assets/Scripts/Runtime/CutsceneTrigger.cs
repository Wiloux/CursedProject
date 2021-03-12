using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneTrigger : MonoBehaviour
{

    private Camera mainCam;
    [SerializeField] private int cutsceneSaveIndex;
    [SerializeField] private GameObject Cutscene_timeline;
    [SerializeField] private float cutscene_duration;

    private void Awake()
    {
        // check if cutscene already played
        if(WorldProgressSaver.instance != null)
        {
            if (WorldProgressSaver.instance.isCutscenesPlayed[cutsceneSaveIndex]) gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            GameHandler.instance.TogglePause();
            mainCam = Camera.main;
            mainCam.gameObject.SetActive(false);
            Cutscene_timeline.SetActive(true);
            StartCoroutine(StopCutscene(cutscene_duration));
            WorldProgressSaver.instance.isCutscenesPlayed[cutsceneSaveIndex] = true;
        }
    }

    private IEnumerator StopCutscene(float duration)
    {
        yield return new WaitForSeconds(duration);
        Cutscene_timeline.SetActive(false);
        mainCam.gameObject.SetActive(true);
        gameObject.SetActive(false);
        GameHandler.instance.TogglePause();
    }
}
