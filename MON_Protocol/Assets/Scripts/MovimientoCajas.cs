using System.Collections;
using UnityEngine;

public class MovimientoCajas : MonoBehaviour
{
    public float gridUnit = 1.0f;
    public float moveSpeed = 5.0f;
    public LayerMask layerBloqueo; // Capa para muros y cajas
    private bool isBusy = false;
    
    private GameObject cajaObjetivo;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            EncenderAura();
        }

        // 2. APAGAR AURA: Al soltar espacio
        if (Input.GetKeyUp(KeyCode.Space))
        {
            ApagarAura();
        }
        
        if (isBusy) return;

        // Detectar entrada de dirección para saber hacia dónde miramos
        Vector3 moveDir = GetInputDirection();

        // ACCIÓN: Presionar Espacio para empujar
        if (Input.GetKey(KeyCode.Space) && moveDir != Vector3.zero)
        {
            TryPushBox(moveDir);
        }
    }
    
    private void EncenderAura()
    {
        // Miramos si hay una caja frente al conejo (usamos transform.forward)
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, gridUnit))
        {
            if (hit.collider.CompareTag("Caja"))
            {
                cajaObjetivo = hit.collider.gameObject;
                SetAura(cajaObjetivo, true);
            }
        }
    }

    private void ApagarAura()
    {
        if (cajaObjetivo != null)
        {
            SetAura(cajaObjetivo, false);
            cajaObjetivo = null;
        }
    }

    private void SetAura(GameObject caja, bool estado)
    {
        Transform aura = caja.transform.Find("AuraEfecto");
        if (aura != null) aura.gameObject.SetActive(estado);
    }

    private Vector3 GetInputDirection()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");
        if (x != 0) z = 0; // Bloqueo diagonal

        return new Vector3(x, 0, z);
    }

    private void TryPushBox(Vector3 direction)
    {
        // Raycast para ver si hay una caja adelante
        if (Physics.Raycast(transform.position, direction, out RaycastHit hit, gridUnit))
        {
            if (hit.collider.CompareTag("Caja"))
            {
                GameObject caja = hit.collider.gameObject;
                Vector3 targetCaja = caja.transform.position + (direction * gridUnit);

                // Raycast secundario: ¿Hay espacio detrás de la caja?
                if (!Physics.Raycast(caja.transform.position, direction, gridUnit))
                {
                    StartCoroutine(PushRoutine(caja.transform, targetCaja));
                }
                else
                {
                    Debug.Log("Protocolo Bloqueado: Espacio obstruido tras el objetivo.");
                }
            }
        }
    }

    private IEnumerator PushRoutine(Transform objeto, Vector3 destino)
    {
        isBusy = true;
        
        while (Vector3.Distance(objeto.position, destino) > 0.01f)
        {
            objeto.position = Vector3.MoveTowards(objeto.position, destino, moveSpeed * Time.deltaTime);
            yield return null;
        }
        objeto.position = new Vector3(Mathf.RoundToInt(destino.x), destino.y, Mathf.RoundToInt(destino.z));

        isBusy = false;
    }
}