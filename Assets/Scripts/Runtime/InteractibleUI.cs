using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class InteractibleUI : MonoBehaviour
{
    [SerializeField] private bool debugMode;
    [SerializeField] private GameObject debugObject;

    [Space(10)]
    [SerializeField] private float interactiveRange;
    [SerializeField] private float disappearTimer;
    [SerializeField] private LayerMask notInteractibleLayerMask;

    [SerializeField] private Canvas canvas;
    [SerializeField] private Sprite doorSprite;
    [SerializeField] private Sprite itemSprite;

    private Transform player;
    private Camera cam;

    private List<InteractibleImage> currentInteractibleImages = new List<InteractibleImage>();

    private void Start()
    {
        player = PlayerHelper.instance.transform;
        cam = Camera.main;
    }

    private void Update()
    {
        Collider[] colliders = Physics.OverlapSphere(player.position, interactiveRange, ~notInteractibleLayerMask);

        for(int i = 0; i < currentInteractibleImages.Count; i++)
        {
            if(currentInteractibleImages[i].gameObject == null) { DestroyInteractibleImage(currentInteractibleImages[i]); i--; continue; }
            if (!isInteractibleImageInColliders(currentInteractibleImages[i], colliders))
            {
                if (!currentInteractibleImages[i].autoDestructing)
                {
                    //Debug.Log("destroy");
                    StartCoroutine(DestroyInteractibleImageCoroutine(currentInteractibleImages[i], disappearTimer));
                    currentInteractibleImages[i].autoDestructing = true;
                }
            }
            else currentInteractibleImages[i].autoDestructing = false;

            Vector2 wantedPos = cam.WorldToScreenPoint(currentInteractibleImages[i].gameObject.transform.position) * 2f;
            if (currentInteractibleImages[i].rectTransform.anchoredPosition != wantedPos) currentInteractibleImages[i].rectTransform.anchoredPosition = wantedPos;
            
        }

        foreach(Collider collider in colliders)
        {
            if (!isInteractibleInCurrentInteractible(collider.gameObject))
            {
                if(collider.GetComponent<DoorScript>() != null)
                {
                    CreateImageForInteraction(collider.transform, doorSprite);
                }
                else if(collider.GetComponent<Collectible>() != null)
                {
                    CreateImageForInteraction(collider.transform, itemSprite);
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (!debugMode) return;
        if (debugObject == null) { Debug.LogWarning("Custom Warning: Please define 'debugObject'."); return; }
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(debugObject.transform.position, interactiveRange);
    }

    public float size;

    private void CreateImageForInteraction(Transform interactibleObject,Sprite sprite)
    {
        //Debug.Log("create");

        // Get the wanted anchored pos of the sprite
        Vector3 pos = cam.WorldToScreenPoint(interactibleObject.position) * 2f;
        pos.z = 0;
        //Debug.Log(pos);

        // Create UI GameObject
        GameObject go = new GameObject("Image for interaction");
        // Set its parent and reset his rect transform
        go.transform.SetParent(canvas.transform);
        RectTransform rectTransform = go.AddComponent<RectTransform>();
        rectTransform.localScale = Vector3.one *size;
        rectTransform.localRotation = Quaternion.identity;
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.zero;

        // Set to the wanted anchored pos
        rectTransform.localPosition = pos;
        rectTransform.anchoredPosition = new Vector3(pos.x, pos.y, 0);

        // Set sprite
        Image image = go.AddComponent<Image>();
        image.sprite = sprite;
        currentInteractibleImages.Add(new InteractibleImage(interactibleObject.gameObject, image));
    }
    private IEnumerator DestroyInteractibleImageCoroutine(InteractibleImage interactibleImage, float seconds)
    {
        //Debug.Log("1");
        float timer = seconds;
        while(true)
        {
            yield return null;
            if (!interactibleImage.autoDestructing) break;
            timer -= Time.deltaTime;
            if(timer < 0)
            {
                //Debug.Log("2");
                DestroyInteractibleImage(interactibleImage);
                break;
            }
        }
    }
    private void DestroyInteractibleImage(InteractibleImage interactibleImage)
    {
        currentInteractibleImages.Remove(interactibleImage);
        if(interactibleImage.image != null) Destroy(interactibleImage.image.gameObject);
    }

    private bool isInteractibleInCurrentInteractible(GameObject interactibleObject)
    {
        foreach(InteractibleImage interactibleImage in currentInteractibleImages)
        {
            if(interactibleImage.gameObject == interactibleObject) return true;
        }
        return false;
    }
    private bool isInteractibleImageInColliders(InteractibleImage interactibleImage, Collider[] colliders)
    {
        foreach(Collider collider in colliders)
        {
            if (interactibleImage.gameObject == collider.gameObject) return true;
        }
        return false;
    }
}
[Serializable] public class InteractibleImage
{
    public GameObject gameObject;
    public Image image;
    public RectTransform rectTransform;

    public bool autoDestructing;

    public InteractibleImage(GameObject interactible, Image image)
    {
        this.gameObject = interactible;
        this.image = image;
        this.rectTransform = image.rectTransform;
        this.autoDestructing = false;
    }
}
