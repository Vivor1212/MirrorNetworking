using UnityEngine;
using Mirror;

public class PlayerMove : NetworkBehaviour
{
    // [SyncVar] automatically sends this value from Server to all Clients
    // The "hook" runs a function on every client when the value changes
    [SyncVar(hook = nameof(SetColor))]
    public Color playerColor = Color.white;

    // Update is called once per frame
    void Update()
    {
        // Authority Check: Only move the square if it belongs to THIS player
        if (!isLocalPlayer) return;

        float moveX = Input.GetAxis("Horizontal") * Time.deltaTime * 5f;
        float moveZ = Input.GetAxis("Vertical") * Time.deltaTime * 5f;
        transform.Translate(moveX, 0, moveZ);

        // Remote Action (Command): If I press Space, tell the Server to change my color
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CmdRequestColorChange();
        }
    }

    // [Command] runs on the Server, even though it's called by the Client
    [Command]
    void CmdRequestColorChange()
    {
        playerColor = new Color(Random.value, Random.value, Random.value);
    }

    // This runs on all clients to update the visual
    void SetColor(Color oldColor, Color newColor)
    {
        GetComponent<Renderer>().material.color = newColor;
    }
}
