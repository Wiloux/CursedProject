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

    public void StartFade(Transform Using, float dur, DoorScript DestinationDoor = null, DoorScript CurrentDoor = null)
    {
        if (Fading == null)
        {
        //    target = new Vector3(DestinationDoor.spawnPoint.x, (DestinationDoor.spawnPoint.y + 1.1f), DestinationDoor.spawnPoint.z);

            Fading = StartCoroutine(FadeTo(Using, dur, DestinationDoor, CurrentDoor));
        }
    }

    IEnumerator FadeTo(Transform target, float dur, DoorScript DestinationDoor = null, DoorScript CurrentDoor = null)
    {

        Time.timeScale = 0;
        var tempColor = FadeImage.color;
        float t = 0;
        target.GetComponent<CharacterController>().enabled = false;
        GameHandler.instance.locking = true;
        PlayerHelper.instance.ToggleControls();

        while (t < dur)
        {
            t += Time.unscaledDeltaTime;

            float blend = Mathf.Clamp01(t / dur);

            tempColor.a = Mathf.Lerp(0, 1, blend);

            FadeImage.color = tempColor;
       
            yield return null;
        }

        target.position = new Vector3(DestinationDoor.spawnPoint.x, (DestinationDoor.spawnPoint.y + 1.1f), DestinationDoor.spawnPoint.z);
       // Vector3 relativePos =  DestinationDoor.transform.position - DestinationDoor.spawnPoint;
      //  target.LookAt(-relativePos);
     //   Debug.Log(-relativePos);
       

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
        GameHandler.instance.locking = false;
        Fading = null;
        target.GetComponent<CharacterController>().enabled = true;
        Time.timeScale = 1;
    }
}
