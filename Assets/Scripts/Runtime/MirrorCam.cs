using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorCam : MonoBehaviour
{
    private Camera mainCam;
    private Camera cam;
    private Vector3 mirrorNormal;

    RenderTexture renderTexture;
    Material renderMaterial;

    [SerializeField] string room;
    [SerializeField] float textureResolution;
    // Start is called before the first frame update
    void Start()
    {
        mainCam = Camera.main;
        cam = GetComponent<Camera>();
        mirrorNormal = transform.forward;

        Vector2 renderTextureDimension = SetRenderTextureDimension(textureResolution);
        renderTexture = new RenderTexture((int)renderTextureDimension.x, (int)renderTextureDimension.y,0);
        cam.targetTexture = renderTexture;

        Material renderMaterial = new Material(Shader.Find("HDRP/Lit"));
        renderMaterial.mainTexture = renderTexture;

        Renderer parentRenderer = transform.parent.GetComponent<Renderer>();
        parentRenderer.material = renderMaterial;
    }

    // Update is called once per frame
    void Update()
    {
        if(room == WorldProgress.instance.locationName)
        {
            Debug.Log("working");
            Vector3 mainCamDir = (mainCam.transform.position - transform.position).normalized;

            Vector3 dir = new Vector3(mainCamDir.x * mirrorNormal.x, mainCamDir.y * mirrorNormal.y, mainCamDir.z * mirrorNormal.z);
            transform.rotation = Quaternion.LookRotation(dir);
        }
    }

    private Vector2 SetRenderTextureDimension(float resolution)
    {
        Vector3 parentScale = transform.parent.localScale;
        Vector2 dimensions = (Vector2)parentScale;

        float multiplier = 1;
        if(parentScale.x >= parentScale.y)
        {
            multiplier = resolution / parentScale.x;
        }
        else
        {
            multiplier = resolution / parentScale.y;
        }
        dimensions *= multiplier;

        dimensions.x = (int)dimensions.x;
        dimensions.y = (int)dimensions.y;
        Debug.Log(dimensions);
        return dimensions;
    }
}
