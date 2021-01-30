using UnityEngine;

public class FootIK : MonoBehaviour
{
    public Transform[] FootTarget;
    public Transform Step;
    public Transform Attraction;

    public void LateUpdate()
    {
        //move step & attraction
        Step.Translate(Vector3.forward * Time.deltaTime * 0.7f);
        if (Step.position.z > 1f)
            Step.position = Step.position + Vector3.forward * -2f;
        Attraction.Translate(Vector3.forward * Time.deltaTime * 0.5f);
        if (Attraction.position.z > 1f)
            Attraction.position = Attraction.position + Vector3.forward * -2f;

        //footsteps
        for (int i = 0; i < FootTarget.Length; i++)
        {
            var foot = FootTarget[i];
            var ray = new Ray(foot.transform.position + Vector3.up * 0.5f, Vector3.down);
            var hitInfo = new RaycastHit();
            if (Physics.SphereCast(ray, 0.05f, out hitInfo, 0.50f))
                foot.position = hitInfo.point + Vector3.up * 0.05f;
        }
    }
}
