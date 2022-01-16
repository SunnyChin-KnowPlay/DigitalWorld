using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TD
{
    public class GodBall : MonoBehaviour
    {
        public Transform neigbhour;
        private LineRenderer lineRenderer;

        private Vector3 startPos;
        private Vector3 prevPos;
        private Vector3 nextPos;

        private float runningTime = 0;
        private float deadTime = 0;
        public float range = 1;

        // Start is called before the first frame update
        void Start()
        {
            startPos = transform.position;
            prevPos = startPos;
            nextPos = startPos;

            lineRenderer = this.GetComponent<LineRenderer>();

            lineRenderer.startWidth = 0.1f;
            lineRenderer.endWidth = 0.1f;
        }

        // Update is called once per frame
        void Update()
        {
            runningTime += Time.deltaTime;
            if (runningTime >= deadTime)
            {
                this.ChangeDir();
            }
            else
            {
                float v = runningTime / deadTime;
                this.transform.position = Vector3.Slerp(prevPos, nextPos, v);
            }

            if (null != neigbhour)
            {
                lineRenderer.enabled = true;
                lineRenderer.SetPosition(0, this.transform.position);
                lineRenderer.SetPosition(1, neigbhour.position);
            }
            else
            {
                lineRenderer.enabled = false;
            }
        }

        private void ChangeDir()
        {
            prevPos = nextPos;
            runningTime = 0;
            deadTime = UnityEngine.Random.Range(2, 4);
            nextPos = startPos + new Vector3(Random.Range(-range, range), Random.Range(-range, range), Random.Range(-range, range));
        }

       
    }

}


