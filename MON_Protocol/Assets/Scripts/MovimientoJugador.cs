using System.Collections;
using UnityEngine;

public class MovimientoJugador : MonoBehaviour
{
    [Header("Configuración de Rejilla")]
    public float gridUnit = 1.0f;        
    public float moveSpeed = 5.0f;       
    public float jumpHeight = 0.5f;
    public LayerMask capasBloqueantes;
    
    private bool isMoving = false;      
    private Vector3 targetPosition;

    void Update()
    {
        if (!isMoving)
        {
            float x = Input.GetAxisRaw("Horizontal");
            float z = Input.GetAxisRaw("Vertical");

            if (x != 0) z = 0; 

            if (x != 0 || z != 0)
            {
                Vector3 direction = new Vector3(x, 0, z);
                TryMove(direction);
            }
        }
    }

    private void TryMove(Vector3 direction)
    {
        transform.forward = direction;
        
        targetPosition = transform.position + (direction * gridUnit);
        
        if (CanMove(direction))
        {
            StartCoroutine(MoveRoutine(targetPosition));
        }
    }

    private bool CanMove(Vector3 direction)
    {
        // Lanzar el Raycast un poco más arriba para no chocar con el suelo
        // 1. Lanzamos un rayo que detecta TODO (sin filtrar capas al principio)
        if (Physics.Raycast(transform.position + Vector3.up * 0.1f, direction, out RaycastHit hit, gridUnit))
        {
            // 2. ¿Es un objeto con el script LlavePick?
            if (hit.collider.TryGetComponent<LlavePick>(out LlavePick llave))
            {
                // Ejecutamos el Debug (o la función de recoger)
                llave.DetectarContacto(); 
                // Retornamos true para que el jugador pueda entrar en esa casilla
                return true; 
            }

            // 3. Si es un muro (está en la capa bloqueante), no podemos movernos
            // Comprobamos si la capa del objeto golpeado está en capasBloqueantes
            if (((1 << hit.collider.gameObject.layer) & capasBloqueantes) != 0)
            {
                return false;
            }
        }
        return true;
    }

    private IEnumerator MoveRoutine(Vector3 target)
    {
        isMoving = true;

        Vector3 startPosition = transform.position;
        float elapsedTime = 0;
        
        float distance = Vector3.Distance(startPosition, target);
        float duration = distance / moveSpeed;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float percent = elapsedTime / duration;
            
            Vector3 currentPos = Vector3.Lerp(startPosition, target, percent);

            // Movimiento vertical (Arco usando una función de seno)
            // Mathf.Sin nos da un valor de 0 a 1 y vuelve a 0 en el rango de 0 a PI
            currentPos.y += Mathf.Sin(percent * Mathf.PI) * jumpHeight;

            transform.position = currentPos;
            yield return null;
        }

        // Ajuste final para precisión perfecta
        transform.position = target;
        isMoving = false;
    }
}