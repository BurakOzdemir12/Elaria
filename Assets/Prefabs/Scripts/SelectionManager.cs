using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionManager : MonoBehaviour
{
    public static SelectionManager instance { get; set; }

    public bool onTarget;
    public GameObject selectedObject;
    public GameObject interaction_Info_UI;
    Text interaction_text;

    private void Start()
    {
        onTarget = false;
        interaction_text = interaction_Info_UI.GetComponent<Text>();
    }
    private void Awake()
    {
        if (instance != null && instance!= this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this; 
        }

    }
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            var selectionTransform = hit.transform;

            InteractableObject Interactable = selectionTransform.GetComponent<InteractableObject>();

            if (Interactable && Interactable.playerInRange)
            {
                onTarget = true;
                selectedObject = Interactable.gameObject;
                interaction_text.text = Interactable.GetItemName();
                interaction_Info_UI.SetActive(true);
            }
            else
            {
                onTarget= false;
                interaction_Info_UI.SetActive(false);
            }

        }
        else
        {
            onTarget = false;
            interaction_Info_UI.SetActive(false); //for sky
        }
    }
}
//My Own Format
/*using UnityEngine;
using UnityEngine.UI;

public class SelectionManager : MonoBehaviour
{
    public GameObject interaction_Info_UI;
    Text interaction_text;

    // Define the maximum interaction distance
    [SerializeField]public float maxInteractionDistance = 1f;

    private void Start()
    {
        interaction_text = interaction_Info_UI.GetComponent<Text>();
    }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            var selectionTransform = hit.transform;

            // Calculate the distance between the camera and the hit point
            float distanceToHit = (hit.point - Camera.main.transform.position).magnitude;

            if (distanceToHit <= maxInteractionDistance && selectionTransform.GetComponent<InteractableObject>())
            {
                interaction_text.text = selectionTransform.GetComponent<InteractableObject>().GetItemName();
                interaction_Info_UI.SetActive(true);
            }
            else
            {
                interaction_Info_UI.SetActive(false);
            }
        }
        else
        {
            interaction_Info_UI.SetActive(false);
        }
    }
}
*/