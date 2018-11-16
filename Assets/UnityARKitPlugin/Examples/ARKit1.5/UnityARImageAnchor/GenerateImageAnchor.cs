using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.iOS;

public class GenerateImageAnchor : MonoBehaviour {


	[SerializeField]
	private ARReferenceImage referenceImage;

	[SerializeField]
	private GameObject prefabToGenerate;

	private GameObject imageAnchorGO;

	public UnityEngine.UI.Text BtnTextRotation;
	public UnityEngine.UI.Text BtnTextPosition;

	// Use this for initialization
	void Start () {
		UnityARSessionNativeInterface.ARImageAnchorAddedEvent += AddImageAnchor;
		UnityARSessionNativeInterface.ARImageAnchorUpdatedEvent += UpdateImageAnchor;
		UnityARSessionNativeInterface.ARImageAnchorRemovedEvent += RemoveImageAnchor;

	}

	void AddImageAnchor(ARImageAnchor arImageAnchor)
	{
		Debug.Log ("image anchor added");
		if (arImageAnchor.referenceImageName == referenceImage.imageName) {
			Vector3 position = UnityARMatrixOps.GetPosition (arImageAnchor.transform);
			Quaternion rotation = UnityARMatrixOps.GetRotation (arImageAnchor.transform);

			imageAnchorGO = Instantiate<GameObject> (prefabToGenerate, position, rotation);
		}
	}

	void UpdateImageAnchor(ARImageAnchor arImageAnchor)
	{
		Debug.Log ("image anchor updated");
		if (arImageAnchor.referenceImageName == referenceImage.imageName) 
		{
			if (isUpdatedPosition) 
			{
				imageAnchorGO.transform.position = UnityARMatrixOps.GetPosition (arImageAnchor.transform);
			}
			if (isUpdatedRotation) 
			{
				imageAnchorGO.transform.rotation = UnityARMatrixOps.GetRotation (arImageAnchor.transform);
			}
		}

	}

	void RemoveImageAnchor(ARImageAnchor arImageAnchor)
	{
		Debug.Log ("image anchor removed");
		if (imageAnchorGO) {
			GameObject.Destroy (imageAnchorGO);
		}

	}

	void OnDestroy()
	{
		UnityARSessionNativeInterface.ARImageAnchorAddedEvent -= AddImageAnchor;
		UnityARSessionNativeInterface.ARImageAnchorUpdatedEvent -= UpdateImageAnchor;
		UnityARSessionNativeInterface.ARImageAnchorRemovedEvent -= RemoveImageAnchor;

	}

	private bool isUpdatedRotation = true;

	public void DontUpdateARImageAnchorRotation()
	{
		isUpdatedRotation = !isUpdatedRotation;	
		string s = isUpdatedRotation ? "Disable Rotation" : "Enable Rotation";
		BtnTextRotation.text = s;
	}

	private bool isUpdatedPosition = true;

	public void DontUpdateARImageAnchorPosition()
	{
		isUpdatedPosition = !isUpdatedPosition;	
		string s = isUpdatedPosition ? "Disable Position" : "Enable Position";
		BtnTextPosition.text = s;
	}

}
