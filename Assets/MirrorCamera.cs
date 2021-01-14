using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorCamera : MonoBehaviour
{
    private Transform mainCam;
    [SerializeField] Transform mirror;
    // Start is called before the first frame update
    void Start()
    {
        mainCam = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 cameraOffsetFromMirror = mainCam.position - mirror.position;
        transform.position = mirror.position + new Vector3(-cameraOffsetFromMirror.x, cameraOffsetFromMirror.y, cameraOffsetFromMirror.z);

        //float angularDifferenceBetweenMirrorRotation = Quaternion.Angle(mirror.rotation, Quaternion.Euler(mirror.eulerAngles + new Vector3(0,0,180f)));
        //Quaternion mirrorRotationalDifference = Quaternion.AngleAxis(angularDifferenceBetweenMirrorRotation, Vector3.up);
        //Vector3 newDir = mirrorRotationalDifference * mainCam.forward;
        transform.rotation =Quaternion.LookRotation(new Vector3(-mainCam.forward.x, mainCam.forward.y, mainCam.forward.z));
    }
}
