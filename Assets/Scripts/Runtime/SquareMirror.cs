using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareMirror : MonoBehaviour
{
    private Camera mainCam;
    [SerializeField] private Transform cameraRotation;
    [SerializeField] private Camera mirrorCam;
    [SerializeField] private Renderer mirror;

    RenderTexture renderTexture;
    Material renderMaterial;

    [SerializeField] float textureResolution;
    [SerializeField] string room;
    // Start is called before the first frame update
    void Start()
    {
        mainCam = Camera.main;

        Vector2 renderTextureDimension = new Vector2(textureResolution, textureResolution);
        renderTexture = new RenderTexture((int)renderTextureDimension.x, (int)renderTextureDimension.y,0);
        mirrorCam.targetTexture = renderTexture;

        Material renderMaterial = new Material(Shader.Find("HDRP/Unlit"));
        renderMaterial.mainTexture = renderTexture;

        mirror.material = renderMaterial;
    }

    // Update is called once per frame
    void Update()
    {
        //if (room == WorldProgress.instance.locationName)
        //{
            Vector3 mainCamDir = (mainCam.transform.position - transform.position).normalized;
            Quaternion rot = Quaternion.LookRotation(mainCamDir);
            Debug.Log("mainCamDir : " + mainCamDir);

            rot.eulerAngles = transform.eulerAngles - rot.eulerAngles;

            cameraRotation.localRotation = rot;
            Debug.Log("ahahhaha");
            //Vector3 dir = new Vector3(mainCamDir.x * -1f, mainCamDir.y, mainCamDir.z);
            //Debug.Log("dir = " + dir);
            //transform.rotation = Quaternion.LookRotation(dir);
        //}
    }

    //private Vector2 SetRenderTextureDimension(float resolution)
    //{
    //    Vector3 parentScale = transform.parent.localScale;
    //    Vector2 dimensions = (Vector2)parentScale;

    //    float multiplier = 1;
    //    if(parentScale.x >= parentScale.y)
    //    {
    //        multiplier = resolution / parentScale.x;
    //    }
    //    else
    //    {
    //        multiplier = resolution / parentScale.y;
    //    }
    //    dimensions *= multiplier;

    //    dimensions.x = (int)dimensions.x;
    //    dimensions.y = (int)dimensions.y;
    //    Debug.Log(dimensions);
    //    return dimensions;
    //}
}
