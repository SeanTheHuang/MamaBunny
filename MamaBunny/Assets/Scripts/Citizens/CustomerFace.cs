using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerFace : MonoBehaviour {

    public enum FACEEMOTION
    {
        HAPPY = 0,
        SAD,
        SURPRISED,
        ANGRY,
        NORMAL
    }

    public SkinnedMeshRenderer FaceRender;
    public Material matHappy, matSad, matSurprised, matAngry;
    Material matNormal;

    private void Start()
    {
        matNormal = FaceRender.material;
    }

    public void SetFaceMaterial(FACEEMOTION _fe)
    {
        switch (_fe)
        {
            case FACEEMOTION.ANGRY:
                FaceRender.material = matAngry;
                break;
            case FACEEMOTION.HAPPY:
                FaceRender.material = matHappy;
                break;
            case FACEEMOTION.NORMAL:
                FaceRender.material = matNormal;
                break;
            case FACEEMOTION.SAD:
                FaceRender.material = matSad;
                break;
            case FACEEMOTION.SURPRISED:
                FaceRender.material = matSurprised;
                break;
            default:
                FaceRender.material = matNormal;
                break;
        }
    }
}
