using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable]
public class HumanBone
{
    public HumanBodyBones bone;
    public float weight = 1.0f;
}

public class WeaponIK : MonoBehaviour
{
    public Transform targetTransform;
    public Transform aimTransform;
    public int iterations = 10;

    [Range(0, 1)]
    public float weight = 1.0f;

    public float angleLimit = 90.0f;
    public float distanceLimit = 1.5f;

    public HumanBone[] humanBones;
    Transform[] boneTransforms;
    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
        boneTransforms = new Transform[humanBones.Length];
        for (int i = 0; i < boneTransforms.Length; i++)
        {
            boneTransforms[i] = anim.GetBoneTransform(humanBones[i].bone);
        }
    }

    Vector3 GetTargetPosition()
    {
        Vector3 targetDir = targetTransform.position - aimTransform.position;
        Vector3 aimDir = aimTransform.forward;
        float blendOut = 0.0f;

        float targetAngle = Vector3.Angle(targetDir, aimDir);
        if (targetAngle > angleLimit)
        {
            blendOut += (targetAngle - angleLimit) / 50.0f;
        }

        float targetDistance = targetDir.magnitude;
        if (targetDistance < distanceLimit)
        {
            blendOut += distanceLimit - targetDistance;
        }

        Vector3 direction = Vector3.Slerp(targetDir, aimDir, blendOut);
        return aimTransform.position + direction;
    }

    void LateUpdate()
    {
        if (aimTransform == null)
        {
            return;
        }
        if(targetTransform == null){
            return;
        }
        Vector3 targetPos = GetTargetPosition();
        for (int i = 0; i < iterations; i++)
        {
            for (int j = 0; j < boneTransforms.Length; j++)
            {
                Transform bone = boneTransforms[j];
                float boneWeight = humanBones[j].weight * weight;
                AimAtTarget(bone, targetPos, boneWeight);
            }
        }
    }

    private void AimAtTarget(Transform bone, Vector3 targetPos, float weight)
    {
        Vector3 aimDir = aimTransform.forward;
        Vector3 targetDir = targetPos - aimTransform.position;
        Quaternion aimTowards = Quaternion.FromToRotation(aimDir, targetDir);
        Quaternion blendedRotation = Quaternion.Slerp(Quaternion.identity, aimTowards, weight);
        bone.rotation = blendedRotation * bone.rotation;
    }

    public void SetTargetTransform(Transform target)
    {
        targetTransform = target;
    }

    public void SetAimTransform(Transform aim)
    {
        aimTransform = aim;
    }
}
