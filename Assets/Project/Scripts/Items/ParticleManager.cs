using System.Collections;
using DG.Tweening;
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

    [Header("Obstacle Damage Particles")]
    [SerializeField] private GameObject stoneParticlePrefab;
    [SerializeField] private GameObject boxParticlePrefab;
    [SerializeField] private GameObject vaseParticlePrefab;

    [Header("Rocket Shard")]
    [SerializeField] private GameObject rocketShardPrefab;

    [Header("Rocket Trial Effects")]
    [SerializeField] private GameObject rocketTrialEffectPrefab;
    [SerializeField] private GameObject rocketTrialSmokePrefab;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        var pool = ObjectPoolManager.Instance;
        pool.WarmPool("cubeParticle", cubeParticlePrefab, 20);
        pool.WarmPool("rocketCreationParticle", rocketCreationParticlePrefab, 10);
        pool.WarmPool("stoneParticle", stoneParticlePrefab, 5);
        pool.WarmPool("boxParticle", boxParticlePrefab, 5);
        pool.WarmPool("vaseParticle", vaseParticlePrefab, 5);
        pool.WarmPool("rocketShard", rocketShardPrefab, 20);
        pool.WarmPool("rocketTrialEffect", rocketTrialEffectPrefab, 10);
        pool.WarmPool("rocketTrialSmoke", rocketTrialSmokePrefab, 10);
    }

    public void PlayCubeParticle(Vector3 pos, Material mat)
    {
        StartCoroutine(SpawnCubeParticle(pos, mat));
    }

    public void PlayRocketCreationParticleFollow(GameObject target)
    {
        StartCoroutine(SpawnRocketParticleAttached(target));
    }

    /// <summary>
    /// Spawns both the trial effect and the trial smoke on the rocket at once.
    /// </summary>
    public void PlayRocketTrialEffectsAttached(GameObject rocket)
    {
        StartCoroutine(SpawnRocketTrialEffect(rocket));
        StartCoroutine(SpawnRocketTrialSmoke(rocket));
    }

    private IEnumerator SpawnCubeParticle(Vector3 pos, Material mat)
    {
        var go = ObjectPoolManager.Instance.Get("cubeParticle");
        if (go == null) yield break;

        go.transform.position = pos;
        var ps = go.GetComponent<ParticleSystem>();
        var renderer = ps.GetComponent<ParticleSystemRenderer>();
        renderer.material = mat;
        ps.Play();

        yield return new WaitForSeconds(ps.main.duration + ps.main.startLifetime.constantMax);
        ObjectPoolManager.Instance.Return("cubeParticle", go);
    }

    private IEnumerator SpawnRocketParticleAttached(GameObject target)
    {
        var go = ObjectPoolManager.Instance.Get("rocketCreationParticle");
        if (go == null || target == null) yield break;

        go.transform.SetParent(target.transform);
        go.transform.localPosition = Vector3.zero;

        var ps = go.GetComponent<ParticleSystem>();
        ps.Play();

        yield return new WaitForSeconds(ps.main.duration + ps.main.startLifetime.constantMax);
        go.transform.SetParent(null);
        ObjectPoolManager.Instance.Return("rocketCreationParticle", go);
    }

    private IEnumerator SpawnRocketTrialEffect(GameObject rocket)
    {
        var go = ObjectPoolManager.Instance.Get("rocketTrialEffect");
        if (go == null || rocket == null) yield break;

        go.transform.SetParent(rocket.transform);
        go.transform.localPosition = Vector3.zero;

        var ps = go.GetComponent<ParticleSystem>();
        ps.Play();

        yield return new WaitForSeconds(ps.main.duration + ps.main.startLifetime.constantMax);
        go.transform.SetParent(null);
        ObjectPoolManager.Instance.Return("rocketTrialEffect", go);
    }

    private IEnumerator SpawnRocketTrialSmoke(GameObject rocket)
    {
        var go = ObjectPoolManager.Instance.Get("rocketTrialSmoke");
        if (go == null || rocket == null) yield break;

        go.transform.SetParent(rocket.transform);
        go.transform.localPosition = Vector3.zero;

        var ps = go.GetComponent<ParticleSystem>();
        ps.Play();

        yield return new WaitForSeconds(ps.main.duration + ps.main.startLifetime.constantMax);
        go.transform.SetParent(null);
        ObjectPoolManager.Instance.Return("rocketTrialSmoke", go);
    }

    public Material GetMaterialByKey(string key)
    {
        return key switch
        {
            "r" => redMaterial,
            "g" => greenMaterial,
            "b" => blueMaterial,
            "y" => yellowMaterial,
            _   => null
        };
    }

    public void PlayObstacleDamageParticle(Vector3 pos, string key)
    {
        string poolKey = key switch
        {
            "s"  => "stoneParticle",
            "bo" => "boxParticle",
            "v"  => "vaseParticle",
            _    => null
        };

        if (poolKey != null)
            StartCoroutine(SpawnObstacleParticle(pos, poolKey));
    }

    private IEnumerator SpawnObstacleParticle(Vector3 pos, string poolKey)
    {
        var go = ObjectPoolManager.Instance.Get(poolKey);
        if (go == null) yield break;

        go.transform.position = pos;
        var ps = go.GetComponent<ParticleSystem>();
        ps.Play();

        yield return new WaitForSeconds(ps.main.duration + ps.main.startLifetime.constantMax);
        ObjectPoolManager.Instance.Return(poolKey, go);
    }
    
    public void SpawnRocketShardWithEffects(Vector3 startPos, Vector2Int direction, int travelCount)
    {
        var shard = ObjectPoolManager.Instance.Get("rocketShard");
        if (shard == null) return;

        shard.transform.position = startPos;
        shard.transform.rotation = Quaternion.Euler(0, 0, DirectionToZ(direction));
        shard.SetActive(true);

        // 🎯 Efektleri instantiate edip shard altına bağlıyoruz
        AttachParticleEffectToShard(shard, "rocketTrialEffect", direction);
        AttachParticleEffectToShard(shard, "rocketTrialSmoke", direction);
        float step = GridManager.Instance.CellSize;
        Vector3 endPos = startPos + (Vector3)((Vector2)direction * travelCount * step);
        float speed = 18f;

        shard.transform
            .DOMove(endPos, speed)
            .SetSpeedBased()
            .SetEase(Ease.Linear)
            .OnComplete(() => ObjectPoolManager.Instance.Return("rocketShard", shard));
    }
    private void AttachParticleEffectToShard(GameObject shard, string poolKey, Vector2Int direction)
    {
        var go = ObjectPoolManager.Instance.Get(poolKey);
        if (go == null) return;

        go.transform.SetParent(shard.transform);
        go.transform.localPosition = new Vector3(0f, -0.4f, 0f); 

        // 🔄 Particle'ın da yönünü değiştiriyoruz
        go.transform.localRotation = Quaternion.Euler(0, 0, DirectionToZ(direction));

        go.SetActive(true);

        var ps = go.GetComponent<ParticleSystem>();
        ps.Play();

        StartCoroutine(DisableAfterParticle(ps, poolKey, go));
    }

    private IEnumerator DisableAfterParticle(ParticleSystem ps, string poolKey, GameObject go)
    {
        yield return new WaitForSeconds(ps.main.duration + ps.main.startLifetime.constantMax);
        go.transform.SetParent(null);
        ObjectPoolManager.Instance.Return(poolKey, go);
    }

    private float DirectionToZ(Vector2Int dir)
    {
        if (dir == Vector2Int.up)    return   0f;
        if (dir == Vector2Int.down)  return 180f;
        if (dir == Vector2Int.left)  return  90f;
        if (dir == Vector2Int.right) return -90f;
        return 0f;
    }
}