using UnityEngine;
using UnityEngine.UI;

public class MouseScript : MonoBehaviour
{
    GameObject cameraMain;


    float dragSpeed = 30f; // Velocidade de movimento ao arrastar
    float zoomSpeed = 5f; // Velocidade de zoom
    float minZoom = 5f;   // Zoom mínimo
    float maxZoom = 20f;  // Zoom máximo
    Vector3 maxMove = new Vector3(220f,170f,0);
    Vector3 minMove = new Vector3(-220f,-170f,0);

    private Vector3 dragOrigin;

    public Button centerButton;

    public void centerButtonAction(){
        cameraMain.transform.position = new Vector3(0,0,-200);
    }

    void Start()
    {
        cameraMain = Camera.main.gameObject;
        centerButton.onClick.AddListener(centerButtonAction);
    }

    void Update()
    {
        HandleDrag();
        HandleZoom();
    }

    // Função para lidar com o arrastar da câmera
    void HandleDrag()
    {
        if (Input.GetMouseButtonDown(0)) // Verifica se o botão esquerdo do mouse foi pressionado
        {
            dragOrigin = Input.mousePosition; // Salva a posição inicial do clique
        }

        if (Input.GetMouseButton(0)) // Verifica se o botão esquerdo do mouse está sendo mantido pressionado
        {
            Vector3 difference = dragOrigin - Input.mousePosition;
            dragOrigin = Input.mousePosition;

            float X = difference.x * dragSpeed * Time.deltaTime;
            float Y = difference.y * dragSpeed * Time.deltaTime;
            Vector3 move = new Vector3(X, Y , 0);

            Vector3 change = cameraMain.transform.position + move;

            if(change.x <= maxMove.x && change.x >= minMove.x && change.y <= maxMove.y && change.y >= minMove.y)
            cameraMain.transform.position += move; // Move a câmera na direção do movimento do mouse
        }
    }

    // Função para lidar com o zoom da câmera
    void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel"); // Verifica o input da rodinha do mouse
        if (scroll != 0.0f)
        {
            Camera.main.orthographicSize -= scroll * zoomSpeed;
            Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, minZoom, maxZoom); // Limita o zoom ao mínimo e máximo definidos
        }
    }
}
