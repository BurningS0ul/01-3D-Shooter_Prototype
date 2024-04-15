using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultState : ActionBaseState
{
    public float scrollDirection;
    public override void EnterState(ActionStateManager actions)
    {
        actions.rHandAim.weight = 1;
        actions.lHandIK.weight = 1;
    }
    public override void UpdateState(ActionStateManager actions)
    {
        actions.rHandAim.weight = Mathf.Lerp(actions.rHandAim.weight, 1, Time.deltaTime * 10);
        if (actions.lHandIK.weight == 0)
        {
            actions.lHandIK.weight = 1;
        }

        if (Input.GetKeyDown(KeyCode.R) && CanReload(actions))
        {
            Debug.Log("Reloading");
            actions.SwitchState(actions.Reload);
        }
        else if (Input.mouseScrollDelta.y != 0)
        {
            scrollDirection = Input.mouseScrollDelta.y;
            actions.SwitchState(actions.Swap);
        }
    }

    bool CanReload(ActionStateManager action)
    {
        if (action.ammo.currentAmmo == action.ammo.clipSize)
        {
            Debug.Log("Clip is full");
            return false;
        }
        else if (action.ammo.extraAmmo == 0)
        {
            Debug.Log("No extra ammo");
            return false;
        }
        else
        {
            Debug.Log("Can reload");
            return true;
        }

    }
}
