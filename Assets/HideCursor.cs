using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideCursor : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Hide the cursor
        Cursor.visible = false;
        DontDestroyOnLoad(gameObject);

        // Lock the cursor to the center of the screen
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Optionally, you can toggle cursor visibility with a key press
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Toggle cursor visibility
            Cursor.visible = !Cursor.visible;

            // Toggle cursor lock state
            if (Cursor.lockState == CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }
}
