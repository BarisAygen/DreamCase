using System.Collections;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    public static ParticleManager Instance;

    [SerializeField] private GameObject cubeParticlePrefab;
    [SerializeField] private GameObject rocketCreationParticlePrefab;

    [Header("Cube Materials")]
    [SerializeField] private Material redMaterial;
    [SerializeField] private Material greenMaterial;
    [SerializeField] private Material blueMaterial;
    [SerializeField] private Material yellowMaterial;
    [Header("Obstacle Materials")]
    [SerializeField] private Material stoneMaterial;
    [SerializeField] private Material boxMaterial;
    [SerializeField] private Material vaseMaterial;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        ObjectPoolManager.Instance.WarmPool("cubeParticle", cubeParticlePrefab, 20);
        ObjectPoolManager.Instance.WarmPool("rocketCreationParticle", rocketCreationParticlePrefab, 10);
    }

    public void PlayCubeParticle(Vector3 pos, Material mat)
    {
        StartCoroutine(SpawnCubeParticle(pos, mat));
    }
    
    public void PlayRocketCreationParticleFollow(GameObject target)
    {
        StartCoroutine(SpawnRocketParticleAttached(target));
    }

    private IEnumerator SpawnRocketParticleAttached(GameObject target)
    {
        GameObject go = ObjectPoolManager.Instance.Get("rocketCreationParticle");
        if (go == null || target == null) yield break;

        // Efekti rocket'ın altına yerleştir
        go.transform.SetParent(target.transform);
        go.transform.localPosition = Vector3.zero;

        var ps = go.GetComponent<ParticleSystem>();
        ps.Play();

        yield return new WaitForSeconds(ps.main.duration + ps.main.startLifetime.constantMax);

        // Efekti serbest bırak ve havuza geri dön
        go.transform.SetParent(null);
        ObjectPoolManager.Instance.Return("rocketCreationParticle", go);
    }

    public Material GetMaterialByKey(string key)
    {
        return key switch
        {
            "r" => redMaterial,
            "g" => greenMaterial,
            "b" => blueMaterial,
            "y" => yellowMaterial,
            "s" => stoneMaterial,
            "bo" => boxMaterial,
            "v" => vaseMaterial,

            _ => null
        };
    }

    private IEnumerator SpawnCubeParticle(Vector3 pos, Material mat)
    {
        GameObject go = ObjectPoolManager.Instance.Get("cubeParticle");
        if (go == null) yield break;

        go.transform.position = pos;

        var ps = go.GetComponent<ParticleSystem>();
        var renderer = ps.GetComponent<ParticleSystemRenderer>();
        renderer.material = mat;

        ps.Play();

        yield return new WaitForSeconds(ps.main.duration + ps.main.startLifetime.constantMax);

        ObjectPoolManager.Instance.Return("cubeParticle", go);
    }

    private IEnumerator SpawnRocketCreationParticle(Vector3 pos)
    {
        GameObject go = ObjectPoolManager.Instance.Get("rocketCreationParticle");
        if (go == null) yield break;

        go.transform.position = pos;

        var ps = go.GetComponent<ParticleSystem>();

        ps.Play();

        yield return new WaitForSeconds(ps.main.duration + ps.main.startLifetime.constantMax);

        ObjectPoolManager.Instance.Return("rocketCreationParticle", go);
    }
    
    public void PlayObstacleDamageParticle(Vector3 pos, string key)
    {
        var mat = GetMaterialByKey(key);
        if (mat != null)
        {
            StartCoroutine(SpawnCubeParticle(pos, mat));
        }
    }
}