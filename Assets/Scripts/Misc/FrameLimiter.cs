using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;


public class FrameLimiter : MonoBehaviour
{
    public Text fpsLimiter;

    private void Update()
    {
        Application.targetFrameRate = int.Parse(fpsLimiter.text);
        QualitySettings.maxQueuedFrames = 0;
        QualitySettings.vSyncCount = 0;
    }
}
public class FrameDataGroupSimulation : ComponentSystemGroup
{

}

/*class FixedRateSystemGroup : ComponentSystemGroup
{
    protected override void OnCreate()
    {
        base.OnCreate();

        // Causes systems to still be updated every frame but does affect the ElapsedTime/DeltaTime that gets reported
        // to that system
        //FixedRateUtils.EnableFixedRateSimple(this, 0.1f);

        // Locks up the editor...
        //FixedRateUtils.EnableFixedRateWithCatchUp(this, 3f);
    }
}*/