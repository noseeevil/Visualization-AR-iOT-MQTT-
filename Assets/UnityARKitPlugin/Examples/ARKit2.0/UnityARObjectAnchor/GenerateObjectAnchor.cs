using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.iOS;
using UnityEngine.UI;

public class GenerateObjectAnchor : MonoBehaviour 
{

	[SerializeField]
	private ARReferenceObjectAsset referenceObjectAsset;

	private GameObject objectAnchorGo;

    public GameObject AxesPrefab;


	// Use this for initialization
	void Start () 
	{
		UnityARSessionNativeInterface.ARObjectAnchorAddedEvent += AddObjectAnchor;
		UnityARSessionNativeInterface.ARObjectAnchorUpdatedEvent += UpdateObjectAnchor;
	}

	void AddObjectAnchor(ARObjectAnchor arObjectAnchor)
	{

		if (arObjectAnchor.referenceObjectName == referenceObjectAsset.objectName) 
		{
            Debug.Log("***AddObjectAnchor");
            Instantiate(AxesPrefab, UnityARMatrixOps.GetPosition(arObjectAnchor.transform), UnityARMatrixOps.GetRotation(arObjectAnchor.transform));
            FindObjectOfType<mqttTest>().Connect();
		}
	}

	void UpdateObjectAnchor(ARObjectAnchor arObjectAnchor)
	{

		if (arObjectAnchor.referenceObjectName == referenceObjectAsset.objectName) 
		{
            Debug.Log("***UpdateObjectAnchor");
        }

	}

	void OnDestroy()
	{
		UnityARSessionNativeInterface.ARObjectAnchorAddedEvent -= AddObjectAnchor;
		UnityARSessionNativeInterface.ARObjectAnchorUpdatedEvent -= UpdateObjectAnchor;
	}
}
