using UnityEngine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;

public class Cube : Item
{
    [Header("Sprite")]
    [SerializeField] private SpriteRenderer spriteRenderer;

    private Sprite normalSprite;
    private Sprite hintSprite;

    public override void Initialize(ItemAsset asset, int x, int y)
    {
        base.Initialize(asset, x, y);
        SetSprites(asset.mainSprite, asset.hintedSprite);
        transform.localScale = Vector3.one;
    }

    public void SetSprites(Sprite normal, Sprite hint)
    {
        normalSprite = normal;
        hintSprite = hint;
        spriteRenderer.sprite = normalSprite;
    }

    public void SetHintState(bool isHint)
    {
        spriteRenderer.sprite = isHint ? hintSprite : normalSprite;
    }

    public override bool TryActivate()
    {
        var group = GameManager.Instance.MatchService.FindConnectedGroup(
            GridManager.Instance.Grid, GridX, GridY, Key);
        if (group.Count < 2) return false;

        GridManager.Instance.DamageObstaclesAroundGroup(group);

        if (group.Count >= 4)
        {
            // Artık efekt burada değil, animasyon sonunda oynatılacak
            StartCoroutine(AnimateAndConvertToRocket(group, this));
        }
        else
        {
            foreach (var cube in group)
            {
                var mat = ParticleManager.Instance.GetMaterialByKey(cube.Key);
                if (mat != null)
                    ParticleManager.Instance.PlayCubeParticle(cube.transform.position, mat);
            }

            GameManager.Instance.MatchService.RemoveGroup(GridManager.Instance.Grid, group);
        }

        return true;
    }

    private IEnumerator AnimateAndConvertToRocket(List<Cube> group, Cube clickedCube)
    {
        ActionTracker.Instance.StartAction();

        Dictionary<Cube, int> originalOrders = new Dictionary<Cube, int>();
        foreach (var cube in group)
        {
            originalOrders[cube] = cube.spriteRenderer.sortingOrder;
            cube.spriteRenderer.sortingOrder = 5;
        }

        clickedCube.transform.DOScale(1.3f, 0.2f).SetEase(Ease.OutQuad);

        Vector3 rocketPos = clickedCube.transform.position;
        float delay = 0f;
        foreach (var cube in group)
        {
            if (cube == clickedCube) continue;
            cube.PlayRocketCandidateAnimation_Part1(rocketPos, delay);
            delay += 0.02f;
        }

        yield return new WaitForSeconds(0.2f);

        foreach (var cube in group)
        {
            if (cube == clickedCube) continue;
            cube.PlayRocketCandidateAnimation_Part2(rocketPos);
        }

        yield return new WaitForSeconds(0.2f);

        GameManager.Instance.MatchService.RemoveGroup(GridManager.Instance.Grid, group);

        string rocketKey = Random.Range(0, 2) == 0 ? "hro" : "vro";
        var rocket = GameManager.Instance.TileSpawner
            .Spawn(rocketKey, clickedCube.GridX, clickedCube.GridY,
                rocketPos, GridManager.Instance.GridParent);
        rocket.SetGridPosition(clickedCube.GridX, clickedCube.GridY);
        GridManager.Instance.Grid[clickedCube.GridX, clickedCube.GridY] = rocket;

        foreach (var cube in group)
        {
            if (originalOrders.TryGetValue(cube, out int order))
                cube.spriteRenderer.sortingOrder = order;
        }

        clickedCube.transform.localScale = Vector3.one;

        ActionTracker.Instance.EndAction();

        // 🔥 Efekt burada tetiklenir — rocket artık oluştu ve animasyon bitti
        ParticleManager.Instance.PlayRocketCreationParticleFollow(rocket.gameObject);
    }

    public void PlayRocketCandidateAnimation_Part1(Vector3 target, float delay)
    {
        Vector3 pushDir = (transform.position - target).normalized;
        Vector3 pushedPos = transform.position + pushDir * 1f;

        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(delay);
        seq.Append(transform.DOMove(pushedPos, 0.1f).SetEase(Ease.OutQuad));
        seq.Join(transform.DOScale(1.3f, 0.1f).SetEase(Ease.OutQuad));
    }

    public void PlayRocketCandidateAnimation_Part2(Vector3 target)
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOMove(target, 0.3f).SetEase(Ease.InQuad));
        seq.Join(transform.DOScale(0.1f, 0.3f).SetEase(Ease.InQuad));
        seq.OnComplete(() =>
        {
            transform.localScale = Vector3.one;
            DestroySelf();
        });
    }
}