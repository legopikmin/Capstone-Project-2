//-JAM
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("(Player)/Camera Setup")]
public class CameraLayerSetup : MonoBehaviour
{
[SerializeField] private Camera cam;
	
	public void SetupCamera(string playerID)
	{
        cam.cullingMask |= 1 << LayerMask.NameToLayer(playerID);
    }
}
