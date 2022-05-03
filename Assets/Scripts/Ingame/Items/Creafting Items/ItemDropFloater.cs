using UnityEngine;

namespace Warborn.Ingame.Items.CraftingItems
{
    public class ItemDropFloater : MonoBehaviour
    {
        // User Inputs
        [SerializeField] private float degreesPerSecond = 15.0f;
        [SerializeField] private float amplitude = 0.5f;
        [SerializeField] private float frequency = 1f;

        // Position Storage Variables
        private Vector3 posOffset = new Vector3();
        private Vector3 tempPos = new Vector3();

        // Use this for initialization
        private void Start()
        {
            // Store the starting position & rotation of the object
            posOffset = transform.position;
        }

        // Update is called once per frame
        private void Update()
        {
            // Spin object around Y-Axis
            transform.Rotate(new Vector3(0f, Time.deltaTime * degreesPerSecond, 0f), Space.World);

            // Float up/down with a Sin()
            tempPos = posOffset;
            tempPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI * frequency) * amplitude;

            transform.position = tempPos;
        }
    }


}
