using DigitalWorld.Game;
using UnityEngine;

namespace DigitalWorld.Behaviour
{
    public class InputBehaviour : MonoBehaviour
    {
        /// <summary>
        /// µ¥Î»¿ØÖÆÆ÷
        /// </summary>
        private ControlCharacter unit;

        private void Awake()
        {
            unit = this.GetComponent<ControlCharacter>();
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            
        }


    }

}

