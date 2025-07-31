using System.Collections;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    public static ParticleManager Instance;

    [SerializeField] private GameObject particlePrefab;

    [Header("Materials")]
    [SerializeField] private Material redMaterial;
    [SerializeField] private Material greenMaterial;
    [SerializeField] private Material blueMaterial;
    [SerializeField] private Material yellowMaterial;

    private void Start()
    {
        Instance = this;
        ObjectPoolManager.Instance.WarmPool("particle", particlePrefab, 20);
    }

    private void OnEnable()
    {
        GameEventManager.OnItemDestroyed += OnItemDestroyed;
    }

    private void OnDisable()
    {
        GameEventManager.OnItemDestroyed -= OnItemDestroyed;
    }

    private void OnItemDestroyed(Item item)
    {
        Material mat = GetMaterialByKey(item.Key);
        if (mat == null) return;

        StartCoroutine(SpawnParticle(item.transform.position, mat));
    }

    private Material GetMaterialByKey(string key)
    {
        return key switch
        {
            "r" => redMaterial,
            "g" => greenMaterial,
            "b" => blueMaterial,
            "y" => yellowMaterial,
            _ => null
        };
    }

    private IEnumerator SpawnParticle(Vector3 pos, Material mat)
    {
        GameObject go = ObjectPoolManager.Instance.Get("particle");
        if (go == null) yield break;

        go.transform.position = pos;

        var ps = go.GetComponent<ParticleSystem>();
        var renderer = ps.GetComponent<ParticleSystemRenderer>();
        renderer.material = mat;

        ps.Play();

        yield return new WaitForSeconds(ps.main.duration + ps.main.startLifetime.constantMax);

        ObjectPoolManager.Instance.Return("particle", go);
    }
}