using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransitionManager : MonoBehaviour
{
    public Image FadeImage;
    public static TransitionManager instance;
    Coroutine Fading;
    private void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartFade(Transform Using, Transform target, float dur, DoorScript DestinationDoor = null, DoorScript CurrentDoor = null)
    {
        if (Fading == null)
        {
            Debug.Log("douleur");
            target.position = new Vector3(DestinationDoor.SpawnPoint.position.x, (Using.transform.position.y - DestinationDoor.SpawnPoint.position.y) + DestinationDoor.SpawnPoint.position.y, DestinationDoor.SpawnPoint.position.z);
            Fading = StartCoroutine(FadeTo(Using, dur, DestinationDoor, CurrentDoor));
        }
    }

    IEnumerator FadeTo(Transform target, float dur, DoorScript DestinationDoor = null, DoorScript CurrentDoor = null)
    {

        Time.timeScale = 0;
        var tempColor = FadeImage.color;
        float t = 0;
        PlayerHelper.instance.ToggleControls();

        while (t < dur)
        {
            t += Time.unscaledDeltaTime;

            float blend = Mathf.Clamp01(t / dur);

            tempColor.a = Mathf.Lerp(0, 1, blend);

            FadeImage.color = tempColor;
       
            yield return null;
        }

        target.position = new Vector3(DestinationDoor.SpawnPoint.position.x, DestinationDoor.SpawnPoint.position.y, DestinationDoor.SpawnPoint.position.z);
        t = 0;

        yield return new WaitForSecondsRealtime(1);

        while (t < dur)
        {
            t += Time.unscaledDeltaTime;

            float blend = Mathf.Clamp01(t / dur);

            tempColor.a = Mathf.Lerp(1, 0, blend);

            FadeImage.color = tempColor;

            yield return null;
        }
        PlayerHelper.instance.ToggleControls();
        Fading = null;
        Time.timeScale = 1;
    }
}
