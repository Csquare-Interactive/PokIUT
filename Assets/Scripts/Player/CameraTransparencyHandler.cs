using UnityEngine;
using System.Collections.Generic;

public class CameraTransparencyHandler
{
    private Transform cameraTransform;
    private Transform playerTransform;
    private LayerMask transparencyLayer;
    private LayerMask invisibleLayer;

    private List<Renderer> transparentRenderers = new List<Renderer>();
    private List<GameObject> invisibleObjects = new List<GameObject>();
    private float transparentAlpha = 0.3f;
    private float fadeSpeed = 5f;

    private Shader originalShader;

    public CameraTransparencyHandler(Transform camera, Transform player, LayerMask layer, LayerMask layer2)
    {
        cameraTransform = camera;
        playerTransform = player;
        transparencyLayer = layer;
        invisibleLayer = layer2;
    }

    public void Update()
    {
        Vector3 direction = playerTransform.position - cameraTransform.position;
        Ray ray = new Ray(cameraTransform.position, direction);
        RaycastHit[] hits = Physics.RaycastAll(ray, direction.magnitude, transparencyLayer);
        RaycastHit[] hits_inv = Physics.RaycastAll(ray, direction.magnitude, invisibleLayer);

        List<Renderer> currentRenderers = new List<Renderer>();
        List<GameObject> currentObjects = new List<GameObject>();

        foreach (RaycastHit hit in hits)
        {
            Renderer renderer = hit.collider.GetComponent<Renderer>();
            if (renderer != null && !currentRenderers.Contains(renderer))
            {
                SetTransparency(renderer, transparentAlpha);
                currentRenderers.Add(renderer);
            }
        }
        foreach (RaycastHit hit in hits_inv)
        {

            GameObject obj = hit.collider.gameObject;

            if (obj != null && !currentObjects.Contains(obj))
            {
                Renderer renderer = obj.GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.enabled = false;
                }
                currentObjects.Add(obj);
            }
        }

        foreach (Renderer renderer in transparentRenderers)
        {
            if (!currentRenderers.Contains(renderer))
            {
                ResetTransparency(renderer);
            }
        }
        foreach (GameObject obj in invisibleObjects)
        {
            if (!currentObjects.Contains(obj))
            {
                Renderer renderer = obj.GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.enabled = true;
                }
            }
        }

        transparentRenderers = currentRenderers;
        invisibleObjects = currentObjects;

        DrawRayGizmos(ray, direction.magnitude, hits);
    }

    private void SetTransparency(Renderer renderer, float alpha, float globalOpacity = 5.0f)
    {
        foreach (Material mat in renderer.materials)
        {
            if (mat.shader.name != "Custom/TransparentShader")
            {
                originalShader = mat.shader; // Stocke le shader original
                mat.shader = Shader.Find("Custom/TransparentShader");
            }

            Color color = mat.color;
            color.a = alpha;
            mat.color = color;

            // Définit l'opacité globale
            if (mat.HasProperty("_Opacity"))
            {
                mat.SetFloat("_Opacity", globalOpacity);
            }
        }
    }


    private void ResetTransparency(Renderer renderer)
    {
        foreach (Material mat in renderer.materials)
        {
            if (originalShader != null)
            {
                mat.shader = originalShader; // Réapplique le shader original
            }
        }
    }

    private void DrawRayGizmos(Ray ray, float maxDistance, RaycastHit[] hits)
    {
        // Dessine une ligne principale pour le Raycast
        Debug.DrawLine(ray.origin, ray.origin + ray.direction * maxDistance, Color.green);

        // Dessine une ligne rouge pour chaque impact
        foreach (RaycastHit hit in hits)
        {
            Debug.DrawLine(ray.origin, hit.point, Color.red);
        }
    }
}
