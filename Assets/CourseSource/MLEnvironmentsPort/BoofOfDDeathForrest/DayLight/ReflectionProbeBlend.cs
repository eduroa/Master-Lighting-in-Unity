using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
using System.Collections.Generic;

public class ReflectionProbeBlend : MonoBehaviour
{
    private List<HDAdditionalReflectionData> afternoonProbes = new();
    private List<HDAdditionalReflectionData> sunsetProbes = new();

    void Awake()
    {
        //var tags = FindObjectsOfType<ReflectionProbeTag>();
        var tags = FindObjectsByType<ReflectionProbeTag>(FindObjectsInactive.Include,FindObjectsSortMode.None);


        foreach (var tag in tags)
        {
            var probe = tag.GetComponent<HDAdditionalReflectionData>();
            if (probe == null) continue;

            if (tag.state == ReflectionProbeTag.ProbeState.Afternoon)
            {
             //   Debug.Log("Insert RP AFT");
                afternoonProbes.Add(probe);
            }
            else
            {
               // Debug.Log("Insert RP SUN");
                sunsetProbes.Add(probe);
            }
        }
    }

    public void SetBlend(float t)
    {
        foreach (var p in afternoonProbes)
        {            
            p.weight = 1f - t;
           // Debug.Log("AFT weight = " + p.weight.ToString() );
        }
            

        foreach (var p in sunsetProbes)
        {
            p.weight = t;
            //Debug.Log("SUN weight = " + p.weight.ToString());
        }
            
    }
}

